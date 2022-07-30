using MOOS.Driver;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Common.Extentions;
using System.Diagnostics;

namespace MOOS
{

    public enum ClassID
    {
        PCIDevice_2_0 = 0x00,
        MassStorageController = 0x01,
        NetworkController = 0x02,
        DisplayController = 0x03,
        MultimediaDevice = 0x04,
        MemoryController = 0x05,
        BridgeDevice = 0x06,
        SimpleCommController = 0x07,
        BaseSystemPreiph = 0x08,
        InputDevice = 0x09,
        DockingStations = 0x0A,
        Proccesors = 0x0B,
        SerialBusController = 0x0C,
        WirelessController = 0x0D,
        InteligentController = 0x0E,
        SateliteCommController = 0x0F,
        EncryptionController = 0x10,
        SignalProcessingController = 0x11,
        ProcessingAccelerators = 0x12,
        NonEssentialInstsrumentation = 0x13,
        Coprocessor = 0x40,
        Unclassified = 0xFF
    }

    public enum SubclassID
    {
        // MassStorageController: 
        SCSIStorageController = 0x00,
        IDEInterface = 0x01,
        FloppyDiskController = 0x02,
        IPIBusController = 0x03,
        RAIDController = 0x04,
        ATAController = 0x05,
        SATAController = 0x06,
        SASController = 0x07,
        NVMController = 0x08,
        UnknownMassStorage = 0x09,
    }

    public enum ProgramIF
    {
        // MassStorageController:
        SATA_VendorSpecific = 0x00,
        SATA_AHCI = 0x01,
        SATA_SerialStorageBus = 0x02,
        SAS_SerialStorageBus = 0x01,
        NVM_NVMHCI = 0x01,
        NVM_NVMExpress = 0x02
    }

    public enum VendorID
    {
        Intel = 0x8086,
        AMD = 0x1022,
        VMWare = 0x15AD,
        Bochs = 0x1234,
        VirtualBox = 0x80EE
    }

    public enum DeviceID
    {
        SVGAIIAdapter = 0x0405,
        PCNETII = 0x2000,
        BGA = 0x1111,
        VBVGA = 0xBEEF,
        VBoxGuest = 0xCAFE
    }

    public class PCI
    {
        public static List<PCIDevice> Devices;

        public static uint Count
        {
            get { return (uint)Devices.Count; }
        }

        public static void Initialise()
        {
            Devices = new List<PCIDevice>();
            if ((PCIDevice.GetHeaderType(0x0, 0x0, 0x0) & 0x80) == 0)
            {
                CheckBus(0x0);
            }
            else
            {
                for (ushort fn = 0; fn < 8; fn++)
                {
                    if (PCIDevice.GetVendorID(0x0, 0x0, fn) != 0xFFFF)
                        break;

                    CheckBus(fn);
                }
            }

            PCIExpress.Initialize();

            Console.Write("[PCI] PCI Initialized. ");
            Console.Write(((ulong)Devices.Count).ToString());
            Console.WriteLine(" Devices");
        }

        /// <summary>
        /// Check bus.
        /// </summary>
        /// <param name="xBus">A bus to check.</param>
        public static void CheckBus(ushort xBus)
        {
            for (ushort device = 0; device < 32; device++)
            {
                if (PCIDevice.GetVendorID(xBus, device, 0x0) == 0xFFFF)
                    continue;

                CheckFunction(new PCIDevice(xBus, device, 0x0));
                if ((PCIDevice.GetHeaderType(xBus, device, 0x0) & 0x80) != 0)
                {
                    for (ushort fn = 1; fn < 8; fn++)
                    {
                        if (PCIDevice.GetVendorID(xBus, device, fn) != 0xFFFF)
                            CheckFunction(new PCIDevice(xBus, device, fn));
                    }
                }
            }
        }

        private static void CheckFunction(PCIDevice xPCIDevice)
        {
            if (xPCIDevice == null || xPCIDevice.VendorID == 0xFFFF)
            {
                return;
            }

            Devices.Add(xPCIDevice);

            if (xPCIDevice.ClassCode == 0x6 && xPCIDevice.Subclass == 0x4)
                CheckBus(xPCIDevice.SecondaryBusNumber);
        }

        public static bool Exists(PCIDevice pciDevice)
        {
            return GetDevice((VendorID)pciDevice.VendorID, (DeviceID)pciDevice.DeviceID) != null;
        }

        public static bool Exists(VendorID aVendorID, DeviceID aDeviceID)
        {
            return GetDevice(aVendorID, aDeviceID) != null;
        }

        /// <summary>
        /// Get device.
        /// </summary>
        /// <param name="aVendorID">A vendor ID.</param>
        /// <param name="aDeviceID">A device ID.</param>
        /// <returns></returns>
        public static PCIDevice GetDevice(VendorID aVendorID, DeviceID aDeviceID)
        {
            for(int i = 0; i < Devices.Count; i++)
            { 
                if ((VendorID)Devices[i].VendorID == aVendorID &&
                    (DeviceID)Devices[i].DeviceID == aDeviceID)
                {
                    return Devices[i];
                }
            }
            return null;
        }
        public static PCIDevice GetDevice(uint aVendorID, uint aDeviceID)
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                if ((VendorID)Devices[i].VendorID == aVendorID &&
                    (DeviceID)Devices[i].DeviceID == aDeviceID)
                {
                    return Devices[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Get device.
        /// </summary>
        /// <param name="bus">Bus ID.</param>
        /// <param name="slot">Slot position ID.</param>
        /// <param name="function">Function ID.</param>
        /// <returns></returns>
        public static PCIDevice GetDevice(uint bus, uint slot, uint function)
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                if (Devices[i].Bus == bus &&
                    Devices[i].Slot == slot &&
                    Devices[i].Function == function)
                {
                    return Devices[i];
                }
            }
            return null;
        }

        public static PCIDevice GetDeviceClass(ClassID Class, SubclassID SubClass)
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                if ((ClassID)Devices[i].ClassCode == Class &&
                    (SubclassID)Devices[i].Subclass == SubClass)
                {
                    return Devices[i];
                }
            }
            return null;
        }

        public static PCIDevice GetDeviceClass(ClassID aClass, SubclassID aSubClass, ProgramIF aProgIF)
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                if ((ClassID)Devices[i].ClassCode == aClass &&
                    (SubclassID)Devices[i].Subclass == aSubClass &&
                    (ProgramIF)Devices[i].ProgIF == aProgIF)
                {
                    return Devices[i];
                }
            }
            return null;
        }

        public static void WriteRegister32(ushort Bus, ushort Slot, ushort Function, byte aRegister, uint Value)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | ((uint)(aRegister & 0xFC));
            Native.Out32(0xCF8, xAddr);
            Native.Out32(0xCFC, Value);
        }

        public static uint ReadRegister32(ushort Bus, ushort Slot, ushort Function, byte aRegister)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | ((uint)(aRegister & 0xFC));
            Native.Out32(0xCF8, xAddr);
            return Native.In32(0xCFC);
        }

        public static ushort ReadRegister16(ushort Bus, ushort Slot, ushort Function, byte aRegister)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | ((uint)(aRegister & 0xFC));
            Native.Out32(0xCF8, xAddr);
            return (ushort)(Native.In32(0xCFC) >> ((aRegister % 4) * 8) & 0xFFFF);
        }

        public static byte ReadRegister8(ushort Bus, ushort Slot, ushort Function, byte aRegister)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | ((uint)(aRegister & 0xFC));
            Native.Out32(0xCF8, xAddr);
            return ((byte)(Native.In32(0xCFC) >> ((aRegister % 4) * 8) & 0xFF));
        }

        public static void WriteRegister16(ushort Bus, ushort Slot, ushort Function, byte aRegister, ushort Value)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | ((uint)(aRegister & 0xFC));
            Native.Out32(0xCF8, xAddr);
            Native.Out16(0xCFC, Value);
        }

        public static ushort GetVendorID(ushort Bus, ushort Slot, ushort Function)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | 0x0 & 0xFC;
            Native.Out32(0xCF8, xAddr);
            return (ushort)(Native.In32(0xCFC) >> ((0x0 % 4) * 8) & 0xFFFF);
        }

        public static ushort GetHeaderType(ushort Bus, ushort Slot, ushort Function)
        {
            uint xAddr = GetAddressBase(Bus, Slot, Function) | 0xE & 0xFC;
            Native.Out32(0xCF8, xAddr);
            return (byte)(Native.In32(0xCFC) >> ((0xE % 4) * 8) & 0xFF);
        }

        public static uint GetAddressBase(ushort Bus, uint Slot, uint Function)
        {
            return (uint)(0x80000000 | (Bus << 16) | ((Slot & 0x1F) << 11) | ((Function & 0x07) << 8));
        }
    }
}
