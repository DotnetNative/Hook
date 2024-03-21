namespace Hook;
public unsafe class Function
{
    Function(string[] parts) : this(Interop.GetProcAddress(Interop.GetModuleHandle(parts[0]), parts[1])) { }

    public Function(void* ptr) => Ptr = ptr;
    public Function(nint addr) : this((void*)addr) { }
    public Function(string dllAndName) : this(dllAndName.Split('.')) { }
    public Function(string dll, string name) : this(new string[] { dll, name }) => Name = $"{dll}.{name}";

    public readonly void* Ptr;
    public readonly string? Name;

    public nint Addr => (nint)Ptr;

    public override string ToString() => Name == null ? Addr.ToString("X") : $"{Name}({Addr:X)})";

    public static explicit operator void*(Function func) => func.Ptr;
    public static explicit operator nint(Function func) => (nint)func.Ptr;
    public static implicit operator Function(void* ptr) => new Function(ptr);
    public static implicit operator Function(nint addr) => new Function(addr);
    public static implicit operator Function(string dllAndName) => new Function(dllAndName);
}