using Cetours;
using Cetours.Hooking;

namespace Hook;
public unsafe class HookFunction
{
    public HookFunction(Function origin, Function ripped) : this(origin, ripped, new()) { }
    public HookFunction(Function origin, Function ripped, HookInnerData data)
    {
        Origin = origin;
        Ripped = ripped;

        hook = Cetour.Create((void*)origin, (void*)ripped, data);
    }

    public bool Modified;
    public readonly Function Origin, Ripped;

    public void* CurrentOrigin => Modified ? hook.New : Origin.Ptr;
    public string NameOrAddr => Origin.Name == null ? Origin.Addr.ToString("X") : $"{Origin.Name}({Origin.Addr.ToString("X")})";
    public string NameAddrOrAddr => Origin.ToString();
    public string NameOrSpace => Origin.Name == null ? "" : Origin.Name;

    readonly Cetours.Hooking.Hook hook;

    public delegate void FuncActionDelegate(HookFunction sender);
    public event FuncActionDelegate? Attached;
    public event FuncActionDelegate? Detached;

    public HookFunction Attach()
    {
        if (Modified)
            return this;

        Modified = true;

        hook.Attach();
        Attached?.Invoke(this);

        return this;
    }

    public HookFunction Detach()
    {
        if (!Modified)
            return this;

        Modified = false;

        hook.Detach();
        Detached?.Invoke(this);

        return this;
    }

    public override string ToString() => $"HookFunction(M: {Modified}, O: {Origin}, R: {Ripped}, N: {(nint)hook.New:X})";

    public static explicit operator void*(HookFunction func) => func.CurrentOrigin;
    public static explicit operator nint(HookFunction func) => (nint)func.CurrentOrigin;
}