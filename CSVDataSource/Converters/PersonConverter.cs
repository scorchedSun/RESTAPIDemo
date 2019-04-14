using Contracts;
using CSVDataSource.Contracts;
using Exceptions;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSVDataSource.Converters
{
    public class PersonConverter : Utils.Converter<(uint id, string data), IPerson>
    {
        private readonly IConverter<string, Color> colourConverter;
        private readonly IConverter<string, IAddress> addressConverter;
        private readonly IPersonBuilderFactory personBuilderFactory;
        private readonly ICSVDataSourceConfiguration configuration;
        private string Separator => configuration.Separator;
        private IList<string> FieldSequence => configuration.FieldSequence;

        private IDictionary<string, Func<dynamic, dynamic>> propertyConverters;

        public PersonConverter(IConverter<string, Color> colourConverter,
                               IConverter<string, IAddress> addressConverter,
                               IPersonBuilderFactory personBuilderFactory,
                               [Named(nameof(IPerson))] ICSVDataSourceConfiguration configuration)

        {
            if (colourConverter is null) throw new ArgumentNullException(nameof(colourConverter));
            if (addressConverter is null) throw new ArgumentNullException(nameof(addressConverter));
            if (personBuilderFactory is null) throw new ArgumentNullException(nameof(personBuilderFactory));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            this.colourConverter = colourConverter;
            this.addressConverter = addressConverter;
            this.personBuilderFactory = personBuilderFactory;
            this.configuration = configuration;

            CreatePropertyConverterMappings();
        }

        public override IPerson Convert((uint id, string data) toConvert)
        {
            if (toConvert.data is null) throw new ArgumentNullException(nameof(toConvert));

            string[] parts = toConvert.data.Split(new[] { Separator }, StringSplitOptions.None);
            parts = TrimEntries(parts);
            EnsureNumberOfPartsIsValid(parts);

            return personBuilderFactory.Create()
                .WithID(toConvert.id)
                .WithName(GetValue(nameof(IPerson.Name), parts))
                .WithLastName(GetValue(nameof(IPerson.LastName), parts))
                .WithAddress(GetValue(nameof(IPerson.Address), parts))
                .WithFavouriteColour(GetValue(nameof(IPerson.FavouriteColour), parts))
                .Build();
        }

        public override (uint id, string data) Convert(IPerson toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));
            return (toConvert.ID, ToString(toConvert));
        }

        private void CreatePropertyConverterMappings()
        {
            propertyConverters = new Dictionary<string, Func<dynamic, dynamic>>
            {
                [nameof(IPerson.Address)] = new Func<dynamic, dynamic>((dynamic value) => this.addressConverter.Convert(value)),
                [nameof(IPerson.FavouriteColour)] = new Func<dynamic, dynamic>((dynamic value) => this.colourConverter.Convert(value))
            };
        }

        private void EnsureNumberOfPartsIsValid(string[] parts)
        {
            if (parts.Length < FieldSequence.Count)
                throw new TooFewFieldsException(parts.Length, FieldSequence.Count);
            else if (parts.Length > FieldSequence.Count)
                throw new TooManyFieldsException(parts.Length, FieldSequence.Count);
        }

        private string[] TrimEntries(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Trim();
            return array;
        }

        private string ToString(IPerson person) => string.Join(Separator, ToArray(person));

        private string[] ToArray(IPerson person)
        {
            string[] result = new string[FieldSequence.Count];
            foreach (string field in FieldSequence)
            {
                int index = FieldSequence.IndexOf(field);
                result[index] = GetValue(field, person, person.GetType()).ToString();
            }
            return result;
        }

        private dynamic GetValue(string property, string[] parts)
            => ConvertProperty(property, parts[FieldSequence.IndexOf(property)]);

        private dynamic GetValue(string property, IPerson person, Type t)
            => ConvertProperty(property, t.GetProperty(property).GetValue(person));

        private dynamic ConvertProperty(string property, dynamic value)
        {
            if (propertyConverters.ContainsKey(property))
                value = propertyConverters[property](value);
            return value;
        }

        /// <summary>
        /// Casts an object to <typeparamref name="T"/>.
        /// Used as a little hack to ensure returning the correct type when
        /// converting properties of a person.
        /// Called using reflection.
        /// </summary>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <param name="o">The <see cref="object"/> to cast</param>
        /// <returns><paramref name="o"/> casted as <typeparamref name="T"/></returns>
        private T Cast<T>(object o) => (T)o;
    }
}
