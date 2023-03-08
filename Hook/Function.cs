using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hook;
public unsafe class Function
{
    public Function(IntPtr addr)
    {
        Addr = addr;
        Ptr = addr.ToPointer();
    }

    public Function(string dll, string name)
    {
        IntPtr module = Interop.GetModuleHandle(dll);
        Addr = Interop.GetProcAddress(module, name);
        Ptr = Addr.ToPointer();
    }

    public Function(string dllAndName)
    {
        string[] splitted = dllAndName.Split('.');
        IntPtr module = Interop.GetModuleHandle(splitted[0]);
        Addr = Interop.GetProcAddress(module, splitted[1]);
        Ptr = Addr.ToPointer();
    }

    public Function(void* ptr)
    {
        Addr = new IntPtr(ptr);
        Ptr = ptr;
    }

    public IntPtr Addr { get; private set; }
    public void* Ptr;

    public static explicit operator void*(Function func) => func.Ptr;
    public static implicit operator Function(void* ptr) => new Function(ptr);
}