namespace Compilation.Interpreter.Parser.Models;

public sealed class OpsElement
{
    public OpsElementType Type { get; }
    public object Value { get; set; } = -1;

    private OpsElement(OpsElementType type, object value)
    {
        Type = type;
        Value = value;
    }

    public static OpsElement EmptyLabel() =>
        new(OpsElementType.TYPE_LABEL, -1);

    public static OpsElement Op(OpCode code) =>
        new(OpsElementType.TYPE_OP, code);

    public static OpsElement Var(int index) =>
        new(OpsElementType.TYPE_VAR, index);

    public static OpsElement Const(int index) =>
        new(OpsElementType.TYPE_CONST, index);

    public static OpsElement Label(int address) =>
        new(OpsElementType.TYPE_LABEL, address);

    public override string ToString() => Type switch
    {
        OpsElementType.TYPE_VAR => $"VAR({Value})",
        OpsElementType.TYPE_CONST => $"CONST({Value})",
        OpsElementType.TYPE_LABEL => $"LABEL({Value})",
        OpsElementType.TYPE_OP => $"{Value}",
        _ => Value.ToString()!
    };
}