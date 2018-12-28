using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TAPPLICATION;
using TLIB;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;

namespace TAPPLICATION_UWP
{
    class IO : StandardIO, IPlatformIO
    {
        public async Task<string> GetCompleteInternPath(Place place)
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
            return new FileInfo(file.Path + file.Name);
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
            var f = await folderPicker.PickSingleFolderAsync();
            return new DirectoryInfo(f.Path);
        }

        public void CreateSaveContainer()
        {
            ApplicationData.Current.LocalSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
        }

        public async Task<bool> GetAccess(DirectoryInfo Info)
        {
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            return false;
        }

        public async Task<bool> GetAccess(FileInfo Info)
        {
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            return false;
        }

        public async Task<bool> OpenFolder(DirectoryInfo Info)
        {
            //var f = await GetFolder(Info);
            return await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(Info.FullName));
        }

        //public async Task<FileInfo> GetFileInfo(FileInfo Info, UserDecision eUser = UserDecision.AskUser)
        //{
        //    try
        //    {
        //        if (!Info.Filepath.EndsWith(@"\"))
        //        {
        //            Info.Filepath += @"\";
        //        }
        //        try
        //        {
        //            var info = await StorageFile.GetFileFromPathAsync(Info.Filepath + Info.Name);
        //            Info.Name = info.Name;
        //            Info.Filepath = info.Path.Replace(info.Name, "");
        //        }
        //        catch (Exception ex)
        //        {
        //            TAPPLICATION.Debugging.TraceException(ex, Info);
        //            if (eUser == UserDecision.AskUser)
        //            {
        //                var info = await GetFile(Info, null, eUser, FileNotFoundDecision.NotCreate);
        //                Info.Filepath = info.Path;
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return Info;
        //    }
        //    catch (Exception ex)
        //    {
        //        TAPPLICATION.Debugging.TraceException(ex, Info);
        //        return null;
        //    }
        //}


        //public async Task<FileInfo> GetFolderInfo(FileInfo Info, UserDecision eUser = UserDecision.AskUser)
        //{
        //    try
        //    {
        //        if (!Info.Filepath.EndsWith(@"\"))
        //        {
        //            Info.Filepath += @"\";
        //        }
        //        try
        //        {
        //            var info = await StorageFolder.GetFolderFromPathAsync(Info.Filepath);
        //            Info.Filepath = info.Path;
        //        }
        //        catch (Exception ex)
        //        {
        //            TAPPLICATION.Debugging.TraceException(ex, Info);
        //            if (eUser == UserDecision.AskUser)
        //            {
        //                var info = await GetFolder(Info, eUser, FileNotFoundDecision.Create);
        //                Info.Filepath = info.Path;
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return Info;
        //    }
        //    catch (Exception ex)
        //    {
        //        TAPPLICATION.Debugging.TraceException(ex, Info);
        //        return null;
        //    }
        //}
        /// <summary>
        /// Extern:
        /// Action depends on the string parameters:
        /// Path and Name are provided correctly -> File is returned
        /// Path is provided incorrectly or is null -> Try to create Folder, then ask User for Folderinput
        /// Name is provided incorrectly or is null -> Try to create File, then ask User for Fileinput
        /// Path and Name are provided incorrectly or null -> User shall input File
        /// 
        /// If Place is asstes, than the apps folder is used as base folder to search there for the Info.Filepath + Info.Name file
        /// </summary>
        /// <exception cref="Shared.Enum"/>
        /// <exception cref="Shared.IO_FolderNotFoundOrNotCreated"/>
        /// <exception cref="Shared.IO_UserDecision"/>
        /// <param name="ePlace"></param>
        /// <param name="strFileName"></param>
        /// <param name="strPath"></param>
        /// <param name="FileTypes"></param>
        /// <returns></returns>
        //internal async static Task<StorageFile> GetFile(FileInfo Info, List<string> FileTypes = null, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        //{
        //    StorageFile File = null;
        //    try
        //    {
        //        try
        //        {
        //            //var Path = Info.Filepath + (!Info.Filepath.EndsWith(@"\") ? @"\" : "");
        //            File = await StorageFile.GetFileFromPathAsync(/*Path + CorrectName(Info.Name)*/Info.FullName);
        //        }
        //        catch (Exception ex)
        //        {
        //            TAPPLICATION.Debugging.TraceException(ex, Info);
        //            if (string.IsNullOrEmpty(Info.Name) && string.IsNullOrEmpty(Info.Directory.FullName)) //TODO Check
        //            { // If path and name are empty´, the intent is to ask the user
        //                throw new IsOKException();
        //            }
        //            // path and name are given, so the folder should be there (but maybe aren't) so we try to create them
        //            StorageFolder Folder = await GetFolder(Info, eUser);
        //            switch (eCreation)
        //            {
        //                case FileNotFoundDecision.NotCreate:
        //                    File = await Folder.GetFileAsync(CorrectName(Info.Name));
        //                    break;
        //                case FileNotFoundDecision.Create:
        //                    File = await Folder.CreateFileAsync(CorrectName(Info.Name), CreationCollisionOption.OpenIfExists);
        //                    break;
        //                default:
        //                    throw new Exception();
        //            }
        //        }
        //    }
        //    //catch (IsOKException)
        //    //{
        //    //    return null;
        //    //}
        //    catch (Exception ex)
        //    {
        //        TAPPLICATION.Debugging.TraceException(ex, Info); // last possibility is to ask the user
        //        if (eUser == UserDecision.AskUser)
        //        {
        //            File = await FilePicker(FileTypes); // get from user
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    try
        //    {
        //        //TODO Reimplemetn
        //        //if (!string.IsNullOrEmpty(Info.Token))
        //        //{
        //        //    StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.Token + File.Name, File,"A Char File");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        TAPPLICATION.Debugging.TraceException(ex, Info);
        //    }
        //    return File;
        //}


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
        //       internal async static Task<StorageFolder> GetFolder(FileInfo Info, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        //       {
        //           StorageFolder ReturnFolder = null;
        //           try
        //           {
        //               if (!Info.Filepath.EndsWith(@"\"))
        //               {
        //                   Info.Filepath += @"\";
        //               }
        //               ReturnFolder = await StorageFolder.GetFolderFromPathAsync(Info.Filepath);
        //           }
        //           catch (Exception ex)
        //           {
        //               TAPPLICATION.Debugging.TraceException(ex, Info);
        //               try
        //               {
        //                   //Ordner ist nicht da, erzeugen wir ihn
        //                   if (eCreation == FileNotFoundDecision.Create)
        //                   {
        //                       if (Info.Fullname.StartsWith(ApplicationData.Current.RoamingFolder.Path))
        //                       {
        //                           ReturnFolder = await CreateFoldersRecursive(Info, ApplicationData.Current.RoamingFolder);
        //                       }
        //                       else if (Info.Fullname.StartsWith(ApplicationData.Current.LocalFolder.Path))
        //                       {
        //                           ReturnFolder = await CreateFoldersRecursive(Info, ApplicationData.Current.LocalFolder);
        //                       }
        //                       else
        //                       {
        //                           var Dir = new DirectoryInfo(Info.Filepath);
        //                           var ParentFolder = await StorageFolder.GetFolderFromPathAsync(Dir.Parent.FullName);
        //                           ReturnFolder = await CreateFoldersRecursive(Info, ParentFolder);
        //                       }
        //                       StorageFolder ReturnFolder2;
        //                       switch (Info.Fileplace)
        //                       {
        //                           case Place.Extern:
        //                               var Dir = new DirectoryInfo(Info.Filepath);
        //                               var ParentFolder = await StorageFolder.GetFolderFromPathAsync(Dir.Parent.FullName);
        //                               ReturnFolder2 = await CreateFoldersRecursive(Info, ParentFolder);
        //                               break;
        //                           case Place.Roaming:
        //                               ReturnFolder2 = await CreateFoldersRecursive(Info, ApplicationData.Current.RoamingFolder);
        //                               break;
        //                           case Place.Local:
        //                               ReturnFolder2 = await CreateFoldersRecursive(Info, ApplicationData.Current.LocalFolder);
        //                               break;
        //                           default:
        //                               throw;
        //                       }
        //                       if(ReturnFolder != null && ReturnFolder2!= null)
        //                       {
        //                           //ReturnFolder2
        //                       }
        //                   }
        //               }
        //               catch (Exception ex2)
        //               {
        //                   TAPPLICATION.Debugging.TraceException(ex2, Info);
        //                   // erzeugen klappte nicht.
        //                   if (eUser == UserDecision.AskUser)
        //                   {
        //                       ReturnFolder = await FolderPicker();
        //                       if (ReturnFolder == null)
        //                       {
        //                           throw new IsOKException();
        //                       }
        //                   }
        //                   else
        //                   {
        //                       throw;
        //                   }
        //               }
        //           }

        //           try
        //           {
        //               if (!string.IsNullOrEmpty(Info.Token))
        //               {
        //                   StorageApplicationPermissions.FutureAccessList.AddOrReplace(Info.Token, ReturnFolder);
        //               }
        //           }
        //           catch (Exception ex)
        //{ TAPPLICATION.Debugging.TraceException(ex, Info);
        //           }
        //           return ReturnFolder;
        //       }

    }
}
