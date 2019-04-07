using Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class PhysicalDataSource : IPhysicalDataSource
    {
        public string FilePath { get; }

        public PhysicalDataSource(string filePath) => FilePath = filePath;
    }
}
