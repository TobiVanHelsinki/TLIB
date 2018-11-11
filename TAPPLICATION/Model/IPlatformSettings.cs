
using System.Collections.Generic;

namespace TLIB.Settings
{
    public interface IPlatformSettings
    {
        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        void SetLocal(string place, object value);
        void SetRoaming(string place, object value);

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        bool GetBoolLocal(string place, bool fallback = default);
        bool GetBoolRoaming(string place, bool fallback = default);

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        string GetStringLocal(string place, string fallback = default);
        string GetStringRoaming(string place, string fallback = default);

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        int GetIntLocal(string place, int fallback = default);
        int GetIntRoaming(string place, int fallback = default);

    }
}
