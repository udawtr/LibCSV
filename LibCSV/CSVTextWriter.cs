using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youworks.Text
{
    public sealed class CSVTextWriter : IDisposable
    {
        private System.IO.StreamWriter baseWriter;

        private CSVTextWriter()
        {
            var provider = System.Text.CodePagesEncodingProvider.Instance;
            System.Text.Encoding.RegisterProvider(provider);
        }

        public CSVTextWriter(System.IO.StreamWriter streamWriter)
        {
            baseWriter = streamWriter;
        }

        public CSVTextWriter(string fileName)
        {
            baseWriter = new System.IO.StreamWriter(fileName, false, System.Text.Encoding.GetEncoding(932));
        }

        public void Write(string text)
        {
            if (text != null)
            {
                //エスケープ処理
                var escaped_str = text.Replace("\"", "\"\"");
                baseWriter.Write("\"" + escaped_str + "\"");
            }
            baseWriter.Write(",");
        }

        public void Write(IEnumerable<object> line)
        {
            if (line != null)
            {
                foreach (var item in line)
                {
                    Write(Convert.ToString(item));
                }
            }
        }

        public void Write(string[] line)
        {
            if (line != null)
            {
                foreach (var item in line)
                {
                    Write(item);
                }
            }
        }

        public void WriteLine(IEnumerable<object> line)
        {
            Write(line);
            baseWriter.Write(baseWriter.NewLine);
        }

        public void WriteLine(string[] line)
        {
            Write(line);
            baseWriter.Write(baseWriter.NewLine);
        }

        public void Close()
        {
            baseWriter.Flush();
            baseWriter.Close();
        }

        #region IDisposable メンバー

        public void Dispose()
        {
            if (baseWriter != null)
            {
                baseWriter.Close();
                baseWriter.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}