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
    [TestClass()]
    public class CSVSourceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
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

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void CSVSourceConstructorTest()
        {
            Stream inputStream = new System.IO.MemoryStream();
            var target = new CSVSource<Sample>(inputStream);
            Assert.IsNotNull(target);
        }

        [TestMethod()]
        [DeploymentItem("TestCSV_932_CRLF.csv")]
        public void CSVSourceConstructorTestWith932EncodedText()
        {
            string filename = "TestCSV_932_CRLF.csv";
            var target = new CSVSource<Sample>(filename);
            Assert.IsNotNull(target.ReadNext());
            Assert.IsNotNull(target.ReadNext());
            Assert.IsNull(target.ReadNext());
        }

        [TestMethod()]
        [DeploymentItem("TestCSV_UTF8_CRLF.csv")]
        public void CSVSourceConstructorTestWithUTF8EncodedText()
        {
            string filename = "TestCSV_UTF8_CRLF.csv";
            var target = new CSVSource<Sample>(filename, System.Text.Encoding.UTF8);
            Assert.IsNotNull(target.ReadNext());
            Assert.IsNotNull(target.ReadNext());
            Assert.IsNull(target.ReadNext());
        }

        [TestMethod()]
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

        [TestMethod()]
        public void HeaderStartedWithSpacesTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "Col1,  Col2,Col3\r\nA,B,C";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<Sample>(stream, System.Text.Encoding.UTF8);
            var obj = target.ReadNext();
            Assert.AreEqual("A", obj.Col1);
            Assert.AreEqual("B", obj.Col2);
            Assert.AreEqual("C", obj.Col3);
        }

        [TestMethod()]
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
                target.ReadNextObject();
                target.ReadNextObject();
                Assert.IsFalse(target.HasMore);
                //Dispose
            }
        }

        private class Sample
        {
            public String Col1;
            public String Col2;
            public String Col3 { get; set; }
            [CSVHeader(Name="Col4")]
            public String Extra { get; set; }
        }
    }
}
