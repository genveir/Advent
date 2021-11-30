using System;
using System.Runtime.Serialization;

namespace Advent2017.Shared.NodeList
{
    [Serializable]
    internal class ListIsCircularException : Exception
    {
        public ListIsCircularException()
        {
        }

        public ListIsCircularException(string message) : base(message)
        {
        }

        public ListIsCircularException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ListIsCircularException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}