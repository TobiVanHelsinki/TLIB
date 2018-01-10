﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TLIB_UWPFRAME.IO
{
    public class FileInfoClass : INotifyPropertyChanged
    {

        string _Filename;
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


        string _Filepath;
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


        Place _Fileplace;
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

        string _FolderToken;
        public string FolderToken
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
        void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            Model.ModelHelper.CallPropertyChangedAtDispatcher(PropertyChanged, this, propertyName);
        }
    }
}