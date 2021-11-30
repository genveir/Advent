using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.Shared.NodeList
{
    public class ListNode<T>
    {
        public ListNode<T> Next { get; set; }

        public ListNode<T> Previous { get; set; }

        public T Value { get; set; }

        public ListNode(T value) => this.Value = value;

        public ListNode<T> AddBefore(T value) => LinkPrevious(new ListNode<T>(value));
        public ListNode<T> LinkPrevious(ListNode<T> previousNode)
        {
            Previous = previousNode;
            Previous.Next = this;

            return Previous;
        }

        public ListNode<T> AddAfter(T value) => LinkNext(new ListNode<T>(value));
        public ListNode<T> LinkNext(ListNode<T> nextNode)
        {
            Next = nextNode;
            Next.Previous = this;

            return Next;
        }

        public ListNode<T> Last() =>_moveToLast(this);
        private ListNode<T> _moveToLast(ListNode<T> start)
        {
            if (ReferenceEquals(this, start)) throw new ListIsCircularException("cannot move to last, list is circular");
            else if (Next == null) return this;

            return Next._moveToLast(start);
        }

        public ListNode<T> First() => _moveToFirst(this);
        private ListNode<T> _moveToFirst(ListNode<T> start)
        {
            if (ReferenceEquals(this, start)) throw new ListIsCircularException("cannot move to first, list is circular");
            else if (Previous == null) return this;

            return Previous._moveToFirst(start);
        }

        public void Circularize() => Last().LinkNext(First());

        public ListNode<T> AddRange(IEnumerable<T> values)
        {
            ListNode<T> current = this;
            foreach (T val in values)
            {
                current = current.Insert(val);
            }

            return current;
        }

        public ListNode<T> Insert(T value)
        {
            var nextNode = Next;
            var newNode = AddAfter(value);

            if (nextNode != null)
            {
                nextNode.Previous = newNode;
                newNode.Next = nextNode;
            }

            return newNode;
        }
    }
}
