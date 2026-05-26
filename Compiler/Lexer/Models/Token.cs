namespace Compilation.Interpreter.Lexer.Models;

public sealed class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Line { get; }
    public int Column { get; }

    public Token(TokenType type, string value, int line, int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }

    public override string ToString() =>
        $"[{Type,-12}] '{Value}' (строка {Line}, позиция {Column})";
}