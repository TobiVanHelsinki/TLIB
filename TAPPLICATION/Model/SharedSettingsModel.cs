using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using TAMARIN.Settings;
using TLIB;
using Microsoft.Toolkit.Uwp.Helpers;

namespace TAPPLICATION.Model
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class UsedSettingAttribute : Attribute
    {
        public UsedSettingAttribute() { }
    }
    public class SharedSettingsModel : INotifyPropertyChanged
    {
        internal Type UsedConstants { get; set; }
        public static IPlatformSettings PlatformSettings =
#if __ANDROID__
            new DroidSettings()
#else
            new WinSettings()
#endif
;
        #region Settinsg
        [UsedSetting]
        public bool INTERN_SYNC
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC, value);
                Instance.NotifyPropertyChanged();
            }
        }
        
        [UsedSetting]
        public bool DEBUG_FEATURES
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_DEBUG_FEATURES);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_DEBUG_FEATURES, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [UsedSetting]
        public bool BETA_FEATURES
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [UsedSetting]
        public bool DISPLAY_REQUEST
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [UsedSetting]
        public bool FOLDERMODE
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [UsedSetting]
        public string FOLDERMODE_PATH
        {
            get => PlatformSettings.GetStringLocal(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH, value);
                NotifyPropertyChanged();
            }
        }

        #endregion
        #region Methods
        public List<(string, object)> ExportAllSettings()
        {
            var propertyinfos = this.GetType().GetProperties();
            var ret = new List<(string, object)>();
            foreach (var item in propertyinfos)
            {
                object result = item.GetValue(this);
                ret.Add((item.Name, result));
            }
            return ret;
        }

        public void ResetAllSettings()
        {
            var settings = ReflectionHelper.GetProperties(this, typeof(UsedSettingAttribute));
            var stdconst = UsedConstants.
                GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly);

            foreach (var item in settings)
            {
                var stdcontent = stdconst.FirstOrDefault(x=>x.Name == SharedConstants.SettingsPrefix + item.Name + SharedConstants.SettingsSTDPostfix);
                if (stdcontent != null)
                {
                    var value = stdcontent.GetValue(null);
                    item.SetValue(this, value);
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                }
            }
            return;
        }
        #endregion
        #region Singleton Model Thigns

        public static SharedSettingsModel Initialize()
        {
            if (instance == null)
            {
                instance = new SharedSettingsModel();
                instance.UsedConstants = typeof(SharedConstants);
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
            ModelHelper.CallPropertyChanged(PropertyChanged, this, propertyName);
        }
        #endregion

    }
}
