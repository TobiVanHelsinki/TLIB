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
    public class FileInfoClass : INotifyPropertyChanged
    {
        string _Filename = "";
        [Obsolete]
        public string Filename
        {
            get { return _Filename; }
            set
            {
                if (value != _Filename)
                {
                    _Filename = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Name { get => SystemFileInfo?.Name ?? _Filename; set { CreateFileInfo(); } }

        string _Filepath = "";
        [Obsolete]
        public string Filepath
        {
            get { return _Filepath; }
            set
            {
                if (value != _Filepath)
                {
                    _Filepath = value;
                    if (!Filepath.EndsWith(@"\"))
                    {
                        Filepath += @"\";
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        public string Path => SystemFileInfo.Directory.FullName;
        public string Fullname => SystemFileInfo.FullName;


        Place _Fileplace = Place.NotDefined;
        [Obsolete]
        public Place Fileplace
        {
            get { return _Fileplace; }
            set
            {
                if (value != _Fileplace)
                {
                    _Fileplace = value;
                    NotifyPropertyChanged();
                }
            }
        }

        DateTimeOffset _DateModified;
        [Obsolete]
        public DateTimeOffset DateModified
        {
            get { return _DateModified; }
            set
            {
                if (value != _DateModified)
                {
                    _DateModified = value;
                    NotifyPropertyChanged();
                }
            }
        }


        ulong _Size;
        [Obsolete]
        public ulong Size
        {
            get { return _Size; }
            set
            {
                if (value != _Size)
                {
                    _Size = value;
                    NotifyPropertyChanged();
                }
            }
        }

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
                    NotifyPropertyChanged();
                }
            }
        }
        
        [Obsolete]
        public event PropertyChangedEventHandler PropertyChanged;

        FileInfo _SystemFileInfo;
        public FileInfo SystemFileInfo
        {
            get { return _SystemFileInfo; }
            set { if (_SystemFileInfo != value) { _SystemFileInfo = value; NotifyPropertyChanged(); } }
        }

        void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PlatformHelper.CallPropertyChanged(PropertyChanged, this, propertyName);
        }

        FileInfoClass()
        {
        }

        public FileInfoClass(FileInfo fi)
        {
            SystemFileInfo = fi;
        }

        public FileInfoClass(Place fileplace, string filename = "", string filepath = "")
        {
            Filename = filename;
            Filepath = filepath;
            Fileplace = fileplace;
            CreateFileInfo();
        }

        private void CreateFileInfo()
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
        public static implicit operator FileInfo(FileInfoClass fic)
        {
            return fic?.SystemFileInfo;
        }
        //  User-defined conversion from double to Digit
        public static implicit operator FileInfoClass(FileInfo fi)
        {
            return new FileInfoClass(fi);
        }

        public FileInfoClass Clone()
        {
            return new FileInfoClass(this.Fileplace, this.Name, this.Path)
            {DateModified = this.DateModified, Token = this.Token, Size = this.Size };
        }

        public override string ToString()
        {
            return Fullname;
        }
    }
}
