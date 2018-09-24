using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLIB.IO;

namespace TLIB.Code.Android
{
    internal class IO : IPlatformIO
    {
        public async Task Copy(FileInfoClass Target, FileInfoClass Source)
        {
        }

        public void CreateSaveContainer()
        {
            
        }

        public string GetCompleteInternPath(Place place)
        {
            return "";   
        }

        public async Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser)
        {
            return new FileInfoClass();
        }

        public async Task<List<FileInfoClass>> GetListofFiles(FileInfoClass Info, UserDecision eUser, List<string> FileTypes = null)
        {
            return new List<FileInfoClass>();
        }

        public async Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            return ( "", new FileInfoClass());
        }

        public async Task<bool> OpenFolder(FileInfoClass Info)
        {
            return false;   
        }

        public async Task RemoveFile(FileInfoClass Info)
        {
        }

        public async Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser)
        {
            return new FileInfoClass();
        }

        public async Task MoveAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            
        }

        public async Task<FileInfoClass> Rename(FileInfoClass SourceFile, string NewName)
        {
            return new FileInfoClass();
        }

        public async Task CopyAllFiles(FileInfoClass Target, FileInfoClass Source, IEnumerable<string> FileTypes = null)
        {
            
        }
    }
}