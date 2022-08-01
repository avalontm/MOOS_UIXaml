using MOOS.Driver;
using MOOS.NET.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOOS.NET.IPv4.UDP.DHCP
{
    /// <summary>
    /// DHCPClient class. Used to manage the DHCP connection to a server.
    /// </summary>
    public class DHCPClient : UdpClient
    {
        /// <summary>
        /// Is DHCP ascked check variable
        /// </summary>
        private bool asked = false;

        /// <summary>
        /// Get the IP address of the DHCP server
        /// </summary>
        /// <returns></returns>
        public static Address DHCPServerAddress(NetworkDevice networkDevice)
        {
            return NetworkConfiguration.Get(networkDevice).DefaultGateway;
        }

        /// <summary>
        /// Create new instance of the <see cref="DHCPClient"/> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown on fatal error (contact support).</exception>
        /// <exception cref="ArgumentException">Thrown if UdpClient with localPort 53 exists.</exception>
        public DHCPClient() : base(68)
        {
            
        }

        /// <summary>
        /// Receive data
        /// </summary>
        /// <param name="timeout">timeout value, default 5000ms</param>
        /// <returns>time value (-1 = timeout)</returns>
        /// <exception cref="InvalidOperationException">Thrown on fatal error (contact support).</exception>
        private int Receive(int timeout = 5000)
        {
            int second = 0;
            int _deltaT = 0;

            while (rxBuffer.Count < 1)
            {
                if (second > (timeout / 1000))
                {
                    return -1;
                }
                if (_deltaT != RTC.Second)
                {
                    second++;
                    _deltaT = RTC.Second;
                }
            }

            var packet = new DHCPPacket(rxBuffer.Dequeue().RawData);
            Console.WriteLine($"[packet] {packet.DataLength}");
            if (packet.MessageType == 2) //Boot Reply
            {
                if (packet.RawData[284] == 0x02) //Offer packet received
                {
                    Console.WriteLine("Offer received.");
                    return SendRequestPacket(packet.Client);
                }
                else if (packet.RawData[284] == 0x05 || packet.RawData[284] == 0x06) //ACK or NAK DHCP packet received
                {
                    var ack = new DHCPAck(packet.RawData);
                    if (asked)
                    {
                        Apply(ack, true);
                    }
                    else
                    {
                        Apply(ack);
                    }
                }
            }

            return second;
        }

        /// <summary>
        /// Send a packet to the DHCP server to make the address available again
        /// </summary>
        public void SendReleasePacket()
        {
            for (int i = 0; i < NetworkDevice.Devices.Count; i++)
            {
                Address source = IPConfig.FindNetwork(DHCPServerAddress(NetworkDevice.Devices[i]));
                var dhcp_release = new DHCPRelease(source, DHCPServerAddress(NetworkDevice.Devices[i]), NetworkDevice.Devices[i].MACAddress);

                OutgoingBuffer.AddPacket(dhcp_release);
                NetworkStack.Update();

                NetworkStack.RemoveAllConfigIP();

                IPConfig.Enable(NetworkDevice.Devices[i], new Address(0, 0, 0, 0), new Address(0, 0, 0, 0), new Address(0, 0, 0, 0));
            }
            Close();
        }

        /// <summary>
        /// Send a packet to find the DHCP server and tell that we want a new IP address
        /// </summary>
        /// <returns>time value (-1 = timeout)</returns>
        public int SendDiscoverPacket()
        {
            NetworkStack.RemoveAllConfigIP();

            for (int i = 0; i < NetworkDevice.Devices.Count; i++)
            {
                IPConfig.Enable(NetworkDevice.Devices[i], new Address(0, 0, 0, 0), new Address(0, 0, 0, 0), new Address(0, 0, 0, 0));
                DHCPDiscover dhcp_discover = new DHCPDiscover(NetworkDevice.Devices[i].MACAddress);
                OutgoingBuffer.AddPacket(dhcp_discover);
                NetworkStack.Update();
          
                asked = true;
            }

            return Receive();
        }

        /// <summary>
        /// Send a request to apply the new IP configuration
        /// </summary>
        /// <returns>time value (-1 = timeout)</returns>
        private int SendRequestPacket(Address RequestedAddress)
        {
            for (int i = 0; i < NetworkDevice.Devices.Count; i++)
            {
                var dhcp_request = new DHCPRequest(NetworkDevice.Devices[i].MACAddress, RequestedAddress);
                OutgoingBuffer.AddPacket(dhcp_request);
                NetworkStack.Update();
            }
            return Receive();
        }

        /*
         * Method called to applied the differents options received in the DHCP packet ACK
         **/
        /// <summary>
        /// Apply the new IP configuration received.
        /// </summary>
        /// <param name="Options">DHCPOption class using the packetData from the received dhcp packet.</param>
        /// <param name="message">Enable/Disable the displaying of messages about DHCP applying and conf. Disabled by default.
        /// </param>
        private void Apply(DHCPAck packet, bool message = false)
        {
            NetworkStack.RemoveAllConfigIP();

            //cf. Roadmap. (have to change this, because some network interfaces are not configured in dhcp mode) [have to be done in 0.5.x]
            for (int i = 0; i < NetworkDevice.Devices.Count; i++)
            {
                if (packet.Client.ToString() == null ||
                    packet.Client.ToString() == null ||
                    packet.Client.ToString() == null ||
                    packet.Client.ToString() == null)
                {
                    //throw new Exception("Parsing DHCP ACK Packet failed, can't apply network configuration.");
                    Console.WriteLine("Parsing DHCP ACK Packet failed, can't apply network configuration.");
                    return;
                }
                else
                {
                    if (message)
                    {
                        Console.WriteLine("[DHCP ACK][" + NetworkDevice.Devices[i].Name + "] Packet received, applying IP configuration...");
                        Console.WriteLine("   IP Address  : " + packet.Client.ToString());
                        Console.WriteLine("   Subnet mask : " + packet.Subnet.ToString());
                        Console.WriteLine("   Gateway     : " + packet.Server.ToString());
                        Console.WriteLine("   DNS server  : " + packet.DNS.ToString());
                    }

                    IPConfig.Enable(NetworkDevice.Devices[i], packet.Client, packet.Subnet, packet.Server);
                    DNSConfig.Add(packet.DNS);

                    if (message)
                    {
                        Console.WriteLine("[DHCP CONFIG][" + NetworkDevice.Devices[i].Name + "] IP configuration applied.");
                        asked = false;
                    }
                }
            }

            Close();
        }
    }
}
