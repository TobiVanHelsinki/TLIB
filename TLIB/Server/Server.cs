using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace TLIB.Server
{
    class Server
    {
        private StreamSocketListener listener;
        private const uint BufferSize = 8192;
        public string currentQuery = "";
        public event EventHandler<string> NewQueryAppeared;
        public string Respond;

        public void Initialise(string Port = "80")
        {
            listener = new StreamSocketListener();
            listener.BindServiceNameAsync(Port);
            listener.ConnectionReceived += HandleRequest;
        }

        private static string GetQuery(StringBuilder request)
        {
            string[] requestLines = request.ToString().Split(' ');
            string url = requestLines.Length > 1 ? requestLines[1] : string.Empty;

            string query = (new Uri("http://localhost" + url)).Query;
            System.Diagnostics.Debug.WriteLine("query: " + query);
            return query;
        }
        public async void HandleRequest(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            StringBuilder request = new StringBuilder();

            try
            {
                byte[] data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await args.Socket.InputStream.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getting Data ex:" + ex.Message);
                return;
            }

            currentQuery = GetQuery(request);
            NewQueryAppeared?.Invoke(this, currentQuery);

            byte[] respond = Encoding.UTF8.GetBytes(Respond);
            Stream responseStream = args.Socket.OutputStream.AsStreamForWrite();
            await responseStream.WriteAsync(respond, 0, respond.Length);
            try
            {
                await responseStream.FlushAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error sending Answere ex:" + ex.Message);
                return;
            }
        }

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


        public async static Task UDPBroadcast(byte[] magicPacket, int sendPort, DatagramSocket datagramSocketSend)
        {
            try
            {
                var portStr = sendPort.ToString(CultureInfo.InvariantCulture);
                using (var stream = await datagramSocketSend.GetOutputStreamAsync(new HostName("255.255.255.255"), portStr))
                {
                    using (var writer = new DataWriter(stream))
                    {
                        writer.WriteBytes(magicPacket);
                        await writer.StoreAsync();
                    }
                }
            }
            catch (Exception ex)
            {
            }
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
            catch (Exception ex)
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
                    sb.Append(":");
            }

            return sb.ToString().ToUpper();
        }
    }

}

