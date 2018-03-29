﻿using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks; //UWP
using TLIB.Net;
using static TLIB.CrossPlatformHelper.Threading;
using static TLIB.CrossPlatformHelper.PrintOut;

namespace TLIB.Server
{
    internal class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;
        
        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }


        private async Task<string> streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n')
                {
                    break;
                }
                if (next_char == '\r')
                {
                    continue;
                }
                if (next_char == -1)
                {
                    await SleepMilliSeconds(1);
                    //Thread.Sleep(1);
                    //await Task.Delay(TimeSpan.FromMilliseconds(1));
                    continue;
                };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public async void process()
        {
            try
            {
                inputStream = /*new BufferedStream*/(socket.GetStream());
                outputStream = new StreamWriter(/*new BufferedStream*/(socket.GetStream()));
            }
            catch (Exception)
            {
                WriteLine("EmerExit");
                return;
            }
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    await handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    await handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            try
            {
                outputStream.Flush();
            }
            catch (Exception e)
            {
                WriteLine("Exception: " + e.ToString());
            }
            // bs.Flush(); // flush any remaining output
            inputStream = null;
            outputStream = null; // bs = null;         
            try
            {
#if WINDOWS_UWP
#elif WINDOWS_DESKTOP
            socket.Close();
#endif
            }
            catch (Exception)
            {
                WriteLine("EmerExit");
                return;
            }
            WriteLine("Exit");
        }

        public async void parseRequest()
        {
            String request = await streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            WriteLine("starting: " + request);
        }

        public async void readHeaders()
        {
            WriteLine("readHeaders()");
            String line;
            while ((line = await streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                WriteLine("header: "+ name + ":"+ value);
                httpHeaders[name] = value;
            }
        }

        public async Task handleGETRequest()
        {
            await srv.HandleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;
        public async Task handlePOSTRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    WriteLine("starting Read, to_read="+ to_read);

                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    WriteLine("read finished, numread="+ numread);
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            WriteLine("get post data end");
            await srv.HandlePOSTRequest(this, new StreamReader(ms));

        }

        public void writeSuccess(string content_type = "text/html")
        {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

}
