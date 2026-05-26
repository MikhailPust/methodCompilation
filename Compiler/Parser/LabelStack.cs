namespace Compilation.Interpreter.Parser;

public sealed class LabelStack
{
    private readonly Stack<int> _stack = new();

    // положить адрес в магазин меток
    public void Push(int address) => _stack.Push(address);

    // взять адрес из магазина меток
    public int Pop() => _stack.Pop();

    // посмотреть верхний адрес без извлечения
    public int Peek() => _stack.Peek();

    public bool IsEmpty => _stack.Count == 0;
}