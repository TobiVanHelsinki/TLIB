using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLIB.IO
{
    internal class DroidIO : IGeneralIO
    {
        public Task Copy(FileInfoClass Target, FileInfoClass Source, string newName = null)
        {
            throw new NotImplementedException();
        }

        public Task CopyLocalRoaming(Place Target, string Path)
        {
            throw new NotImplementedException();
        }

        public Task<FileInfoClass> GetFileInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
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

        public Task RemoveFile(FileInfoClass Info)
        {
            throw new NotImplementedException();
        }

        public Task SaveFileContent(string saveChar, FileInfoClass Info)
        {
            throw new NotImplementedException();
        }
    }
}