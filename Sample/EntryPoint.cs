using System.Runtime.InteropServices;
using Hook;

namespace Sample;
public unsafe class EntryPoint
{
    [DllImport("user32", CharSet = CharSet.Auto)]
    static extern int MessageBox(nint hWnd, string text, string caption, uint type);

    [DllImport("kernel32")]
    static extern void Sleep(uint dwMilliseconds);

    static string logPath = @"C:\log.txt";
    public static void Log(object obj)
    {
        File.AppendAllText(logPath, $"{obj}\n");
    }

    static int counter = 0;
    static void Show(object message) => MessageBox(0, message == null ? "null" : message.ToString(), counter++.ToString(), 0);

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