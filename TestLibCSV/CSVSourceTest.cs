using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestLibCSV
{
    
    
    /// <summary>
    ///CSVSourceTest のテスト クラスです。すべての
    ///CSVSourceTest 単体テストをここに含めます
    ///</summary>
    [TestClass]
    public class CSVSourceTest
    {
        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext { get; set; }

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

        [TestMethod]
        public void CSVSourceConstructorTest()
        {
            Stream inputStream = new MemoryStream();
            var target = new CSVSource<Sample>(inputStream);
            Assert.IsNotNull(target);
        }

        [TestMethod]
        [DeploymentItem("TestCSV_932_CRLF.csv")]
        public void CSVSourceConstructorTestWith932EncodedText()
        {
            string filename = "TestCSV_932_CRLF.csv";
            using (var target = new CSVSource<Sample>(filename))
            {
                Assert.IsNotNull(target.ReadNext());
                Assert.IsNotNull(target.ReadNext());
                Assert.IsNull(target.ReadNext());
                target.Close();
            }
        }

        [TestMethod]
        [DeploymentItem("TestCSV_UTF8_CRLF.csv")]
        public void CSVSourceConstructorTestWithUTF8EncodedText()
        {
            string filename = "TestCSV_UTF8_CRLF.csv";
            using (var target = new CSVSource<Sample>(filename, System.Text.Encoding.UTF8))
            {
                Assert.IsNotNull(target.ReadNext());
                Assert.IsNotNull(target.ReadNext());
                Assert.IsNull(target.ReadNext());
                target.Close();
            }
        }

        [TestMethod]
        [DeploymentItem("TestCSV_Escape.csv")]
        public void GetCSVLineTestOfSimpleEscape()
        {
            string filename = "TestCSV_Escape.csv";
            var target = new CSVSource<Sample>(filename);
            var line = target.ReadNext();
            Assert.AreEqual("日本,語", line.Col3);
            line = target.ReadNext();
            Assert.AreEqual("ちり\"ぬるを", line.Col3);
            line = target.ReadNext();
            Assert.AreEqual("吾輩は\n\"\"猫\"\"である。", line.Col3);
            line = target.ReadNext();
            Assert.AreEqual("ちがうことも\\あります。", line.Col3);
        }

        [TestMethod]
        public void HeaderStartedWithSpacesTest()
        {
            const string text = "Col1,  Col2,Col3\r\nA,B,C";
            var target = CreateCSVSourceFromText<Sample>(text);
            var obj = target.ReadNext();
            Assert.AreEqual("A", obj.Col1);
            Assert.AreEqual("B", obj.Col2);
            Assert.AreEqual("C", obj.Col3);
        }

        [TestMethod]
        [DeploymentItem("TestCSV_Escape.csv")]
        public void BasicProperyTest()
        {
            string filename = "TestCSV_Escape.csv";
            using (var target = new CSVSource<Sample>(filename))
            {
                //HasHeader
                Assert.IsTrue(target.HasHeader);

                //Filename get/set
                Assert.AreEqual(filename, target.Filename);
                target.Filename = "update.csv";
                Assert.AreEqual("update.csv", target.Filename);

                //Header
                Assert.AreEqual("Col1", target.Header[0]);
                Assert.AreEqual("Col2", target.Header[1]);
                Assert.AreEqual("Col3", target.Header[2]);
                Assert.AreEqual("Col4", target.Header[3]);

                Assert.IsTrue(target.HasMore);
                dynamic line = target.ReadNextObject();
                Assert.AreEqual("日本,語", line.Col3);
                target.ReadNextObject();
                target.ReadNextObject();
                Assert.AreEqual(4, target.LineNo);
                target.ReadNextObject();
                target.ReadNextObject();
                Assert.IsFalse(target.HasMore);
                //Dispose
            }
        }

        /// <summary>
        /// For issue#13
        /// </summary>
        [TestMethod]
        public void LastEmptyValuesAreNotParseTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nA,B,C,\r\nD,E,F,";
            var target = CreateCSVSourceFromText<Sample>(text);

            var firstLine = target.ReadNext();
            Assert.AreEqual("", firstLine.Col4);

            var lastLine = target.ReadNext();
            Assert.AreEqual("", lastLine.Col4);
        }

        [TestMethod]
        public void AutoTypeRecognitionTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nLiteral,3.4,100,2010/1/3";
            var target = CreateCSVSourceFromText<AutoTypeSample>(text);

            var firstLine = target.ReadNext();
            Assert.AreEqual("Literal", firstLine.Col1);
            Assert.AreEqual(3.4D, firstLine.Col2);
            Assert.AreEqual(100, firstLine.Col3);
            Assert.AreEqual(new DateTime(2010, 1, 3), firstLine.Col4);
        }


        [TestMethod]
        public void AutoTypeRecognitionCastExceptionTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nLiteral,3.4,100.5,2010/1/3";
            var target = CreateCSVSourceFromText<AutoTypeSample>(text);

            try
            {
                target.ReadNext();
                Assert.Fail();
            }
            catch (CSVValueInvalidException ex)
            {
                Assert.AreEqual("カラム'Col3'(\"100.5\")をInt32型に変換できません。[2行3列]", ex.Message);
                Assert.AreEqual(2, ex.RowNumber);
                Assert.AreEqual(3, ex.ColumnNumber);
                Assert.IsInstanceOfType(ex.InnerException, typeof(CSVValueInvalidException));
            }
        }

        [TestMethod]
        public void AutoTypeRecognitionUnknownTypeExceptionTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nLiteral,3.4,100.5,2010/1/3";
            var target = CreateCSVSourceFromText<AutoTypeSample2>(text);

            try
            {
                target.ReadNext();
                Assert.Fail();
            }
            catch (CSVValueInvalidException ex)
            {
                Assert.AreEqual("カラム'Col1'(\"Literal\")をObject型に変換できません。[2行1列]", ex.Message);
                Assert.AreEqual(2, ex.RowNumber);
                Assert.AreEqual(1, ex.ColumnNumber);
                Assert.IsInstanceOfType(ex.InnerException, typeof (CSVValueInvalidException));
            }
        }

        [TestMethod]
        public void AutoTypeRecognitionNullableTest()
        {
            const string text = "Col1,Col2,Col3,Col4\r\nLiteral,3.4,100,2010/1/3\n,,,";
            var target = CreateCSVSourceFromText<AutoTypeSample3>(text);

            var firstLine = target.ReadNext();
            Assert.AreEqual("Literal", firstLine.Col1);
            Assert.AreEqual(3.4D, firstLine.Col2);
            Assert.AreEqual(100, firstLine.Col3);
            Assert.AreEqual(new DateTime(2010, 1, 3), firstLine.Col4);

            var secondLine = target.ReadNext();
            Assert.AreEqual("", secondLine.Col1);
            Assert.IsNull(secondLine.Col2);
            Assert.IsNull(secondLine.Col3);
            Assert.IsNull(secondLine.Col4);
        }

        [TestMethod]
        public void AutoTypeRecognitionEnumTest()
        {
            const string text = "Col1,Col2,Col3\r\nSunday,Monday,Tuesday";
            var target = CreateCSVSourceFromText<AutoTypeSample4>(text);

            var firstLine = target.ReadNext();
            Assert.AreEqual(AutoTypeEnumSample.Sunday, firstLine.Col1);
            Assert.AreEqual(AutoTypeEnumSample.Monday, firstLine.Col2);
            Assert.AreEqual(AutoTypeEnumSample.Tuesday, firstLine.Col3);
        }

        [TestMethod]
        [DeploymentItem("TestCSV_932_CRLF.csv")]
        public void ToListTest()
        {
            const string filename = "TestCSV_932_CRLF.csv";
            var target = CSVSource<Sample>.ToList(filename, System.Text.Encoding.GetEncoding(932));
            Assert.IsNotNull(target);
            Assert.AreEqual(2, target.Count);
        }

        private class Sample
        {
            public String Col1;
            public String Col2;
            public String Col3 { get; set; }
            public String Col4 { get; set; }
        }

        private class AutoTypeSample
        {
            public String Col1;
            public Double Col2;
            public Int32 Col3 { get; set; }
            public DateTime Col4 { get; set; }
        }

        private class AutoTypeSample2
        {
            public object Col1;
            public Double Col2;
            public Int32 Col3 { get; set; }
            public object Col4 { get; set; }
        }

        private class AutoTypeSample3
        {
            public string Col1;
            public Double? Col2;
            public Int32? Col3;
            public DateTime? Col4;
        }

        private enum AutoTypeEnumSample
        {
            Sunday, Monday, Tuesday
        }

        private class AutoTypeSample4
        {
            public AutoTypeEnumSample Col1;
            public AutoTypeEnumSample Col2;
            public AutoTypeEnumSample Col3;
        }
    }
}
