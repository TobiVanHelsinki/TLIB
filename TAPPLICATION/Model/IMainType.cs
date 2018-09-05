using Newtonsoft.Json;
using System;
using TLIB.IO;

namespace TAPPLICATION.Model
{
    public interface IMainType
    {
        string APP_VERSION_NUMBER { get; } 
        string FILE_VERSION_NUMBER { get; }

        FileInfoClass FileInfo { get; set; }

        event EventHandler SaveRequest;
        [JsonIgnore]
        bool HasChanges { get; set; }

        string MakeName();
    }
}
