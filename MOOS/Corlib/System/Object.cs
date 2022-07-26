using Internal.Runtime;
using Internal.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    public unsafe class Object
    {
        // The layout of object is a contract with the compiler.
        internal unsafe EEType* m_pEEType;

        [StructLayout(LayoutKind.Sequential)]
        private class RawData
        {
            public byte Data;
        }

        internal ref byte GetRawData()
        {
            return ref Unsafe.As<RawData>(this).Data;
        }

        internal uint GetRawDataSize()
        {
            return m_pEEType->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(EEType*);
        }

        public Object() { }
        ~Object() { }


        public virtual bool Equals(object o)
            => false;

        public virtual int GetHashCode()
            => 0;

        public virtual string ToString()
        {
             return "System.Object";
        }
        public virtual string ToString(string value)
        {
            return value;
        }

        public virtual void Dispose()
        {
#if Kernel
            var obj = this;
            Allocator.Free(Unsafe.As<object, IntPtr>(ref obj));
#endif
        }

        public static implicit operator bool(object obj)=> obj != null;
    }
}