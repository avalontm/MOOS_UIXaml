using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MOOS.NET.IPv4
{
    /// <summary>
    /// EndPoint class.
    /// </summary>
    public class EndPoint //: IComparable
    {
        /// <summary>
        /// Address.
        /// </summary>
        public Address Address;
        /// <summary>
        /// Port.
        /// </summary>
        public ushort Port;

        /// <summary>
        /// Create new instance of the <see cref="EndPoint"/> class.
        /// </summary>
        /// <param name="addr">Adress.</param>
        /// <param name="port">Port.</param>
        public EndPoint(Address addr, ushort port)
        {
            Address = addr;
            Port = port;
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <returns>string value.</returns>
        public override string ToString()
        {
            return Address.ToString() + ":" + Port.ToString();
        }

        /// <summary>
        /// Compare end points.
        /// </summary>
        /// <param name="obj">Other end point to compare to.</param>
        /// <returns>-1 if end points are diffrent, 0 if equal.</returns>
        /// <exception cref="ArgumentException">Thrown if obj is not a EndPoint.</exception>
        public int CompareTo(object obj)
        {
            if (obj is EndPoint)
            {
                EndPoint other = (EndPoint)obj;

                if ((other.Address.CompareTo(Address) != 0) || (other.Port != Port))
                {
                    return -1;
                }

                return 0;
            }
            else
            {
                //throw new ArgumentException("obj is not a IPv4EndPoint", "obj");
                Console.WriteLine("obj is not a IPv4EndPoint");
                return -1;
            }
        }
    }
}
