using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TLIB.Server;
#if WINDOWS_UWP
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#elif WINDOWS_DESKTOP
using System.Threading;
#endif


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
                try
                {
                    TcpClient socket = await listener.AcceptTcpClientAsync();
                    HttpProcessor processor = new HttpProcessor(socket, this);
#if WINDOWS_UWP
                    IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
                    (workItem) =>
                    {
                        processor.process();
                    });
#elif WINDOWS_DESKTOP
                    Thread thread = new Thread(new ThreadStart(processor.process));
#endif
                }
                catch (Exception)
                {
                }
                await CrossPlatformHelper.Threading.SleepMilliSeconds(1);
            }
        }

        public abstract void HandleGETRequest(HttpProcessor p);
        public abstract void HandlePOSTRequest(HttpProcessor p, StreamReader inputData);

#if WINDOWS_UWP
        public static async Task UDPBroadcast(byte[] magicPacket, int sendPort, DatagramSocket datagramSocketSend)
        {
            try
            {
                var portStr = sendPort.ToString(CultureInfo.InvariantCulture);
                using (var stream = await datagramSocketSend?.GetOutputStreamAsync(new HostName("255.255.255.255"), portStr))
                {
                    using (var writer = new DataWriter(stream))
                    {
                        writer.WriteBytes(magicPacket);
                        await writer.StoreAsync();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
#endif


        #region Magical Package Stuff
        public static byte[] CreateMagicalPackage(string MACAddrString)
        {
            var MACAddr = MACAddrString.Split(':');
            List<byte> MACAddrList = new List<byte>();
            foreach (var item in MACAddr)
            {
                MACAddrList.Add(Byte.Parse(item, NumberStyles.HexNumber));
            }
            List<byte> PacketList = new List<byte>();
            for (int i = 0; i < 6; i++)
            {
                PacketList.Add(0xFF);
            }
            for (int i = 0; i < 16; i++)
            {
                PacketList.AddRange(MACAddrList);
            }
            return PacketList.ToArray();
        }

        public static bool IsByteArrayMagicPacket(byte[] bArr)
        {
            try
            {
                // Header 6x FF.
                for (int i = 0; i < 6; i++)
                {
                    if (bArr[i] != 0xFF)
                        return false;
                }

                // Get the MAC address.
                var macArr = new byte[6];

                for (int i = 0; i < macArr.Length; i++)
                {
                    macArr[i] = bArr[i + 6];
                }

                // This MAC adress has to be the same 16 times in a row.
                for (int i = 1; i < 17; i++)
                {
                    var checkArr = new byte[6];
                    Array.Copy(bArr, 6 * i, checkArr, 0, 6);

                    for (int j = 0; j < checkArr.Length; j++)
                    {
                        if (checkArr[j] != macArr[j])
                            return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetMacStringFromMagicPacket(byte[] magicPacket)
        {
            var macArr = new byte[6];

            for (int i = 0; i < macArr.Length; i++)
            {
                macArr[i] = magicPacket[i + 6];
            }

            return ConvertMacByteArrayToString(macArr);
        }

        public static string ConvertMacByteArrayToString([ReadOnlyArray]byte[] macByte)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < macByte.Length; i++)
            {
                sb.Append(macByte[i].ToString("x2"));

                if (i % 1 == 0 && i != macByte.Length - 1)
                    sb.Append(": ");
            }

            return sb.ToString().ToUpper();
        }
#endregion
    }


}

