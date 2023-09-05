using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sample;

public class Program
{
    [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int MessageBox(nint hWnd, string text, string caption, uint type);

    const uint DLL_PROCESS_ATTACH = 1;

    static bool loaded;

    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new Type[] { typeof(CallConvStdcall) })]
    public static bool DllMain(nint module, uint reason, nint reserved)
    {
        switch (reason)
        {
            case DLL_PROCESS_ATTACH:
                if (loaded)
                    return false;
                loaded = true;

                new EntryPoint().Load();
                break;
        }

        return true;
    }
}
