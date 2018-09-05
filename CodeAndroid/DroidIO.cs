using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLIB.Code.Android
{
    internal class DroidIO : IPlatformIO
    {
        public Task Copy(FileInfoClass Target, FileInfoClass Source)
        {
            throw new NotImplementedException();
        }

        public void CreateSaveContainer()
        {
            throw new NotImplementedException();
        }

        public string GetCompleteInternPath(Place place)
        {
            throw new NotImplementedException();
        }

        public Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            throw new NotImplementedException();
        }

        public Task<List<FileInfoClass>> GetListofFiles(FileInfoClass Info, UserDecision eUser, List<string> FileTypes = null)
        {
            throw new NotImplementedException();
        }

        public Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenFolder(FileInfoClass Info)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFile(FileInfoClass Info)
        {
            throw new NotImplementedException();
        }

        public Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser)
        {
            throw new NotImplementedException();
        }

        public Task MoveAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            throw new NotImplementedException();
        }

        public Task<FileInfoClass> Rename(FileInfoClass SourceFile, string NewName)
        {
            throw new NotImplementedException();
        }

        public Task CopyAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            throw new NotImplementedException();
        }
    }
}