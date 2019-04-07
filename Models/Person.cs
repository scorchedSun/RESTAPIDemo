﻿using Contracts;
using System.Drawing;

namespace Models
{
    public class Person : IPerson
    {
        public string Name { get; }

        public string LastName { get; }

        public IAddress Address { get; }

        public Color FavouriteColour { get; }

        public int ID { get; }

        internal Person(int id, string name, string lastName, IAddress address, Color favouriteColour)
        {
            ID = id;
            Name = name;
            LastName = lastName;
            Address = address;
            FavouriteColour = favouriteColour;
        }
    }
}