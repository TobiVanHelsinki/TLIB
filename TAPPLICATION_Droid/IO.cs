using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION.IO;
using TLIB;

namespace TAPPLICATION_Droid
{
    class IO : StandardIO, IPlatformIO
    {
        public override async Task<string> LoadFileContent(FileInfo Info)
        {
            if (Cache.TryGetValue(Info.FullName, out string retval))
            {
                return retval;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public async Task CreateFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }

        public void CreateSaveContainer()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetCompleteInternPath(Place place)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<ExtendetFileInfo>> GetFiles(DirectoryInfo Info, IEnumerable<string> FileTypes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> OpenFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }
        static Dictionary<string, string> Cache = new Dictionary<string, string>();
        static void AddToCache(string key, string data)
        {
            if (Cache.Count > 2)
            {
                Cache.Remove(Cache.Keys.First());
            }
            Cache.Add(key, data);
        }
        public async Task<FileInfo> PickFile(IEnumerable<string> lststrFileEndings, string Token = null)
        {
            FileData fileData = await CrossFilePicker.Current.PickFile();
            if (fileData != null)
            {
                var file = new FileInfo(Path.Combine(fileData.FilePath, fileData.FileName));
                var contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                AddToCache(file.FullName, contents);
                return file;
            }
            else
            {
                throw new IsOKException();
            }
        }

        public async Task<DirectoryInfo> PickFolder(string Token = null)
        {
            throw new NotImplementedException();
        }
    }
}
