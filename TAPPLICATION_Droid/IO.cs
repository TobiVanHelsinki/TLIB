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
            var ret = "";
            if (Cache.TryGetValue(Info.FullName, out Stream retval))
            {
                if (retval.CanRead)
                {
                    using (var r = new StreamReader(retval))
                    {
                        ret = r.ReadToEnd();
                    }
                }
                return ret;
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
        static Dictionary<string, Stream> Cache = new Dictionary<string, Stream>();
        static void AddToCache(string key, Stream data)
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
                //var contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                //AddToCache(file.FullName, contents);
                AddToCache(file.FullName, fileData.GetStream());
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
