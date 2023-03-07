using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hook;

namespace Sample;
public unsafe class EntryPoint
{
    //[DllImport("Detours", CallingConvention = CallingConvention.Cdecl)]
    //public static extern int DetourAttach(void** pp, void* pd);

    private static string logPath = @"C:\log.txt";
    public static void Log(object obj)
    {
        File.AppendAllText(logPath, $"{obj}\n");
    }

    private static int counter = 0;
    private static void Show(object message)
    {
        Interop.MessageBox(0, message.ToString(), counter.ToString(),0);
        counter++;
    }

    [UnmanagedCallersOnly(EntryPoint = "MessageBox2")]
    public static void Sleep2(uint ms)
    {
        Show("WWWOOOWWW " + ms);
    }

    public void Load()
    {
        File.WriteAllText(logPath, "");
        Log($"Injected at {DateTime.Now}");

        IntPtr d = Interop.LoadLibrary(@"D:\VS\repos\Detours\x64\Debug\Detours.dll");

        IntPtr draw = Interop.GetProcAddress(d, "DllDetourRestoreAfterWith");
        IntPtr da = Interop.GetProcAddress(d, "DetourAttach");
        IntPtr dd = Interop.GetProcAddress(d, "DetourDetach");
        IntPtr dtb = Interop.GetProcAddress(d, "DetourTransactionBegin");
        IntPtr dut = Interop.GetProcAddress(d, "DetourUpdateThread");
        IntPtr dtc = Interop.GetProcAddress(d, "DetourTransactionCommit");

        void* pp = Interop.GetProcAddress(Interop.GetModuleHandle("kernel32"), "Sleep").ToPointer();
        void* pd = (delegate* unmanaged<uint, void>)&Sleep2;

        ((delegate* unmanaged<int>)draw.ToPointer())();
        ((delegate* unmanaged<int>)dtb.ToPointer())();
        ((delegate* unmanaged<IntPtr, int>)dut.ToPointer())(Interop.GetCurrentThread());

        ((delegate* unmanaged<void**, void*, IntPtr>) da.ToPointer())(&pp, pd);

        ((delegate* unmanaged<int>)dtc.ToPointer())();

        Show("I started");
        Interop.Sleep(5000);
        Show("I end");
    }

    public void Unload()
    {
        Log($"Uninjected at {DateTime.Now}");
    }
}