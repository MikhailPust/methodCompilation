namespace Compilation.Interpreter.Parser.Models;

public enum OpsElementType
{
    TYPE_VAR,    // переменная
    TYPE_CONST,  // константа
    TYPE_LABEL,  // метка (адрес перехода в ОПС)
    TYPE_OP      // операция
}