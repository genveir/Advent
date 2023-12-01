using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent2023.Shared
{
    public class CircularList<T> : ICollection<T>
    {
        private List<T> innerList;

        public CircularList() : this(new List<T>()) { }
        public CircularList(IEnumerable<T> collection)
        {
            innerList = new List<T>(collection);
        }

        public T this[long n] {
            get { return innerList[PosMod(n)]; }
            set { innerList[PosMod(n)] = value; }
        }

        private int PosMod(long n)
        {
            int remainder = (int)(n % innerList.Count);
            return (remainder < 0) ? remainder += innerList.Count : remainder;
        }

        public void Insert(long index, T item) => Insert(index, new T[] { item });
        public void Insert(long index, IEnumerable<T> items)
        {
            var currentArray = innerList.ToArray();
            var itemArray = items.ToArray();

            index = PosMod(index);

            var newArray = new T[this.Count + itemArray.Length];

            Array.Copy(currentArray, newArray, index);
            Array.Copy(itemArray, 0, newArray, index, itemArray.Length);
            Array.Copy(currentArray, index, newArray, index + itemArray.Length, innerList.Count - index);

            innerList = newArray.ToList();
        }

        public void ReverseRange(long index, int number)
        {
            var copy = new CircularList<T>(this);

            index = PosMod(index);

            for (int n = 0; n < number; n++)
            {
                this[index + n] = copy[index + number - n - 1];
            }
        }

        bool ICollection<T>.IsReadOnly => true;
        public int Count => innerList.Count;
        public void Add(T item) => innerList.Add(item);
        public void Clear() => innerList.Clear();
        public bool Contains(T item) => innerList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => innerList.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => innerList.GetEnumerator();
        public bool Remove(T item) => innerList.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => innerList.GetEnumerator();
    }
}
