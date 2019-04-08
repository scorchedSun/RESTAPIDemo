using Contracts;
using CSVDataSource.Contracts;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSVDataSource.Converters
{
    public class PersonConverter : Utils.Converter<(int id, string data), IPerson>
    {
        private readonly IConverter<string, Color> colourConverter;
        private readonly IConverter<string, IAddress> addressConverter;
        private readonly IPersonBuilder personBuilder;
        private readonly ICSVDataSourceConfiguration configuration;
        private string separator => configuration.Separator;
        private IList<string> fieldSequence => configuration.FieldSequence;

        public PersonConverter(IConverter<string, Color> colourConverter,
                               IConverter<string, IAddress> addressConverter,
                               IPersonBuilder personBuilder,
                               [Named(nameof(IPerson))] ICSVDataSourceConfiguration configuration)

        {
            this.colourConverter = colourConverter;
            this.addressConverter = addressConverter;
            this.personBuilder = personBuilder;
            this.configuration = configuration;
        }

        public override IPerson Convert((int id, string data) toConvert)
        {
            if (toConvert.data is null) throw new ArgumentNullException(nameof(toConvert.data));

            string[] parts = toConvert.data.Split(new[] { separator }, StringSplitOptions.None);
            if (parts.Length != fieldSequence.Count)
                throw new FormatException(nameof(toConvert));
            return personBuilder
                .WithID(toConvert.id)
                .WithName(parts[fieldSequence.IndexOf(nameof(IPerson.Name))])
                .WithLastName(parts[fieldSequence.IndexOf(nameof(IPerson.LastName))])
                .WithAddress(addressConverter.Convert(parts[fieldSequence.IndexOf(nameof(IPerson.Address))]))
                .WithFavouriteColour(colourConverter.Convert(parts[fieldSequence.IndexOf(nameof(IPerson.FavouriteColour))]))
                .Build();
        }

        public override (int id, string data) Convert(IPerson toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));
            return (toConvert.ID, ToString(toConvert));
        }

        private string ToString(IPerson person) => string.Join(separator, ToArray(person));

        private string[] ToArray(IPerson person)
        {
            string[] result = new string[fieldSequence.Count];
            Type t = person.GetType();
            foreach (string field in fieldSequence)
            {
                int index = fieldSequence.IndexOf(field);
                object value = t.GetProperty(field).GetValue(person);
                if (field.Equals(nameof(IPerson.Address)))
                    result[index] = addressConverter.Convert((IAddress)value);
                else if (field.Equals(nameof(IPerson.FavouriteColour)))
                    result[index] = colourConverter.Convert((Color)value);
                else
                    result[index] = value.ToString();
            }
            return result;
        }
    }
}
