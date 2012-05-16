using System.IO;
using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLibCSV
{
    [TestClass]
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
        /// Helper for instantiation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        private CSVSource<T> CreateCSVSourceFromText<T>(string text) where T : new()
        {
            var stream = new MemoryStream();
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<T>(stream, System.Text.Encoding.UTF8);
            return target;
        }


        /// <summary>
        ///Test for CSVHeaderAttribute
        ///</summary>
        [TestMethod]
        public void CSVHeaderAttributeNameTest()
        {
            const string text = "Col1,Col2\r\nA,B\r\nD,E";
            var target = CreateCSVSourceFromText<TestClass>(text);

            var firstLine = target.ReadNext();
            Assert.AreEqual("A", firstLine.Field1);
            Assert.AreEqual("B", firstLine.Field2);
        }

        /// <summary>
        ///Test for CSVHeaderAttribute
        ///</summary>
        [TestMethod]
        public void CSVHeaderAttributeIndexTest()
        {
            const string text = "Foo,Bar,Buzz\r\nA,B,C\r\nD,E,F";
            var target = CreateCSVSourceFromText<IndexTestClass>(text);

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

        [TestMethod]
        public void DefaultValueTest()
        {
            const string text = ",,,";
            var target = CreateCSVSourceFromText<DefaultValueTestRow>(text);

            var line = target.ReadNext();
            Assert.AreEqual(100, line.Int1);
            Assert.AreEqual(100, line.Int2);
            Assert.AreEqual(200, line.Int3);
            Assert.AreEqual(200, line.Int4);
        }

        [TestMethod]
        public void SetDefaultIfInvalidValueGivenTest()
        {
            const string text = "1,2,aaa,bbb";
            var target = CreateCSVSourceFromText<DefaultValueTestRow>(text);

            var line = target.ReadNext();
            Assert.AreEqual(1, line.Int1);
            Assert.AreEqual(2, line.Int2);
            Assert.AreEqual(200, line.Int3);
            Assert.AreEqual(200, line.Int4);
        }

        [CSVFile(HasHeader = false, SkipRowCount = 0)]
        private class DefaultValueTestRow
        {
            [CSVHeader(Index = 0, DefaultValue = 100)]
            public int Int1;

            [CSVHeader(Index = 1, DefaultValue = 100)]
            public int Int2 { get; set; }

            [CSVHeader(Index = 2, DefaultValue = 200, InvalidAction = CSVValueInvalidAction.DefaultValue)]
            public int Int3;

            [CSVHeader(Index = 3, DefaultValue = 200, InvalidAction = CSVValueInvalidAction.DefaultValue)]
            public int Int4 { get; set; }
        }
    }
}
