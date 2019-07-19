﻿using System;
using System.Linq;
using TAPPLICATION;
using TAPPLICATION.Model;
using TLIB.Settings;
using Windows.Storage;

namespace TAPPLICATION_UWP
{
    public class Settings : IPlatformSettings
    {
        ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;

        public void SetLocal(string place, object value)
        {
            try
            {
                LocalSettings.Values[place] = value;
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
        public void SetRoaming(string place, object value)
        {
            try
            {
                RoamingSettings.Values[place] = value;
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        public object GetLocal(string place)
        {
            if (LocalSettings.Values.ContainsKey(place))
            {
                return LocalSettings.Values[place];
            }
            else
            {
                throw new SettingNotPresentException();
            }
        }
        public object GetRoaming(string place)
        {
            if (RoamingSettings.Values.ContainsKey(place))
            {
                return RoamingSettings.Values[place];
            }
            else if (LocalSettings.Values.ContainsKey(place))
            {
                return RoamingSettings.Values[place];
            }
            else
            {
                throw new SettingNotPresentException();
            }
        }

        public void RemoveAllSettings()
        {
            foreach (var item in LocalSettings.Containers.ToArray())
            {
                LocalSettings.DeleteContainer(item.Key);
            }
            foreach (var item in RoamingSettings.Containers.ToArray())
            {
                RoamingSettings.DeleteContainer(item.Key);
            }
        }

        public void PrepareSettingsSavePlace()
        {
            ApplicationData.Current.LocalSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
            ApplicationData.Current.RoamingSettings.CreateContainer(SharedConstants.CONTAINER_SETTINGS, ApplicationDataCreateDisposition.Always);
        }
    }
}
