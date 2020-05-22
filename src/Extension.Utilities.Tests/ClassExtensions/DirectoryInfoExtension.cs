using Extension.Utilities.ClassExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Extension.Utilities.Tests.ClassExtensions
{
    [TestFixture]
    public class DirectoryInfoExtension
    {
        [TestCase("D:\\TestFolder\\Level1\\Level2", "../../HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2", "D:\\TestFolder\\HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2", "..\\..\\HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:/TestFolder/Level1/Level2", "../../HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2", "/../../HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        [TestCase("D:\\TestFolder\\Level1\\Level2", "\\..\\..\\HalloWelt.txt", "D:\\TestFolder\\HalloWelt.txt")]
        public void TestGetReferencedFile(string dirPath, string relativePath, string expectedPath)
        {
            var instance = new DirectoryInfo(dirPath);
            var path = instance.GetReferencedFile(relativePath);
            Assert.AreEqual(path, expectedPath);
        }

    }
}
