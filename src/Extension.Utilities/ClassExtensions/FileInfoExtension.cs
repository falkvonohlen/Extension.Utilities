using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Extension.Utilities.ClassExtensions
{
    /// <summary>
    /// Extensions for file infos
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        /// Returns the relative path from the reference to this file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string GetRelativePath(this FileInfo instance, FileInfo reference)
        {
            var from = new Uri(reference.FullName);
            var supposedToBeRelative = new Uri(instance.FullName);
            return from.MakeRelativeUri(supposedToBeRelative).ToString().MakeDotNetRelativePath();
        }

        /// <summary>
        /// Returns the relative path from the reference to this file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static string GetRelativePath(this FileInfo instance, DirectoryInfo reference)
        {
            var tmpFile = new FileInfo(Path.Combine(reference.FullName, "tmp.tmp"));
            return instance.GetRelativePath(tmpFile);
        }

        /// <summary>
        /// Gets the absolute path to a file which is referenced via a relative path
        /// from within this file instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetReferencedFile(this FileInfo instance, string relativePath)
        {
            var netRelative = relativePath.MakeDotNetRelativePath();

            if (Path.IsPathRooted(netRelative))//In case the path is already rooted it is return 
            {
                return netRelative;
            }
            else 
            {
                return Path.GetFullPath(Path.Combine(instance.Directory.FullName, netRelative));
            }
        }

    }
}
