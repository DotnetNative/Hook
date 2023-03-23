<h1>NAOT Hook</h1>

A library for fuction hooking.\
Works with NAOT.\
NuGet - https://www.nuget.org/packages/Yotic.Hook/

<h1>Example</h1>

```
using Hook;

[DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

[DllImport("kernel32")]
static extern void Sleep(uint ms);

static HookFunction sleepHook;
static void Main()
{
   HookApi.AltInit();
   
   // Put hooks here
   sleepHook = new HookFunction("kernel32.Sleep", (delegate* unmanaged<uint, void>)&Sleep).Attach();
   
   HookApi.Commit();

   // Will be hooked
   Sleep(90000);

   // Will not be hooked
   ((delegate* unmanaged<uint, void>)sleepHook)(90000); // Can be used 'sleepHook', 'origin', or 'origin.Ptr'
}

[UnmanagedCallersOnly]
static void Sleep(uint ms)
{
    if (ReceiveSleep(ref ms))
        ((delegate* unmanaged<uint, void>)sleepHook)(ms);
}

static bool ReceiveSleep(ref uint ms)
{
    if (ms > 60000)
    {
        MessageBox(0, $"The sleep function was called, with a delay of more than a minute. The function has been cancelled (delay - {ms})", "Warning", 0);
        return false; // or ms = 0;
    }

    return true;
}
```
