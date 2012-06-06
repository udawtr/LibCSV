using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Youworks.Text;

namespace TestLibCSV
{
    /// <summary>
    /// CSVSource.TypeResolver のテスト
    /// </summary>
    [TestClass]
    public class TypeResolverTest
    {
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

        [CSVFile(HasHeader = false, SkipRowCount = 0)]
        private class TestCSVRow
        {
            [CSVHeader(Index = 0)]
            public int IntColumn { get; set; }

            [CSVHeader(Index = 1)]
            public double DoubleColumn { get; set; }
        }

        private class TypeResolverWithDefaultValue : CSVGeneralTypeResolver
        {
            public override int ResolveInt(CSVTypeResolverArgs args)
            {
                try
                {
                    return base.ResolveInt(args);
                }
                catch(InvalidValueException)
                {
                    return 100;
                }
            }

            public override double ResolveDouble(CSVTypeResolverArgs args)
            {
                try
                {
                    return base.ResolveDouble(args);
                }
                catch(InvalidValueException)
                {
                    return 200.0D;
                }
            }
        }

        [TestMethod]
        public void CustomTypeResolverTest()
        {
            const string text = "-,aa";
            var target = CreateCSVSourceFromText<TestCSVRow>(text);
            target.TypeResolver = new TypeResolverWithDefaultValue();
            var line = target.ReadNext();

            // 他のテストに影響を与えないために、 TypeResolver を元に戻す
            target.TypeResolver = new CSVGeneralTypeResolver();

            Assert.AreEqual(100, line.IntColumn);
            Assert.AreEqual(200.0D, line.DoubleColumn);
        }
    }
}
