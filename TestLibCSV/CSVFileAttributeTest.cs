using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLibCSV
{
    [TestClass()]
    public class CSVAttributeTest
    {

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        /// <summary>
        ///Test for CSVAttribute
        ///</summary>
        [TestMethod()]
        public void CSVAttributeNoHeaderTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "A,B\r\nD,E";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<TestClass>(stream, System.Text.Encoding.UTF8);

            var firstLine = target.ReadNext();
            Assert.AreEqual("A", firstLine.Col1);
            Assert.AreEqual("B", firstLine.Col2);

            //Without Index
            stream = new System.IO.MemoryStream();
            text = "A,B\r\nD,E";
            bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target2 = new CSVSource<TestClass2>(stream, System.Text.Encoding.UTF8);

            var firstLine2 = target2.ReadNext();
            Assert.IsNull(firstLine2.Col1);
            Assert.IsNull(firstLine2.Col2);
        }

        /// <summary>
        ///Test for CSVAttribute
        ///</summary>
        [TestMethod()]
        public void CSVAttributeWithSkipRowCountTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "\r\n\r\nCol1,Col2\r\nA,B\r\nD,E";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<TestClass3>(stream, System.Text.Encoding.UTF8);

            var firstLine = target.ReadNext();
            Assert.AreEqual("A", firstLine.Col1);
            Assert.AreEqual("B", firstLine.Col2);
        }

        /// <summary>
        ///Test for CSVAttribute
        ///</summary>
        [TestMethod()]
        public void CSVAttributeWithSkipRowCountOnReadingEmptyTextTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<TestClass3>(stream, System.Text.Encoding.UTF8);

            var firstLine = target.ReadNext();
            Assert.IsNull(firstLine);
        }
 
        [CSVFile(HasHeader = false)]
        private class TestClass
        {
            [CSVHeader(Index=0)]
            public String Col1;

            [CSVHeader(Index=1)]
            public String Col2 { get; set; }
        }

        [CSVFile(HasHeader = false)]
        private class TestClass2
        {
            public String Col1;
            public String Col2 { get; set; }
        }

        [CSVFile(SkipRowCount = 2)]
        private class TestClass3
        {
            public String Col1;
            public String Col2 { get; set; }
        }
    }
}
