namespace Compilation.Interpreter.Parser.Models;

public enum OpsElementType
{
    TYPE_VAR,       // переменная
    TYPE_CONST,     // константа (число)
    TYPE_STR_CONST, //  строковая константа
    TYPE_LABEL,     // метка (адрес перехода в ОПС)
    TYPE_OP         // операция
}