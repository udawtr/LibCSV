using System;
using System.Runtime.Serialization;

namespace Youworks.Text
{
    /// <summary>
    /// CSV読み込み時のエラー種別
    /// </summary>
    public enum CSVValueErrorType
    {
        /// <summary>
        /// 文字列の解析エラー ex)intにならない
        /// </summary>
        Parsing,

        /// <summary>
        /// 必須項目にも関わらず値が設定されていない
        /// </summary>
        Required,

        /// <summary>
        /// <c>List</c>に指定された値以外の値が入力された
        /// </summary>
        List
    }

    [Serializable]
    public class CSVValueInvalidException : Exception
    {
        public readonly CSVValueErrorType Error;
        public readonly int? RowNumber;
        public readonly int? ColumnNumber;

        public CSVValueInvalidException()
        {
        }

        public CSVValueInvalidException(CSVValueErrorType error, string message)
            : base(message)
        {
            Error = error;
        }

        public CSVValueInvalidException(CSVValueErrorType error, string message, Exception inner)
            : base(message, inner)
        {
            Error = error;
        }

        protected CSVValueInvalidException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public CSVValueInvalidException(CSVValueErrorType error, string message, int rowNumber, int columnNumber)
            : base(message)
        {
            Error = error;
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }

        public CSVValueInvalidException(CSVValueErrorType error, string message, Exception inner, int rowNumber, int columnNumber)
            : base(message, inner)
        {
            Error = error;
            RowNumber = rowNumber;
            ColumnNumber = columnNumber;
        }
    }
}
