using System;
using System.Runtime.InteropServices;
using InteropDotNet;

namespace ManagedApplication
{
    public interface INative
    {
        [RuntimeDllImport("NativeLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sum")]
        int Sum(int a, int b);
    }

    class Program
    {
        static void Main()
        {
            var native = InteropRuntimeImplementer.CreateInstance<INative>();
            Console.WriteLine("2 + 3 = " + native.Sum(2, 3));
        }
    }
}