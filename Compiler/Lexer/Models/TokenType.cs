namespace Compilation.Interpreter.Lexer.Models;

public enum TokenType
{
    ID,
    NUMBER,

    PLUS,
    MINUS,
    MUL,
    DIV,

    ASSIGN,
    SEMICOLON,
    COMMA,

    LPAREN,
    RPAREN,
    LBRACE,
    RBRACE,
    LBRACKET,
    RBRACKET,

    LT,
    GT,
    LE,
    GE,
    EQ,
    NE,

    IF,
    ELSE,
    WHILE,
    READ,
    WRITE,
    SQRT,
    EXP,
    LOG,
    ARRAY,

    EOF
}