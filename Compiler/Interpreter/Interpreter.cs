using Compilation.Interpreter.Parser.Models;
using Compilation.Interpreter.Parser.Tables;

namespace Compilation.Interpreter.Interpreter;

public sealed class Interpreter
{
    private readonly List<OpsElement> _ops;
    private readonly VariableTable _varTable;
    private readonly ConstantTable _constTable;

    private readonly Stack<StackItem> _stack = new();
    private readonly Dictionary<int, double> _varMemory = new();
    private readonly Dictionary<long, double> _arrayMemory = new();
    private readonly Dictionary<int, double[]> _arraySize = new();

    private int _ip = 0;

    public Interpreter(
        List<OpsElement> ops,
        VariableTable varTable,
        ConstantTable constTable)
    {
        _ops = ops;
        _varTable = varTable;
        _constTable = constTable;
    }

    public void Run()
    {
        while (_ip < _ops.Count)
        {
            var el = _ops[_ip];
            _ip++;

            switch (el.Type)
            {
                case OpsElementType.TYPE_VAR:
                    _stack.Push(StackItem.Ref((int)el.Value));
                    break;

                case OpsElementType.TYPE_CONST:
                    _stack.Push(StackItem.Val(_constTable.GetValue((int)el.Value)));
                    break;

                case OpsElementType.TYPE_STR_CONST: 
                    _stack.Push(StackItem.Val((int)el.Value)); // Просто кладем индекс на стек
                    break;

                case OpsElementType.TYPE_LABEL:
                    _stack.Push(StackItem.Val((int)el.Value));
                    break;

                case OpsElementType.TYPE_OP:
                    ExecuteOp((OpCode)el.Value);
                    break;
            }
        }
    }

    private void ExecuteOp(OpCode op)
    {
        switch (op)
        {
            case OpCode.OP_ADD:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a + b));
                    break;
                }
            case OpCode.OP_SUB:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a - b));
                    break;
                }
            case OpCode.OP_MUL:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a * b));
                    break;
                }
            case OpCode.OP_DIV:
                {
                    var b = PopValue(); var a = PopValue();
                    if (b == 0) throw new Exception("Ошибка: деление на ноль");
                    _stack.Push(StackItem.Val(a / b));
                    break;
                }
            case OpCode.OP_NEG:
                {
                    _stack.Push(StackItem.Val(-PopValue()));
                    break;
                }
            case OpCode.OP_ASSIGN:
                {
                    var value = PopValue();
                    var item = _stack.Pop();

                    if (item.IsArrayRef)
                        _arrayMemory[item.ArrayKey] = value;
                    else
                        _varMemory[item.VarIndex] = value;
                    break;
                }
            case OpCode.OP_INDEX:
                {
                    var index = (int)PopValue();
                    var varItem = _stack.Pop();
                    _stack.Push(StackItem.ArrayRef(varItem.VarIndex, index));
                    break;
                }
            case OpCode.OP_ARRAY:
                {
                    var size = (int)PopValue();
                    var varItem = _stack.Pop();
                    _arraySize[varItem.VarIndex] = new double[size];
                    break;
                }
            case OpCode.OP_JF:
                {
                    var addr = (int)PopValue();
                    var condition = PopValue();
                    if (condition == 0) _ip = addr;
                    break;
                }
            case OpCode.OP_J:
                {
                    _ip = (int)PopValue();
                    break;
                }
            case OpCode.OP_READ:
                {
                    var item = _stack.Pop();
                    double val;

                    while (true)
                    {
                     
                        Console.Write("> ");
                        var input = Console.ReadLine();

                        if (double.TryParse(input,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out val))
                            break;

                        Console.WriteLine("Ошибка: введите число");
                    }

                    if (item.IsArrayRef)
                        _arrayMemory[item.ArrayKey] = val;
                    else
                        _varMemory[item.VarIndex] = val;
                    break;
                }
            case OpCode.OP_WRITE:
                {
                    Console.WriteLine(PopValue());
                    break;
                }
            case OpCode.OP_WRITE_STR:
                {
                    var idx = (int)PopValue();
                    Console.WriteLine(_constTable.GetString(idx));
                    break;
                }
            case OpCode.OP_LT:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a < b ? 1 : 0));
                    break;
                }
            case OpCode.OP_GT:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a > b ? 1 : 0));
                    break;
                }
            case OpCode.OP_LE:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a <= b ? 1 : 0));
                    break;
                }
            case OpCode.OP_GE:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a >= b ? 1 : 0));
                    break;
                }
            case OpCode.OP_EQ:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a == b ? 1 : 0));
                    break;
                }
            case OpCode.OP_NE:
                {
                    var b = PopValue(); var a = PopValue();
                    _stack.Push(StackItem.Val(a != b ? 1 : 0));
                    break;
                }
            case OpCode.OP_SQRT:
                _stack.Push(StackItem.Val(Math.Sqrt(PopValue())));
                break;
            case OpCode.OP_EXP:
                _stack.Push(StackItem.Val(Math.Exp(PopValue())));
                break;
            case OpCode.OP_LOG:
                _stack.Push(StackItem.Val(Math.Log(PopValue())));
                break;
        }
    }

    private double PopValue()
    {
        var item = _stack.Pop();

        if (item.IsArrayRef)
        {
            _arrayMemory.TryGetValue(item.ArrayKey, out var arrVal);
            return arrVal;
        }

        if (item.IsRef)
        {
            _varMemory.TryGetValue(item.VarIndex, out var varVal);
            return varVal;
        }

        return item.RawValue;
    }

    private readonly struct StackItem
    {
        public bool IsRef { get; }
        public bool IsArrayRef { get; }
        public int VarIndex { get; }
        public long ArrayKey { get; }
        public double RawValue { get; }

        private StackItem(bool isRef, bool isArrayRef, int varIndex, long arrayKey, double raw)
        {
            IsRef = isRef;
            IsArrayRef = isArrayRef;
            VarIndex = varIndex;
            ArrayKey = arrayKey;
            RawValue = raw;
        }

        public static StackItem Ref(int varIndex) =>
            new(true, false, varIndex, 0, 0);

        public static StackItem ArrayRef(int varIndex, int arrIndex) =>
            new(false, true, varIndex, (long)varIndex * 100000 + arrIndex, 0);

        public static StackItem Val(double value) =>
            new(false, false, 0, 0, value);
    }
}