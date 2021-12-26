using System;
using System.Runtime.Serialization;

namespace Advent2021.Advent24.Expressions
{
    [Serializable]
    internal class ImmutableObjectException : Exception
    {
        public ImmutableObjectException()
        {
        }

        public ImmutableObjectException(string message) : base(message)
        {
        }

        public ImmutableObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImmutableObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}