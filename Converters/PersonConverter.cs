using Contracts;
using Models;
using System;
using System.Drawing;

namespace Converters
{
    public class PersonConverter : Converter<(int id, string data), IPerson>
    {
        private readonly IConverter<string, Color> colourConverter;
        private readonly IConverter<string, IAddress> addressConverter;

        public PersonConverter(IConverter<string, Color> colourConverter,
                               IConverter<string, IAddress> addressConverter)
        {
            if (colourConverter is null) throw new ArgumentNullException(nameof(colourConverter));
            if (addressConverter is null) throw new ArgumentNullException(nameof(addressConverter));

            this.colourConverter = colourConverter;
            this.addressConverter = addressConverter;
        }

        public override IPerson Convert((int id, string data) toConvert)
        {
            if (toConvert.data is null) throw new ArgumentNullException(nameof(toConvert.data));

            string[] parts = toConvert.data.Split(new[] { ", " }, StringSplitOptions.None);
            if (parts.Length != 4)
                throw new FormatException(nameof(toConvert));

            return new Person(toConvert.id,
                              parts[0],
                              parts[1],
                              addressConverter.Convert(parts[2]),
                              colourConverter.Convert(parts[3]));
        }

        public override (int id, string data) Convert(IPerson toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            return (toConvert.ID, ToString(toConvert));
        }

        private string ToString(IPerson person) => $"{person.Name}, {person.LastName}, {addressConverter.Convert(person.Address)}, {colourConverter.Convert(person.FavouriteColour)}";
    }
}
