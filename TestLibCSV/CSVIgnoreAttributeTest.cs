using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLibCSV
{
    
    
    /// <summary>
    ///CSVIgnoreAttributeTest のテスト クラスです。すべての
    ///CSVIgnoreAttributeTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class CSVIgnoreAttributeTest
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


        /// <summary>
        ///CSVIgnoreAttribute Test
        ///</summary>
        [TestMethod()]
        public void CSVIgnoreAttributeBasicTest()
        {
            var stream = new System.IO.MemoryStream();
            var text = "Col1,Col2,Col3\r\nA,B,C\r\nD,E,F";
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<TestClass>(stream, System.Text.Encoding.UTF8);

            var firstLine = target.ReadNext();
            Assert.AreNotEqual("A", firstLine.Col1);
            Assert.AreNotEqual("B", firstLine.Col2);
            Assert.AreEqual("C", firstLine.Col3);
        }

        private class TestClass
        {
            [CSVIgnore]
            public String Col1;

            [CSVIgnore]
            public String Col2 { get; set; }

            public String Col3;
        }
    }
}
