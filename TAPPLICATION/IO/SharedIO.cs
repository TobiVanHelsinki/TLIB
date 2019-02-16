using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPLICATION.Model;

namespace TAPPLICATION.IO
{

    /// <summary>
    /// Provides Basic IO for the Framework
    /// </summary>
    public class SharedIO 
    {
        public static IPlatformIO CurrentIO;

        public static DirectoryInfo CurrentSaveDir => new DirectoryInfo(CurrentSavePath);
        public static string CurrentSavePath
        {
            get
            {
                string ret;
                if (SharedSettingsModel.I.FOLDERMODE)
                {
                    ret = SharedSettingsModel.I.FOLDERMODE_PATH;
                }
                else
                {
                    var t = CurrentIO?.GetCompleteInternPath(CurrentSavePlace);
                    t.Wait();
                    ret = t.Result + SharedConstants.INTERN_SAVE_CONTAINER;
                }
                return ret.LastOrDefault() == Path.DirectorySeparatorChar ? ret : ret + Path.DirectorySeparatorChar;
            }
        }

        public static Place CurrentSavePlace { get { return SharedSettingsModel.I.FOLDERMODE ? Place.Extern : SharedSettingsModel.I.INTERN_SYNC ? Place.Roaming : Place.Local; } }

        /// <summary>
        /// Creates multiple files at the folder specified at info.
        /// </summary>
        /// <param name="FileContents">List of FileName and Content</param>
        /// <param name="Dir">Folder to save to</param>
        public async static void SaveTextesToFiles(IEnumerable<(string Name, string Content)> FileContents, DirectoryInfo Dir)
        {
            foreach (var (Name, Content) in FileContents)
            {
                try
                {
                    var f = new FileInfo(Path.Combine(Dir.FullName,Name));
                    await CurrentIO?.SaveFileContent(Content, f);
                }
                catch (Exception x)
                {
                    SharedAppModel.Instance?.NewNotification("Writing Error", x);
                }
            }
        }

        #region Saving

        /// <summary>
        /// Saves the MainType Object at the place, that is descriped at the Object. If saveplace is not defined or temp, it will be saved to current default location
        /// </summary>
        /// <param name="Object"></param>
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfo> The place where it is actually saved</returns>
        public static async Task<FileInfo> SaveAtOriginPlace(IMainType Object)
        {
            if (Object.FileInfo.Directory.FullName.Contains(await CurrentIO.GetCompleteInternPath(Place.Temp))
                || Object.FileInfo.Directory.FullName.Contains(await CurrentIO.GetCompleteInternPath(Place.Assets))) 
            {
                return await SaveAtCurrentPlace(Object);
            }
            else
            {
                return await Save(Object);
            }
        }

        /// <summary>
        /// Saves the MainType Object to current default location
        /// </summary>
        /// <param name="Object"></param>
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfo> The place where it is actually saved</returns>
        public static async Task<FileInfo> SaveAtCurrentPlace(IMainType Object)
        {
            Object.FileInfo = await Save(Object, new FileInfo(Path.Combine(CurrentSavePath, Object.FileInfo.Name)));
            return Object.FileInfo;
        }

        /// <summary>
        /// Saves the MainType Object to current tmp location
        /// </summary>
        /// <param name="Object"></param>
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfo> The place where it is actually saved</returns>
        public static async Task<FileInfo> SaveAtTempPlace(IMainType Object)
        {
            string path = await CurrentIO?.GetCompleteInternPath(Place.Temp);
            return await Save(Object, new FileInfo(Path.Combine(path, Object.FileInfo.Name)));
        }
        
        /// <summary>
        /// Saves the Object to the specified location at "info" or if null to the info at the object
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="Info"></param>
        /// 
        /// 
        /// <exception cref="Exception"/>
        /// <returns>Task<FileInfo> The place where it is actually saved</returns>
        public static async Task<FileInfo> Save(IMainType Object, FileInfo Info = null)
        {
            if (Object == null)
            {
                throw new ArgumentNullException("MainObject was Empty");
            }
            System.Diagnostics.Debug.WriteLine("Saving" + Object.ToString());
            var InfoToUse = Info ?? Object.FileInfo;
            await CurrentIO?.SaveFileContent(Serialize(Object), InfoToUse);
            return InfoToUse;
        }

        #endregion
        /// <summary>
        /// Default Error Handler. Gives a notification and set .Handled = true 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="a"></param>
        public static void ErrorHandler(object o, Newtonsoft.Json.Serialization.ErrorEventArgs a)
        {
            if (SharedAppModel.Instance?.lstNotifications.Contains(JSON_Error_Notification) == false)
            {
                SharedAppModel.Instance?.lstNotifications.Insert(0, JSON_Error_Notification);
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
                    _JSON_Error_Notification = new Notification("There was an Error during deserialization. Your content migth be not complete.");
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
        /// <param name="eUD"></param>
        /// <exception cref="Exception"/>
        /// <returns></returns>
        public static async Task<CurrentType> Load(FileInfo Info)
        {
            var FileContent = await CurrentIO?.LoadFileContent(Info);
            var NewMainObject = Deserialize(FileContent);
            NewMainObject.FileInfo = Info;
            return NewMainObject;
        }
        #endregion

    }
}
