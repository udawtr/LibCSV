using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLibCSV
{
    [TestClass()]
    public class CSVHeaderAttributeTest
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
        ///Test for CSVHeaderAttribute
        ///</summary>
        [TestMethod()]
        public void CSVHeaderAttributeNameTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "Col1,Col2\r\nA,B\r\nD,E";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<TestClass>(stream, System.Text.Encoding.UTF8);

            var firstLine = target.ReadNext();
            Assert.AreEqual("A", firstLine.Field1);
            Assert.AreEqual("B", firstLine.Field2);
        }

        /// <summary>
        ///Test for CSVHeaderAttribute
        ///</summary>
        [TestMethod()]
        public void CSVHeaderAttributeIndexTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "Foo,Bar,Buzz\r\nA,B,C\r\nD,E,F";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<IndexTestClass>(stream, System.Text.Encoding.UTF8);

            var firstLine = target.ReadNext();
            Assert.AreEqual("A", firstLine.Col1);
            Assert.AreEqual("C", firstLine.Col3);
        }

        private class TestClass
        {
            [CSVHeader("Col1")]
            public String Field1;
            [CSVHeader("Col2")]
            public String Field2 { get; set; }
        }

        private class IndexTestClass
        {
            [CSVHeader(Index = 0)]
            public string Col1;

            [CSVHeader(Index = 2)]
            public string Col3 { get; set; }
        }
    }
}
