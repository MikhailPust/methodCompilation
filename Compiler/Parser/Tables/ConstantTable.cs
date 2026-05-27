namespace Compilation.Interpreter.Parser.Tables;

public sealed class ConstantTable
{
    private readonly List<double> _constants = new();
    private readonly List<string> _strings = new();

    public int AddOrGet(double value)
    {
        var index = _constants.IndexOf(value);
        if (index != -1) return index;
        _constants.Add(value);
        return _constants.Count - 1;
    }

    public int AddOrGetString(string value)
    {
        var index = _strings.IndexOf(value);
        if (index != -1) return index;
        _strings.Add(value);
        return _strings.Count - 1;
    }

    public double GetValue(int index) => _constants[index];
    public string GetString(int index) => _strings[index];
    public int Count => _constants.Count;

    public void Print()
    {
        Console.WriteLine("=== Таблица констант ===");
        for (int i = 0; i < _constants.Count; i++)
            Console.WriteLine($"  [{i}] {_constants[i]}");
        for (int i = 0; i < _strings.Count; i++)
            Console.WriteLine($"  [s{i}] \"{_strings[i]}\"");
    }
}