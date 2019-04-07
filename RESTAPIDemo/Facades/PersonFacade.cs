using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPIDemo.Facades
{
    public class PersonFacade
    {
        private readonly IPerson person;

        public int id => person.ID;
        public string name => person.Name;
        public string lastname => person.LastName;
        public string zipcode => person.Address.ZipCode;
        public string city => person.Address.City;
        public string color => person.FavouriteColour.Name.ToLower();

        public PersonFacade(IPerson person) => this.person = person;
    }
}
