using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION;
using TLIB;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.System;

namespace TAPPLICATION_UWP
{
    internal class IO : IPlatformIO
    {
        // ##############################
        public async Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser)
        {
            StorageFile x = await GetFile(Info, eUser:eUD);
            try
            {
                if (true)
                {
                    await FileIO.WriteTextAsync(x, saveChar);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Writingerror", ex);
            }
            FileInfoClass i = Info.Clone();
            i.Filename = x.Name;
            i.Filepath = x.Path.Remove(x.Path.Length - x.Name.Length, x.Name.Length);
            return i;
        }

        public async Task RemoveFile(FileInfoClass Info)
        {
            StorageFile x = await GetFile(Info);
            await x.DeleteAsync();
        }

        public async Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            StorageFile x = await GetFile(Info, FileTypes, eUD, FileNotFoundDecision.NotCreate);
            var rettext = await FileIO.ReadTextAsync(x);
            var info = Info.Clone();
            info.Filename = x.Name;
            info.Filepath = x.Path.Substring(0, x.Path.Length - x.Name.Length);
            return (rettext, info);
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
 { TAPPLICATION.Debugging.TraceException(ex);
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
            catch (IsOKException)
            {
                return null;
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex); // last possibility is to ask the user
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
                if (!string.IsNullOrEmpty(Info.Token))
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.Token + File.Name, File,"A Char File");
                }
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
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

        public async Task<FileInfoClass> GetFileInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            try
            {
                if (!Info.Filepath.EndsWith(@"\"))
                {
                    Info.Filepath += @"\";
                }
                try
                {
                    var info = await StorageFile.GetFileFromPathAsync(Info.Filepath + Info.Filename);
                    Info.Filename = info.Name;
                    Info.Filepath = info.Path.Replace(info.Name, "");
                }
                catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                    if (eUser == UserDecision.AskUser)
                    {
                        var info = await GetFile(Info,null, eUser, FileNotFoundDecision.NotCreate);
                        Info.Filepath = info.Path;
                    }
                    else
                    {
                        throw;
                    }
                }
                return Info;
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                return null;
            }
        }


        public async Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            try
            {
                if (!Info.Filepath.EndsWith(@"\"))
                {
                    Info.Filepath += @"\";
                }
                try
                {
                    var info = await StorageFolder.GetFolderFromPathAsync(Info.Filepath);
                    Info.Filepath = info.Path;
                }
                catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                    if (eUser == UserDecision.AskUser)
                    {
                        var info = await GetFolder(Info, eUser, FileNotFoundDecision.Create);
                        Info.Filepath = info.Path;
                    }
                    else
                    {
                        throw;
                    }
                }
                return Info;
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
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
            StorageFolder ReturnFolder = null;
            try
            {
                if (!Info.Filepath.EndsWith(@"\"))
                {
                    Info.Filepath += @"\";
                }
                ReturnFolder = await StorageFolder.GetFolderFromPathAsync(Info.Filepath);
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                try
                {
                    //Ordner ist nicht da, erzeugen wir ihn
                    if (eCreation == FileNotFoundDecision.Create)
                    {
                        switch (Info.Fileplace)
                        {
                            case Place.Extern:
                                var Dir = new DirectoryInfo(Info.Filepath);
                                var ParentFolder = await StorageFolder.GetFolderFromPathAsync(Dir.Parent.FullName);
                                ReturnFolder = await CreateFoldersRecursive(Info, ParentFolder);
                                break;
                            case Place.Roaming:
                                ReturnFolder = await CreateFoldersRecursive(Info, ApplicationData.Current.RoamingFolder);
                                break;
                            case Place.Local:
                                ReturnFolder = await CreateFoldersRecursive(Info, ApplicationData.Current.LocalFolder);
                                break;
                            default:
                                throw;
                        }
                    }
                }
                catch (Exception ex2)
 { TAPPLICATION.Debugging.TraceException(ex2);
                    // erzeugen klappte nicht.
                    if (eUser == UserDecision.AskUser)
                    {
                        ReturnFolder = await FolderPicker();
                        if (ReturnFolder == null)
                        {
                            throw new IsOKException();
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(Info.Token))
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.Token, ReturnFolder);
                }
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
            }
            return ReturnFolder;
        }

        static async Task<StorageFolder> CreateFoldersRecursive(FileInfoClass Info, StorageFolder StartFolder)
        {
            StorageFolder Folder;
            var path = Info.Filepath.Replace(StartFolder.Path, "");
            var folders = path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            Folder = StartFolder;
            foreach (var item in folders)
            {
                Folder = await Folder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
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
            StorageFolder TargetFolder = await GetFolder(Target, UserDecision.ThrowError);
            StorageFolder SourceFolder = await GetFolder(Source, UserDecision.ThrowError);
            foreach (var item in await SourceFolder.GetFilesAsync())
            {
                if (FileTypes?.Contains(item.FileType) != false)
                {
                    try
                    {
                        await item.MoveAsync(TargetFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    }
                    catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
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
        {
            ApplicationData.Current.LocalSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
        }

        public async Task<FileInfoClass> Rename(FileInfoClass Source, string NewName)
        {
            StorageFile SourceFile = await GetFile(Source);
            await SourceFile.RenameAsync(NewName, NameCollisionOption.GenerateUniqueName);
            var Info = Source.Clone();
            Info.Filepath = SourceFile.Path.Remove(SourceFile.Path.Length - SourceFile.Name.Length);
            return Info;
        }

        public async Task CopyAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            StorageFolder TargetFolder = await GetFolder(Target);
            StorageFolder SourceFolder = await GetFolder(Source);
            foreach (var item in await SourceFolder.GetFilesAsync())
            {
                if (FileTypes?.Contains(item.FileType) != false)
                {
                    try
                    {
                        await item.CopyAsync(TargetFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    }
                    catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                    }
                }
                else
                {

                }
            }
        }
    }
}
