<h1>Hook</h1>

Internal Hooking library for Native code \
NuGet - https://www.nuget.org/packages/Yotic.Hook/

<h2>Warning</h2>

Cetours is newly created library and may contain bugs. Please, if you can’t hook a some function, then create a new issue about it.

<h1>Example</h1>

```
using System.Runtime.InteropServices;
using Hook;

namespace Sample;
public unsafe class EntryPoint
{
    [DllImport("user32", CharSet = CharSet.Auto)]
    static extern int MessageBox(nint hWnd, string text, string caption, uint type);

    [DllImport("kernel32")]
    static extern void Sleep(uint dwMilliseconds);

    static int counter = 0;
    static void Show(object message) => MessageBox(0, message == null ? "null" : message.ToString(), counter++.ToString(), 0);

    [UnmanagedCallersOnly]
    public static void HookedSleep(uint ms)
    {
        Show("sleep time: " + ms);
    }

    public void Load()
    {
        var origin = new Function("kernel32.Sleep");
        var ripped = new Function((delegate* unmanaged<uint, void>)&HookedSleep);
        var hook = new HookFunction(origin, ripped);

        hook.Attach();

        Show("Call original (will call HookedSleep)");
        Sleep(10000);

        Show("Call hook (will call original function)");
        ((delegate* unmanaged<uint, void>)hook)(10000);
        Show("End");
    }
}
```

<h2>Setup for build</h2>

Remove reference to local library **Cetours** and add **Cetours** via Nuget
