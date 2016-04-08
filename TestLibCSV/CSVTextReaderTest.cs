using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestLibCSV
{
    [TestClass()]
    public class CSVTextReaderTest
    {
        private CSVTextReader CreateCSVTextReaderFromText(string text)
        {
            var stream = new MemoryStream();
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
            var target = new CSVTextReader(reader);
            return target;
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void GetNextLineTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nLiteral,3.4,100.5,2010/1/3";
            var target = CreateCSVTextReaderFromText(text);

            var line = target.GetNextLine();
            Assert.AreEqual("Col1", line[0]);
            Assert.AreEqual("Col2", line[1]);
            Assert.AreEqual("Col3", line[2]);
            Assert.AreEqual("Col4", line[3]);
            Assert.AreEqual(1, target.LineNo);

            line = target.GetNextLine();
            Assert.AreEqual("Literal", line[0]);
            Assert.AreEqual("3.4", line[1]);
            Assert.AreEqual("100.5", line[2]);
            Assert.AreEqual("2010/1/3", line[3]);
            Assert.AreEqual(2, target.LineNo);
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void SkipLinesTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nLiteral,3.4,100.5,2010/1/3";
            var target = CreateCSVTextReaderFromText(text);

            target.SkipLines(1);
            Assert.AreEqual(1, target.LineNo);

            var line = target.GetNextLine();
            Assert.AreEqual("Literal", line[0]);
            Assert.AreEqual("3.4", line[1]);
            Assert.AreEqual("100.5", line[2]);
            Assert.AreEqual("2010/1/3", line[3]);
            Assert.AreEqual(2, target.LineNo);
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void DoubleQuotationTest()
        {
            const string text = "01234,\"AB\"\"CD\",\"EFG\"";
            var target = CreateCSVTextReaderFromText(text);

            var line = target.GetNextLine();
            Assert.AreEqual("01234", line[0]);
            Assert.AreEqual("AB\"CD", line[1]);
            Assert.AreEqual("EFG", line[2]);
        }
    }
}
