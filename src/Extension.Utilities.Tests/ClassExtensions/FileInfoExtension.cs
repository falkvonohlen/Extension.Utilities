using Extension.Utilities.ClassExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Extension.Utilities.Tests.ClassExtensions
{
    [TestFixture]
    public class FileInfoExtension
    {
        [TestCase("D:\\TestFolder\\Level1\\Level2\\Test.txt", "../../HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2\\Test.txt", "D:\\TestFolder\\HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2\\Test.txt", "..\\..\\HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:/TestFolder/Level1/Level2\\Test.txt", "../../HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2\\Test.txt", "/../../HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2\\Test.txt", "\\..\\..\\HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        public void TestGetReferencedFile(string dirPath, string relativePath, string expectedPath)
        {
            var instance = new FileInfo(dirPath);
            var path = instance.GetReferencedFile(relativePath);
            Assert.AreEqual(path, expectedPath);
        }

        [TestCase("D:\\TestFolder\\HalloWelt.txt", "D:\\TestFolder\\Level1\\Level2", "..\\..\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\HalloWelt.txt", "D:/TestFolder/Level1/Level2", "..\\..\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\HalloWelt.txt", "D:\\TestFolder\\Level1\\Level2\\", "..\\..\\HalloWelt.txt")]
        [TestCase("D:/TestFolder/HalloWelt.txt", "D:\\TestFolder\\Level1\\Level2", "..\\..\\HalloWelt.txt")]
        public void TestGetRelativePath(string rootFile, string referencedDir, string expectedPath)
        {
            var root = new FileInfo(rootFile);
            var path = root.GetRelativePath(new DirectoryInfo(referencedDir));
            Assert.AreEqual(path, expectedPath);
        }
    }
}
