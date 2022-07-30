using System;
using System.Collections.Generic;
using System.Text;

namespace MOOS.IO
{
    /// <summary>
    /// IOPortBase abstract class.
    /// </summary>
    public abstract class IOPortBase
    {
        protected readonly ushort Port;
        protected IOPortBase(ushort aPort)
        {
            Port = aPort;
        }

        protected IOPortBase(ushort aBase, ushort aOffset)
        {
            Port = (ushort)(aBase + aOffset);
        }

        static protected void Write8(ushort aPort, byte aData) 
        {
            Native.Out8(aPort, aData);
        }

        static protected void Write16(ushort aPort, ushort aData) 
        {
            Native.Out16(aPort, aData);
        }

        static protected void Write32(ushort aPort, uint aData) 
        {
            Native.Out32(aPort, aData);
        }

        static protected byte Read8(ushort aPort) { 
            return Native.In8(aPort); 
        }

        static protected ushort Read16(ushort aPort) { 
            return Native.In16(aPort); 
        }

        static protected uint Read32(ushort aPort) { 
            return Native.In32(aPort);
        }
        
        public void Read8(byte[] aData)
        {
            for (int i = 0; i < aData.Length / 2; i++)
            {
                var xValue = Read16(Port);
                aData[i * 2] = (byte)xValue;
                aData[i * 2 + 1] = (byte)(xValue >> 8);
            }
        }

        public void Read16(ushort[] aData)
        {
            for (int i = 0; i < aData.Length; i++)
            {
                aData[i] = Read16(Port);
            }
        }

        public void Read32(uint[] aData)
        {
            for (int i = 0; i < aData.Length; i++)
            {
                aData[i] = Read32(Port);
            }
        }
    }

    public class IOPort : IOPortBase
    {
        public IOPort(ushort aPort): base(aPort)
        {
        }

        public IOPort(ushort aBase, ushort aOffset) : base(aBase, aOffset)
        {
        }

        static public void Wait()
        {
            Write8(0x80, 0x22);
        }

        public byte Byte
        {
            get => Read8(Port);
            set => Write8(Port, value);
        }

        public ushort Word
        {
            get => Read16(Port);
            set => Write16(Port, value);
        }

        public uint DWord
        {
            get => Read32(Port);
            set => Write32(Port, value);
        }
    }
    public class IOPortRead : IOPortBase
    {
        public IOPortRead(ushort aPort) : base(aPort)
        {
        }

        public IOPortRead(ushort aBase, ushort aOffset) : base(aBase, aOffset)
        {
        }

        public byte Byte => Read8(Port);

        public ushort Word => Read16(Port);

        public uint DWord => Read32(Port);
    }

    public class IOPortWrite : IOPortBase
    {
        public IOPortWrite(ushort aPort) : base(aPort)
        {
        }

        public IOPortWrite(ushort aBase, ushort aOffset) : base(aBase, aOffset)
        {
        }

        public byte Byte
        {
            set => Write8(Port, value);
        }

        public ushort Word
        {
            set => Write16(Port, value);
        }

        public uint DWord
        {
            set => Write32(Port, value);
        }
    }
}
