using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION.IO;
using TLIB;
using Xamarin.Essentials;

namespace TAPPLICATION_Xamarin
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
        public Task CreateFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }

        public void CreateSaveContainer()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetCompleteInternPath(Place place)
        {
            switch (place)
            {
                case Place.Roaming:
                    break;
                case Place.Local:
                    break;
                case Place.Assets:
                    return FileSystem.AppDataDirectory; //TODO Test. Eventuell muss noch ein "assets" dran
                    //TODO aber vielleicht kann man assets auch durch app dir ersetzen, wäre sinnvoller
                case Place.Temp:
                    return FileSystem.CacheDirectory;
                default:
                    break;
            }
            throw new NotImplementedException();
        }


        public Task<bool> OpenFolder(DirectoryInfo Info)
        {
            throw new NotImplementedException();
        }
        static Dictionary<string, Stream> Cache = new Dictionary<string, Stream>();
        static void AddToCache(string key, Stream data)
        {
            if (Cache.Count > 10)
            {
                Cache.Remove(Cache.Keys.First());
            }
            Cache.Add(key, data);
        }
        public async Task<FileInfo> PickFile(IEnumerable<string> lststrFileEndings, string Token = null)
        {
            var fileData = await CrossFilePicker.Current.PickFile();
            if (fileData != null)
            {
                var file = new FileInfo(Path.Combine(fileData.FilePath, fileData.FileName));
                AddToCache(file.FullName, fileData.GetStream());
                return file;
            }
            else
            {
                throw new IsOKException();
            }
        }

        public Task<DirectoryInfo> PickFolder(string Token = null)
        {
            throw new NotImplementedException();
        }
    }
}
