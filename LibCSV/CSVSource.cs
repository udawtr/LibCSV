using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Youworks.Text
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CSVIgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CSVHeaderAttribute : Attribute
    {
        /// <summary>
        /// CSVヘッダ名
        /// </summary>
        public string Name { get; set; }

        public int Index { get; set; }

        public CSVHeaderAttribute()
            : base()
        {
            this.Index = -1;
        }

        public CSVHeaderAttribute(string name)
            : base()
        {
            this.Name = name;
            this.Index = -1;
        }

        public bool HasName
        {
            get
            {
                return Name != null;
            }
        }

        public bool HasIndex
        {
            get
            {
                return Index >= 0;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class CSVFileAttribute : Attribute
    {
        /// <summary>
        /// CSVヘッダの有無
        /// </summary>
        public bool HasHeader { get; set; }

        /// <summary>
        /// 読み飛ばす先頭行数
        /// </summary>
        public int SkipRowCount { get; set; }
        public CSVFileAttribute()
            : base()
        {
            this.HasHeader = true;
            this.SkipRowCount = 0;
        }
    }

    internal class CSVColumn
    {
        private FieldInfo Field;
        private PropertyInfo Property;
        internal CSVColumn(FieldInfo field)
        {
            this.Field = field;
        }

        internal CSVColumn(PropertyInfo property)
        {
            this.Property = property;
        }

        internal Type ColumnType
        {
            get
            {
                if (Field != null) return Field.FieldType;
                else return Property.PropertyType;
            }
        }

        internal object TypeResolve(object value, Type type)
        {
            if (type == typeof(string))
            {
                return Convert.ToString(value);
            }
            else if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }
            else if (type == typeof(int))
            {
                return Convert.ToInt32(value);
            }
            else if (type == typeof(DateTime))
            {
                return Convert.ToDateTime(value);
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, (string)value);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty((string)value))
                    return null;
                else
                    return TypeResolve(value, Nullable.GetUnderlyingType(type));
            }

            throw new FormatException(String.Format("{0}型を解決できませんでした。", type.Name));
        }

        internal void SetValue(object obj, object value)
        {
            if (Field != null)
            {
                Field.SetValue(obj, TypeResolve(value, Field.FieldType));
            }
            else
            {
                Property.SetValue(obj, TypeResolve(value, Property.PropertyType), null);
            }
        }
    }

    public interface ICSVSource
    {
        object ReadNextObject();
    }

    public sealed class CSVSource<T> : ICSVSource, IDisposable where T : new()
    {
        private string filename;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public bool HasMore
        {
            get
            {
                return sr!=null && !sr.EndOfStream;
            }
        }

        private StreamReader sr;

        private string[] header;
        private CSVColumn[] columns;

        private int lineNo = 0;

        public int LineNo
        {
            get
            {
                return lineNo;
            }
        }

        public string[] Header
        {
            get { return header; }
        }

        public CSVSource(string filename)
            : this(filename, Encoding.GetEncoding(932))
        {
        }

        public CSVSource(string filename, Encoding encoding)
        {
            //ファイルを開く
            sr = new StreamReader(filename, encoding);
            this.filename = filename;

            Init();
        }

        public CSVSource(Stream inputStream)
            : this(inputStream, Encoding.GetEncoding(932))
        {
        }

        public CSVSource(Stream inputStream, Encoding encoding)
        {
            //ファイルを開く
            sr = new StreamReader(inputStream, encoding);

            Init();
        }

        private void Init()
        {
            SkipHeaders();

            //ヘッダを読み取る
            if (HasHeader)
            {
                header = ReadHeader();
                PrepareFieldInfoWithHeader();
            }
            else
            {
                PrepareFieldInfoWithoutHeader();
            }
        }

        /// <summary>
        /// ヘッダの有無
        /// </summary>
        public bool HasHeader
        {
            get
            {
                var attrs = typeof(T).GetCustomAttributes(typeof(CSVFileAttribute), true) as CSVFileAttribute[];
                if (attrs == null || attrs.Length == 0)
                {
                    return true;
                }
                else
                {
                    return attrs[0].HasHeader;
                }
            }
        }

        private string[] ReadHeader()
        {
            var cols  = GetCSVLine();
            for ( var i = 0; i < cols.Length; i++ ) 
            {
                cols[i] = cols[i].TrimStart(new char[] { ' '});
            }
            return cols;
        }

        private void PrepareFieldInfoWithoutHeader()
        {
            var dict = new Dictionary<int, CSVColumn>();
            var maxIndex = -1;

            var typeFields = typeof(T).GetFields();
            foreach (System.Reflection.FieldInfo field in typeFields)
            {
                var attrs = (CSVHeaderAttribute[])field.GetCustomAttributes(typeof(CSVHeaderAttribute), true);
                if ( attrs.Length > 0 && attrs[0].HasIndex )
                {
                    dict[attrs[0].Index] = new CSVColumn(field);
                    if (maxIndex < attrs[0].Index)
                    {
                        maxIndex = attrs[0].Index;
                    }
                }
            }

            var typeProperties = typeof(T).GetProperties();
            foreach (System.Reflection.PropertyInfo property in typeProperties)
            {
                var attrs = (CSVHeaderAttribute[])property.GetCustomAttributes(typeof(CSVHeaderAttribute), true);
                if ( attrs.Length > 0 && attrs[0].HasIndex )
                {
                    dict[attrs[0].Index] = new CSVColumn(property);
                    if (maxIndex < attrs[0].Index)
                    {
                        maxIndex = attrs[0].Index;
                    }
                }
            }
            if (maxIndex >= 0)
            {
                columns = new CSVColumn[maxIndex + 1];
                for (var i = 0; i <= maxIndex; i++)
                {
                    if ( dict.ContainsKey(i) )
                    {
                        columns[i] = dict[i];
                    }
                }
            }
            else
            {
                columns = new CSVColumn[0];
            }
        }

        private void PrepareFieldInfoWithHeader()
        {
            columns = new CSVColumn[header.Length];

            var typeFields = typeof(T).GetFields();
            foreach (System.Reflection.FieldInfo field in typeFields)
            {
                var ignores = (CSVIgnoreAttribute[])field.GetCustomAttributes(typeof(CSVIgnoreAttribute), true);
                if (ignores != null && ignores.Length != 0) continue;

                var attrs = (CSVHeaderAttribute[])field.GetCustomAttributes(typeof(CSVHeaderAttribute), true);
                for (int i = 0; i < header.Length; i++)
                {
                    if (attrs.Length == 0)
                    {
                        if (field.Name == header[i])
                        {
                            columns[i] = new CSVColumn(field);
                        }
                    }
                    else if (attrs[0].HasName && attrs[0].Name == header[i])
                    {
                        columns[i] = new CSVColumn(field);
                    }
                    else if (attrs[0].HasIndex && attrs[0].Index == i)
                    {
                        columns[i] = new CSVColumn(field);
                    }
                }
            }

            var typeProperties = typeof(T).GetProperties();
            foreach (System.Reflection.PropertyInfo property in typeProperties)
            {
                var ignores = (CSVIgnoreAttribute[])property.GetCustomAttributes(typeof(CSVIgnoreAttribute), true);
                if (ignores != null && ignores.Length != 0) continue;

                var attrs = (CSVHeaderAttribute[])property.GetCustomAttributes(typeof(CSVHeaderAttribute), true);
                for (int i = 0; i < header.Length; i++)
                {
                    if (attrs.Length == 0)
                    {
                        if (property.Name == header[i])
                        {
                            columns[i] = new CSVColumn(property);
                        }
                    }
                    else if (attrs[0].HasName && attrs[0].Name == header[i])
                    {
                        columns[i] = new CSVColumn(property);
                    }
                    else if (attrs[0].HasIndex && attrs[0].Index == i)
                    {
                        columns[i] = new CSVColumn(property);
                    }
                }
            }
        }

        private int GetSkipRowCount()
        {
            var attrs = typeof(T).GetCustomAttributes(typeof(CSVFileAttribute), true) as CSVFileAttribute[];
            if (attrs == null || attrs.Length == 0)
            {
                return 0;
            }
            else
            {
                return attrs[0].SkipRowCount;
            }
        }

        private void SkipHeaders()
        {
            int skipRowCount = GetSkipRowCount();
            while (skipRowCount-- > 0)
            {
                GetCSVLine();
            }
        }

        /// <summary>
        /// 一行読み出す
        /// </summary>
        /// <returns></returns>
        private string[] GetCSVLine()
        {
            System.Collections.Specialized.StringCollection values = new System.Collections.Specialized.StringCollection();
            StringBuilder sb = new StringBuilder(sr.ReadLine());
            StringBuilder tmp = new StringBuilder();
            bool inDoubleQuote = false;
            bool escape = false;
            lineNo++;
            for (int off = 0; off < sb.Length || inDoubleQuote; off++)
            {
                if (!(off < sb.Length) && inDoubleQuote)
                {
                    sb.Append('\n');
                    sb.Append(sr.ReadLine());
                }
                char c = sb[off];
                if (inDoubleQuote)
                {
                    if (escape)
                    {
                        if (c == '\"')
                        {
                            //エスケープ成立
                            tmp.Append(c);
                        }
                        else
                        {
                            //エスケープ不成立
                            tmp.Append('\\');
                            tmp.Append(c);
                        }
                        //エスケープ処理の終了
                        escape = false;
                    }
                    else
                    {
                        if (c == '\"')
                        {
                            //""(2連続するダブルクオート)の場合は例外
                            if (off + 1 < sb.Length && sb[off + 1] == '\"')
                            {
                                tmp.Append("\"\"");
                                off++;
                            }
                            else
                            {
                                //二重引用符の終了
                                inDoubleQuote = false;
                            }
                        }
                        else if (c == '\\')
                        {
                            //エスケープ開始
                            escape = true;
                        }
                        else
                        {
                            //一時バッファに文字追加
                            tmp.Append(c);
                        }
                    }
                }
                else
                {
                    if (c == '\"')
                    {
                        //二重引用符の開始
                        inDoubleQuote = true;
                        tmp.Length = 0;
                    }
                    else if (c == ',')
                    {
                        values.Add(tmp.ToString());
                        tmp.Length = 0;
                    }
                    else
                    {
                        tmp.Append(c);
                    }
                }
            }
            values.Add(tmp.ToString());

            string[] lines = new string[values.Count];
            values.CopyTo(lines, 0);
            return lines;
        }

        public static List<T> ToList(string filename)
        {
            return ToList(filename, Encoding.GetEncoding(932));
        }

        public static List<T> ToList(string filename, Encoding encoding)
        {
            using(var stream =new FileStream(filename, FileMode.Open))
            {
                return ToList(stream, encoding);
            }
        }

        public static List<T> ToList(Stream stream)
        {
            return ToList(stream, Encoding.GetEncoding(932));
        }

        public static List<T> ToList(Stream stream, Encoding encoding)
        {
            var rows = new List<T>();
            using(var reader =new CSVSource<T>(stream, encoding))
            {
                while(reader.HasMore)
                {
                    rows.Add(reader.ReadNext());
                }
            }
            return rows;
        }

        public void Close()
        {
            if (this.sr != null)
            {
                sr.Close();
                sr = null;
            }
        }

        #region ImportSource メンバ

        public object ReadNextObject()
        {
            return ReadNext();
        }

        /// <summary>
        /// CSVLine型の各フィールドと同じ名前のヘッダー文字列にデータを読み込む
        /// </summary>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public T ReadNext()
        {
            int colNo = 0;
            try
            {
                if (!sr.EndOfStream)
                {
                    string[] data = GetCSVLine();
                    T csvLine = new T();

                    int l = Math.Min(columns.Length, data.Length);
                    for (colNo = 0; colNo < l; colNo++)
                    {
                        if (columns[colNo] != null)
                        {
                            columns[colNo].SetValue(csvLine, data[colNo]);
                        }
                    }

                    return csvLine;
                }
                return default(T);
            }
            catch (FormatException fmtex)
            {
                throw new InvalidOperationException(String.Format("カラム'{2}'を{3}型に変換できません。[{0}行{1}列]", lineNo, colNo+1, header[colNo], columns[colNo].ColumnType.Name), fmtex);
            }
        }

        #endregion

        #region IDisposable メンバ

        public void Dispose()
        {
            if (this.sr != null)
                sr.Close();
        }

        #endregion
    }
}