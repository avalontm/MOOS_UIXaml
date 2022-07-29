using MOOS.NET.IPv4;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOOS.NET.Config
{
    /// <summary>
    /// Network Configuration (link network device to an ip address)
    /// </summary>
    public class NetworkConfig
    {
        /// <summary>
        /// Network device
        /// </summary>
        public NetworkDevice Device;

        /// <summary>
        /// IPv4 Configuration
        /// </summary>
        public IPConfig IPConfig;

        /// <summary>
        /// NetworkConfig ctor
        /// </summary>
        /// <param name="device">Network device.</param>
        /// <param name="config">IP Config</param>
        internal NetworkConfig(NetworkDevice device, IPConfig config)
        {
            Device = device;
            IPConfig = config;
        }
    }

    /// <summary>
    /// Network stack configuration
    /// </summary>
    public static class NetworkConfiguration
    {
        /// <summary>
        /// Current network configuration used by the network stack
        /// </summary>
        public static NetworkConfig CurrentNetworkConfig { get; set; }

        /// <summary>
        /// Current network configuration used by the network stack
        /// </summary>
        public static List<NetworkConfig> NetworkConfigs = new List<NetworkConfig>();

        /// <summary>
        /// Network congiruations count
        /// </summary>
        public static int Count
        {
            get { return NetworkConfigs.Count; }
        }

        /// <summary>
        /// Current IPv4 address
        /// </summary>
        public static Address CurrentAddress
        {
            get { return CurrentNetworkConfig.IPConfig.IPAddress; }
        }

        /// <summary>
        /// Set current network config
        /// </summary>
        /// <param name="device">Network device.</param>
        /// <param name="config">IP Config</param>
        public static void SetCurrentConfig(NetworkDevice device, IPConfig config)
        {
            CurrentNetworkConfig = new NetworkConfig(device, config);
        }

        /// <summary>
        /// Add new network config
        /// </summary>
        /// <param name="device">Network device.</param>
        /// <param name="config">IP Config</param>
        public static void AddConfig(NetworkDevice device, IPConfig config)
        {
            NetworkConfigs.Add(new NetworkConfig(device, config));
        }

        /// <summary>
        /// Network stack contains device
        /// </summary>
        /// <param name="device">Network device.</param>
        public static bool ConfigsContainsDevice(NetworkDevice k)
        {
            if (NetworkConfigs == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < NetworkConfigs.Count; i++)
                {
                    if (k == NetworkConfigs[i].Device)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Clear network configurations
        /// </summary>
        public static void ClearConfigs()
        {
            NetworkConfigs.Clear();
        }

        /// <summary>
        /// Get ip config for network device
        /// </summary>
        /// <param name="device">Network device.</param>
        public static IPConfig Get(NetworkDevice device)
        {
            for (int i = 0; i < NetworkConfigs.Count; i++)
            {
                if (device == NetworkConfigs[i].Device)
                {
                    return NetworkConfigs[i].IPConfig;
                }
            }

            return null;
        }

        /// <summary>
        /// Remove Config for network device
        /// </summary>
        /// <param name="device">Network device.</param>
        public static void Remove(NetworkDevice key)
        {
            int index = 0;

            for (int i = 0; i < NetworkConfigs.Count; i++)
            {
                if (key == NetworkConfigs[i].Device)
                {
                    break;
                }
                index++;
            }
            NetworkConfigs.RemoveAt(index);
        }
    }
}
