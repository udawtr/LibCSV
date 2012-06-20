using System;
using System.Runtime.Serialization;

namespace Youworks.Text
{
    /// <summary>
    /// 選択式項目に選択肢以外の値が与えられたときにスローされる例外。
    /// </summary>
    public class CSVValueWrongOptionException : Exception
    {
        public readonly int? RowNumber;
        public readonly int? ColumnNumber;

        public CSVValueWrongOptionException()
        {
        }

        public CSVValueWrongOptionException(string message) : base(message)
        {
        }

        public CSVValueWrongOptionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CSVValueWrongOptionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public CSVValueWrongOptionException(string message, int rowNumber, int columnNumber) : base(message)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }

        public CSVValueWrongOptionException(string message, Exception inner, int rowNumber, int columnNumber) : base(message, inner)
        {
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }
    }
}
