using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TLIB
{
    public enum UserDecision
    {
        AskUser = 0,
        ThrowError = 1
    }

    public enum FileNotFoundDecision
    {
        NotCreate,
        Create,
    }

    public interface IPlatformIO
    {
        /// <summary>
        /// returns a system wide valid path to the given place
        /// </summary>
        /// <param name="relativ_local_path"></param>
        /// <returns></returns>
        Task<string> GetCompleteInternPath(Place place);

        /// <summary>
        /// Save a string to the specified target file
        /// Can throw
        /// </summary>
        /// <param name="saveChar"></param>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        Task SaveFileContent(string saveChar, FileInfo Info);

        /// <summary>
        /// Remove a string from the specified target file
        /// Can throw
        /// </summary>
        /// <param name="delChar"></param>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        Task RemoveFile(FileInfo Info);

        /// <summary>
        /// Load the specified target file and returns the content and filename
        /// Can throw
        /// </summary>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        /// <param name="FileTypes"></param>
        /// <param name="eUD"></param>
        /// <returns></returns>
        Task<string> LoadFileContent(FileInfo Info);

        /// <summary>
        /// Can throw
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        Task<IEnumerable<FileInfo>> GetListofFiles(DirectoryInfo Info, IEnumerable<string> FileTypes = null);

        /// <summary>
        /// Copys a File to a Folder with the new Name (if provided in Target) (do not forget the fileextension!)
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        Task<FileInfo> CopyTo(FileInfo Target, FileInfo Source);
        Task<FileInfo> Rename(FileInfo SourceFile, string NewName);

        Task MoveAllFiles(DirectoryInfo Target, DirectoryInfo Source, IEnumerable<string> FileTypes = null);
        Task CopyAllFiles(DirectoryInfo Target, DirectoryInfo Source, IEnumerable<string> FileTypes = null);
        
        //Task<FileInfo> GetFolderInfo(FileInfo Info, UserDecision eUser = UserDecision.AskUser);
        //Task<FileInfo> GetFileInfo(FileInfo Info, UserDecision eUser = UserDecision.AskUser);

        Task<bool> OpenFolder(DirectoryInfo Info);
        void CreateSaveContainer();

        Task<DirectoryInfo> FolderPicker();
        Task<FileInfo> FilePicker(IEnumerable<string> lststrFileEndings);

        Task<bool> GetAccess(DirectoryInfo Info);
        Task<bool> GetAccess(FileInfo Info);

    }
}
