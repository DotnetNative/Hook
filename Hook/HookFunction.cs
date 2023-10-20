using Cetours;

namespace Hook;
public unsafe class HookFunction
{
    public HookFunction(Function origin, Function ripped)
    {
        Origin = origin;
        Ripped = ripped;

        NewFunc = Cetour.Create(Origin.Ptr, Ripped.Ptr, out len);
    }

    public Function Origin, Ripped;
    public bool Modified;
    public void* NewFunc;
    public void* CurrentOrigin => Modified ? NewFunc : Origin.Ptr;

    int len;

    public delegate void FuncActionDelegate(HookFunction sender);
    public event FuncActionDelegate? Attached;
    public event FuncActionDelegate? Detached;

    public HookFunction Attach()
    {
        if (Modified)
            return this;

        Modified = true;

        Cetour.Attach(Origin.Ptr, Ripped.Ptr, len);
        Attached?.Invoke(this);

        return this;
    }

    public HookFunction Detach()
    {
        if (!Modified)
            return this;

        Modified = false;

        Cetour.Detach(Origin.Ptr, NewFunc, len);
        Detached?.Invoke(this);

        return this;
    }

    public override string ToString() => $"HookFunction(M: {Modified}, O: {Origin}, R: {Ripped}, N: {((nint)NewFunc).ToString("X")})";

    public static explicit operator void*(HookFunction func) => func.CurrentOrigin;
    public static explicit operator nint(HookFunction func) => (nint)func.CurrentOrigin;
}