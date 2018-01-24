using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TLIB.Server;

namespace TLIB.Net
{
    internal abstract class HttpServer
    {

        protected int port;
        TcpListener listener;
        public bool is_active = true;
        IPAddress ipaddr;

        public HttpServer(int port, IPAddress ipaddr)
        {
            this.port = port;
            this.ipaddr = ipaddr;
        }

        public async void listen()
        {
            listener = new TcpListener(ipaddr, port);
            listener.Start();
            while (is_active)
            {
                TcpClient socket = await listener.AcceptTcpClientAsync();
                HttpProcessor processor = new HttpProcessor(socket, this);
#if UWP
                IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
                (workItem) =>
                {
                    processor.process();
                });
                await Task.Delay(TimeSpan.FromMilliseconds(1));
#else
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
#endif

            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }


}

