using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION;
using TAPPLICATION.IO;
using TLIB;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;

namespace TAPPLICATION_UWP
{
    public static class CustomFileInfoExtension
    {
        public static ExtendetFileInfo ToFileInfo(this StorageFile d)
        {
            var T = d.GetBasicPropertiesAsync();
            T.AsTask().Wait();
            var att = T.GetResults();
            return new ExtendetFileInfo(d.Path, att.DateModified.DateTime, (long)att.Size);
        }
    }
    class IO : StandardIO, IPlatformIO
    {
        public override async Task<IEnumerable<ExtendetFileInfo>> GetFiles(DirectoryInfo Info, IEnumerable<string> FileTypes = null)
        {
            var folderhandle = await GetFolder(Info);
            return (await folderhandle.GetFilesAsync()).Where(x => FileTypes != null ? FileTypes.Contains(x.FileType) : true).Select(x => x.ToFileInfo());
        }
        #region Basic File Operations

        public override async Task RemoveFile(FileInfo Info)
        {
            var filehandle = await GetFile(Info);
            await filehandle.DeleteAsync();
        }

        public override async Task<FileInfo> Rename(FileInfo Source, string NewName)
        {
            var filehandle = await GetFile(Source);
            await filehandle.RenameAsync(NewName);
            return new FileInfo(filehandle.Path);
        }

        public override async Task<FileInfo> CopyTo(FileInfo Source, FileInfo Target)
        {
            var sourcehandle = await GetFile(Source);
            var targethandle = await GetFolder(Target.Directory);
            var newfile = await sourcehandle.CopyAsync(targethandle, Target.Name, NameCollisionOption.GenerateUniqueName);
            return new FileInfo(newfile.Path);
        }

        public override async Task MoveTo(FileInfo Source, FileInfo Target)
        {
            var sourcehandle = await GetFile(Source);
            var targethandle = await GetFolder(Target.Directory);
            await sourcehandle.MoveAsync(targethandle, Target.Name, NameCollisionOption.GenerateUniqueName);
        }

        #endregion
        #region File Content
        public override async Task SaveFileContent(string saveChar, FileInfo Info)
        {
            var filehandle = await GetOrCreateFile(Info);
            await FileIO.WriteTextAsync(filehandle, saveChar);
        }
        public override async Task<string> LoadFileContent(FileInfo Info)
        {
            var filehandle = await GetFile(Info);
            return await FileIO.ReadTextAsync(filehandle);
        }


        #endregion
        #region Helper
        async Task<StorageFile> GetFile(FileInfo Info)
        {
            return await StorageFile.GetFileFromPathAsync(Info.FullName);
        }
        async Task<StorageFile> GetOrCreateFile(FileInfo Info)
        {
            try
            {
                return await GetFile(Info);
            }
            catch (Exception)
            {
                var folderhandle = await StorageFolder.GetFolderFromPathAsync(Info.Directory.FullName);
                return await folderhandle.CreateFileAsync(Info.Name, CreationCollisionOption.OpenIfExists);
            }
        }
        async Task<StorageFolder> GetFolder(DirectoryInfo Info)
        {
            try
            {
                return await StorageFolder.GetFolderFromPathAsync(Info.FullName);
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex, Info.FullName);
                throw;
            }
        }
        async Task<StorageFolder> GetOrCreateFolder(DirectoryInfo Info)
        {
            try
            {
                return await GetFolder(Info);
            }
            catch (Exception)
            {
                var folderhandle = await StorageFolder.GetFolderFromPathAsync(Info.Parent.FullName);
                return await folderhandle.CreateFolderAsync(Info.Name, CreationCollisionOption.OpenIfExists);
            }
        }
        public async Task CreateFolder(DirectoryInfo Info)
        {
            await GetOrCreateFolder(Info);
        }

        #endregion
        #region Picker
        public async Task<FileInfo> PickFile(IEnumerable<string> lststrFileEndings, string Token = null)
        {
            FileOpenPicker openPicker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };
            foreach (var item in lststrFileEndings)
            {
                openPicker.FileTypeFilter.Add(item);
            }

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null)
            {
                throw new IsOKException();
            }
            AddToFutureRequestList(Token, file);
            return new FileInfo(file.Path);
        }

        private static void AddToFutureRequestList(string Token, IStorageItem item)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                try
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(Token, item);
                }
                catch (Exception ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                    TAPPLICATION.Debugging.TraceException(ex, Token);
                }
            }
        }

        /// <summary>
        /// Throws things
        /// </summary>
        /// <param name="strSuggestedStartLocation"></param>
        /// <returns></returns>
        public async Task<DirectoryInfo> PickFolder(string Token = null)
        {
            var folderPicker = new FolderPicker()
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
                ViewMode = PickerViewMode.List
            };
            folderPicker.FileTypeFilter.Add(".");
            var dir = await folderPicker.PickSingleFolderAsync();
            if (dir == null)
            {
                throw new IsOKException();
            }
            AddToFutureRequestList(Token, dir);
            return new DirectoryInfo(dir.Path);
        }

        #endregion
        #region Other

        public async Task<string> GetCompleteInternPath(Place place)
        {
            switch (place)
            {
                case Place.Temp:
                    return ApplicationData.Current.TemporaryFolder.Path + Path.DirectorySeparatorChar;
                case Place.Local:
                    return ApplicationData.Current.LocalFolder.Path + Path.DirectorySeparatorChar;
                case Place.Roaming:
                    return ApplicationData.Current.RoamingFolder.Path + Path.DirectorySeparatorChar;
                case Place.Assets:
                    return Windows.ApplicationModel.Package.Current.InstalledLocation.Path + Path.DirectorySeparatorChar;
                default:
                    throw new NotImplementedException();
            }
        }

        public void CreateSaveContainer()
        {
            ApplicationData.Current.LocalSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
            //ApplicationData.Current.LocalFolder.CreateFolderAsync(Constants., ApplicationDataCreateDisposition.Always);
            ApplicationData.Current.RoamingSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
        }

        public async Task<bool> OpenFolder(DirectoryInfo Info)
        {
            return await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(Info.FullName));
        }
        #endregion
    }
}
