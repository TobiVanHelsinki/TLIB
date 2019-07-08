using System;
using System.IO;

namespace TLIB
{
    /// <summary>
    /// Provides Extensional Methods for the FileInfo class
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        /// returns a new FileInfo, that has the given name
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Name"></param>
        public static FileInfo ChangeName(this FileInfo File, string Name)
        {
            return new FileInfo(
                File.Directory.FullName 
                + (File.Directory.FullName.EndsWith(Environment.NewLine) ? "" : Environment.NewLine)
                + Name);
        }

        /// <summary>
        /// Create a copy of this instance 
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static FileInfo Clone(this FileInfo File)
        {
            return new FileInfo(File.FullName);
        }

        /// <summary>
        /// Create a copy of this instance 
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        public static DirectoryInfo Clone(this DirectoryInfo Dir)
        {
            return new DirectoryInfo(Dir.FullName);
        }

        /// <summary>
        /// returns the Fulle Path of this File
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static string Path(this FileInfo File)
        {
            return File.Directory.FullName + System.IO.Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// returns the Fulle Path of this Directory
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        public static string Path(this DirectoryInfo Dir)
        {
            return Dir.FullName + System.IO.Path.DirectorySeparatorChar;
        }
    }
}
