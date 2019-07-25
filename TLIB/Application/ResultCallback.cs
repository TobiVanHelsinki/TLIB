using System;

namespace TLIB
{
    /// <summary>
    /// stores an callback action for answering 
    /// </summary>
    public class ResultCallback
    {
        Action<int> callback;
        /// <summary>
        /// Use this to send the number of the selected choice back to sender
        /// the callback will just work once
        /// </summary>
        /// <param name="n">the selected index</param>
        public void SendResultNo(int n)
        {
            callback?.Invoke(n);
            callback = null;
        }

        /// <summary>
        /// specify the callback to get executed when a selection is made
        /// </summary>
        /// <param name="callback">the callback to executes, parameter is int index</param>
        public ResultCallback(Action<int> callback) => this.callback = callback;
    }
}
