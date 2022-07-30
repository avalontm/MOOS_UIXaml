using System;
using System.Collections.Generic;
using System.Text;

namespace MOOS.NET.IPv4.UDP.DHCP
{
    /// <summary>
    /// DHCPAck class.
    /// </summary>
    internal class DHCPAck : DHCPPacket
    {
        /// <summary>
        /// Create new instance of the <see cref="DHCPAck"/> class.
        /// </summary>
        internal DHCPAck() : base()
        { }

        /// <summary>
        /// Create new instance of the <see cref="DHCPAck"/> class.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        internal DHCPAck(byte[] rawData) : base(rawData)
        { }

        /// <summary>
        /// Init DHCPAck fields.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if RawData is invalid or null.</exception>
        protected override void InitFields()
        {
            base.InitFields();

            for(int i =0; i < Options.Count; i++)
            {
                if (Options[i].Type == 1) //Mask
                {
                    Subnet = new Address(Options[i].Data, 0);
                }
                else if (Options[i].Type == 3) //Router
                {
                    Server = new Address(Options[i].Data, 0);
                }
                else if (Options[i].Type == 6) //DNS
                {
                    DNS = new Address(Options[i].Data, 0);
                }
            }
        }

        /// <summary>
        /// Get Subnet IPv4 Address
        /// </summary>
        internal Address Subnet { get; private set; }

        /// <summary>
        /// Get DNS IPv4 Address
        /// </summary>
        internal Address DNS { get; private set; }

        /// <summary>
        /// Get DHCP Server IPv4 Address
        /// </summary>
        internal Address Server { get; private set; }
    }
}
