using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.MSBUILD
{
    internal class FileHelper
    {

        /// <summary>
        /// Deletes the key.
        /// </summary>
        /// <param name="tempFolder">The temporary folder.</param>
        internal static void DeleteKey(string tempFolder)
        {
            var dir = new DirectoryInfo(tempFolder);

            foreach (var file in dir.EnumerateFiles("*.pfx"))
            {
                file.Delete();
            }
        }


        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        internal static string ReadFile(string file)
        {
            string xml;
            using (StreamReader sr = new StreamReader(file))
            {
                xml = sr.ReadToEnd();
            }

            return xml;
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="xml">The XML.</param>
        internal static void WriteFile(string filePath, string xml)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                file.WriteLine(xml);
            }
        }




        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/bb762914.aspx
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        internal static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            if (sourceDirName.EndsWith("\\bin") || sourceDirName.EndsWith("\\obj"))
            {
                return;
            }

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}
