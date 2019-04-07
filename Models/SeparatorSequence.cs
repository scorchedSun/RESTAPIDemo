using Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class SeparatorSequence : ISeparatorSequence
    {
        public string Value { get; }

        public SeparatorSequence(string value)
        {
            if (value is null) throw new ArgumentException(nameof(value));
            Value = value;
        }
    }
}
