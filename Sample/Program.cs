using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sample;

public class Program
{
    const uint DLL_PROCESS_ATTACH = 1;

    static bool loaded;

    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = [typeof(CallConvStdcall)])]
    public static bool DllMain(nint module, uint reason, nint reserved)
    {
        if (reason == DLL_PROCESS_ATTACH)
        {
            if (loaded)
                return true;

            loaded = true;

            EntryPoint.Load();
        }

        return true;
    }
}
