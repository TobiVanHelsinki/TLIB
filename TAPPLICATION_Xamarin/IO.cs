using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TAPPLICATION.IO;

namespace TAPPLICATION_Xamarin
{
    class IO : StandardIO, IPlatformIO
    {
        public Task CreateFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }

        public void CreateSaveContainer()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCompleteInternPath(Place place)
        {
            throw new NotImplementedException();
        }


        public Task<bool> OpenFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }

        public Task<FileInfo> PickFile(IEnumerable<string> lststrFileEndings, string Token = null)
        {
            throw new NotImplementedException();
        }

        public Task<DirectoryInfo> PickFolder(string Token = null)
        {
            throw new NotImplementedException();
        }
    }
}
