using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPLICATION_UWP
{
    public class StandardIO
    {
        public virtual async Task<IEnumerable<FileInfo>> GetListofFiles(DirectoryInfo Info, IEnumerable<string> FileTypes = null)
        {
            //List<FileInfoClass> ReturnList = new List<FileInfoClass>();
            var Liste = Info.GetFiles();
            //if (FileTypes == null || FileTypes.Count == 0)
            //{
            //    FileTypes = new List<string>
            //    {
            //        "."
            //    };
            //}
            return Liste.Where(x => FileTypes.Contains(x.Extension));
            //foreach (var item in Liste.Where(x=> FileTypes.Contains (x.Extension)))
            //{
            //    //BasicProperties props = await item.GetBasicPropertiesAsync();
            //    ReturnList.Add(new FileInfoClass(Place.NotDefined, item.Name, Info.FullName));
            //}
            //return ReturnList;
        }

        public virtual async Task SaveFileContent(string saveChar, FileInfo Info)
        {
            File.WriteAllText(Info.FullName, saveChar);
            //StorageFile x = await GetFile(Info, eUser:eUD);

            //try
            //{

            //    if (true)
            //    {
            //        await FileIO.WriteTextAsync(x, saveChar);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Writingerror", ex);
            //}
            //FileInfoClass i = Info.Clone();
            //i.Name = x.Name;
            //i.Filepath = x.Path.Remove(x.Path.Length - x.Name.Length, x.Name.Length);
            //return i;
        }

        public virtual async Task<string> LoadFileContent(FileInfo Info)
        {
            return File.ReadAllText(Info.FullName);
            //StorageFile x = await GetFile(Info, FileTypes, eUD, FileNotFoundDecision.NotCreate);
            //var rettext = await FileIO.ReadTextAsync(x);
            //var info = Info.Clone();
            //info.Name = x.Name;
            //info.Filepath = x.Path.Substring(0, x.Path.Length - x.Name.Length);
            //return (rettext, info);
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

        public virtual async Task RemoveFile(FileInfo Info)
        {
            Info.Delete();
            //StorageFile x = await GetFile(Info);
            //await x.DeleteAsync();
        }

        public virtual async Task<FileInfo> Rename(FileInfo Source, string NewName)
        {
            string fileName = Source.Directory.FullName + NewName;
            Source.MoveTo(fileName);
            return new FileInfo(fileName);
            //Source.SystemFileInfo.MoveTo
            //StorageFile SourceFile = await GetFile(Source);
            //await SourceFile.RenameAsync(NewName, NameCollisionOption.GenerateUniqueName);
            //var Info = Source.Clone();
            //Info.Filepath = SourceFile.Path.Remove(SourceFile.Path.Length - SourceFile.Name.Length);
            //return Info;
        }

        /// <summary>
        /// simples kopieren, bei fehlern wird abgebrochen, dateien werden ueberschrieben
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public virtual async Task<FileInfo> CopyTo(FileInfo Source, FileInfo Target)
        {
            //StorageFile SourceFile = await GetFile(Source, eUser: UD);
            //StorageFolder TargetFolder = await GetFolder(Target, UD);
            return Source.CopyTo(Target.FullName, true);
            //await SourceFile.CopyAsync(TargetFolder, Target.Name ?? SourceFile.Name, NameCollisionOption.ReplaceExisting);
        }

        public virtual async Task CopyAllFiles(DirectoryInfo Source, DirectoryInfo Target, IEnumerable<string> FileTypes = null)
        {
            //StorageFolder TargetFolder = await GetFolder(Target);
            //StorageFolder SourceFolder = await GetFolder(Source);
            foreach (var item in Source.GetFiles())
            {
                if (FileTypes?.Contains(item.Extension) != false)
                {
                    try
                    {
                        item.CopyTo(Target.FullName, true);
                    }
                    catch (Exception ex)
                    {
                        TAPPLICATION.Debugging.TraceException(ex, Target.ToString() + Source.ToString());
                    }
                }
            }
        }

        public virtual async Task MoveAllFiles(DirectoryInfo Source, DirectoryInfo Target, IEnumerable<string> FileTypes = null)
        {
            //StorageFolder TargetFolder = await GetFolder(Target, UserDecision.ThrowError);
            //StorageFolder SourceFolder = await GetFolder(Source, UserDecision.ThrowError);
            foreach (var item in Source.GetFiles())
            {
                if (FileTypes?.Contains(item.Extension) != false)
                {
                    try
                    {
                        item.MoveTo(Target.FullName + item.Name);
                    }
                    catch (Exception ex)
                    {
                        TAPPLICATION.Debugging.TraceException(ex, Target.ToString() + Source.ToString());
                    }
                }
            }
        }

        public virtual async Task<DirectoryInfo> CreateFoldersRecursive(DirectoryInfo Info, DirectoryInfo StartFolder)
        {
            DirectoryInfo Folder;
            var path = Info.FullName.Replace(StartFolder.FullName, "");
            var folders = path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            Folder = StartFolder;
            foreach (var item in folders)
            {
                Folder = Folder.CreateSubdirectory(item);
            }

            return Folder;
        }

    }
}
