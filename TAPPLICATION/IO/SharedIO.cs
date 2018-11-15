using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TAPPLICATION.Model;
using TLIB.IO;
using TLIB.PlatformHelper;

namespace TAPPLICATION.IO
{

    /// <summary>
    /// Provides Basic IO for the Framework
    /// </summary>
    public class SharedIO 
    {
        const string Prefix_Emergency = "EmergencySave_";

        public static IPlatformIO CurrentIO;

        /// <summary>
        /// returns the current default save path
        /// </summary>
        public static string GetCurrentSavePath()
        {
            if (SharedSettingsModel.I.FOLDERMODE)
            {
                return SharedSettingsModel.I.FOLDERMODE_PATH;
            }
            else
            {
                return CurrentIO.GetCompleteInternPath(GetCurrentSavePlace()) + SharedConstants.INTERN_SAVE_CONTAINER + @"\";
            }
        }

        /// <summary>
        /// returns the current default save place
        /// </summary>
        public static Place GetCurrentSavePlace()
        {
            if (SharedSettingsModel.I.FOLDERMODE)
            {
                return Place.Extern;
            }
            else
            {
                if (SharedSettingsModel.I.INTERN_SYNC)
                {
                    return Place.Roaming;
                }
                else
                {
                    return Place.Local;
                }

            }
        }

        /// <summary>
        /// Creates multiple files at the folder specified at info.
        /// </summary>
        /// <param name="FileContents">List of FileName and Content</param>
        /// <param name="FileInfo">Folder to save to</param>
        public async static void SaveTextesToFiles(IEnumerable<(string Name, string Content)> FileContents, FileInfoClass FileInfo)
        {
            FileInfo = await CurrentIO.GetFolderInfo(FileInfo, UserDecision.AskUser);
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
        /// Saves the MainType Object at the place, that is descriped at the Object. If saveplace is not defined or temp, it will be saved to current default location
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="eUD"></param>
        /// 
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfoClass> The place where it is actually saved</returns>
        public static async Task<FileInfoClass> SaveAtOriginPlace(IMainType Object, UserDecision eUD = UserDecision.AskUser)
        {
            if (Object.FileInfo.Fileplace != Place.NotDefined 
                && Object.FileInfo.Fileplace != Place.Temp
                && Object.FileInfo.Fileplace != Place.Assets)
            {
                return await Save(Object, eUD);
            }
            else
            {
                return await SaveAtCurrentPlace(Object, eUD);
            }
        }

        /// <summary>
        /// Saves the MainType Object to current default location
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="eUD"></param>
        /// 
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfoClass> The place where it is actually saved</returns>
        public static async Task<FileInfoClass> SaveAtCurrentPlace(IMainType Object, UserDecision eUD = UserDecision.ThrowError)
        {
            Object.FileInfo.Fileplace = GetCurrentSavePlace();
            Object.FileInfo.Filepath = GetCurrentSavePath();
            return await Save(Object, eUD, Object.FileInfo);
        }

        /// <summary>
        /// Saves the MainType Object to current tmp location
        /// </summary>
        /// <param name="Object"></param>
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfoClass> The place where it is actually saved</returns>
        public static async Task<FileInfoClass> SaveAtTempPlace(IMainType Object)
        {
            return await Save(Object, UserDecision.ThrowError, Info: new FileInfoClass(Place.Temp, Object.FileInfo.Filename, CurrentIO.GetCompleteInternPath(Place.Temp)));
        }

        static Mutex mut = new Mutex();
        /// <summary>
        /// Saves the Object to the specified location at "info" or if null to the info at the object
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="eUD"></param>
        /// <param name="Info"></param>
        /// 
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfoClass> The place where it is actually saved</returns>
        public static async Task<FileInfoClass> Save(IMainType Object, UserDecision eUD = UserDecision.AskUser, FileInfoClass Info = null)
        {
            if (Object == null)
            {
                throw new ArgumentNullException("MainObject was Empty");
            }
            System.Diagnostics.Debug.WriteLine("Saving" + Object.ToString());
            var retinfo = await CurrentIO.SaveFileContent(Serialize(Object), Info ?? Object.FileInfo, eUD);
            return retinfo;
        }

        #endregion
        /// <summary>
        /// Default Error Handler. Gives a notification and set .Handled = true 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="a"></param>
        public static void ErrorHandler(object o, Newtonsoft.Json.Serialization.ErrorEventArgs a)
        {
            if (!SharedAppModel.Instance.lstNotifications.Contains(JSON_Error_Notification))
            {
                SharedAppModel.Instance.lstNotifications.Insert(0, JSON_Error_Notification);
            }
            JSON_Error_Notification.Message += "\n\t" + a.ErrorContext.Path;
            a.ErrorContext.Handled = true;
        }

        #region Serialization
        /// <summary>
        /// can throw
        /// </summary>
        /// <param name="Object to serialize"></param>
        /// <returns></returns>
        public static string Serialize(IMainType ObjectToSerialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Error = ErrorHandler
            };
            return JsonConvert.SerializeObject(ObjectToSerialize, settings);
        }
        static Notification _JSON_Error_Notification;
        static Notification JSON_Error_Notification
        {
            get
            {
                if (_JSON_Error_Notification == null)
                {
                    _JSON_Error_Notification = new Notification(StringHelper.GetString("Notification_Error_Loader_Error1/Text"));
                }
                return _JSON_Error_Notification;
            }
        }

        public static string CorrectFilenameExtension(string Filename, string Extension)
        {
            if (!Filename.EndsWith(Extension))
            {
                Filename += Extension;
            }
            return Filename;
        }

        #endregion
    }

    /// <summary>
    /// Provides MainType Specific IO for the framework. 
    /// Use this class to derive from in your application
    /// </summary>
    /// <typeparam name="CurrentType"></typeparam>
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
        /// Converter, that does to actual deserialization, you can provide your own by setting this var
        /// </summary>
        /// <param name="strFileVersion"></param>
        /// <param name="fileContent"></param>
        /// <exception cref="Exception"/>
        /// <returns></returns>
        public static Func<string, string, string, CurrentType> MainTypeConvert =
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
        /// gets file and deserialize it
        /// </summary>
        /// <param name="ePlace"></param>
        /// <param name="strSaveName"></param>
        /// <param name="strSavePath"></param>
        /// <param name="FileTypes"></param>
        /// <param name="eUD"></param>
        /// <exception cref="Exception"/>
        /// <returns></returns>
        public static async Task<CurrentType> Load(FileInfoClass Info, List<string> FileTypes = null, UserDecision eUD = UserDecision.AskUser)
        {
            var A = DateTimeOffset.Now;
            var File = await CurrentIO.LoadFileContent(Info, FileTypes, eUD);
            var B = DateTimeOffset.Now;
            var NewMainObject = Deserialize(File.strFileContent);
            var C = DateTimeOffset.Now;
            NewMainObject.FileInfo = File.Info;
            if (SharedSettingsModel.I.DEBUG_FEATURES)
            {
                SharedAppModel.Instance.NewNotification("LoadTime:\nFileloading\t" + (B - A).ToString()+"\nDeserialize\t" + (C - B).ToString());
            }
            return NewMainObject;
        }
        #endregion

    }
}
