using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.Extensions
{
    /// <summary>
    /// Contains various helper methods to store UInt value into byte array.
    /// The UInt value would be stored in LittleEndian format.
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Store unsigned 16-bit integer value to a specified byte array, starting from a specified byte offset.
        /// </summary>
        /// <param name="n">A byte array to store the value into.</param>
        /// <param name="aPos">The offset (in bytes) from the start of the array, to where to store the value.</param>
        /// <param name="value">The value to be stored. 16-bit unsigned integer.</param>
        public static void SetUInt16(this byte[] n, ulong aPos, ushort value)
        {
            n[aPos + 0] = (byte)value;
            n[aPos + 1] = (byte)(value >> 8);
        }

        /// <summary>
        /// Store unsigned 32-bit integer value to a specified byte array, starting from a specified byte offset.
        /// </summary>
        /// <param name="n">A byte array to store the value into.</param>
        /// <param name="aPos">The offset (in bytes) from the start of the array, to where to store the value.</param>
        /// <param name="value">The value to be stored. 32-bit unsigned integer.</param>
        public static void SetUInt32(this byte[] n, ulong aPos, uint value)
        {
            n[aPos + 0] = (byte)value;
            n[aPos + 1] = (byte)(value >> 8);
            n[aPos + 2] = (byte)(value >> 16);
            n[aPos + 3] = (byte)(value >> 24);
        }

        /// <summary>
        /// Store unsigned 64-bit integer value to a specified byte array, starting from a specified byte offset.
        /// </summary>
        /// <param name="n">A byte array to store the value into.</param>
        /// <param name="aPos">The offset (in bytes) from the start of the array, to where to store the value.</param>
        /// <param name="value">The value to be stored. 64-bit unsigned integer.</param>
        public static void SetUInt64(this byte[] n, ulong aPos, ulong value)
        {
            n[aPos + 0] = (byte)value;
            n[aPos + 1] = (byte)(value >> 8);
            n[aPos + 2] = (byte)(value >> 16);
            n[aPos + 3] = (byte)(value >> 24);
            n[aPos + 4] = (byte)(value >> 32);
            n[aPos + 5] = (byte)(value >> 40);
            n[aPos + 6] = (byte)(value >> 48);
            n[aPos + 7] = (byte)(value >> 56);
        }
    }
}
