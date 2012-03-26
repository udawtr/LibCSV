﻿using Youworks.Text;
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
    }

    public class Sample
    {
        public String Col1;
        public String Col2;
        public String Col3;
    }
}
