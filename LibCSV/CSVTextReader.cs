using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youworks.Text
{
    public sealed class CSVTextReader : IDisposable
    {
        private System.IO.StreamReader baseReader;

        public CSVTextReader(System.IO.StreamReader streamReader)
        {
            baseReader = streamReader;
        }

        public CSVTextReader(string fileName)
            : this(fileName, System.Text.Encoding.GetEncoding(932))
        {
        }

        public CSVTextReader(string fileName, System.Text.Encoding enc)
            : this(new System.IO.StreamReader(fileName, enc))
        {
        }

        public CSVTextReader(System.IO.Stream stream, System.Text.Encoding enc)
            : this(new System.IO.StreamReader(stream, enc))
        {
        }

        public static IEnumerable<string[]> ReadAll(string fileName, System.Text.Encoding enc, int skipLines = 1)
        {
            using (var reader = new CSVTextReader(fileName, enc))
            {
                reader.SkipLines(skipLines);
                while (reader.HasMore)
                {
                    yield return reader.GetNextLine();
                }
                reader.Close();
            }
        }

        private int lineNo = 0;

        public int LineNo
        {
            get
            {
                return lineNo;
            }
        }

        public void SkipLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                baseReader.ReadLine();
                lineNo++;
            }
        }

        public bool HasMore
        {
            get
            {
                return !baseReader.EndOfStream;
            }
        }

        /// <summary>
        /// 一行読み出す
        /// </summary>
        public string[] GetNextLine()
        {
            System.Collections.Specialized.StringCollection values = new System.Collections.Specialized.StringCollection();
            StringBuilder sb = new StringBuilder(baseReader.ReadLine());
            StringBuilder tmp = new StringBuilder();
            bool inDoubleQuote = false;
            bool escape = false;
            lineNo++;
            for (int off = 0; off < sb.Length || inDoubleQuote; off++)
            {
                if (!(off < sb.Length) && inDoubleQuote)
                {
                    sb.Append('\n');
                    sb.Append(baseReader.ReadLine());
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
                                tmp.Append("\"");
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

        public void Close()
        {
            baseReader.Close();
        }

        #region IDisposable メンバー

        public void Dispose()
        {
            if (baseReader != null)
            {
                baseReader.Close();
                baseReader.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion    }
    }
}
