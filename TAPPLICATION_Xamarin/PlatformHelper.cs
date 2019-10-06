﻿using System;
using System.ComponentModel;
using TAPPLICATION;
using TLIB;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TAPPLICATION_Xamarin
{
    public class PlatformHelper : IPlatformHelper
    {
        public void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
            try
            {
                Event?.Invoke(o, new PropertyChangedEventArgs(property));
            }
            catch (Exception)
            {
                try
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    });
                }
                catch (Exception ex)
                {
                    Log.Write("Could not CallPropertyChanged", ex, logType: LogType.Error);
                }
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            try
            {
                Device.BeginInvokeOnMainThread(p);
            }
            catch (Exception ex)
            {
                Log.Write("Could not ExecuteOnUIThreadAsync", ex, logType: LogType.Error);
            }
        }
    }
}
