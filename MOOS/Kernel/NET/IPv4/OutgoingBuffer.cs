using MOOS.NET.ARP;
using MOOS.NET.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MOOS.NET.IPv4
{
    /// <summary>
    /// OutgoingBuffer class.
    /// </summary>
    internal static class OutgoingBuffer
    {
        /// <summary>
        /// BufferEntry class.
        /// </summary>
        private class BufferEntry
        {
            /// <summary>
            /// Entry status.
            /// </summary>
            public enum EntryStatus
            {
                /// <summary>
                /// Added.
                /// </summary>
                ADDED,
                /// <summary>
                /// ARP sent.
                /// </summary>
                ARP_SENT,
                /// <summary>
                /// Route ARP sent.
                /// </summary>
                ROUTE_ARP_SENT,
                /// <summary>
                /// Just send.
                /// </summary>
                JUST_SEND,
                /// <summary>
                /// Done.
                /// </summary>
                DONE,
                /// <summary>
                /// DHCP request.
                /// </summary>
                DHCP_REQUEST
            };

            /// <summary>
            /// Network Interface Controller.
            /// </summary>
            public NetworkDevice NIC;
            /// <summary>
            /// IP packet.
            /// </summary>
            public IPPacket Packet;
            /// <summary>
            /// Entry status
            /// </summary>
            public EntryStatus Status;
            /// <summary>
            /// Next hop.
            /// </summary>
            public Address nextHop;

            /// <summary>
            /// Create new instance of the <see cref="BufferEntry"/> class.
            /// </summary>
            /// <param name="nic">Network device.</param>
            /// <param name="packet">IP packet.</param>
            public BufferEntry(NetworkDevice nic, IPPacket packet)
            {
                this.NIC = nic;
                this.Packet = packet;

                if (Packet.DestinationIP.IsBroadcastAddress())
                {
                    this.Status = EntryStatus.DHCP_REQUEST;
                }
                else
                {
                    this.Status = EntryStatus.ADDED;
                }
            }
        }

        /// <summary>
        /// Buffer queue.
        /// </summary>
        private static List<BufferEntry> queue;

        /// <summary>
        /// Ensure queue exists.
        /// </summary>
        private static void EnsureQueueExists()
        {
            if (queue == null)
            {
                queue = new List<BufferEntry>();
            }
        }

        /// <summary>
        /// Add packet.
        /// </summary>
        /// <param name="packet">IP packet.</param>
        internal static void AddPacket(IPPacket packet)
        {
            EnsureQueueExists();
            NetworkDevice nic = IPConfig.FindInterface(packet.SourceIP);
            packet.SourceMAC = nic.MACAddress;
            queue.Add(new BufferEntry(nic, packet));

        }

        /// <summary>
        /// Add packet.
        /// </summary>
        /// <param name="packet">IP packet.</param>
        /// <param name="device">Network Interface Controller.</param>
        internal static void AddPacket(IPPacket packet, NetworkDevice device)
        {
            EnsureQueueExists();
            packet.SourceMAC = device.MACAddress;
            queue.Add(new BufferEntry(device, packet));
        }

        /// <summary>
        /// Send packet.
        /// </summary>
        /// <exception cref="sys.ArgumentException">Thrown on fatal error (contact support).</exception>
        /// <exception cref="sys.ArgumentOutOfRangeException">Thrown on memory error.</exception>
        /// <exception cref="sys.OverflowException">Thrown if RawData length is bigger than Int32.MaxValue.</exception>
        internal static void Send()
        {
            EnsureQueueExists();
            int _deltaT = 0;
            int second = 0;

            while (queue.Count > 0)
            {
                if (_deltaT != RTC.Second)
                {
                    second++;
                    _deltaT = RTC.Second;
                }

                if (second >= 4)
                {
                    Console.WriteLine("No response in 4 secondes...");
                    break;
                }

                for (int e = 0; e < queue.Count; e++)
                {
                    if (queue[e].Status == BufferEntry.EntryStatus.ADDED)
                    {
                        if (IPConfig.IsLocalAddress(queue[e].Packet.DestinationIP) == false)
                        {
                            queue[e].nextHop = IPConfig.FindRoute(queue[e].Packet.DestinationIP);
                            if (queue[e].nextHop == null)
                            {
                                queue[e].Status = BufferEntry.EntryStatus.DONE;
                                continue;
                            }

                            if (ARPCache.Contains(queue[e].nextHop) == true)
                            {
                                queue[e].Packet.DestinationMAC = ARPCache.Resolve(queue[e].nextHop);
                                queue[e].NIC.QueueBytes(queue[e].Packet.RawData);

                                queue[e].Status = BufferEntry.EntryStatus.DONE;
                            }
                            else
                            {
                                ARPRequest_Ethernet arp_request = new ARPRequest_Ethernet(queue[e].NIC.MACAddress, queue[e].Packet.SourceIP,
                                    MACAddress.Broadcast, queue[e].nextHop, MACAddress.None);

                                queue[e].NIC.QueueBytes(arp_request.RawData);

                                queue[e].Status = BufferEntry.EntryStatus.ROUTE_ARP_SENT;
                            }
                            continue;
                        }
                        if (ARPCache.Contains(queue[e].Packet.DestinationIP) == true)
                        {
                            queue[e].Packet.DestinationMAC = ARPCache.Resolve(queue[e].Packet.DestinationIP);
                            queue[e].NIC.QueueBytes(queue[e].Packet.RawData);

                            queue[e].Status = BufferEntry.EntryStatus.DONE;
                        }
                        else
                        {
                            ARPRequest_Ethernet arp_request = new ARPRequest_Ethernet(queue[e].NIC.MACAddress, queue[e].Packet.SourceIP,
                                MACAddress.Broadcast, queue[e].Packet.DestinationIP, MACAddress.None);

                            queue[e].NIC.QueueBytes(arp_request.RawData);

                            queue[e].Status = BufferEntry.EntryStatus.ARP_SENT;
                        }
                    }
                    else if (queue[e].Status == BufferEntry.EntryStatus.DHCP_REQUEST)
                    {
                        queue[e].NIC.QueueBytes(queue[e].Packet.RawData);
                        queue[e].Status = BufferEntry.EntryStatus.DONE;
                       
                    }
                    else if (queue[e].Status == BufferEntry.EntryStatus.JUST_SEND)
                    {
                        queue[e].NIC.QueueBytes(queue[e].Packet.RawData);

                        queue[e].Status = BufferEntry.EntryStatus.DONE;
                    }
                }
                int i = 0;
                while (i < queue.Count)
                {
                    if (queue[i].Status == BufferEntry.EntryStatus.DONE)
                    {
                        queue.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// ARP cache update.
        /// </summary>
        /// <param name="arp_reply">ARP reply.</param>
        /// <exception cref="sys.ArgumentException">Thrown if arp_reply.SenderIP is not a IPv4Address.</exception>
        internal static void ARPCache_Update(ARPReply_Ethernet arp_reply)
        {
            EnsureQueueExists();

            for (int e = 0; e < queue.Count; e++)
            {
                if (queue[e].Status == BufferEntry.EntryStatus.ARP_SENT)
                {
                    var xDestIP = queue[e].Packet.DestinationIP.Hash;
                    var xSenderIP = arp_reply.SenderIP.Hash;
                    if (queue[e].Packet.DestinationIP.CompareTo(arp_reply.SenderIP) == 0)
                    {
                        queue[e].Packet.DestinationMAC = arp_reply.SenderMAC;

                        queue[e].Status = BufferEntry.EntryStatus.JUST_SEND;
                    }
                }
                else if (queue[e].Status == BufferEntry.EntryStatus.ROUTE_ARP_SENT)
                {
                    if (queue[e].nextHop.CompareTo(arp_reply.SenderIP) == 0)
                    {
                        queue[e].Packet.DestinationMAC = arp_reply.SenderMAC;

                        queue[e].Status = BufferEntry.EntryStatus.JUST_SEND;
                    }
                }
            }
        }
    }
}
