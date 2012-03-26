using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace Youworks.Text
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
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

        private StreamReader sr;

        private string[] header;
        private System.Reflection.FieldInfo[] fields;

        public string[] Header
        {
            get { return header; }
            private set { header = value; }
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

            //ヘッダを読み取る
            header = ReadHeader();
            PrepareFieldInfo();
        }

        public CSVSource(Stream inputStream)
            : this(inputStream, Encoding.GetEncoding(932))
        {
        }

        public CSVSource(Stream inputStream, Encoding encoding)
        {
            //ファイルを開く
            sr = new StreamReader(inputStream, encoding);

            //ヘッダを読み取る
            header = ReadHeader();
            PrepareFieldInfo();
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

        private void PrepareFieldInfo()
        {
            T csvLine = new T();
            Type csvLineType = csvLine.GetType();
            var typeFields = csvLineType.GetFields();

            fields = new FieldInfo[header.Length];
            foreach (System.Reflection.FieldInfo field in typeFields)
            {
                var attrs = (CSVHeaderAttribute[])field.GetCustomAttributes(typeof(CSVHeaderAttribute), true);
                for (int i = 0; i < header.Length; i++)
                {
                    if (attrs.Length == 0)
                    {
                        if (field.Name == header[i])
                        {
                            fields[i] = field;
                        }
                    }
                    else if (attrs[0].HasName && attrs[0].Name == header[i])
                    {
                        fields[i] = field;
                    }
                    else if (attrs[0].HasIndex && attrs[0].Index == i)
                    {
                        fields[i] = field;
                    }
                }
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
            for (int off = 0; off < sb.Length || inDoubleQuote; off++)
            {
                if (!(off < sb.Length))
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
                            tmp.Append('\"');
                            tmp.Append(c);
                        }
                        //エスケープ処理の終了
                        escape = false;
                    }
                    else
                    {
                        if (c == '\"')
                        {
                            //二重引用符の終了
                            inDoubleQuote = false;
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
            if (tmp.Length > 0)
                values.Add(tmp.ToString());

            string[] lines = new string[values.Count];
            values.CopyTo(lines, 0);
            return lines;
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
            if (!sr.EndOfStream)
            {
                string[] data = GetCSVLine();
                T csvLine = new T();

                int l = Math.Min(fields.Length, data.Length);
                for (int i = 0; i < l; i++)
                {
                    System.Reflection.FieldInfo field = fields[i];
                    if (field != null)
                    {
                        field.SetValue(csvLine, data[i]);
                    }
                }

                return csvLine;
            }

            return default(T);
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