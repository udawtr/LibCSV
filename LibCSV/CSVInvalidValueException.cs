using System;
using System.Runtime.Serialization;

namespace Youworks.Text
{
    [Serializable]
    public class CSVValueInvalidException : Exception
    {
        public readonly int? RowNumber;
        public readonly int? ColumnNumber;

        public CSVValueInvalidException()
        {
        }

        public CSVValueInvalidException(string message) : base(message)
        {
        }

        public CSVValueInvalidException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CSVValueInvalidException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public CSVValueInvalidException(string message, int rowNumber, int columnNumber) : base(message)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }

        public CSVValueInvalidException(string message, Exception inner, int rowNumber, int columnNumber) : base(message, inner)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }
    }
}
