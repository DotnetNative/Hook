using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hook;
public unsafe class Function
{
    public Function(IntPtr addr)
    {
        Addr = addr;
    }

    public Function(string dll, string name)
    {
        IntPtr module = Interop.GetModuleHandle(dll);
        Addr = Interop.GetProcAddress(module, name);
    }

    public Function(void* link)
    {
        Addr = new IntPtr(link);
    }

    public IntPtr Addr { get; private set; }
}