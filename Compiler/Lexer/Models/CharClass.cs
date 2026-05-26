namespace Compilation.Interpreter.Lexer.Models;

public enum CharClass
{
    Letter,
    Digit,
    Dot,
    WhiteSpace,
    Plus,
    Minus,
    Star,
    Slash,
    Equal,
    Exclamation,
    Less,
    Greater,
    LeftParen,
    RightParen,
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,
    Semicolon,
    Comma,
    EOF,
    Other
}