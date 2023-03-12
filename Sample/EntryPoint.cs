using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hook;

namespace Sample;
public unsafe class EntryPoint
{
    [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("kernel32")]
    internal static extern void Sleep(uint dwMilliseconds);

    private static string logPath = @"C:\log.txt";
    public static void Log(object obj)
    {
        File.AppendAllText(logPath, $"{obj}\n");
    }

    private static int counter = 0;
    private static void Show(object message)
    {
        MessageBox(0, message.ToString(), counter.ToString(),0);
        counter++;
    }

    [UnmanagedCallersOnly(EntryPoint = "Sleep2")]
    public static void Sleep2(uint ms)
    {
        Show("sleep time: " + ms);
    }

    public void Load()
    {
        File.WriteAllText(logPath, "");
        Log($"Injected at {DateTime.Now}");

        HookApi.AltInit();

        Function origin = new Function("kernel32.Sleep");
        Function ripped = new Function((delegate* unmanaged<uint, void>)&Sleep2);
        HookFunction hook = new HookFunction(origin, ripped);

        hook.Attach();
        
        HookApi.Commit();

        Show("First");
        // Will be hooked
        Sleep(10000);

        Show("Second");
        // Will not be hooked
        ((delegate* unmanaged<uint, void>)hook)(10000); //can be used 'hook', 'origin', or 'origin.Ptr'
        Show("End");
    }

    public void Unload()
    {
        Log($"Uninjected at {DateTime.Now}");
    }
}