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

    public HookFunction Attach()
    {
        if (Modified)
            return this;

        Modified = true;
        fixed (void** ptr = &Origin.Ptr)
            HookApi.Attach(ptr, Ripped.Ptr);

        return this;
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