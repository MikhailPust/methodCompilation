using Compilation.Interpreter.Lexer.Models;
using System.Text;

namespace Compilation.Interpreter.Lexer;

public sealed class Lexer
{
    private readonly string _source;
    private int _position;
    private int _line = 1;
    private int _column = 1;

    public Lexer(string source)
    {
        _source = source;
    }

    public Token NextToken()
    {
        var state = State.S;
        var buffer = new StringBuilder();
        var tokenLine = _line;
        var tokenColumn = _column;

        while (true)
        {
            var c = PeekChar();
            var charClass = LexerCharClassifier.GetCharClass(c);
            var nextState = DfaTransitionTable.GetNextState(state, charClass);

            // пропуск пробелов
            if (state == State.S && charClass == CharClass.WhiteSpace)
            {
                Advance(c);
                tokenLine = _line;
                tokenColumn = _column;
                continue;
            }

            // строковый литерал
            if (state == State.S && charClass == CharClass.Quote)
            {
                Advance(c); // пропускаем открывающую кавычку
                while (true)
                {
                    var sc = PeekChar();
                    if (sc == '\0')
                        throw new Exception($"Ошибка: строка {_line}, позиция {_column} — незакрытая строка");
                    if (sc == '"')
                    {
                        Advance(sc); // пропускаем закрывающую кавычку
                        break;
                    }
                    buffer.Append(sc);
                    Advance(sc);
                }
                return new Token(TokenType.STRING, buffer.ToString(), tokenLine, tokenColumn);
            }

            if (charClass == CharClass.EOF)
                return new Token(TokenType.EOF, string.Empty, _line, _column);

            // односимвольная лексема
            if (state == State.S && nextState == State.Z)
            {
                Advance(c);
                return BuildSingleCharToken(c, tokenLine, tokenColumn);
            }

            if (nextState == State.ERR)
            {
                if (IsReturnState(state))
                    return BuildToken(state, buffer.ToString(), tokenLine, tokenColumn);

                throw new Exception(
                    $"Ошибка: строка {_line}, позиция {_column} — недопустимый символ '{c}'");
            }

            // двухсимвольный финал (==, !=, <=, >=)
            if (IsFinalTwoCharState(nextState))
            {
                buffer.Append(c);
                Advance(c);
                return BuildToken(nextState, buffer.ToString(), tokenLine, tokenColumn);
            }

            // накапливаем символ
            state = nextState;
            buffer.Append(c);
            Advance(c);
        }
    }

    private static bool IsReturnState(State s) => s is
        State.I or State.N or State.F or
        State.A or State.E or State.H;

    private static bool IsFinalTwoCharState(State s) => s is
        State.B or State.D or State.G or State.K;

    private static Token BuildToken(State state, string value, int line, int col)
    {
        return state switch
        {
            State.I => BuildKeywordOrId(value, line, col),
            State.N or
            State.F => new Token(TokenType.NUMBER, value, line, col),
            State.A => new Token(TokenType.ASSIGN, value, line, col),
            State.E => new Token(TokenType.LT, value, line, col),
            State.H => new Token(TokenType.GT, value, line, col),
            State.B => new Token(TokenType.EQ, value, line, col),
            State.D => new Token(TokenType.NE, value, line, col),
            State.G => new Token(TokenType.LE, value, line, col),
            State.K => new Token(TokenType.GE, value, line, col),
            _ => throw new Exception($"Неизвестное состояние: {state}")
        };
    }

    private static Token BuildKeywordOrId(string value, int line, int col)
    {
        TokenType type = value switch
        {
            "if" => TokenType.IF,
            "else" => TokenType.ELSE,
            "while" => TokenType.WHILE,
            "read" => TokenType.READ,
            "write" => TokenType.WRITE,
            "sqrt" => TokenType.SQRT,
            "exp" => TokenType.EXP,
            "log" => TokenType.LOG,
            "array" => TokenType.ARRAY,
            _ => TokenType.ID
        };
        return new Token(type, value, line, col);
    }

    private static Token BuildSingleCharToken(char c, int line, int col)
    {
        TokenType type = c switch
        {
            '+' => TokenType.PLUS,
            '-' => TokenType.MINUS,
            '*' => TokenType.MUL,
            '/' => TokenType.DIV,
            '(' => TokenType.LPAREN,
            ')' => TokenType.RPAREN,
            '{' => TokenType.LBRACE,
            '}' => TokenType.RBRACE,
            '[' => TokenType.LBRACKET,
            ']' => TokenType.RBRACKET,
            ';' => TokenType.SEMICOLON,
            ',' => TokenType.COMMA,
            _ => throw new Exception($"Неизвестный символ: '{c}'")
        };
        return new Token(type, c.ToString(), line, col);
    }

    private char PeekChar() =>
        _position < _source.Length ? _source[_position] : '\0';

    private void Advance(char c)
    {
        _position++;
        if (c == '\n') { _line++; _column = 1; }
        else { _column++; }
    }
}