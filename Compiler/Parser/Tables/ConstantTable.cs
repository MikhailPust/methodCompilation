namespace Compilation.Interpreter.Parser.Tables;

public sealed class ConstantTable
{
    private readonly List<double> _constants = new();

    // добавить константу если её ещё нет, вернуть индекс
    public int AddOrGet(double value)
    {
        var index = _constants.IndexOf(value);
        if (index != -1) return index;

        _constants.Add(value);
        return _constants.Count - 1;
    }

    public double GetValue(int index) => _constants[index];

    public int Count => _constants.Count;

    public void Print()
    {
        Console.WriteLine("=== Таблица констант ===");
        for (int i = 0; i < _constants.Count; i++)
            Console.WriteLine($"  [{i}] {_constants[i]}");
    }
}