using System.Diagnostics;
using System.Net;

namespace Hook;
public unsafe class HookApi
{
    public static void AltInit()
    {
        Load();
        Restore();
        Begin();
        UpdateCurrentThread();
    }

    public static void Load()
    {
        detours = LoadDetours();

        detourRestoreAfterWith = (delegate* unmanaged<int>)Interop.GetProcAddressPtr(detours, "DllDetourRestoreAfterWith");
        detourAttach = (delegate* unmanaged<void**, void*, IntPtr>)Interop.GetProcAddressPtr(detours, "DetourAttach");
        detourDetach = (delegate* unmanaged<void**, void*, IntPtr>)Interop.GetProcAddressPtr(detours, "DetourDetach");
        detourTransactionBegin = (delegate* unmanaged<int>)Interop.GetProcAddressPtr(detours, "DetourTransactionBegin");
        detourUpdateThread = (delegate* unmanaged<IntPtr, int>)Interop.GetProcAddressPtr(detours, "DetourUpdateThread");
        detourTransactionCommit = (delegate* unmanaged<int>)Interop.GetProcAddressPtr(detours, "DetourTransactionCommit");
    }

    private static IntPtr LoadDetours()
    {
        IntPtr handle;
        if ((handle = Interop.GetModuleHandle("Detours")) != IntPtr.Zero)
            return handle;

        try
        {
            List<string> checkedPaths = new List<string>();
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                string path = Path.GetDirectoryName(module.FileName);
                if (checkedPaths.Contains(path))
                    continue;

                checkedPaths.Add(path);

                if (path.Contains(@"C:\Windows\"))
                    continue;

                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (Path.GetFileName(file) == "Detours.dll")
                    {
                        return Interop.LoadLibrary(file);
                    }
                }
            }

            Interop.MessageBox(0, "Could not find Detours.dll near the main dll", "C# Exception", 0);
        }
        catch (Exception ex) { Interop.MessageBox(0, ex.Message, "C# Exception", 0); }

        return IntPtr.Zero;
    }

    private static IntPtr detours;
    private static delegate* unmanaged<int> detourRestoreAfterWith;
    private static delegate* unmanaged<void**, void*, IntPtr> detourAttach;
    private static delegate* unmanaged<void**, void*, IntPtr> detourDetach;
    private static delegate* unmanaged<int> detourTransactionBegin;
    private static delegate* unmanaged<IntPtr, int> detourUpdateThread;
    private static delegate* unmanaged<int> detourTransactionCommit;

    public static int Restore() => detourRestoreAfterWith();
    public static IntPtr Attach(void** pe, void* nis) => detourAttach(pe, nis);
    public static IntPtr Detach(void** pe, void* nis) => detourDetach(pe, nis);
    public static int Begin() => detourTransactionBegin();
    public static int UpdateThread(IntPtr handle) => detourUpdateThread(handle);
    public static int Commit() => detourTransactionCommit();

    public static int UpdateCurrentThread() => UpdateThread(Interop.GetCurrentThread());
}