
namespace TLIB.Settings
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
        bool getBool(string place, bool fallback = false);

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        string getString(string place, string fallback = "");

        /// <summary>
        /// don't throw
        /// </summary>
        /// <param name="place"></param>
        /// <param name="value"></param>
        int getInt(string place, int fallback = 0);
    }
}
