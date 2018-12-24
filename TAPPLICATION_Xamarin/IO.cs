using PCLStorage;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLIB;

namespace TAPPLICATION_Xamarin
{
    public static class MyClass
    {
        public static string FileType(this IFile File)
        {
            return string.Concat(File.Name.SkipWhile(x => x != '.'));
        }
    }
    internal class IO : IPlatformIO
    {
        public async Task Copy(FileInfoClass Target, FileInfoClass Source, UserDecision UD)
        {
            //TODO Test
            IFile SourceFile = await GetFile(Source, eUser: UD);
            IFolder TargetFolder = await GetFolder(Target, UD);
            var NewFile = await TargetFolder.CreateFileAsync(Target.Filename ?? SourceFile.Name, CreationCollisionOption.ReplaceExisting);
            await NewFile.WriteAllTextAsync(await SourceFile.ReadAllTextAsync());
        }

        public async Task CopyAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            IFolder TargetFolder = await GetFolder(Target);
            IFolder SourceFolder = await GetFolder(Source);
            foreach (var SourceFile in await SourceFolder.GetFilesAsync())
            {
                if (FileTypes?.Contains(SourceFile.FileType()) != false) //TODO Check Extension code
                {
                    try
                    {
                        //await SourceFile.CopyAsync(TargetFolder, SourceFile.Name, NameCollisionOption.GenerateUniqueName);
                        //TODO Check this
                        var NewFile = await TargetFolder.CreateFileAsync(Target.Filename ?? SourceFile.Name, CreationCollisionOption.ReplaceExisting);
                        await NewFile.WriteAllTextAsync(await SourceFile.ReadAllTextAsync());
                    }
                    catch (Exception ex)
                    {
                        TAPPLICATION.Debugging.TraceException(ex, Target.ToString() + Source.ToString());
                    }
                }
                else
                {

                }
            }
        }

        public void CreateSaveContainer()
        {
           
        }

        public string GetCompleteInternPath(Place place)
        {
            switch (place)
            {
                case Place.Temp:
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                    return "";
                    //return FileSystem.Current.TemporaryFolder.Path + @"\";
                case Place.Local:
                    return FileSystem.Current.LocalStorage.Path;
                case Place.Roaming:
                    if (FileSystem.Current.RoamingStorage == null)
                    {
                        return FileSystem.Current.LocalStorage.Path;
                    }
                    return FileSystem.Current.RoamingStorage.Path;
                case Place.Assets:
                    //return Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\";
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                    return "";
                default:
                    throw new NotImplementedException();
            }
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
                    var info = await FileSystem.Current.GetFileFromPathAsync(Info.Filepath + Info.Filename);
                    Info.Filename = info.Name;
                    Info.Filepath = info.Path.Replace(info.Name, "");
                }
                catch (Exception ex)
                {
                    TAPPLICATION.Debugging.TraceException(ex, Info);
                    if (eUser == UserDecision.AskUser)
                    {
                        var info = await GetFile(Info, null, eUser, FileNotFoundDecision.NotCreate);
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
            {
                TAPPLICATION.Debugging.TraceException(ex, Info);
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
                    var info = await FileSystem.Current.GetFolderFromPathAsync(Info.Filepath);
                    Info.Filepath = info.Path;
                }
                catch (Exception ex)
                {
                    TAPPLICATION.Debugging.TraceException(ex, Info);
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
            {
                TAPPLICATION.Debugging.TraceException(ex, Info);
                return null;
            }
        }

        public async Task<List<FileInfoClass>> GetListofFiles(FileInfoClass Info, UserDecision eUser, List<string> FileTypes = null)
        {
            var ReturnList = new List<FileInfoClass>();
            var Folder = await GetFolder(Info, eUser);
            var Liste = await Folder.GetFilesAsync();
            if (FileTypes == null || FileTypes.Count == 0)
            {
                FileTypes = new List<string>
                {
                    "."
                };
            }
            foreach (var item in Liste.Where(x => FileTypes.Contains(x.FileType())))
            {
                //BasicProperties props = await item.GetBasicPropertiesAsync();
                //ReturnList.Add(new FileInfoClass() { Filename = item.Name, Filepath = Folder.Path, Fileplace = Info.Fileplace, DateModified = props.DateModified, Size = props.Size });
                ReturnList.Add(new FileInfoClass() { Filename = item.Name, Filepath = Folder.Path, Fileplace = Info.Fileplace, DateModified = DateTimeOffset.Now, Size = 666});
            }
            return ReturnList;
        }

        public async Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            IFile x = await GetFile(Info, FileTypes, eUD, FileNotFoundDecision.NotCreate);
            string rettext = await x.ReadAllTextAsync();
            var info = Info.Clone();
            info.Filename = x.Name;
            info.Filepath = x.Path.Substring(0, x.Path.Length - x.Name.Length);
            return (rettext, info);
        }

        internal async static Task<IFile> GetFile(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        {
            IFile File = null;
            try
            {
                try
                {
                    if (!Info.Filepath.EndsWith(@"\"))
                    {
                        Info.Filepath += @"\";
                    }
                    File = await FileSystem.Current.GetFileFromPathAsync(Info.Filepath + CorrectName(Info.Filename));
                    if (File == null)
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
                        //throw new Exception();
                    IFolder Folder = await GetFolder(Info, eUser);
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

        public static async Task<IFile> FilePicker(List<string> lststrFileEndings)
        {
            var file = await CrossFilePicker.Current.PickFile();
            if (file == null)
            {
                throw new IsOKException();
            }
            else
            {
                return await FileSystem.Current.GetFileFromPathAsync(file.FileName);
            }
        }

        internal async static Task<IFolder> GetFolder(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser, FileNotFoundDecision eCreation = FileNotFoundDecision.Create)
        {
            IFolder ReturnFolder = null;
            try
            {
                ReturnFolder = await FileSystem.Current.GetFolderFromPathAsync(Info.Filepath);
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
                                var ParDirPath = Info.Filepath.Replace(new Uri(Info.Filepath).Segments.LastOrDefault(), "");
                                var ParentFolder = await FileSystem.Current.GetFolderFromPathAsync(ParDirPath);
                                ReturnFolder = await CreateFoldersRecursive(Info, ParentFolder);
                                break;
                            case Place.Roaming:
                                ReturnFolder = await CreateFoldersRecursive(Info, FileSystem.Current.RoamingStorage);
                                break;
                            case Place.Local:
                                ReturnFolder = await CreateFoldersRecursive(Info, FileSystem.Current.LocalStorage);
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

        static async Task<IFolder> CreateFoldersRecursive(FileInfoClass Info, IFolder StartFolder)
        {
            IFolder Folder;
            var path = Info.Filepath.Replace(StartFolder.Name, "");
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
        public static async Task<IFolder> FolderPicker()
        {
            var a = await CrossFilePicker.Current.PickFile();
            Uri uri = new Uri(a.FileName);
            var FolderName = a.FileName.Replace(uri.Segments.LastOrDefault(), ""); //TODO Check
            return await FileSystem.Current.GetFolderFromPathAsync(FolderName);
        }


        public async Task MoveAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            var TargetFolder = await GetFolder(Target, UserDecision.ThrowError);
            var SourceFolder = await GetFolder(Source, UserDecision.ThrowError);
            foreach (var item in await SourceFolder.GetFilesAsync())
            {
                if (FileTypes?.Contains(item.FileType()) != false)
                {
                    try
                    {
                        await item.MoveAsync(TargetFolder.Path + @"\" + item.Name, NameCollisionOption.GenerateUniqueName);
                    }
                    catch (Exception ex)
                    {
                        TAPPLICATION.Debugging.TraceException(ex, Target.ToString() + Source.ToString());
                    }
                }
                else
                {

                }
            }
        }


        public Task<bool> OpenFolder(FileInfoClass Info)
        {
            //TODO
            return default;
        }

        public async Task RemoveFile(FileInfoClass Info)
        {

            var x = await GetFile(Info);
            await x.DeleteAsync();
        }

        public async Task<FileInfoClass> Rename(FileInfoClass Source, string NewName)
        {
            var SourceFile = await GetFile(Source);
            await SourceFile.RenameAsync(NewName, NameCollisionOption.GenerateUniqueName);
            var Info = Source.Clone();
            Info.Filepath = SourceFile.Path.Remove(SourceFile.Path.Length - SourceFile.Name.Length);
            return Info;
        }

        public async Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser)
        {
            IFile x = await GetFile(Info, eUser: eUD);
            try
            {
                if (true)
                {
                    await x.WriteAllTextAsync(saveChar);
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
