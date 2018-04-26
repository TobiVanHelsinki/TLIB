using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAMARIN.IO;
using TAPPLICATION.Model;
using TLIB;

namespace TAPPLICATION.IO
{
    public enum SaveType
    {
        Unknown = 0,
        Manually = 1,
        Auto = 2,
        Emergency = 3,
        Temp = 4
    }
    public class SharedIO 
    {
        const string Prefix_Emergency = "EmergencySave_";

        public static IPlatformIO CurrentIO =
#if __ANDROID__
                new DroidIO();
#elif WINDOWS_UWP
                new UwpIO();
#endif

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

        public async static void SaveTextesToFiles(IEnumerable<(string Name, string Content)> FileContents, FileInfoClass FileInfo)
        {
            FileInfo = await CurrentIO.GetFolderInfo(FileInfo);
            foreach (var (Name, Content) in FileContents)
            {
                FileInfo.Filename = Name;
                try
                {
                    await CurrentIO.SaveFileContent(Content, FileInfo);
                }
                catch (Exception x)
                {
                    SharedAppModel.Instance.NewNotification("Writing Error", x);
                }
            }
        }

        #region Saving

        /// <summary>
        /// Can Throw
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="eSaveType"></param>
        /// <returns></returns>
        public static async Task<FileInfoClass> SaveAtOriginPlace(IMainType Object, SaveType eSaveType = SaveType.Unknown, UserDecision eUD = UserDecision.AskUser)
        {
            if (Object.FileInfo.Fileplace != Place.NotDefined && Object.FileInfo.Fileplace != Place.Temp)
            {
                return await Save(Object, eUD, eSaveType);
            }
            else
            {
                return await SaveAtCurrentPlace(Object, eSaveType, eUD);
            }
        }
        /// <summary>
        /// Can Throw
        /// </summary>
        /// <param name="strDelChar"></param>
        /// <returns></returns>
        public static async Task<FileInfoClass> SaveAtCurrentPlace(IMainType Object, SaveType eSaveType = SaveType.Unknown, UserDecision eUD = UserDecision.ThrowError)
        {
            Object.FileInfo.Fileplace = GetCurrentSavePlace();
            Object.FileInfo.Filepath = GetCurrentSavePath();
            return await Save(Object, eUD, eSaveType, Object.FileInfo);
        }
        public static async Task<FileInfoClass> SaveAtTempPlace(IMainType Object)
        {
            return await Save(Object, UserDecision.ThrowError, Info: new FileInfoClass() { Fileplace = Place.Temp, Filename = Object.FileInfo.Filename});
        }
        public static async Task<FileInfoClass> Save(IMainType Object, UserDecision eUD = UserDecision.AskUser, SaveType eSaveType = SaveType.Unknown, FileInfoClass Info = null)
        {
            if (Object == null)
            {
                throw new ArgumentNullException("Char was Empty");
            }
            FileInfoClass CurrentInfo = Info ?? Object.FileInfo;
            string strAdditionalName = "";
            switch (eSaveType)
            {
                case SaveType.Temp:
                    break;
                case SaveType.Emergency:
                    strAdditionalName = Prefix_Emergency;
                    break;
                case SaveType.Unknown:
                case SaveType.Manually:
                case SaveType.Auto:
                    break;
                default:
                    break;
            }
            CurrentInfo.Filename = Object.MakeName();
            if (!CurrentInfo.Filename.StartsWith(strAdditionalName))
            {
                CurrentInfo.Filename = strAdditionalName + CurrentInfo.Filename;
            }
            CurrentInfo.Filename = string.IsNullOrEmpty(CurrentInfo.Filename) ? "$$" : "" + CurrentInfo.Filename;
            if (CurrentInfo.Fileplace != Place.Extern && CurrentInfo.Fileplace != Place.Temp)
            {
                CurrentInfo.Fileplace = SharedSettingsModel.I.InternSync ? Place.Roaming : Place.Local;
            }
            return await CurrentIO.SaveFileContent(Serialize(Object), CurrentInfo, eUD);
        }

        #endregion
        #region Serialization
        public static void ErrorHandler(object o, Newtonsoft.Json.Serialization.ErrorEventArgs a)
        {
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
        public static string Serialize(IMainType SaveChar)
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

        public static CurrentType Deserialize(string fileContent)
        {
            JObject o = JObject.Parse(fileContent);
            string strAppVersion = o.Value<string>(SharedConstants.STRING_APP_VERSION_NUMBER);
            string strFileVersion = o.Value<string>(SharedConstants.STRING_FILE_VERSION_NUMBER);

            return MainTypeConvert(strAppVersion, strFileVersion, fileContent);
        }
        /// <summary>
        /// Can throw
        /// </summary>
        /// <param name="strFileVersion"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        internal static Func<string, string, string, CurrentType> MainTypeConvert =
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
            var File = await CurrentIO.LoadFileContent(Info, FileTypes, eUD);
            var NewMainObject = Deserialize(File.strFileContent);
            NewMainObject.FileInfo = File.Info;
            return NewMainObject;
        }
        #endregion

    }
}
