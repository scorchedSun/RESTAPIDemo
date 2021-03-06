﻿using Contracts;
using CSVDataSource.Converters;
using Ninject.Modules;
using System.Drawing;

namespace CSVDataSource
{
    /// <summary>
    /// Binding for Ninject
    /// </summary>
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IConverter<string, IAddress>>().To(typeof(AddressConverter));
            Bind<IConverter<string, Color>>().To(typeof(ColourConverter));
            Bind<IConverter<(uint, string), IPerson>>().To(typeof(PersonConverter));
            Bind<IDataSource<IPerson>>().To(typeof(CSVPersonDataSource));
        }
    }
}
