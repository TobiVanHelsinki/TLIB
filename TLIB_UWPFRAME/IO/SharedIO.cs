using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLIB;
using TLIB_UWPFRAME.Model;

namespace TLIB_UWPFRAME.IO
{
    public enum Place
    {
        NotDefined = 0,
        Extern = 2,
        Roaming = 3,
        Local = 4,
        Assets = 5
    }

    public enum SaveType
    {
        Unknown = 0,
        Manually = 1,
        Auto = 2,
        Emergency = 3
    }
    public class SharedIO 
    {

        //#####################################################################
        internal static string GetCurrentSavePath()
        {
            if (SharedSettingsModel.I.ORDNERMODE)
            {
                return SharedSettingsModel.I.ORDNERMODE_PFAD;
            }
            else
            {
                return SharedConstants.INTERN_SAVE_CONTAINER;
            }

        }

        internal static Place GetCurrentSavePlace()
        {
            if (SharedSettingsModel.I.ORDNERMODE)
            {
                return Place.Extern;
            }
            else
            {
                if (SharedSettingsModel.I.InternSync)
                {
                    return Place.Roaming;
                }
                else
                {
                    return Place.Local;
                }

            }
        }

        protected static IGeneralIO GetIO()
        {
            return
#if __ANDROID__
                new DroidIO();
#else
                new WinIO();
#endif
        }

        public static async Task<List<FileInfoClass>> GetListOfFiles(FileInfoClass Info, UserDecision eUD = UserDecision.AskUser, List<string> FileTypes = null)
        {
            return await GetIO().GetListofFiles(Info, eUD, FileTypes);
        }

        public async static Task CopyLocalRoaming(Place NewTarget = Place.NotDefined)
        {
            if (NewTarget == Place.NotDefined)
            {
                NewTarget = GetCurrentSavePlace();
            }

            await GetIO().CopyLocalRoaming(NewTarget, SharedConstants.INTERN_SAVE_CONTAINER);
        }

        public async static void SaveTextesToFiles(IEnumerable<(string Name, string Content)> FileContents, FileInfoClass FileInfo)
        {
            FileInfo = await GetIO().GetFolderInfo(FileInfo);
            foreach (var (Name, Content) in FileContents)
            {
                FileInfo.Filename = Name;
                try
                {
                    await GetIO().SaveFileContent(Content, FileInfo);
                }
                catch (Exception x)
                {
                    SharedAppModel.Instance.NewNotification("Writing Error", x);
                }
            }
        }

        public async static void SaveTextToFile(FileInfoClass FileInfo, string Content)
        {
            await GetIO().SaveFileContent(Content, FileInfo);
        }

        public static async Task<string> ReadTextFromFile(FileInfoClass FileInfo, List<string> lST_FILETYPES_CSV, UserDecision askUser)
        {
            var res = await GetIO().LoadFileContent(FileInfo, lST_FILETYPES_CSV, askUser);
            return res.strFileContent;
        }
        #region Saving

        /// <summary>
        /// Can Throw
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="eSaveType"></param>
        /// <returns></returns>
        public static async Task SaveAtOriginPlace(IMainType Object, SaveType eSaveType = SaveType.Unknown, UserDecision eUD = UserDecision.AskUser)
        {
            if (Object.FileInfo.Fileplace != Place.NotDefined)
            {
                await Save(Object, eUD, eSaveType: eSaveType);
            }
            else
            {
                await SaveAtCurrentPlace(Object, eUD, eSaveType: eSaveType);
            }
        }
        /// <summary>
        /// Can Throw
        /// </summary>
        /// <param name="strDelChar"></param>
        /// <returns></returns>
        public static async Task SaveAtCurrentPlace(IMainType Object, UserDecision eUD = UserDecision.ThrowError, SaveType eSaveType = SaveType.Unknown)
        {
            Object.FileInfo.Fileplace = GetCurrentSavePlace();
            Object.FileInfo.Filepath = GetCurrentSavePath();
            await Save(Object, eUD, Object.FileInfo);
        }
        /// <summary>
        /// Can Throw
        /// </summary>
        /// <param name="strDelChar"></param>
        /// <returns></returns>
        public async static Task RemoveAtCurrentPlace(string strDelChar)
        {
            await GetIO().RemoveFile(new FileInfoClass() { Fileplace = GetCurrentSavePlace(), Filepath = GetCurrentSavePath(), Filename = strDelChar });
        }
        public async static Task Remove(FileInfoClass Info)
        {
            await GetIO().RemoveFile(Info);
        }
        public static async Task Save(IMainType Object, UserDecision eUD = UserDecision.AskUser, FileInfoClass Info = null, SaveType eSaveType = SaveType.Unknown)
        {
            if (Object == null)
            {
                throw new ArgumentNullException("Char was Empty");
            }
            string strAdditionalName = "";
            switch (eSaveType)
            {
                case SaveType.Unknown:
                    break;
                case SaveType.Manually:
                    break;
                case SaveType.Auto:
                    strAdditionalName = "AutoSave_";
                    break;
                case SaveType.Emergency:
                    strAdditionalName = "EmergencySave_";
                    break;
                default:
                    break;
            }
            FileInfoClass CurrentInfo = Info ?? Object.FileInfo;
            CurrentInfo.Filename = Object.MakeName();
            if (!CurrentInfo.Filename.StartsWith(strAdditionalName))
            {
                CurrentInfo.Filename = strAdditionalName + CurrentInfo.Filename;
            }
            CurrentInfo.Filename = string.IsNullOrEmpty(CurrentInfo.Filename) ? "$$" : "" + CurrentInfo.Filename;
            await GetIO().SaveFileContent(Serialize(Object), CurrentInfo, eUD);
        }

        #endregion
        #region Serialization
        protected static void ErrorHandler(object o, Newtonsoft.Json.Serialization.ErrorEventArgs a)
        {
            //#if DEBUG
            //if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            //#endif
            //            SharedAppModel.Instance.NewNotification(
            //                CrossPlatformHelper.GetString("Notification_Error_Loader_Error1/Text") +
            //                "ErrorContextData: " + a.ErrorContext.Error.Message +
            //                "ErrorContextData: " + a.ErrorContext.Error.Data +
            //                "CurrentObject: " + a.CurrentObject +
            //                "OriginalObject: " + a.ErrorContext.OriginalObject
            //#if __ANDROID__
            //#else
            //                + "Path: " + a.ErrorContext.Path
            //#endif
            //                );
            if (!SharedAppModel.Instance.lstNotifications.Contains(JSON_Error_Notification))
            {
                SharedAppModel.Instance.NewNotification(JSON_Error_Notification);
            }
            a.ErrorContext.Handled = true;
        }
        /// <summary>
        /// can throw
        /// </summary>
        /// <param name="SaveChar"></param>
        /// <returns></returns>
        protected static string Serialize(IMainType SaveChar)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                //settings.NullValueHandling = NullValueHandling.Include; 
#if DEBUG
                Formatting = Formatting.Indented,
#endif
                PreserveReferencesHandling = PreserveReferencesHandling.All, //war vorher objects
                Error = ErrorHandler
            };
#if __ANDROID__
            throw new NotImplementedException();
            //return JsonConvert.SerializeObject(SaveChar, null, settings);
#else
            return JsonConvert.SerializeObject(SaveChar, settings);
#endif
        }
        static Notification JSON_Error_Notification = new Notification(StringHelper.GetString("Notification_Error_Loader_Error1/Text"));

        #endregion
    }

    public class SharedIO<CurrentType> : SharedIO where CurrentType : IMainType, new()
    {
        #region Deserialization

        protected static CurrentType Deserialize(string fileContent)
        {
            string strAppVersion = "";
            string strFileVersion = "";

            int nAppVersionPos = fileContent.IndexOf(SharedConstants.STRING_APP_VERSION_NUMBER);
            strAppVersion = fileContent.Substring(nAppVersionPos + SharedConstants.STRING_APP_VERSION_NUMBER.Length + SharedConstants.JSON_FILE_GAP, SharedConstants.STRING_VERSION_LENGTH);

            int nFileVersionPos = fileContent.IndexOf(SharedConstants.STRING_FILE_VERSION_NUMBER);
            strFileVersion = fileContent.Substring(nFileVersionPos + SharedConstants.STRING_FILE_VERSION_NUMBER.Length + SharedConstants.JSON_FILE_GAP, SharedConstants.STRING_VERSION_LENGTH);

            return (CurrentType)(new CurrentType().Converter ?? STDConvert)(strAppVersion, strFileVersion, fileContent);
        }
        /// <summary>
        /// Can throw
        /// </summary>
        /// <param name="strFileVersion"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        static Func<string, string, string, IMainType> STDConvert =
            (string strFileVersion, string strAppVersion, string fileContent) =>
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    Error = ErrorHandler,
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                };
                return JsonConvert.DeserializeObject<CurrentType>(fileContent, settings);
            };

        #endregion
        #region Loading

        public static async Task<CurrentType> LoadAtCurrentPlace(string strLoadChar, UserDecision eUD = UserDecision.AskUser)
        {
            return await Load(new FileInfoClass() { Fileplace = GetCurrentSavePlace(), Filepath = GetCurrentSavePath(), Filename = strLoadChar, FolderToken = SharedConstants.ACCESSTOKEN_FOLDERMODE}, null, eUD);
        }

        /// <summary>
        /// Can throw
        /// </summary>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        /// <param name="FileTypes"></param>
        /// <param name="eUD"></param>
        /// <returns></returns>
        public static async Task<CurrentType> Load(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            var File = await GetIO().LoadFileContent(Info, FileTypes, eUD);
            var NewMainObject = Deserialize(File.strFileContent);
            NewMainObject.FileInfo = File.Info;
            return NewMainObject;
        }
        #endregion

    }
}
