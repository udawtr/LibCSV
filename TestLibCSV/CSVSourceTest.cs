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


        /// <summary>
        ///CSVSource`1 コンストラクター のテスト
        ///</summary>
        public void CSVSourceConstructorTestHelper<T>()
            where T : new()
        {
            Stream inputStream = null; // TODO: 適切な値に初期化してください
            CSVSource<T> target = new CSVSource<T>(inputStream);
            Assert.Inconclusive("TODO: ターゲットを確認するためのコードを実装してください");
        }

        [TestMethod()]
        public void CSVSourceConstructorTest()
        {
            CSVSourceConstructorTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///CSVSource`1 コンストラクター のテスト
        ///</summary>
        public void CSVSourceConstructorTest1Helper<T>()
            where T : new()
        {
            string filename = string.Empty; // TODO: 適切な値に初期化してください
            CSVSource<T> target = new CSVSource<T>(filename);
            Assert.Inconclusive("TODO: ターゲットを確認するためのコードを実装してください");
        }

        [TestMethod()]
        public void CSVSourceConstructorTest1()
        {
            CSVSourceConstructorTest1Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///GetCSVLine のテスト
        ///</summary>
        public void GetCSVLineTestHelper<T>()
            where T : new()
        {
            PrivateObject param0 = null; // TODO: 適切な値に初期化してください
            CSVSource_Accessor<T> target = new CSVSource_Accessor<T>(param0); // TODO: 適切な値に初期化してください
            string[] expected = null; // TODO: 適切な値に初期化してください
            string[] actual;
            actual = target.GetCSVLine();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void GetCSVLineTest()
        {
            GetCSVLineTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///PrepareFieldInfo のテスト
        ///</summary>
        public void PrepareFieldInfoTestHelper<T>()
            where T : new()
        {
            PrivateObject param0 = null; // TODO: 適切な値に初期化してください
            CSVSource_Accessor<T> target = new CSVSource_Accessor<T>(param0); // TODO: 適切な値に初期化してください
            target.PrepareFieldInfo();
            Assert.Inconclusive("値を返さないメソッドは確認できません。");
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void PrepareFieldInfoTest()
        {
            PrepareFieldInfoTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///ReadNext のテスト
        ///</summary>
        public void ReadNextTestHelper<T>()
            where T : new()
        {
            Stream inputStream = null; // TODO: 適切な値に初期化してください
            CSVSource<T> target = new CSVSource<T>(inputStream); // TODO: 適切な値に初期化してください
            T expected = new T(); // TODO: 適切な値に初期化してください
            T actual;
            actual = target.ReadNext();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        public void ReadNextTest()
        {
            ReadNextTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///ReadNextObject のテスト
        ///</summary>
        public void ReadNextObjectTestHelper<T>()
            where T : new()
        {
            Stream inputStream = null; // TODO: 適切な値に初期化してください
            CSVSource<T> target = new CSVSource<T>(inputStream); // TODO: 適切な値に初期化してください
            object expected = null; // TODO: 適切な値に初期化してください
            object actual;
            actual = target.ReadNextObject();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        public void ReadNextObjectTest()
        {
            ReadNextObjectTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///System.IDisposable.Dispose のテスト
        ///</summary>
        public void DisposeTestHelper<T>()
            where T : new()
        {
            Stream inputStream = null; // TODO: 適切な値に初期化してください
            IDisposable target = new CSVSource<T>(inputStream); // TODO: 適切な値に初期化してください
            target.Dispose();
            Assert.Inconclusive("値を返さないメソッドは確認できません。");
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void DisposeTest()
        {
            DisposeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///Filename のテスト
        ///</summary>
        public void FilenameTestHelper<T>()
            where T : new()
        {
            Stream inputStream = null; // TODO: 適切な値に初期化してください
            CSVSource<T> target = new CSVSource<T>(inputStream); // TODO: 適切な値に初期化してください
            string expected = string.Empty; // TODO: 適切な値に初期化してください
            string actual;
            target.Filename = expected;
            actual = target.Filename;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        public void FilenameTest()
        {
            FilenameTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///Header のテスト
        ///</summary>
        public void HeaderTestHelper<T>()
            where T : new()
        {
            PrivateObject param0 = null; // TODO: 適切な値に初期化してください
            CSVSource_Accessor<T> target = new CSVSource_Accessor<T>(param0); // TODO: 適切な値に初期化してください
            string[] expected = null; // TODO: 適切な値に初期化してください
            string[] actual;
            target.Header = expected;
            actual = target.Header;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        [DeploymentItem("LibCSV.dll")]
        public void HeaderTest()
        {
            HeaderTestHelper<GenericParameterHelper>();
        }
    }
}
