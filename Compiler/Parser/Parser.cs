using Compilation.Interpreter.Lexer.Models;
using Compilation.Interpreter.Parser.Models;
using Compilation.Interpreter.Parser.Tables;

namespace Compilation.Interpreter.Parser;

public sealed class Parser
{
    private readonly List<Token> _tokens;
    private readonly List<OpsElement> _ops = new();
    private readonly VariableTable _varTable = new();
    private readonly ConstantTable _constTable = new();
    private readonly LabelStack _labelStack = new();

    private int _pos = 0;

    // принять список токенов от лексера
    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public List<OpsElement> Ops => _ops;
    public VariableTable VarTable => _varTable;
    public ConstantTable ConstTable => _constTable;
    public void Parse()
    {
        ParseStatementList();
        Expect(TokenType.EOF);
    }

    // посмотреть текущий токен без потребления
    private Token Current => _tokens[_pos];

    // потребить текущий токен и вернуть его
    private Token Consume()
    {
        var t = _tokens[_pos];
        _pos++;
        return t;
    }

    // потребить токен ожидаемого типа, иначе бросить синтаксическую ошибку
    private Token Expect(TokenType type)
    {
        if (Current.Type != type)
            throw new Exception(
                $"Ошибка: строка {Current.Line}, позиция {Current.Column} — " +
                $"ожидалось {type}, получено {Current.Type} '{Current.Value}'");
        return Consume();
    }

    // текущий размер ОПС 
    private int K => _ops.Count;

    // добавить элемент в конец ОПС
    private void Emit(OpsElement el) => _ops.Add(el);

    // СП 1 — после условия if/while
    private void Sem1()
    {
        _labelStack.Push(K);
        Emit(OpsElement.EmptyLabel());
        Emit(OpsElement.Op(OpCode.OP_JF));
    }

    // СП 2 — начало else
    private void Sem2()
    {
        var jfLabelAddr = _labelStack.Pop();
        _ops[jfLabelAddr].Value = K + 2;
        _labelStack.Push(K);
        Emit(OpsElement.EmptyLabel());
        Emit(OpsElement.Op(OpCode.OP_J));
    }

    // СП 3 — конец if/if-else: заполняет последнюю метку текущим адресом
    private void Sem3()
    {
        var labelAddr = _labelStack.Pop();
        _ops[labelAddr].Value = K;
    }

    // СП 4 — перед условием while: запоминает адрес начала цикла
    private void Sem4()
    {
        _labelStack.Push(K);
    }

    // СП 5 — после тела while: заполняет метку jf, кладёт обратный переход к началу цикла
    private void Sem5()
    {
        var jfLabelAddr = _labelStack.Pop();
        var loopStart = _labelStack.Pop();
        _ops[jfLabelAddr].Value = K + 2;
        Emit(OpsElement.Label(loopStart));
        Emit(OpsElement.Op(OpCode.OP_J));
    }

    // разобрать список операторов пока встречаются начала операторов
    private void ParseStatementList()
    {
        while (Current.Type is
            TokenType.ID or
            TokenType.IF or
            TokenType.WHILE or
            TokenType.READ or
            TokenType.WRITE or
            TokenType.LBRACE)
        {
            ParseStatement();
        }
    }

    // выбрать и разобрать один оператор по первому токену
    private void ParseStatement()
    {
        switch (Current.Type)
        {
            case TokenType.ID: ParseAssignment(); break;
            case TokenType.IF: ParseIf(); break;
            case TokenType.WHILE: ParseWhile(); break;
            case TokenType.READ: ParseRead(); break;
            case TokenType.WRITE: ParseWrite(); break;
            case TokenType.LBRACE: ParseBlock(); break;
            default:
                throw new Exception(
                    $"Ошибка: строка {Current.Line}, позиция {Current.Column} — " +
                    $"неожиданный токен '{Current.Value}'");
        }
    }

    // разобрать оператор присваивания (скалярного или индексного) и array(n)
    private void ParseAssignment()
    {
        var name = Expect(TokenType.ID).Value;
        var varIndex = _varTable.AddOrGet(name);

        if (Current.Type == TokenType.LBRACKET)
        {
            Emit(OpsElement.Var(varIndex));
            Consume();
            ParseExpression();
            Expect(TokenType.RBRACKET);
            Emit(OpsElement.Op(OpCode.OP_INDEX));
            Expect(TokenType.ASSIGN);
            ParseExpression();
            Expect(TokenType.SEMICOLON);
            Emit(OpsElement.Op(OpCode.OP_ASSIGN));
        }
        else
        {
            Expect(TokenType.ASSIGN);

            if (Current.Type == TokenType.ARRAY)
            {
                Emit(OpsElement.Var(varIndex));
                Consume();
                Expect(TokenType.LPAREN);
                ParseExpression();
                Expect(TokenType.RPAREN);
                Expect(TokenType.SEMICOLON);
                Emit(OpsElement.Op(OpCode.OP_ARRAY));
            }
            else
            {
                Emit(OpsElement.Var(varIndex));
                ParseExpression();
                Expect(TokenType.SEMICOLON);
                Emit(OpsElement.Op(OpCode.OP_ASSIGN));
            }
        }
    }

    // разобрать условный оператор if с опциональным else
    private void ParseIf()
    {
        Expect(TokenType.IF);
        Expect(TokenType.LPAREN);
        ParseCondition();
        Expect(TokenType.RPAREN);
        Sem1();
        ParseBlock();

        if (Current.Type == TokenType.ELSE)
        {
            Sem2();
            Consume();
            ParseBlock();
        }

        Sem3();
    }

    // разобрать цикл while
    private void ParseWhile()
    {
        Expect(TokenType.WHILE);
        Sem4();
        Expect(TokenType.LPAREN);
        ParseCondition();
        Expect(TokenType.RPAREN);
        Sem1();
        ParseBlock();
        Sem5();
    }

    // разобрать оператор ввода read (скалярного или индексного)
    private void ParseRead()
    {
        Expect(TokenType.READ);
        Expect(TokenType.LPAREN);

        var name = Expect(TokenType.ID).Value;
        var varIndex = _varTable.AddOrGet(name);
        Emit(OpsElement.Var(varIndex));

        if (Current.Type == TokenType.LBRACKET)
        {
            Consume();
            ParseExpression();
            Expect(TokenType.RBRACKET);
            Emit(OpsElement.Op(OpCode.OP_INDEX));
        }

        Emit(OpsElement.Op(OpCode.OP_READ));
        Expect(TokenType.RPAREN);
        Expect(TokenType.SEMICOLON);
    }

    // разобрать оператор вывода write с одним или несколькими аргументами
    private void ParseWrite()
    {
        Expect(TokenType.WRITE);
        Expect(TokenType.LPAREN);

        ParseWriteArg();

        while (Current.Type == TokenType.COMMA)
        {
            Consume();
            ParseWriteArg();
        }

        Emit(OpsElement.Op(OpCode.OP_WRITELN));
        Expect(TokenType.RPAREN);
        Expect(TokenType.SEMICOLON);
    }

    // разобрать один аргумент write — строку или выражение
    private void ParseWriteArg()
    {
        if (Current.Type == TokenType.STRING)
        {
            var strIndex = _constTable.AddOrGetString(Current.Value);
            Emit(OpsElement.StrConst(strIndex));
            Consume();
            Emit(OpsElement.Op(OpCode.OP_WRITE_STR));
        }
        else
        {
            ParseExpression();
            Emit(OpsElement.Op(OpCode.OP_WRITE));
        }
    }

    // разобрать блок операторов в фигурных скобках
    private void ParseBlock()
    {
        Expect(TokenType.LBRACE);
        ParseStatementList();
        Expect(TokenType.RBRACE);
    }

    // разобрать условие: левое выражение, оператор сравнения, правое выражение
    private void ParseCondition()
    {
        ParseExpression();
        var op = ParseRelOp();
        ParseExpression();
        Emit(OpsElement.Op(op));
    }

    // потребить оператор сравнения и вернуть соответствующий OpCode
    private OpCode ParseRelOp()
    {
        var type = Current.Type;
        Consume();
        return type switch
        {
            TokenType.LT => OpCode.OP_LT,
            TokenType.GT => OpCode.OP_GT,
            TokenType.LE => OpCode.OP_LE,
            TokenType.GE => OpCode.OP_GE,
            TokenType.EQ => OpCode.OP_EQ,
            TokenType.NE => OpCode.OP_NE,
            _ => throw new Exception(
                $"Ошибка: строка {Current.Line}, позиция {Current.Column} — " +
                $"ожидался оператор сравнения")
        };
    }

    // разобрать выражение: терм и хвост со сложением/вычитанием
    private void ParseExpression()
    {
        ParseTerm();
        ParseExpressionTail();
    }

    // разобрать хвост выражения: повторяющиеся +/- term
    private void ParseExpressionTail()
    {
        while (Current.Type is TokenType.PLUS or TokenType.MINUS)
        {
            var op = Current.Type == TokenType.PLUS ? OpCode.OP_ADD : OpCode.OP_SUB;
            Consume();
            ParseTerm();
            Emit(OpsElement.Op(op));
        }
    }

    // разобрать терм: множитель и хвост с умножением/делением
    private void ParseTerm()
    {
        ParseFactor();
        ParseTermTail();
    }

    // разобрать хвост терма: повторяющиеся */ factor
    private void ParseTermTail()
    {
        while (Current.Type is TokenType.MUL or TokenType.DIV)
        {
            var op = Current.Type == TokenType.MUL ? OpCode.OP_MUL : OpCode.OP_DIV;
            Consume();
            ParseFactor();
            Emit(OpsElement.Op(op));
        }
    }

    // разобрать множитель: число, переменная, скобки, унарный минус, функция
    private void ParseFactor()
    {
        switch (Current.Type)
        {
            case TokenType.NUMBER:
                {
                    var value = double.Parse(Current.Value,
                        System.Globalization.CultureInfo.InvariantCulture);
                    var constIdx = _constTable.AddOrGet(value);
                    Emit(OpsElement.Const(constIdx));
                    Consume();
                    break;
                }
            case TokenType.ID:
                {
                    var name = Consume().Value;
                    var varIndex = _varTable.AddOrGet(name);
                    Emit(OpsElement.Var(varIndex));

                    if (Current.Type == TokenType.LBRACKET)
                    {
                        Consume();
                        ParseExpression();
                        Expect(TokenType.RBRACKET);
                        Emit(OpsElement.Op(OpCode.OP_INDEX));
                    }
                    break;
                }
            case TokenType.LPAREN:
                {
                    Consume();
                    ParseExpression();
                    Expect(TokenType.RPAREN);
                    break;
                }
            case TokenType.MINUS:
                {
                    Consume();
                    ParseFactor();
                    Emit(OpsElement.Op(OpCode.OP_NEG));
                    break;
                }
            case TokenType.SQRT:
            case TokenType.EXP:
            case TokenType.LOG:
            case TokenType.ARRAY:
                {
                    ParseFunctionCall();
                    break;
                }
            default:
                throw new Exception(
                    $"Ошибка: строка {Current.Line}, позиция {Current.Column} — " +
                    $"неожиданный токен '{Current.Value}'");
        }
    }

    // разобрать вызов встроенной функции: имя ( выражение )
    private void ParseFunctionCall()
    {
        var op = Current.Type switch
        {
            TokenType.SQRT => OpCode.OP_SQRT,
            TokenType.EXP => OpCode.OP_EXP,
            TokenType.LOG => OpCode.OP_LOG,
            TokenType.ARRAY => OpCode.OP_ARRAY,
            _ => throw new Exception($"Неизвестная функция: {Current.Value}")
        };

        Consume();
        Expect(TokenType.LPAREN);
        ParseExpression();
        Expect(TokenType.RPAREN);
        Emit(OpsElement.Op(op));
    }
}