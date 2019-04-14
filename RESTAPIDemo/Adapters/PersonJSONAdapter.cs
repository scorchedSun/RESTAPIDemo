﻿using Contracts;
using Newtonsoft.Json;
using System;

namespace RESTAPIDemo.Facades
{
    /// <summary>
    /// Adapts an instance of <see cref="IPerson"/> to be represented as JSON.
    /// </summary>
    public class PersonJSONAdapter
    {
        private readonly IPerson person;

        [JsonProperty("id")]
        public uint ID => person.ID;

        [JsonProperty("name")]
        public string Name => person.Name;

        [JsonProperty("lastname")]
        public string LastName => person.LastName;

        [JsonProperty("zipcode")]
        public string ZipCode => person.Address.ZipCode;

        [JsonProperty("city")]
        public string City => person.Address.City;

        [JsonProperty("color")]
        public string Color => person.FavouriteColour.Name.ToLower();

        public PersonJSONAdapter(IPerson person)
        {
            if (person is null) throw new ArgumentNullException(nameof(person));
            this.person = person;
        }
    }
}
