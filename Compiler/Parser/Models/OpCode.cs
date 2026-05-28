namespace Compilation.Interpreter.Parser.Models;

public enum OpCode
{
    OP_ADD,    // +
    OP_SUB,    // -
    OP_MUL,    // *
    OP_DIV,    // /
    OP_NEG,    // унарный минус

    OP_ASSIGN, // :=

    OP_INDEX,  // i — индексирование массива

    OP_LT,     // 
    OP_GT,     // >
    OP_LE,     // <=
    OP_GE,     // >=
    OP_EQ,     // ==
    OP_NE,     // !=

    OP_JF,     // jf — переход по false
    OP_J,      // j  — безусловный переход

    OP_READ,   // r  — ввод
    OP_WRITE,  // w  — вывод

    OP_SQRT,   // sqrt
    OP_EXP,    // exp
    OP_LOG,     // log
    OP_ARRAY,
    OP_WRITELN,
    OP_WRITE_STR  // вывод строки
}