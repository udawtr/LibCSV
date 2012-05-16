using System;
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
        [CSVFile(HasHeader = false, SkipRowCount = 0)]
        private class TestCSVRow
        {
            [CSVHeader(Index = 0)]
            public int firstInt { get; set; }

            [CSVHeader(Index = 1)]
            public double secondDouble { get; set; }
        }

        private class TypeResolverWithDefaultValue : DefaultTypeResolver
        {
            public override int ResolveInt(object value)
            {
                try
                {
                    return base.ResolveInt(value);
                }
                catch(FormatException)
                {
                    return 100;
                }
            }

            public override double ResolveDouble(object value)
            {
                try
                {
                    return base.ResolveDouble(value);
                }
                catch(FormatException)
                {
                    return 200.0D;
                }
            }
        }

        [TestMethod]
        public void InvalidNumbersTest()
        {
            const string text = "-,";
            var stream = new MemoryStream();
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var target = new CSVSource<TestCSVRow>(stream, System.Text.Encoding.UTF8);
            target.TypeResolver = new TypeResolverWithDefaultValue();
            var line = target.ReadNext();

            Assert.AreEqual(100, line.firstInt);
            Assert.AreEqual(200.0D, line.secondDouble);
        }
    }
}
