﻿using PCLStorage;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION_UWP;
using TLIB;

namespace TAPPLICATION_Xamarin
{
    class IO : StandardIO, IPlatformIO
    {
        public async Task<string> GetCompleteInternPath(Place place)
        {
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            return "";
            switch (place)
            {
                case Place.Temp:
                case Place.Local:
                case Place.Roaming:
                case Place.Assets:
                default:
                    throw new NotImplementedException();
            }
        }

        public void CreateSaveContainer()
        {

        }

        public async Task<FileInfo> PickFile(IEnumerable<string> lststrFileEndings, string Token = null)
        {
            throw new NotImplementedException();
        }

        public async Task<DirectoryInfo> PickFolder(string Token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetAccess(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetAccess(FileInfo Info)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> OpenFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }
    }
}
