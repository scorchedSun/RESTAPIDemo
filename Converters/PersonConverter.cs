using Contracts;
using System;
using System.Drawing;

namespace Converters
{
    public class PersonConverter : Converter<(int id, string data), IPerson>
    {
        private readonly IConverter<string, Color> colourConverter;
        private readonly IConverter<string, IAddress> addressConverter;
        private readonly IPersonBuilder personBuilder;

        public PersonConverter(IConverter<string, Color> colourConverter,
                               IConverter<string, IAddress> addressConverter,
                               IPersonBuilder personBuilder)
        {
            this.colourConverter = colourConverter;
            this.addressConverter = addressConverter;
            this.personBuilder = personBuilder;
        }

        public override IPerson Convert((int id, string data) toConvert)
        {
            if (toConvert.data is null) throw new ArgumentNullException(nameof(toConvert.data));

            string[] parts = toConvert.data.Split(new[] { ", " }, StringSplitOptions.None);
            if (parts.Length != 4)
                throw new FormatException(nameof(toConvert));
            return personBuilder
                .WithID(toConvert.id)
                .WithName(parts[0])
                .WithLastName(parts[1])
                .WithAddress(addressConverter.Convert(parts[2]))
                .WithFavouriteColour(colourConverter.Convert(parts[3]))
                .Build();
        }

        public override (int id, string data) Convert(IPerson toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            return (toConvert.ID, ToString(toConvert));
        }

        private string ToString(IPerson person) => $"{person.Name}, {person.LastName}, {addressConverter.Convert(person.Address)}, {colourConverter.Convert(person.FavouriteColour)}";
    }
}
