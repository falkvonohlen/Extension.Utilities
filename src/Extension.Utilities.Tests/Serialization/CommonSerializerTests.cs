using Extension.Utilities.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Extension.Utilities.Tests.Serialization
{
    public class CommonSerializerTests
    {
        string tmpDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "tmp");

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(tmpDir);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(tmpDir, true);
        }

        [Test]
        public void TestSaveJson()
        {
            var fileName = Path.Combine(tmpDir, "example.json");
            var test = new SerializableObject();
            test.ValueA = "Hallo";
            test.ValueB = 1;

            CommonSerializer.SaveAsJsonFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void TestOverrideJson()
        {
            var fileName = Path.Combine(tmpDir, "example.json");
            var test = new SerializableObject();
            test.ValueA = "Hallo";
            test.ValueB = 1;

            CommonSerializer.SaveAsJsonFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));

            CommonSerializer.SaveAsJsonFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void TestOverrideAndLoadJson()
        {
            var fileName = Path.Combine(tmpDir, "example.json");
            var test = new SerializableObject();
            test.ValueA = "Hallo";
            test.ValueB = 1;

            CommonSerializer.SaveAsJsonFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));

            test.ValueA = "Changed";

            CommonSerializer.SaveAsJsonFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));

            var loaded = CommonSerializer.LoadJsonFile<SerializableObject>(fileName);
            Assert.AreEqual("Changed", loaded.ValueA);
        }

        [Test]
        public void TestSaveXML()
        {
            var fileName = Path.Combine(tmpDir, "example.xml");
            var test = new SerializableObject();
            test.ValueA = "Hallo";
            test.ValueB = 1;

            CommonSerializer.SaveAsXMLFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void TestOverrideXML()
        {
            var fileName = Path.Combine(tmpDir, "example.xml");
            var test = new SerializableObject();
            test.ValueA = "Hallo";
            test.ValueB = 1;

            CommonSerializer.SaveAsXMLFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));

            CommonSerializer.SaveAsXMLFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void TestOverrideAndLoadXML()
        {
            var fileName = Path.Combine(tmpDir, "example.xml");
            var test = new SerializableObject();
            test.ValueA = "Hallo";
            test.ValueB = 1;

            CommonSerializer.SaveAsXMLFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));

            test.ValueA = "Changed";

            CommonSerializer.SaveAsXMLFile(fileName, test);
            Assert.IsTrue(File.Exists(fileName));

            var loaded = CommonSerializer.LoadXMLFile<SerializableObject>(fileName);
            Assert.AreEqual("Changed", loaded.ValueA);
        }
    }
}
