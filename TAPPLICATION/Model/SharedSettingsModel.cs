using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using TLIB;
using TLIB.Settings;
using TLIB.IO;

namespace TAPPLICATION.Model
{
    public class SharedSettingsModel : INotifyPropertyChanged
    {
        IEnumerable<PropertyInfo> Settings => ReflectionHelper.GetProperties(this, typeof(SettingAttribute));
        //public Type UsedConstants { get; set; }

        #region Attributes

        protected enum SaveType
        {
            Roaming, Local, Nothing
        }
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        protected sealed class SettingAttribute : Attribute
        {
            public string SaveString;
            public object DefaultValue;
            public SaveType Sync;
            public Type DeviatingType;

            public SettingAttribute(string saveString, object defaultValue, SaveType sync, Type deviatingType = null)
            {
                SaveString = saveString;
                DefaultValue = defaultValue;
                Sync = sync;
                DeviatingType = deviatingType;
            }
        }
        #endregion

        #region Generic Set / Get
        public static IPlatformSettings PlatformSettings;
        protected dynamic Get([CallerMemberName] string Name = "")
        {
            var Setting = Settings?.FirstOrDefault(x => x.Name == Name);
            var Attribute = Setting?.GetCustomAttribute<SettingAttribute>(true);
            switch (Attribute?.Sync)
            {
                case SaveType.Roaming:
                    switch (Attribute.DeviatingType ?? Setting.PropertyType)
                    {
                        case Type namedType when namedType == typeof(int):
                            return PlatformSettings.GetIntRoaming(Attribute.SaveString);
                        case Type namedType when namedType == typeof(bool):
                            return PlatformSettings.GetBoolRoaming(Attribute.SaveString);
                        case Type namedType when namedType == typeof(string):
                            return PlatformSettings.GetStringRoaming(Attribute.SaveString);
                        default:
                            return Attribute.DefaultValue;
                    }
                case SaveType.Local:
                    switch (Attribute.DeviatingType ?? Setting.PropertyType)
                    {
                        case Type namedType when namedType == typeof(int):
                            return PlatformSettings.GetIntLocal(Attribute.SaveString);
                        case Type namedType when namedType == typeof(bool):
                            return PlatformSettings.GetBoolLocal(Attribute.SaveString);
                        case Type namedType when namedType == typeof(string):
                            return PlatformSettings.GetStringLocal(Attribute.SaveString);
                        default:
                            return Attribute.DefaultValue;
                    }
                default:
                    return Attribute.DefaultValue;
            }
        }
        protected void Set(object value, [CallerMemberName] string Name = "")
        {
            var Setting = Settings?.FirstOrDefault(x => x.Name == Name);
            var Attribute = Setting?.GetCustomAttribute<SettingAttribute>(true);
            if (Attribute.DeviatingType != null)
            {

            }
            value = Attribute.DeviatingType == null ? value : Convert.ChangeType(value, Attribute.DeviatingType);
            switch (Attribute.Sync)
            {
                case SaveType.Roaming:
                    PlatformSettings.SetRoaming(Attribute.SaveString, value);
                    break;
                case SaveType.Local:
                    PlatformSettings.SetLocal(Attribute.SaveString, value);
                    break;
                default:
                    break;
            }
            Instance.NotifyPropertyChanged(Name);
        }
        #endregion

        #region Settings

        [Setting("SETTINGS_INTERN_SYNC", true, SaveType.Local)]
        public bool INTERN_SYNC { get => Get(); set => Set(value); }

        [Setting("SETTINGS_DEBUG_FEATURES", false, SaveType.Local)]
        public bool DEBUG_FEATURES { get => Get(); set => Set(value); }

        [Setting("SETTINGS_BETA_FEATURES", false, SaveType.Local)]
        public bool BETA_FEATURES { get => Get(); set => Set(value); }

        [Setting("SETTINGS_DISPLAY_REQUEST", true, SaveType.Local)]
        public bool DISPLAY_REQUEST { get => Get(); set => Set(value); }

        [Setting("SETTINGS_FOLDERMODE", false, SaveType.Local)]
        public bool FOLDERMODE { get => Get(); set => Set(value); }

        [Setting("SETTINGS_FOLDERMODE_PATH", "", SaveType.Local)]
        public string FOLDERMODE_PATH { get => Get(); set => Set(value); }

        [Setting("LAST_SAVE_INFO", null, SaveType.Nothing)]
        public FileInfoClass LAST_SAVE_INFO
        {
            get
            {
                return new FileInfoClass()
                {
                    Filename = LAST_SAVE_INFO_NAME,
                    Filepath = LAST_SAVE_INFO_PATH,
                    Fileplace = LAST_SAVE_INFO_PLACE,
                    Token = LAST_SAVE_INFO_TOKEN
                };
            }
            set
            {
                if (value == null)
                {
                    LAST_SAVE_INFO_NAME = "";
                    LAST_SAVE_INFO_PATH = "";
                    LAST_SAVE_INFO_PLACE = Place.NotDefined;
                    LAST_SAVE_INFO_TOKEN = "";
                }
                else
                {
                    LAST_SAVE_INFO_NAME = value.Filename;
                    LAST_SAVE_INFO_PATH = value.Filepath;
                    LAST_SAVE_INFO_PLACE = value.Fileplace;
                    LAST_SAVE_INFO_TOKEN = value.Token;
                }
                Instance.NotifyPropertyChanged();
            }
        }

        [Setting("SETTINGS_LAST_CHAR_NAME", "", SaveType.Local)]
        public string LAST_SAVE_INFO_NAME { get => Get(); set => Set(value); }

        [Setting("SETTINGS_LAST_SAVE_PATH", "", SaveType.Local)]
        public string LAST_SAVE_INFO_PATH { get => Get(); set => Set(value); }

        [Setting("SETTINGS_LAST_SAVE_PLACE", Place.NotDefined, SaveType.Local, typeof(int))]
        public Place LAST_SAVE_INFO_PLACE { get => (Place)Get(); set => Set(value); }

        [Setting("SETTINGS_LAST_SAVE_TOKEN", "", SaveType.Local)]
        public string LAST_SAVE_INFO_TOKEN { get => Get(); set => Set(value); }

        #endregion
        #region Methods
        public List<(string, object)> ExportAllSettings()
        {
            var propertyinfos = this.GetType().GetRuntimeProperties();
            var ret = new List<(string, object)>();
            foreach (var item in propertyinfos)
            {
                object result = item.GetValue(this);
                ret.Add((item.Name, result));
            }
            return ret;
        }

        public void InitSettings()
        {
            foreach (var SettingInfo in Settings)
            {
                try
                {
                    var Attribute = SettingInfo?.GetCustomAttribute<SettingAttribute>(true);
                    SettingInfo.SetMethod.Invoke(this, new object[] { Attribute.DefaultValue });
                }
                catch (Exception)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }
            //var settings = ReflectionHelper.GetProperties(this, typeof(LocalSettingAttribute)).ToList();
            //var stdconst = UsedConstants.GetRuntimeFields()
            //    .Concat(typeof(SharedConstants).GetRuntimeFields())
            //    .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.IsStatic && fi.IsPublic).ToList();

            //foreach (var item in settings)
            //{
            //    var stdcontent = stdconst.FirstOrDefault(x=>x.Name == SharedConstants.SettingsPrefix + item.Name + SharedConstants.SettingsSTDPostfix);
            //    if (stdcontent != null)
            //    {
            //        var value = stdcontent.GetValue(null);
            //        item.SetValue(this, value);
            //    }
            //    else
            //    {
            //        //item.Name has no STD Value
            //        if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            //    }
            //}
        }
        #endregion
        #region Singleton Model Thigns

        public static SharedSettingsModel Initialize()
        {
            if (instance == null)
            {
                instance = new SharedSettingsModel
                {
                    //UsedConstants = typeof(SharedConstants)
                };
            }
            return Instance;
        }
        public static SharedSettingsModel Instance
        {
            get
            {
                return instance;
            }
        }

        public static SharedSettingsModel I
        {
            get
            {
                return instance;
            }
        }

        protected static SharedSettingsModel instance;


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PlatformHelper.CallPropertyChanged(PropertyChanged, this, propertyName);
        }
        #endregion

    }
}
