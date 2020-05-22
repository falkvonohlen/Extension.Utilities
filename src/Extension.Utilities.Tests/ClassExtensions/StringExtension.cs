using Extension.Utilities.ClassExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Extension.Utilities.Tests.ClassExtensions
{
    [TestFixture]
    public class StringExtension
    {
        [TestCase("/../../Test.txt", "..\\..\\Test.txt")]
        [TestCase("\\..\\..\\Test.txt", "..\\..\\Test.txt")]
        [TestCase("..\\..\\Test.txt", "..\\..\\Test.txt")]
        [TestCase("../../Test.txt", "..\\..\\Test.txt")]
        public void TestMakeDotNetRelativePath(string input, string expectedOutput)
        {
            Assert.AreEqual(input.MakeDotNetRelativePath(), expectedOutput);
        }

        [TestCase("HalloWelt", "")]
        [TestCase("HalloWelt1", "1")]
        [TestCase("HalloWelt5463", "5463")]
        public void TestGetNumericTail(string value, string tail)
        {
            Assert.AreEqual(value.GetNumericTail(), tail);
        }

        [TestCase("HalloWelt", "")]
        [TestCase("HalloWelt1", "")]
        [TestCase("HalloWelt5463", "")]
        [TestCase("HalloWelt", "")]
        [TestCase("HalloWelt_1", "1")]
        [TestCase("HalloWelt_5463", "5463")]
        public void TestGetNumericTailSep(string value, string tail)
        {
            Assert.AreEqual(value.GetNumericTail("_"), tail);
        }

        [TestCase("HalloWelt", "HalloWelt1")]
        [TestCase("HalloWelt9", "HalloWelt10")]
        [TestCase("HalloWelt5463", "HalloWelt5464")]
        public void TestIterateNumericTail(string value, string newValue)
        {
            Assert.AreEqual(value.IterateNumericTail(), newValue);
        }

        [TestCase("HalloWelt", "HalloWelt_1")]
        [TestCase("HalloWelt9", "HalloWelt9_1")]
        [TestCase("HalloWelt_5463", "HalloWelt_5464")]
        public void TestIterateNumericTailSep(string value, string newValue)
        {
            Assert.AreEqual(value.IterateNumericTail("_"), newValue);
        }

        [TestCase("HalloWelt", "ow", true)]
        [TestCase("HalloWelt", "hallo", true)]
        [TestCase("Hallo, Welt", "hallo", true)]
        [TestCase("Hallo, Welt", "test", false)]
        [TestCase("HalloWelt", "lloWe", true)]
        public void TestContainsIgnoreCase(string instance, string value, bool outCome)
        {
            Assert.IsTrue(instance.ContainsIgnoreCase(value) == outCome);
        }

        [Test, TestCaseSource(typeof(TestDataFactory), "MakeUniqueTestCases")]
        public string TestMakeUnique(string[] knownValues, string value)
        {
            return value.MakeUnique(knownValues);
        }

        [Test, TestCaseSource(typeof(TestDataFactory), "MakeUniqueSepTestCases")]
        public string TestMakeUniqueSep(string[] knownValues, string value)
        {
            return value.MakeUnique(knownValues, "_");
        }

        [Test, TestCaseSource(typeof(TestDataFactory), "MakeCaseUniqueTestCases")]
        public string TestMakeCaseUnique(string[] knownValues, string value)
        {
            return value.MakeCaseUnique(knownValues);
        }

        [Test, TestCaseSource(typeof(TestDataFactory), "MakeCaseUniqueSepTestCases")]
        public string TestMakeCaseUniqueSep(string[] knownValues, string value)
        {
            return value.MakeCaseUnique(knownValues, "_");
        }

        [TestCase("Z", "AA")]
        [TestCase("ZZZ", "AAAA")]
        [TestCase("A", "B")]
        [TestCase("H", "I")]
        [TestCase("ABIZ", "ABJA")]
        [TestCase("ABiz", "ABJA")]
        public void TestIterateUpper(string input, string expetedOutput)
        {
            Assert.AreEqual(expetedOutput, input.IterateUpper());
        }

        [TestCase("Z", "aa")]
        [TestCase("ZZZ", "aaaa")]
        [TestCase("A", "b")]
        [TestCase("H", "i")]
        [TestCase("ABIZ", "abja")]
        [TestCase("ABiz", "abja")]
        public void TestIterateLower(string input, string expetedOutput)
        {
            Assert.AreEqual(expetedOutput, input.IterateLower());
        }

        [TestCase("AA", "Z")]
        [TestCase("AAAA", "ZZZ")]
        [TestCase("B", "A")]
        [TestCase("I", "H")]
        [TestCase("ABJA", "ABIZ")]
        public void TestReverseIterateUpper(string input, string expetedOutput)
        {
            Assert.AreEqual(expetedOutput, input.ReverseIterateUpper());
        }

        [TestCase("AA", "z")]
        [TestCase("AAAA", "zzz")]
        [TestCase("B", "a")]
        [TestCase("I", "h")]
        [TestCase("ABJA", "abiz")]
        public void TestReverseIterateLower(string input, string expetedOutput)
        {
            Assert.AreEqual(expetedOutput, input.ReverseIterateLower());
        }
    }

    public class TestDataFactory
    {
        public static IEnumerable<TestCaseData> MakeUniqueTestCases
        {
            get
            {
                yield return new TestCaseData(new string[] { "HalloWelt", "HalloWelt1", "HalloWelt2" }, "HalloWelt").Returns("HalloWelt3");
                yield return new TestCaseData(new string[] { "HalloWelt", "Hallowelt1", "HalloWelt2" }, "HalloWelt").Returns("HalloWelt1");
                yield return new TestCaseData(new string[] { "Hallowelt1", "HalloWelt2" }, "HalloWelt").Returns("HalloWelt");
                yield return new TestCaseData(new string[] { "Test", "test1", "Test1" }, "HalloWelt").Returns("HalloWelt");
                yield return new TestCaseData(new string[] { "halloWelt", "Hallowelt1", "hallowelt2" }, "HalloWelt").Returns("HalloWelt");
            }
        }

        public static IEnumerable<TestCaseData> MakeUniqueSepTestCases
        {
            get
            {
                yield return new TestCaseData(new string[] { "HalloWelt", "HalloWelt_1", "HalloWelt_2" }, "HalloWelt").Returns("HalloWelt_3");
                yield return new TestCaseData(new string[] { "HalloWelt", "HalloWelt1", "HalloWelt_2" }, "HalloWelt1").Returns("HalloWelt1_1");
                yield return new TestCaseData(new string[] { "HalloWelt", "Hallowelt_1", "HalloWelt_2" }, "HalloWelt").Returns("HalloWelt_1");
                yield return new TestCaseData(new string[] { "Hallowelt_1", "HalloWelt_2" }, "HalloWelt").Returns("HalloWelt");
                yield return new TestCaseData(new string[] { "Test", "test_1", "Test_1" }, "HalloWelt").Returns("HalloWelt");
                yield return new TestCaseData(new string[] { "halloWelt", "Hallowelt_1", "hallowelt_2" }, "HalloWelt").Returns("HalloWelt");
            }
        }

        public static IEnumerable<TestCaseData> MakeCaseUniqueTestCases
        {
            get
            {
                yield return new TestCaseData(new string[] { "HalloWelt", "Hallowelt1", "halloWelt2" }, "HalloWelt").Returns("HalloWelt3");
                yield return new TestCaseData(new string[] { "HalloWelt", "HalloWelt2" }, "HalloWelt").Returns("HalloWelt1");
                yield return new TestCaseData(new string[] { "Test", "test1", "Test1" }, "HalloWelt").Returns("HalloWelt");
                yield return new TestCaseData(new string[] { "hallowelt1", "hallowelt2" }, "HalloWelt").Returns("HalloWelt");
            }
        }

        public static IEnumerable<TestCaseData> MakeCaseUniqueSepTestCases
        {
            get
            {
                yield return new TestCaseData(new string[] { "HalloWelt", "Hallowelt_1", "halloWelt_2" }, "HalloWelt").Returns("HalloWelt_3");
                yield return new TestCaseData(new string[] { "HalloWelt", "Hallowelt1", "halloWelt_2" }, "HalloWelt1").Returns("HalloWelt1_1");
                yield return new TestCaseData(new string[] { "HalloWelt", "HalloWelt_2" }, "HalloWelt").Returns("HalloWelt_1");
                yield return new TestCaseData(new string[] { "Test", "test1", "Test1" }, "HalloWelt").Returns("HalloWelt");
                yield return new TestCaseData(new string[] { "hallowelt_1", "hallowelt_2" }, "HalloWelt").Returns("HalloWelt");
            }
        }
    }
}
