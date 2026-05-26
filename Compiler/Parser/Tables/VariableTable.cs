namespace Compilation.Interpreter.Parser.Tables;

public sealed class VariableTable
{
    private readonly List<string> _variables = new();

    // добавить переменную если её ещё нет, вернуть индекс
    public int AddOrGet(string name)
    {
        var index = _variables.IndexOf(name);
        if (index != -1) return index;

        _variables.Add(name);
        return _variables.Count - 1;
    }

    public string GetName(int index) => _variables[index];

    public int Count => _variables.Count;

    public void Print()
    {
        Console.WriteLine("=== Таблица переменных ===");
        for (int i = 0; i < _variables.Count; i++)
            Console.WriteLine($"  [{i}] {_variables[i]}");
    }
}