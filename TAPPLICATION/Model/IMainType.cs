using System;
using TAMARIN.IO;

namespace TAPPLICATION.Model
{
    public interface IMainType
    {
        string MakeName();
        // Admin Version Numbers
        string APP_VERSION_NUMBER { get; } 
        string FILE_VERSION_NUMBER { get; }

        FileInfoClass FileInfo { get; set; }

        event EventHandler SaveRequest;

        #region AUTO_SAVE_STUFF 
        [Newtonsoft.Json.JsonIgnore]
        bool HasChanges { get; set; }
        #endregion

    }
}
