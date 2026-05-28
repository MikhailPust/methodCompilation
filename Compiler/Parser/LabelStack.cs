namespace Compilation.Interpreter.Parser;

public sealed class LabelStack
{
    private readonly Stack<int> _stack = new();

    public void Push(int address) => _stack.Push(address);

    public int Pop() => _stack.Pop();
    public int Peek() => _stack.Peek();

    public bool IsEmpty => _stack.Count == 0;
}