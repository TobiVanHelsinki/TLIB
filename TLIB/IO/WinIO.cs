﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLIB.Model;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace TLIB.IO
{
    internal class WinIO : IGeneralIO
    {
        // ##############################
        public async Task SaveFileContent(string saveChar, FileInfoClass Info)
        {
            if (Info.Fileplace != Place.Extern)
            {
                if (SharedSettingsModel.I.InternSync)
                {
                    Info.Fileplace = Place.Roaming;
                }
                else
                {
                    Info.Fileplace = Place.Local;
                }
            }
            StorageFile x = await GetFile(Info);
            await FileIO.WriteTextAsync(x, saveChar);
        }

        public async Task RemoveFile(FileInfoClass Info)
        {
            StorageFile x = await GetFile(Info);
            await x.DeleteAsync();
        }

        public async Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            StorageFile x = await GetFile(Info, FileTypes, eUD, FileNotFoundDecision.NotCreate);
            if (x == null)
            {
                x = await FilePicker(FileTypes);
            }
            return (await FileIO.ReadTextAsync(x), new FileInfoClass()
            {
                Filename = x.Name,
                Fileplace = Info.Fileplace,
                FolderToken = Info.FolderToken,
                Filepath = Info.Fileplace == Place.Extern? x.Path.Substring(0,x.Path.Length-x.Name.Length) : SharedIO.GetCurrentSavePath()
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
                Windows.Storage.FileProperties.BasicProperties props = await item.GetBasicPropertiesAsync();
                ReturnList.Add(new FileInfoClass() { Filename = item.Name, Filepath = Info.Fileplace == Place.Extern ? Folder.Path : SharedIO.GetCurrentSavePath(), Fileplace = Info.Fileplace, DateModified = props.DateModified, Size = props.Size });
            }
            return ReturnList;
        }

        public async Task<FileInfoClass> GetFileInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            var info = (await GetFile(Info, eUser:UserDecision.AskUser));
            return new FileInfoClass() { Filepath = info.Path, Filename = info.Name, Fileplace = Info.Fileplace };
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
            if (Info.Fileplace == Place.Assets)
            {
                try
                {
                    File = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Info.Filepath + Info.Filename));
                }
                catch (Exception)
                {
                }
                return File;
            }
            try
            {
                try
                {
                    if (!Info.Filepath.EndsWith("\\"))
                    {
                        Info.Filepath += "\\";
                    }
                    File = await StorageFile.GetFileFromPathAsync(Info.Filepath + CorrectName(Info.Filename));
                }
                catch (Exception ex)
                {
                    if (Info.Filename == null && Info.Filepath == null || Info.Filename == "" && Info.Filepath == "")
                    {
                        throw new Exception();
                    }
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
            {
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
                Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList.AddOrReplace(Info.FolderToken + File.Name ?? "" + File.Name, File,"A Char File",Windows.Storage.AccessCache.RecentStorageItemVisibility.AppAndSystem);
            }
            catch (Exception ex)
            {
                //TLIB.Model.SharedAppModel.Instance.NewNotification("test", ex);
            }
            try
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.FolderToken + File.Name ?? "" + File.Name, File,"A Char File");
            }
            catch (Exception ex)
            {
                //TLIB.Model.SharedAppModel.Instance.NewNotification("test2", ex);
            }
            return File;
        }
        public static async Task<StorageFile> FilePicker(List<string> lststrFileEndings)
        {
            //if (lststrFileEndings == null)
            //{
            //    lststrFileEndings = SharedConstants.LST_FILETYPES_ALL;
            //}
            FileOpenPicker openPicker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };
            foreach (var item in lststrFileEndings)
            {
                openPicker.FileTypeFilter.Add(item);
            }

            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;
        }

        
        public async Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            var info = (await GetFolder(Info, UserDecision.AskUser));
            Info.Filepath = info.Path;
            return Info;
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
        internal async static Task<StorageFolder> GetFolder(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            StorageFolder Folder = null;
            switch (Info.Fileplace)
            {
                case Place.Local:
                    Folder= await ApplicationData.Current.LocalFolder.CreateFolderAsync(Info.Filepath, CreationCollisionOption.OpenIfExists);
                    break;
                case Place.Roaming:
                    Folder = await ApplicationData.Current.RoamingFolder.CreateFolderAsync(Info.Filepath, CreationCollisionOption.OpenIfExists);
                    break;
                case Place.Extern:
                    try // to get it
                    {
                        Folder = await StorageFolder.GetFolderFromPathAsync(Info.Filepath);
                    }
                    catch (Exception ex)
                    {
                        if (eUser == UserDecision.AskUser)
                        {
                            Folder = await FolderPicker();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    break;
            }
            try
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList.AddOrReplace(Info.FolderToken, Folder, "A folder that contains a sr char file", Windows.Storage.AccessCache.RecentStorageItemVisibility.AppAndSystem);
            }
            catch (Exception ex)
            {
                //TLIB.Model.SharedAppModel.Instance.NewNotification("test", ex);
            }
            try
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.FolderToken, Folder);
            }
            catch (Exception ex)
            {
                //TLIB.Model.SharedAppModel.Instance.NewNotification("test2", ex);
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

        public async Task CopyLocalRoaming(Place Target, string Path)
        {
            StorageFolder TargetFolder = await GetFolder(new FileInfoClass() { Fileplace = Target, Filepath = Path });
            StorageFolder SourceFolder = await GetFolder(new FileInfoClass() { Fileplace = Target == Place.Local? Place.Roaming : Place.Local, Filepath = Path });
            foreach (var item in await SourceFolder.GetFilesAsync())
            {
                try
                {
                    await item.MoveAsync(TargetFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// simples kopieren, bei fehlern wird abgebrochen, dateien werden überschrieben
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Source"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public async Task Copy(FileInfoClass Target, FileInfoClass Source, string newName = null)
        {//TODO Test it
            StorageFile SourceFile = await GetFile(Source);
            StorageFolder TargetFolder = await GetFolder(Target, UserDecision.ThrowError);
            await SourceFile.CopyAsync(TargetFolder, newName ?? SourceFile.Name, NameCollisionOption.ReplaceExisting);
        }
        /// <summary>
        /// converts the name to an allowd string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string CorrectName(string name)
        {
            string ReturnValue = "";
            foreach (char item in name)
            {
                if (item == '/' || item == '"'|| item == '\\')
                {
                    ReturnValue += '_';
                }
                else
                {
                    ReturnValue += item;
                }
            }
            return ReturnValue;
        }
    }
}