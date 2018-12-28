﻿using System.IO;

namespace TLIB
{

    public static class FileInfoExtension
    {
        public static void ChangeName(this FileInfo File, string Name)
        {
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            File = new FileInfo(File.Directory.FullName + File.Name);
        }

        public static FileInfo Clone(this FileInfo File)
        {
            return new FileInfo(File.FullName);
        }

        public static DirectoryInfo Clone(this DirectoryInfo Dir)
        {
            return new DirectoryInfo(Dir.FullName);
        }

        public static string Path(this FileInfo File)
        {
            return File.Directory.FullName + System.IO.Path.DirectorySeparatorChar;
        }

        public static string Path(this DirectoryInfo Dir)
        {
            return Dir.FullName + System.IO.Path.DirectorySeparatorChar;
        }
    }
}