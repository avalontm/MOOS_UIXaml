using MOOS.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOOS.IOGroup
{
    public class PCI : IOGroup
    {
        /// <summary>
        /// Configuration address port.
        /// </summary>
        public IOPort ConfigAddressPort = new IOPort(0xCF8);
        /// <summary>
        /// Configuration data port.
        /// </summary>
        public IOPort ConfigDataPort = new IOPort(0xCFC);
    }
}
