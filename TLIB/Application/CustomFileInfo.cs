using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TLIB
{
    public enum Place
    {
        NotDefined = 0,
        Extern = 2,
        Roaming = 3,
        Local = 4,
        Assets = 5,
        Temp = 6
    }

    /// <summary>
    /// Symbolizes a File. Has no link to an actual file at the filesystem, provides binding
    /// </summary>
    public class CustomFileInfo
    {
        public string Name => SystemFileInfo.Name;
        public DirectoryInfo Directory => SystemFileInfo?.Directory;
        public string Fullname => SystemFileInfo?.FullName;

        string _FolderToken = "";
        [Obsolete]
        public string Token
        {
            get { return _FolderToken; }
            set
            {
                if (value != _FolderToken)
                {
                    _FolderToken = value;
                }
            }
        }

        FileInfo _SystemFileInfo;
        public FileInfo SystemFileInfo
        {
            get { return _SystemFileInfo; }
            set { if (_SystemFileInfo != value) { _SystemFileInfo = value;} }
        }
        CustomFileInfo()
        {
        }

        public CustomFileInfo(FileInfo fi)
        {
            SystemFileInfo = fi;
        }

        public CustomFileInfo(string Filename, string Filepath)
        {
            try
            {
                SystemFileInfo = new FileInfo(Filepath + Filename);
            }
            catch (Exception)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            }
        }

        // User-defined conversion from Digit to double
        public static implicit operator FileInfo(CustomFileInfo fic)
        {
            return fic?.SystemFileInfo;
        }
        //  User-defined conversion from double to Digit
        public static implicit operator CustomFileInfo(FileInfo fi)
        {
            return new CustomFileInfo(fi);
        }

        public void ChangeName(string Name)
        {
            SystemFileInfo = new FileInfo(Directory.FullName + Name);
        }

        public CustomFileInfo Clone()
        {
            return new CustomFileInfo(this.Name, this.Directory.FullName)
            { Token = this.Token};
        }

        public override string ToString()
        {
            return Fullname;
        }
    }
}
