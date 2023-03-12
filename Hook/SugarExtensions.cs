using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hook;
internal static unsafe class SugarExtensions
{
    public static void** Ptr(void* ptr) => &ptr;

    public static char* Ptr(this string str)
    {
        fixed (char* cptr = str)
            return cptr;
    }

    public static byte* AnsiPtrTest(this string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        Array.Resize(ref bytes, bytes.Length + 1);
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        return (byte*)handle.AddrOfPinnedObject().ToPointer();
    }

    public static byte* AnsiPtr(this string str)
    {
        str += '\0';
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        return bytes.Ptr();
    }

    public static byte* UnicodePtr(this string str)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(str);
        fixed (byte* cbytes = bytes)
            return cbytes;
    }

    public static T* Ptr<T>(this T obj) where T : unmanaged
    {
        TypedReference reference = __makeref(obj);
        IntPtr addr = (IntPtr)(&reference);
        return (T*)addr.ToPointer();
    }

    public static T* Ptr<T>(this T[] obj) where T : unmanaged
    {
        fixed (T* ptr = obj)
            return ptr;
    }

    public static T[] ToArr<T>(this T[] arr, T* ptr)
    {
        for (int i = 0; i < arr.Length; i++)
            arr[i] = *(ptr + i);
        return arr;
    }

    public static IntPtr Addr<T>(this T obj) where T : unmanaged
    {
        TypedReference reference = __makeref(obj);
        return (IntPtr)(&reference);
    }

    public static T ToStruct<T>(this IntPtr addr) where T : struct => Marshal.PtrToStructure<T>(addr);
}