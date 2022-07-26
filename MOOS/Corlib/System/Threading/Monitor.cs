#if Kernel
using Internal.Runtime.CompilerServices;
using MOOS;
using MOOS.Driver;
using MOOS.Misc;

namespace System.Threading
{
    public static unsafe class Monitor
    {
        public static void Enter(object obj)
        {
            if (ThreadPool.CanLock)
            {
                if(!ThreadPool.Locked)
                {
                    ThreadPool.Lock();
                }
            }
        }

        public static void Exit(object obj)
        {
            if (ThreadPool.CanLock)
            {
                if (ThreadPool.Locked)
                {
                    if (ThreadPool.Locker == SMP.ThisCPU)
                    {
                        ThreadPool.UnLock();
                    }
                }
            }
        }
    }
}
#endif