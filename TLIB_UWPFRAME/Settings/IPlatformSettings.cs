
using System.Collections.Generic;

namespace TLIB_UWPFRAME.Settings
{
    public interface IPlatformSettings
    {
        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        void set(string place, object value);
        
        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        bool getBool(string place, bool fallback = default);

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        string getString(string place, string fallback = default);

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        int getInt(string place, int fallback = default);


        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        IEnumerable<T> getIEnumerable<T>(string place, T fallback = default); 
    }
}
