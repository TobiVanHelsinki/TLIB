using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION;
using TLIB;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.System;

namespace TAMARIN.IO
{
    internal class UwpIO : IPlatformIO
    {
        // ##############################
        public async Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser)
        {
            StorageFile x = await GetFile(Info, eUser:eUD);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            try
            {
                await FileIO.WriteTextAsync(x, saveChar);
            }
            catch (Exception ex)
            {
                throw new Exception("Writingerror", ex);
            }
            await Task.Delay(TimeSpan.FromMilliseconds(50));
            var i = new FileInfoClass(Info.Fileplace, x.Name, x.Path.Remove(x.Path.Length - x.Name.Length, x.Name.Length));
            return i;
        }

        public async Task RemoveFile(FileInfoClass Info)
        {
            StorageFile x = await GetFile(Info);
            await x.DeleteAsync();
        }

        public async Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            StorageFile x = await GetFile(Info, FileTypes, UserDecision.AskUser, FileNotFoundDecision.NotCreate);
            return (await FileIO.ReadTextAsync(x), new FileInfoClass()
            {
                Filename = x.Name,
                Fileplace = Info.Fileplace,
                FolderToken = Info.FolderToken,
                Filepath = x.Path.Substring(0,x.Path.Length-x.Name.Length) 
            });
        }

        // ##############################
        public async Task<List<FileInfoClass>> GetListofFiles(FileInfoClass Info, UserDecision eUser = UserDecision.ThrowError, List<string> FileTypes = null)
        {
            List<FileInfoClass> ReturnList = new List<FileInfoClass>();
            StorageFolder Folder = await GetFolder(Info, eUser);
            IReadOnlyList<StorageFile> Liste = await Folder.GetFilesAsync();
            if (FileTypes == null || FileTypes.Count == 0)
            {
                FileTypes = new List<string>
                {
                    "."
                };
            }
            foreach (var item in Liste.Where(x=> FileTypes.Contains (x.FileType)))
            {
                BasicProperties props = await item.GetBasicPropertiesAsync();
                ReturnList.Add(new FileInfoClass() { Filename = item.Name, Filepath = Folder.Path , Fileplace = Info.Fileplace, DateModified = props.DateModified, Size = props.Size });
            }
            return ReturnList;
        }


        /// <summary>
        /// Extern:
        /// Action depends on the string parameters:
        /// Path and Name are provided correctly -> File is returned
        /// Path is provided incorrectly or is null -> Try to create Folder, then ask User for Folderinput
        /// Name is provided incorrectly or is null -> Try to create File, then ask User for Fileinput
        /// Path and Name are provided incorrectly or null -> User shall input File
        /// 
        /// If Place is asstes, than the apps folder is used as base folder to search there for the Info.Filepath + Info.Filename file
        /// </summary>
        /// <exception cref="Shared.Enum"/>
        /// <exception cref="Shared.IO_FolderNotFoundOrNotCreated"/>
        /// <exception cref="Shared.IO_UserDecision"/>
        /// <param name="ePlace"></param>
        /// <param name="strFileName"></param>
        /// <param name="strPath"></param>
        /// <param name="FileTypes"></param>
        /// <returns></returns>
        internal async static Task<StorageFile> GetFile(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        {
            StorageFile File = null;
            try
            {
                try
                {
                    if (!Info.Filepath.EndsWith(@"\"))
                    {
                        Info.Filepath += @"\";
                    }
                    File = await StorageFile.GetFileFromPathAsync(Info.Filepath + CorrectName(Info.Filename));
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrEmpty(Info.Filename) && string.IsNullOrEmpty(Info.Filepath))
                    { // If path and name are empty´, the intent is to ask the user
                        throw new IsOKException();
                    }
                    // path and name are given, so the folder should be there (but maybe aren't) so we try to create them
                    StorageFolder Folder = await GetFolder(Info, eUser);
                    switch (eCreation)
                    {
                        case FileNotFoundDecision.NotCreate:
                            File = await Folder.GetFileAsync(CorrectName(Info.Filename));
                            break;
                        case FileNotFoundDecision.Create:
                            File = await Folder.CreateFileAsync(CorrectName(Info.Filename), CreationCollisionOption.OpenIfExists);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            { // last possibility is to ask the user
                if (eUser == UserDecision.AskUser)
                {
                    File = await FilePicker(FileTypes); // get from user
                }
                else
                {
                    throw;
                }
            }

            try
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.FolderToken + File.Name ?? "", File,"A Char File");
            }
            catch (Exception ex)
            {
            }
            return File;
        }
        public static async Task<StorageFile> FilePicker(List<string> lststrFileEndings)
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
            return file;
        }

        
        public async Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            try
            {
                var info = (await GetFolder(Info, UserDecision.AskUser, FileNotFoundDecision.NotCreate));
                Info.Filepath = info.Path;
                return Info;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a Folder. 
        /// Intern Folder: Param Path has to be a either "Local" or "Roaming"
        /// Extern Folder: Param Path has to be a valid Path. If it's not: first attempt is to create this file Then the Folder Picker will be displayed.
        /// </summary>
        /// <param name="ePlace"></param>
        /// <param name="eCreateOptions"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        /// <throws>ArgumentException</throws>
        internal async static Task<StorageFolder> GetFolder(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        {
            StorageFolder Folder = null;
            try
            {
                Folder = await StorageFolder.GetFolderFromPathAsync(Info.Filepath);
            }
            catch (Exception)
            {
                switch (Info.Fileplace)
                {
                    case Place.Local:
                        if (eCreation == FileNotFoundDecision.Create)
                        {
                            string path = CorrectName(Info.Filepath.Remove(0, ApplicationData.Current.LocalFolder.Path.Length), false);
                            Folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(path, CreationCollisionOption.OpenIfExists);
                        }
                        else
                        {
                            throw new Exception(StringHelper.GetString("Error_GetFolder"));
                        }
                        break;
                    case Place.Roaming:
                        if (eCreation == FileNotFoundDecision.Create)
                        {
                            string path = CorrectName(Info.Filepath.Remove(0, ApplicationData.Current.RoamingFolder.Path.Length), false);
                            Folder = await ApplicationData.Current.RoamingFolder.CreateFolderAsync(path, CreationCollisionOption.OpenIfExists);
                        }
                        else
                        {
                            throw new Exception(StringHelper.GetString("Error_GetFolder"));
                        }
                        break;
                    case Place.Extern:
                        // TODO Maybe create recusivly?
                        if (eUser == UserDecision.AskUser && eCreation == FileNotFoundDecision.Create)
                        {
                            Folder = await FolderPicker();
                            if (Folder == null)
                            {
                                throw new IsOKException();
                            }
                        }
                        else
                        {
                            throw new Exception(StringHelper.GetString("Error_GetFolder"));
                        }
                        break;
                }
            }

            try
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.FolderToken, Folder);
            }
            catch (Exception ex)
            {
            }
            return Folder;
        }
        /// <summary>
        /// Throws things
        /// </summary>
        /// <param name="strSuggestedStartLocation"></param>
        /// <returns></returns>
        public static async Task<StorageFolder> FolderPicker()
        {
            var folderPicker = new FolderPicker()
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder, ViewMode = PickerViewMode.List
            };
            folderPicker.FileTypeFilter.Add(".");
            return await folderPicker.PickSingleFolderAsync();
        }

        public async Task MoveAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            StorageFolder TargetFolder = await GetFolder(Target);
            StorageFolder SourceFolder = await GetFolder(Source);
            foreach (var item in await SourceFolder.GetFilesAsync())
            {
                if (FileTypes?.Contains(item.FileType) != false)
                {
                    try
                    {
                        await item.MoveAsync(TargetFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {

                }
            }
        }
        /// <summary>
        /// simples kopieren, bei fehlern wird abgebrochen, dateien werden ueberschrieben
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public async Task Copy(FileInfoClass Target, FileInfoClass Source)
        {
            StorageFile SourceFile = await GetFile(Source);
            StorageFolder TargetFolder = await GetFolder(Target, UserDecision.ThrowError);
            await SourceFile.CopyAsync(TargetFolder, Target.Filename ?? SourceFile.Name, NameCollisionOption.ReplaceExisting);
        }
        /// <summary>
        /// converts the name to an allowd string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string CorrectName(string name, bool ReplaceInsteadOfRemove = true)
        {
            string ReturnValue = "";
            foreach (char item in name)
            {
                if (item == '/' || item == '"' || item == '\\')
                {
                    if (ReplaceInsteadOfRemove)
                    {
                        ReturnValue += '_';
                    }
                }
                else
                {
                    ReturnValue += item;
                }
            }
            return ReturnValue;
        }

        public async Task<bool> OpenFolder(FileInfoClass Info)
        {
            var f = await GetFolder(Info);
            return await Launcher.LaunchFolderAsync(f);
        }

        public string GetCompleteInternPath(Place place)
        {
            switch (place)
            {
                case Place.Temp:
                    return ApplicationData.Current.TemporaryFolder.Path + @"\";
                case Place.Local:
                    return ApplicationData.Current.LocalFolder.Path + @"\";
                case Place.Roaming:
                    return ApplicationData.Current.RoamingFolder.Path + @"\";
                case Place.Assets:
                    return Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\";
                default:
                    throw new NotImplementedException();
            }
        }

        public void CreateSaveContainer()
        {//TODO falsche stelle
            ApplicationData.Current.LocalSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
        }
    }
}
