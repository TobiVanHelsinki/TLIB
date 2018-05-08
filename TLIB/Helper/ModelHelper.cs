#if WINDOWS_UWP
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
#endif

namespace TLIB
{
#if WINDOWS_UWP
    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
#endif
    public static class ModelHelper
    {
#if WINDOWS_UWP
        public static CoreDispatcher CDispatcher;
        public static void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {

            Event?.Invoke(o, new PropertyChangedEventArgs(property));
            return;
            CoreDispatcher C = Window.Current?.Dispatcher ?? CDispatcher;
            if (C != null)
            {
                var AsyncAction = C.RunAsync(CoreDispatcherPriority.High, () => Event?.Invoke(o, new PropertyChangedEventArgs(property)));
                var T = AsyncAction.AsTask();
                AsyncHelpers.RunSync(()=>T);

                //C.RunAsync(CoreDispatcherPriority.High, () => Event?.Invoke(o, new PropertyChangedEventArgs(property))).AsTask().RunSynchronously();
            }
        }

        public static async void CallPropertyChangedAsync(PropertyChangedEventHandler Event, object o, string property, CoreDispatcherPriority Prio = CoreDispatcherPriority.Normal)
        {
            CoreDispatcher C = Window.Current?.Dispatcher ?? CDispatcher;
            
            if (C == null)
            {
                try
                {
                    //TODO Multiple Views, hier ForEach CoreApplication.Views [...]
                    C = CoreApplication.GetCurrentView()?.CoreWindow?.Dispatcher;
                }
                catch (Exception) { }
            }
            if (C != null)
            {
                await C.RunAsync(CoreDispatcherPriority.High, () => Event?.Invoke(o, new PropertyChangedEventArgs(property)));
            }
        }
        public static async void AtGui(Action x, CoreDispatcherPriority Priority = CoreDispatcherPriority.Low)
        {
            CoreDispatcher C = CDispatcher ?? Window.Current?.Dispatcher ?? CoreApplication.MainView?.CoreWindow?.Dispatcher ;
            if (C != null)
            {
                await CDispatcher.RunAsync(Priority, () => x());
            }
        }
#endif
    }
}
