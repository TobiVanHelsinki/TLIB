using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLIB.IO;

namespace TAPPLICATION_Xamarin
{
    internal class IO : IPlatformIO
    {
        public Task Copy(FileInfoClass Target, FileInfoClass Source)
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

        public Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            return default;
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
    }
}
