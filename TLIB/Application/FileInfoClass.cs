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


        string _Filepath = "";
        public string Filepath
        {
            get { return _Filepath; }
            set
            {
                if (value != _Filepath)
                {
                    _Filepath = value;
                    NotifyPropertyChanged();
                }
            }
        }


        Place _Fileplace = Place.NotDefined;
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

        public FileInfoClass()
        {
            try
            {
                SystemFileInfo = new FileInfo(Fileplace + Filename);
            }
            catch (Exception)
            {
            }
            //Size = SystemFileInfo.Length;
        }

        public FileInfoClass(Place place) : this()
        {
            Fileplace = place;
        }

        public FileInfoClass(Place fileplace, string filename, string filepath) : this(fileplace)
        {
            Filename = filename;
            Filepath = filepath;
        }

        public FileInfoClass Clone()
        {
            return new FileInfoClass(this.Fileplace, this.Filename, this.Filepath)
            {DateModified = this.DateModified, Token = this.Token, Size = this.Size };
        }

        public override string ToString()
        {
            return Filepath + "_" + Filename;
        }
    }
}
