using MOOS.NET.Config;
using MOOS.NET.IPv4;
using MOOS.NET.IPv4.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MOOS.NET
{
    
    public class NetworkConsoleger
    {
        /// <summary>
        /// TCP Server.
        /// </summary>
        private TcpListener xListener = null;

        /// <summary>
        /// TCP Client.
        /// </summary>
        private TcpClient xClient = null;

        /// <summary>
        /// Remote IP Address
        /// </summary>
        public Address Ip { get; set; }

        /// <summary>
        /// Port used
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Create NetworkConsoleger class (used to listen for a Consoleger connection)
        /// </summary>
        /// <param name="port">Port used for TCP connection.</param>
        public NetworkConsoleger(int port)
        {
            Port = port;
            xListener = new TcpListener((ushort)port);
        }

        /// <summary>
        /// Create NetworkConsoleger class (used to connect to a remote Consoleger)
        /// </summary>
        /// <param name="ip">IP Address of the remote Consoleger.</param>
        /// <param name="port">Port used for TCP connection.</param>
        public NetworkConsoleger(Address ip, int port)
        {
            Ip = ip;
            Port = port;
            xClient = new TcpClient(port);
        }

        /// <summary>
        /// Start Consoleger
        /// </summary>
        public void Start()
        {
            if (xClient == null)
            {
                xListener.Start();

                Console.WriteLine("Waiting for remote Consoleger connection at " + NetworkConfiguration.CurrentAddress.ToString() + ":" + Port);
                xClient = xListener.AcceptTcpClient(); //blocking
            }
            else if (xListener == null)
            {
                xClient.Connect(Ip, Port);
            }

            Send("--- MOOS Network Consoleger ---");
            Send("Consoleger Connected!");
        }

        /// <summary>
        /// Send text to the Consoleger
        /// </summary>
        /// <param name="message">Text to send to the Consoleger.</param>
        public void Send(string message)
        {
            xClient.Send(Encoding.ASCII.GetBytes("[" + DateTime.Now.ToString("HH:mm:ss") + "] - " + message + "\r\n"));
        }

        /// <summary>
        /// Stop the Consoleger by closing TCP Connection
        /// </summary>
        public void Stop()
        {
            Console.WriteLine("Closing Consoleger connection");
            Send("Closing...");
            xClient.Close();
        }
    }
}
