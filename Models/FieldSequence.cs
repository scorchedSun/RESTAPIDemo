using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class FieldSequence : IFieldSequence
    {
        private readonly IList<string> fields;

        public FieldSequence(string[] fields)
        {
            if (fields is null) throw new ArgumentNullException(nameof(fields));
            this.fields = new List<string>(fields);
        }

        public string this[int index] { get => fields[index]; set => fields[index] = value; }

        public int Count => fields.Count;

        public bool IsReadOnly => true;

        public void Add(string item) => throw new InvalidOperationException(nameof(FieldSequence) + "is readonly");

        public void Clear() => throw new InvalidOperationException(nameof(FieldSequence) + "is readonly");

        public bool Contains(string item) => fields.Contains(item);

        public void CopyTo(string[] array, int arrayIndex) => fields.CopyTo(array, arrayIndex);

        public IEnumerator<string> GetEnumerator() => fields.GetEnumerator();

        public int IndexOf(string item) => fields.IndexOf(item);

        public void Insert(int index, string item) => throw new InvalidOperationException(nameof(FieldSequence) + "is readonly");

        public bool Remove(string item) => throw new InvalidOperationException(nameof(FieldSequence) + "is readonly");

        public void RemoveAt(int index) => throw new InvalidOperationException(nameof(FieldSequence) + "is readonly");

        IEnumerator IEnumerable.GetEnumerator() => fields.GetEnumerator();
    }
}
