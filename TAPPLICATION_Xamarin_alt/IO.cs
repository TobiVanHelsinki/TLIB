using PCLStorage;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TLIB;

namespace TAPPLICATION_Droid
{
    class IO : StandardIO, IPlatformIO
    {
        public Task<string> GetCompleteInternPath(Place place)
        {
            switch (place)
            {
                case Place.Temp:
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                    return "";
                    //return FileSystem.Current.TemporaryFolder.Path + @"\";
                case Place.Local:
                    return FileSystem.Current.LocalStorage.Path;
                case Place.Roaming:
                    if (FileSystem.Current.RoamingStorage == null)
                    {
                        return FileSystem.Current.LocalStorage.Path;
                    }
                    return FileSystem.Current.RoamingStorage.Path;
                case Place.Assets:
                    //return Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\";
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                    return "";
                default:
                    throw new NotImplementedException();
            }
        }

        public void CreateSaveContainer()
        {

        }

        public Task<FileInfo> PickFile(IEnumerable<string> lststrFileEndings, string Token = null)
        {
            throw new NotImplementedException();
        }

        Task<DirectoryInfo> IPlatformIO.PickFolder(string Token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAccess(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAccess(FileInfo Info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }
    }
}
