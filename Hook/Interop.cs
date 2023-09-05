using System.Runtime.InteropServices;

namespace Hook;

#region Enum
[Flags]
internal enum LoadLibraryFlags : uint
{
    None = 0,
    DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
    LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
    LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
    LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
    LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
    LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
    LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
    LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
    LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
    LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
    LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008,
    LOAD_LIBRARY_REQUIRE_SIGNED_TARGET = 0x00000080,
    LOAD_LIBRARY_SAFE_CURRENT_DIRS = 0x00002000,
}
#endregion
internal unsafe class Interop
{
    #region DLLImport
    const string user = "user32", kernel = "kernel32";

    [DllImport(user, CharSet = CharSet.Auto)] internal static extern 
        int MessageBox(nint hWnd, string text, string caption, uint type);


    [DllImport(kernel, CharSet = CharSet.Unicode)] internal static extern 
        nint GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string moduleName);

    [DllImport(kernel, CharSet = CharSet.Ansi, ExactSpelling = true)] internal static extern 
        nint GetProcAddress(nint hModule, string procName);

    [DllImport(kernel, CharSet = CharSet.Ansi)] internal static extern 
        nint LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport(kernel)] internal static extern 
        nint GetCurrentThread();    

    [DllImport(kernel)] internal static extern 
        nint LoadLibraryEx(string fileName, nint reservedNull, LoadLibraryFlags dwFlags);

    [DllImport(kernel)] internal static extern 
        void Sleep(uint ms);
    #endregion
}