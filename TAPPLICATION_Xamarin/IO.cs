using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TLIB;

namespace TAPPLICATION_Xamarin
{
    internal class IO : IPlatformIO
    {
        public Task Copy(FileInfoClass Target, FileInfoClass Source, UserDecision euser)
        {
            return default;
        }

        public Task CopyAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            return default;
        }

        public void CreateSaveContainer()
        {
        }

        public string GetCompleteInternPath(Place place)
        {
            return default;
        }

        public Task<FileInfoClass> GetFileInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            return default;
        }

        public Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            return default;
        }

        public Task<List<FileInfoClass>> GetListofFiles(FileInfoClass Info, UserDecision eUser, List<string> FileTypes = null)
        {
            return default;
        }

        public async Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            FileInfo x = await GetFile(Info, FileTypes, eUD, FileNotFoundDecision.NotCreate);
            string rettext = File.ReadAllText(x.FullName);
            var info = Info.Clone();
            //info.Filename = x.Name;
            //info.Filepath = x.Path.Substring(0, x.Path.Length - x.Name.Length);
            return (rettext, info);
        }

        internal async static Task<FileInfo> GetFile(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        {
            FileInfo File = null;
            try
            {
                try
                {
                    if (!Info.Filepath.EndsWith(@"\"))
                    {
                        Info.Filepath += @"\";
                    }
                    var a = new FileInfo(Info.Filepath + CorrectName(Info.Filename));
                    if (a.Exists)
                    {
                        return a;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    TAPPLICATION.Debugging.TraceException(ex, Info);
                    if (string.IsNullOrEmpty(Info.Filename) && string.IsNullOrEmpty(Info.Filepath))
                    { // If path and name are empty´, the intent is to ask the user
                        throw new IsOKException();
                    }
                    // path and name are given, so the folder should be there (but maybe aren't) so we try to create them
                        throw new Exception();
                    //DirectoryInfo Folder = await GetFolder(Info, eUser);
                    //switch (eCreation)
                    //{
                    //    case FileNotFoundDecision.NotCreate:
                    //        File = await Folder.Getf(CorrectName(Info.Filename));
                    //        break;
                    //    case FileNotFoundDecision.Create:
                    //        File = await Folder.CreateFileAsync(CorrectName(Info.Filename), CreationCollisionOption.OpenIfExists);
                    //        break;
                    //    default:
                    //        throw new Exception();
                    //}
                }
            }
            catch (IsOKException)
            {
                return null;
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex, Info); // last possibility is to ask the user
                if (eUser == UserDecision.AskUser)
                {
                    File = await FilePicker(FileTypes); // get from user
                }
                else
                {
                    throw;
                }
            }

            return File;
        }

        public static async Task<FileInfo> FilePicker(List<string> lststrFileEndings)
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file == null)
            {
                throw new IsOKException();
            }
            else
            {
                return new FileInfo(file.FileName);
            }
        }

        internal async static Task<DirectoryInfo> GetFolder(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        {
            DirectoryInfo ReturnFolder = null;
            try
            {
                if (!Info.Filepath.EndsWith(@"\"))
                {
                    Info.Filepath += @"\";
                }
                ReturnFolder = new DirectoryInfo(Info.Filepath);
                if (ReturnFolder.Exists)
                {
                    return ReturnFolder;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex, Info);
                try
                {
                    //Ordner ist nicht da, erzeugen wir ihn
                    if (eCreation == FileNotFoundDecision.Create)
                    {
                        switch (Info.Fileplace)
                        {
                            case Place.Extern:
                                var Dir = new DirectoryInfo(Info.Filepath);
                                var ParentFolder = new DirectoryInfo(Dir.Parent.FullName);
                                ReturnFolder = await CreateFoldersRecursive(Info, ParentFolder);
                                break;
                            case Place.Roaming:
                            case Place.Local:
                                ReturnFolder = await CreateFoldersRecursive(Info, new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
                                break;
                            default:
                                throw;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    TAPPLICATION.Debugging.TraceException(ex2, Info);
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

            return ReturnFolder;
        }

        static async Task<DirectoryInfo> CreateFoldersRecursive(FileInfoClass Info, DirectoryInfo StartFolder)
        {
            DirectoryInfo Folder;
            var path = Info.Filepath.Replace(StartFolder.FullName, "");
            var folders = path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            Folder = StartFolder;
            foreach (var item in folders)
            {
                Folder = Folder.CreateSubdirectory(item);
            }

            return Folder;
        }

        /// <summary>
        /// Throws things
        /// </summary>
        /// <param name="strSuggestedStartLocation"></param>
        /// <returns></returns>
        public static async Task<DirectoryInfo> FolderPicker()
        {
            throw new NotImplementedException();
            //var a = await CrossFilePicker.Current.PickFile();
            //a.
            //var folderPicker = new FolderPicker()
            //{
            //    SuggestedStartLocation = PickerLocationId.ComputerFolder,
            //    ViewMode = PickerViewMode.List
            //};
            //folderPicker.FileTypeFilter.Add(".");
            //return await folderPicker.PickSingleFolderAsync();
        }


        public Task MoveAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            return default;
        }

        public Task<bool> OpenFolder(FileInfoClass Info)
        {
            return default;
        }

        public Task RemoveFile(FileInfoClass Info)
        {
            return default;
        }

        public Task<FileInfoClass> Rename(FileInfoClass SourceFile, string NewName)
        {
            return default;
        }

        public Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser)
        {
            return default;
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

    }
}
