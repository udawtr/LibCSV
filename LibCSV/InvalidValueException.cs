using System;
using System.Runtime.Serialization;

namespace Youworks.Text
{
    [Serializable]
    public class InvalidValueException : Exception
    {
        public readonly int? RowNumber;
        public readonly int? ColumnNumber;

        public InvalidValueException()
        {
        }

        public InvalidValueException(string message) : base(message)
        {
        }

        public InvalidValueException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidValueException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public InvalidValueException(string message, int rowNumber, int columnNumber) : base(message)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }

        public InvalidValueException(string message, Exception inner, int rowNumber, int columnNumber) : base(message, inner)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }
    }
}
