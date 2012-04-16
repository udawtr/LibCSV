using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace TestLibCSV
{
    
    
    /// <summary>
    ///CSVTextWriterTest のテスト クラスです。すべての
    ///CSVTextWriterTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class CSVTextWriterTest
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
        ///Write のテスト
        ///</summary>
        [TestMethod()]
        public void WriteSingleStringTest()
        {
            var ms = new System.IO.MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                CSVTextWriter target = new CSVTextWriter(sw);
                target.Write("Col1");
                target.Write("Col2");
                target.Write("Col3");
                target.Close();
            }
        }

        /// <summary>
        ///Write のテスト
        ///</summary>
        [TestMethod()]
        public void WriteStringArrayTest()
        {
            var ms = new System.IO.MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                CSVTextWriter target = new CSVTextWriter(sw);
                target.Write(new []{"Col", "Col2", "Col3"});
                target.Close();
            }
        }

        /// <summary>
        ///Write のテスト
        ///</summary>
        [TestMethod()]
        public void WriteObjectEnumerableTest()
        {
            var ms = new System.IO.MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                CSVTextWriter target = new CSVTextWriter(sw);
                target.Write(new object[] { 1, 2.0, 3.0D, "TEST" });
                target.Close();
            }
        }

        /// <summary>
        ///WriteLine のテスト
        ///</summary>
        [TestMethod()]
        public void WriteStringArrayLineTest()
        {
            var ms = new System.IO.MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                CSVTextWriter target = new CSVTextWriter(sw);
                target.WriteLine(new[] { "Col", "Col2", "Col3" });
                target.Close();
            }
        }

        /// <summary>
        ///WriteLine のテスト
        ///</summary>
        [TestMethod()]
        public void WriteObjectEnumerableLineTest()
        {
            var ms = new System.IO.MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                CSVTextWriter target = new CSVTextWriter(sw);
                target.WriteLine(new object[] { 1, 2.0, 3.0D, "TEST" });
                target.Close();
            }
        }

        /// <summary>
        ///Dispose のテスト
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            var ms = new System.IO.MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                using (CSVTextWriter target = new CSVTextWriter(sw))
                {
                    //Auto Dispose
                }
            }
        }

        /// <summary>
        ///CSVTextWriter コンストラクター のテスト
        ///</summary>
        [TestMethod()]
        public void CSVTextWriterConstructorTest()
        {
            string fileName = System.IO.Path.GetTempFileName();
            using (CSVTextWriter target = new CSVTextWriter(fileName))
            {
                target.WriteLine(new[] { "Col1", "Col2" });
                target.Close();
            }
            System.IO.File.Delete(fileName);
        }
    }
}
