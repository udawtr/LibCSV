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

        [CSVFile(HasHeader = false, SkipRowCount = 0)]
        private class DifferentDefaultCSVRow
        {
            [CSVHeader(Index = 0), CSVColumnConverter(typeof (Default100))]
            public int FirstInt { get; set; }

            [CSVHeader(Index = 1), CSVColumnConverter(typeof (Default200))]
            public int SecondInt;

            [CSVHeader(Index = 2), CSVColumnConverter(typeof (Default100))]
            public int ThirdInt { get; set; }

            [CSVHeader(Index = 3), CSVColumnConverter(typeof (Default100))]
            public int FourthInt;

            private class Default100 : IColumnConverter
            {
                public object Convert(object value)
                {
                    try
                    {
                        return System.Convert.ToInt32(value);
                    }
                    catch(FormatException)
                    {
                        return 100;
                    }
                }
            }

            private class Default200 : IColumnConverter
            {
                public object Convert(object value)
                {
                    try
                    {
                        return System.Convert.ToInt32(value);
                    }
                    catch(FormatException)
                    {
                        return 200;
                    }
                }
            }
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
        public void CustomTypeResolverTest()
        {
            const string text = "-,aa";
            var target = CreateCSVSourceFromText<TestCSVRow>(text);
            target.TypeResolver = new TypeResolverWithDefaultValue();
            var line = target.ReadNext();

            Assert.AreEqual(100, line.IntColumn);
            Assert.AreEqual(200.0D, line.DoubleColumn);
        }

        [TestMethod]
        public void DifferentDefaultValueTest()
        {
            const string text = "-,,300,400";
            var target = CreateCSVSourceFromText<DifferentDefaultCSVRow>(text);
            var line = target.ReadNext();

            Assert.AreEqual(100, line.FirstInt);
            Assert.AreEqual(200, line.SecondInt);
            Assert.AreEqual(300, line.ThirdInt);
            Assert.AreEqual(400, line.FourthInt);
        }
    }
}
