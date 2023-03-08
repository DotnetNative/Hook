using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        detours = Interop.LoadLibrary(@"D:\VS\repos\Detours\x64\Debug\Detours.dll");

        detourRestoreAfterWith = (delegate* unmanaged<int>)Interop.GetProcAddressPtr(detours, "DllDetourRestoreAfterWith");
        detourAttach = (delegate* unmanaged<void**, void*, IntPtr>)Interop.GetProcAddressPtr(detours, "DetourAttach");
        detourDetach = (delegate* unmanaged<void**, void*, IntPtr>)Interop.GetProcAddressPtr(detours, "DetourDetach");
        detourTransactionBegin = (delegate* unmanaged<int>)Interop.GetProcAddressPtr(detours, "DetourTransactionBegin");
        detourUpdateThread = (delegate* unmanaged<IntPtr, int>)Interop.GetProcAddressPtr(detours, "DetourUpdateThread");
        detourTransactionCommit = (delegate* unmanaged<int>)Interop.GetProcAddressPtr(detours, "DetourTransactionCommit");
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
    //public static IntPtr Attach(void* pe, void* nis) => detourAttach(&pe, nis);
    //public static IntPtr Detach(void* pe, void* nis) => detourDetach(&pe, nis);
}