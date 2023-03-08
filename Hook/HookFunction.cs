using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hook;
public unsafe class HookFunction
{
    public HookFunction(Function origin, Function ripped)
    {
        Origin = origin;
        Ripped = ripped;
    }

    public Function Origin { get; private set; }
    public Function Ripped { get; private set; }
    public bool Modified { get; private set; }

    public void Attach()
    {
        if (Modified)
            return;

        Modified = true;
        fixed (void** ptr = &Origin.Ptr)
            HookApi.Attach(ptr, Ripped.Ptr);
    }

    public void Detach()
    {
        if (!Modified)
            return;

        Modified = false;
        fixed (void** ptr = &Origin.Ptr)
            HookApi.Detach(ptr, Ripped.Ptr);
    }

    public static explicit operator void*(HookFunction func) => func.Origin.Ptr;
}