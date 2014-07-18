# InteropDotNet #

The library allows you work with native libraries. Standard approach with the `DllImport` attribute may be inconvenient if you want to build AnyCPU assembly with MS.NET/Mono support. The `InteropRuntimeImplementer` class can generate implementation of interface with target signatures of native methods.

For example, let's create a native library (`NativeLib`) with the function `int sum(int a, int b) { return a + b; }` and build it in four configuration (Windows/Unix, x86/x64):

	x86/NativeLib.dll
	x86/libNativeLib.so
	x64/NativeLib.dll
	x64/libNativeLib.so

Now we can write the following code:

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

In the program, we declared interface `INative` with signatures of the target native methods. Each signature should be marked with the `RuntimeDllImport` attribute with name of a native library and other options. The instance of `InteropRuntimeImplementer` helped us to create instance of the `INative` interface on the fly. The implementation of the `sum` method call corresponding native method from library that correspond to current architecture and OS. The `InteropRuntimeImplementer` generates the following code:

    namespace InteropRuntimeImplementer.NativeInstance
    {
      [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = (CharSet) 0, SetLastError = false, ThrowOnUnmappableChar = false)]
      [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
      public delegate int SumDelegate(int a, int b);

      public class NativeImplementation : INative
      {
        private SumDelegate SumField;

        public NativeImplementation(LibraryLoader loader)
        {
          IntPtr dllHandle = loader.LoadLibrary("NativeLib", (string) null);
          this.SumField = (SumDelegate) Marshal.GetDelegateForFunctionPointer(loader.GetProcAddress(dllHandle, "sum"), typeof (SumDelegate));
        }

        public int Sum(int a, int b)
        {
          return this.SumField(a, b);
        }
      }
    } 


As a result, we received a single .NET cross-platform AnyCPU-program with calls of native methods because of the `LibraryLoader` class loaded handles for specific user environment.