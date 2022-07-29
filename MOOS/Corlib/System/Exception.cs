using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System
{
    public class Exception : ISerializable
    {
        public Exception()
        { 
        }

        public Exception(string? message)
        { 
        }

        public Exception(string? message, Exception? innerException)
        { 
        }
    }
}
