using System;
using System.Runtime.Serialization;

namespace Youworks.Text
{
    /// <summary>
    /// 必須項目に値が与えられなかった場合にスローされる例外。
    /// </summary>
    public class CSVValueEmptyException : Exception
    {
        public readonly int? RowNumber;
        public readonly int? ColumnNumber;

        public CSVValueEmptyException()
        {
        }

        public CSVValueEmptyException(string message) : base(message)
        {
        }

        public CSVValueEmptyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CSVValueEmptyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public CSVValueEmptyException(string message, int rowNumber, int columnNumber) : base(message)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }

        public CSVValueEmptyException(string message, Exception inner, int rowNumber, int columnNumber) : base(message, inner)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }
    }
}
