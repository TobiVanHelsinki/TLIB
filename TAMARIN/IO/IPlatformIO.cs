using System.Collections.Generic;
using System.Threading.Tasks;

namespace TAMARIN.IO
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

    public enum Place
    {
        NotDefined = 0,
        Extern = 2,
        Roaming = 3,
        Local = 4,
        Assets = 5,
        Temp = 6
    }

    public interface IPlatformIO
    {
        /// <summary>
        /// Save a string to the specified target file
        /// Can throw
        /// </summary>
        /// <param name="saveChar"></param>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        Task<FileInfoClass> SaveFileContent(string saveChar, FileInfoClass Info, UserDecision eUD = UserDecision.AskUser);

        /// <summary>
        /// Remove a string from the specified target file
        /// Can throw
        /// </summary>
        /// <param name="delChar"></param>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        Task RemoveFile(FileInfoClass Info);

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
        Task<(string strFileContent, FileInfoClass Info)> LoadFileContent(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser);

        /// <summary>
        /// Can throw
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        Task<List<FileInfoClass>> GetListofFiles(FileInfoClass Info, UserDecision eUser, List<string> FileTypes = null);

        /// <summary>
        /// Copys a File to a Folder with the new Name (if provided in Target) (do not forget the fileextension!)
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        Task Copy(FileInfoClass Target, FileInfoClass Source);

        Task CopyAllFiles(FileInfoClass Target, FileInfoClass Source);
        
        Task<FileInfoClass> GetFolderInfo(FileInfoClass Info, UserDecision eUser = UserDecision.AskUser);

        Task<bool> OpenFolder(FileInfoClass Info);

    }
}
