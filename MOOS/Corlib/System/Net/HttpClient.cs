using MOOS;
using MOOS.Driver;
using MOOS.Misc;
using MOOS.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Net
{
    public class HttpClient
    {
        TcpClient client;
        bool timeout;
        string response;
        bool isConectiong;
        public HttpClient(string host, int port)
        {

        }

        public void Get()
        {
            response = string.Empty;
           
            //Make sure this IP is pointing your gateway
            if (client == null || client != null && !client.Connected)
            {
                client = TcpClient.Connect(IPAddress.Parse(192, 168, 137, 2), 5000);
                client.OnData += Client_OnData;
            }

           // new Thread(() =>
            //{
                ulong t = Timer.Ticks + 3000;

                while ((Timer.Ticks < t) && !client.Connected)
                {
                    Native.Hlt();
                }

                if (client.State == TCPStatus.SynSent)
                {
                    Console.WriteLine("Connection timeout");
                    Debug.WriteLine("Connection timeout");
                }

                if (client.State == TCPStatus.SynSent)
                {
                    Console.WriteLine("Connection timeout");
                    Debug.WriteLine("Connection timeout");
                }

            if (client.Connected)
            {
                client.Send(ToASCII("GET /WeatherForecast HTTP/1.1\r\nHost: 192.168.137.2:5000\r\nUser-Agent: Mozilla/4.0 (compatible; MOOS Operating System)\r\n\r\n"));
            }
            else
            {
                client.Close();
                client = null;
            }

            //}).Start();
        }

        void Client_OnData(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                response += (char)data[i];
                Debug.Write((char)data[i]);
            }

            Debug.WriteLine();
            timeout = true;

            client.Close();
            client = null;
        }

        byte[] ToASCII(string s)
        {
            byte[] buffer = new byte[s.Length];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)s[i];
            return buffer;
        }
    }
}
