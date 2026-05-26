using Compilation.Interpreter.Lexer.Models;

namespace Compilation.Interpreter.Lexer;

public static class DfaTransitionTable
{
    private static readonly Dictionary<(State, CharClass), State> _table = new()
    {
        // S
        { (State.S, CharClass.Letter),       State.I },
        { (State.S, CharClass.Digit),        State.N },
        { (State.S, CharClass.WhiteSpace),   State.S },
        { (State.S, CharClass.Equal),        State.A },
        { (State.S, CharClass.Exclamation),  State.C },
        { (State.S, CharClass.Less),         State.E },
        { (State.S, CharClass.Greater),      State.H },
        { (State.S, CharClass.Plus),         State.Z },
        { (State.S, CharClass.Minus),        State.Z },
        { (State.S, CharClass.Star),         State.Z },
        { (State.S, CharClass.Slash),        State.Z },
        { (State.S, CharClass.LeftParen),    State.Z },
        { (State.S, CharClass.RightParen),   State.Z },
        { (State.S, CharClass.LeftBrace),    State.Z },
        { (State.S, CharClass.RightBrace),   State.Z },
        { (State.S, CharClass.LeftBracket),  State.Z },
        { (State.S, CharClass.RightBracket), State.Z },
        { (State.S, CharClass.Semicolon),    State.Z },
        { (State.S, CharClass.Comma),        State.Z },
        { (State.S, CharClass.EOF),          State.Z },

        // I — идентификатор
        { (State.I, CharClass.Letter), State.I },
        { (State.I, CharClass.Digit),  State.I },

        // N — целое число
        { (State.N, CharClass.Digit), State.N },
        { (State.N, CharClass.Dot),   State.F },

        // F — дробная часть
        { (State.F, CharClass.Digit), State.F },

        // A — '='  →  '==' или ASSIGN
        { (State.A, CharClass.Equal), State.B },

        // C — '!'  →  '!='
        { (State.C, CharClass.Equal), State.D },

        // E — '<'  →  '<=' или LT
        { (State.E, CharClass.Equal), State.G },

        // H — '>'  →  '>=' или GT
        { (State.H, CharClass.Equal), State.K },
    };

    public static State GetNextState(State current, CharClass charClass)
    {
        return _table.TryGetValue((current, charClass), out var next)
            ? next
            : State.ERR;
    }
}