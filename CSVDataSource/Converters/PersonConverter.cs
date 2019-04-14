using Contracts;
using CSVDataSource.Contracts;
using Exceptions;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSVDataSource.Converters
{
    /// <summary>
    /// Converts a <see cref="Tuple{uint, string}"/> to an <see cref="IPerson"/> and vice versa.
    /// </summary>
    public class PersonConverter : Utils.Converter<(uint id, string data), IPerson>
    {
        private readonly IConverter<string, Color> colourConverter;
        private readonly IConverter<string, IAddress> addressConverter;
        private readonly IPersonBuilderFactory personBuilderFactory;
        private readonly ICSVDataSourceConfiguration configuration;
        private string Separator => configuration.Separator;
        private IList<string> FieldSequence => configuration.FieldSequence;

        private IDictionary<string, Func<dynamic, dynamic>> propertyConverters;

        /// <summary>
        /// Create a new <see cref="PersonConverter"/>.
        /// </summary>
        /// <param name="colourConverter">Converter for <see cref="Color"/>s</param>
        /// <param name="addressConverter">Converter for <see cref="IAddress"/>es</param>
        /// <param name="personBuilderFactory">Factory for <see cref="IPersonBuilder"/>s</param>
        /// <param name="configuration">Configuration</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
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

        /// <summary>
        /// Convert a <see cref="string"/> to an <see cref="IPerson"/>.
        /// </summary>
        /// <param name="toConvert">The <see cref="string"/> to convert</param>
        /// <returns>The converted <see cref="string"/></returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="TooFewFieldsException">The number of fields in the given <see cref="string"/> is lower than the number of fields defined in the configuration</exception>
        /// <exception cref="TooManyFieldsException">The number of fields in the given <see cref="string"/> is higher than the number of fields defined in the configuration</exception>
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

        /// <summary>
        /// Convert an <see cref="IPerson"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="toConvert">The <see cref="IPerson"/> to convert</param>
        /// <returns>The converted <see cref="IPerson"/></returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public override (uint id, string data) Convert(IPerson toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));
            return (toConvert.ID, ToString(toConvert));
        }

        /// <summary>
        /// Creates mappings between properties of <see cref="IPerson"/> and functions to convert
        /// the properties' value to another value.
        /// </summary>
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

        /// <summary>
        /// Trims all whitespaces from the beginning and end of each entry.
        /// </summary>
        /// <param name="array">The values to be trimmed</param>
        /// <returns>The trimmed values</returns>
        private string[] TrimEntries(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Trim();
            return array;
        }

        /// <summary>
        /// Converts an <see cref="IPerson"/> to a <see cref="string"/>.
        /// Used for storing the <see cref="IPerson"/> in the CSV file.
        /// </summary>
        /// <param name="person">The <see cref="IPerson"/> to convert</param>
        /// <returns>A <see cref="string"/> representing <paramref name="person"/></returns>
        private string ToString(IPerson person) => string.Join(Separator, ToArray(person));

        /// <summary>
        /// Converts an <see cref="IPerson"/> to a string array.
        /// Uses the <see cref="FieldSequence"/> to ensure the <see cref="IPerson"/>'s properties
        /// are in the correct order for the CSV file.
        /// </summary>
        /// <param name="person">The <see cref="IPerson"/> to convert</param>
        /// <returns>A string array representing <paramref name="person"/></returns>
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

        /// <summary>
        /// Gets the value for the property of a person from the parts string.
        /// Used when reading from the CSV.
        /// </summary>
        /// <param name="property">Name of the property</param>
        /// <param name="parts">The <see cref="string"/>s representing the person</param>
        /// <returns>The value for the property</returns>
        private dynamic GetValue(string property, string[] parts)
            => ConvertProperty(property, parts[FieldSequence.IndexOf(property)]);

        /// <summary>
        /// Gets the value of a person's property from an instance of <see cref="IPerson"/>.
        /// Used when writing to the CSV.
        /// </summary>
        /// <param name="property">Name of the property</param>
        /// <param name="person">The instance of <see cref="IPerson"/> to read from</param>
        /// <param name="t">The <see cref="Type"/> implementing <see cref="IPerson"/></param>
        /// <returns>The property's value from <paramref name="person"/></returns>
        private dynamic GetValue(string property, IPerson person, Type t)
            => ConvertProperty(property, t.GetProperty(property).GetValue(person));

        /// <summary>
        /// Converts the value of a property using the property converters.
        /// </summary>
        /// <param name="property">Name of the property</param>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted <paramref name="value"/></returns>
        private dynamic ConvertProperty(string property, dynamic value)
        {
            if (propertyConverters.ContainsKey(property))
                value = propertyConverters[property](value);
            return value;
        }
    }
}
