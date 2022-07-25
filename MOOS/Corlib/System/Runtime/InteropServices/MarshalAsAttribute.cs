using System;
using System.Collections.Generic;


namespace System.Runtime.InteropServices
{
    public class MarshalAsAttribute : Attribute
    {
        UnmanagedType ArraySubType;
        int IidParameterIndex;
        string MarshalCookie;
        string MarshalType;
        int SizeConst;
        short SizeParamIndex;

        public MarshalAsAttribute(short unmanagedType)
        {
            ArraySubType = (UnmanagedType)unmanagedType;
        }

        public MarshalAsAttribute(UnmanagedType unmanagedType)
        {
            ArraySubType = unmanagedType;
        }

        public UnmanagedType Value { get { return ArraySubType; } }
    }
}
