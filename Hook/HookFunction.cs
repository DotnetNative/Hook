namespace Hook;
public unsafe record HookFunction(Function Origin, Function Ripped)
{
    public bool Modified;

    public HookFunction Attach()
    {
        if (Modified)
            return this;

        Modified = true;
        fixed (void** ptr = &Origin.Ptr)
            HookApi.Attach(ptr, Ripped.Ptr);

        return this;
    }

    public HookFunction Detach()
    {
        if (!Modified)
            return this;

        Modified = false;
        fixed (void** ptr = &Origin.Ptr)
            HookApi.Detach(ptr, Ripped.Ptr);

        return this;
    }

    public static explicit operator void*(HookFunction func) => func.Origin.Ptr;
    public static explicit operator nint(HookFunction func) => func.Origin.Addr;
}