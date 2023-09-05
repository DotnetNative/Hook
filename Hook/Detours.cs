using System.Diagnostics;

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

    static nint ProcAddr(string name) => Interop.GetProcAddress(detours, name);

    public static void Load()
    {
        detours = LoadDetours();

        detourRestoreAfterWith = (delegate* unmanaged<int>)ProcAddr("DllDetourRestoreAfterWith");
        detourAttach = (delegate* unmanaged<void**, void*, nint>)ProcAddr("DetourAttach");
        detourDetach = (delegate* unmanaged<void**, void*, nint>)ProcAddr("DetourDetach");
        detourTransactionBegin = (delegate* unmanaged<int>)ProcAddr("DetourTransactionBegin");
        detourUpdateThread = (delegate* unmanaged<nint, int>)ProcAddr("DetourUpdateThread");
        detourTransactionCommit = (delegate* unmanaged<int>)ProcAddr("DetourTransactionCommit");
    }

    static nint LoadDetours()
    {
        nint handle;
        if ((handle = Interop.GetModuleHandle("Detours")) != 0)
            return handle;

        try
        {
            var checkedPaths = new List<string>();
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                string path = Path.GetDirectoryName(module.FileName);
                if (path == null || checkedPaths.Contains(path))
                    continue;

                checkedPaths.Add(path);

                if (path.Contains(@"C:\Windows\"))
                    continue;

                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                    if (Path.GetFileName(file) == "Detours.dll")
                        return Interop.LoadLibrary(file);
            }

            Interop.MessageBox(0, "Could not find Detours.dll near the main dll", "C# Exception", 0);
        }
        catch (Exception ex) { Interop.MessageBox(0, ex.Message, "C# Exception", 0); }

        return 0;
    }

    static nint detours;
    static delegate* unmanaged<int> detourRestoreAfterWith;
    static delegate* unmanaged<void**, void*, nint> detourAttach;
    static delegate* unmanaged<void**, void*, nint> detourDetach;
    static delegate* unmanaged<int> detourTransactionBegin;
    static delegate* unmanaged<nint, int> detourUpdateThread;
    static delegate* unmanaged<int> detourTransactionCommit;

    public static int Restore() => detourRestoreAfterWith();
    public static nint Attach(void** pe, void* nis) => detourAttach(pe, nis);
    public static nint Detach(void** pe, void* nis) => detourDetach(pe, nis);
    public static int Begin() => detourTransactionBegin();
    public static int UpdateThread(nint handle) => detourUpdateThread(handle);
    public static int Commit() => detourTransactionCommit();

    public static int UpdateCurrentThread() => UpdateThread(Interop.GetCurrentThread());
}