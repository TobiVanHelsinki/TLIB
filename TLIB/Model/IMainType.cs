using System;

namespace TLIB.Model
{
    public interface IMainType
    {
        string MakeName();
        /// <summary>
        /// use this to provide a own converter method. leave it null to use a std converter
        /// </summary>
        Func < string, string, string, IMainType> Converter { get; }
        // Admin Version Numbers
        string APP_VERSION_NUMBER { get; } 
        string FILE_VERSION_NUMBER { get; }

        IO.FileInfoClass FileInfo { get; set; }

        event EventHandler SaveRequest;
        #region AUTO_SAVE_STUFF 
        [Newtonsoft.Json.JsonIgnore]
        bool HasChanges { get; set; }
        #endregion

    }
}
