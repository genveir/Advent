using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared
{
    public class ModList<T> : ICollection<T>
    {
        private List<T> innerList;

        public ModList() : this(new List<T>()) { }
        public ModList(IEnumerable<T> collection)
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
            // Should do copyto's.
            var itemArray = items.ToArray();

            index = PosMod(index);

            var newList = new List<T>();
            for (long n = 0; n < index; n++) newList.Add(this[n]);
            for (long i = 0; i < items.Count(); i++) newList.Add(itemArray[i]);
            for (long n = index; n < innerList.Count; n++) newList.Add(this[n]);

            innerList = newList;
        }

        public void ReverseRange(long index, int number)
        {
            var copy = new ModList<T>(this);

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
