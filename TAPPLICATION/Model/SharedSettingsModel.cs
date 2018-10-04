using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using TLIB;
using TLIB.Settings;
using TLIB.PlatformHelper;

namespace TAPPLICATION.Model
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class LocalSettingAttribute : Attribute
    {
        public LocalSettingAttribute() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RoamingSettingAttribute : Attribute
    {
        public RoamingSettingAttribute() { }
    }
    
    public class SharedSettingsModel : INotifyPropertyChanged
    {
        public Type UsedConstants { get; set; }
        public static IPlatformSettings PlatformSettings;
        #region Settinsg
        [LocalSettingAttribute]
        public bool INTERN_SYNC
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC, value);
                Instance.NotifyPropertyChanged();
            }
        }
        
        [LocalSettingAttribute]
        public bool DEBUG_FEATURES
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_DEBUG_FEATURES);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_DEBUG_FEATURES, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [LocalSettingAttribute]
        public bool BETA_FEATURES
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [LocalSettingAttribute]
        public bool DISPLAY_REQUEST
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [LocalSettingAttribute]
        public bool FOLDERMODE
        {
            get => PlatformSettings.GetBoolLocal(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE);
            set
            {
                PlatformSettings.SetLocal(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE, value);
                Instance.NotifyPropertyChanged();
            }
        }

        [LocalSettingAttribute]
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
            var settings = ReflectionHelper.GetProperties(this, typeof(LocalSettingAttribute));
            var stdconst = UsedConstants.
                GetRuntimeFields()
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.IsStatic && fi.IsPublic);
    //        var stdconst = UsedConstants.
    //GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
    //.Where(fi => fi.IsLiteral && !fi.IsInitOnly);

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
