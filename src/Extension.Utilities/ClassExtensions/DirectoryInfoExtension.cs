using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Extension.Utilities.ClassExtensions
{
    public static class DirectoryInfoExtension
    {

        /// <summary>
        /// Gets the absolute path to a file which is referenced via a relative path
        /// from within this directory instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetReferencedFile(this DirectoryInfo instance, string relativePath)
        {
            var netRelative = relativePath.MakeDotNetRelativePath(); ;

            if (Path.IsPathRooted(netRelative))//In case the path is already rooted it is return 
            {
                return netRelative;
            }
            else
            {
                return Path.GetFullPath(Path.Combine(instance.FullName, netRelative));
            }
        }

    }
}
