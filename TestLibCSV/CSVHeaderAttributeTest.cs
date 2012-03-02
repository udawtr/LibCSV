using Youworks.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLibCSV
{
    
    
    /// <summary>
    ///CSVHeaderAttributeTest のテスト クラスです。すべての
    ///CSVHeaderAttributeTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class CSVHeaderAttributeTest
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
        ///CSVHeaderAttribute コンストラクター のテスト
        ///</summary>
        [TestMethod()]
        public void CSVHeaderAttributeConstructorTest()
        {
            string name = string.Empty; // TODO: 適切な値に初期化してください
            CSVHeaderAttribute target = new CSVHeaderAttribute(name);
            Assert.Inconclusive("TODO: ターゲットを確認するためのコードを実装してください");
        }

        /// <summary>
        ///Name のテスト
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            string name = string.Empty; // TODO: 適切な値に初期化してください
            CSVHeaderAttribute target = new CSVHeaderAttribute(name); // TODO: 適切な値に初期化してください
            string expected = string.Empty; // TODO: 適切な値に初期化してください
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }
    }
}
