using Compilation.Interpreter.Lexer.Models;

namespace Compilation.Interpreter.Lexer;
public static class LexerCharClassifier
{

    public static CharClass GetCharClass(char c)
    {
        if (char.IsLetter(c)) return CharClass.Letter;
        if (char.IsDigit(c)) return CharClass.Digit;

        return c switch
        {
            '.' => CharClass.Dot,
            ' ' or '\t' or '\r' or '\n' => CharClass.WhiteSpace,
            '"' => CharClass.Quote,
            '+' => CharClass.Plus,
            '-' => CharClass.Minus,
            '*' => CharClass.Star,
            '/' => CharClass.Slash,
            '=' => CharClass.Equal,
            '!' => CharClass.Exclamation,
            '<' => CharClass.Less,
            '>' => CharClass.Greater,
            '(' => CharClass.LeftParen,
            ')' => CharClass.RightParen,
            '{' => CharClass.LeftBrace,
            '}' => CharClass.RightBrace,
            '[' => CharClass.LeftBracket,
            ']' => CharClass.RightBracket,
            ';' => CharClass.Semicolon,
            ',' => CharClass.Comma,
            '\0' => CharClass.EOF,
            _ => CharClass.Other
        };
    }
}