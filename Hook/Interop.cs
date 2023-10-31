using System.Runtime.InteropServices;

namespace Hook;

internal unsafe class Interop
{
    #region DLLImport
    const string kernel = "kernel32";

    [DllImport(kernel, CharSet = CharSet.Unicode)]
    internal static extern
        nint GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string moduleName);

    [DllImport(kernel, CharSet = CharSet.Ansi, ExactSpelling = true)]
    internal static extern
        nint GetProcAddress(nint hModule, string procName);
    #endregion
}