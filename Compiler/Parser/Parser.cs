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

    private Token Current => _tokens[_pos];

    private Token Consume()
    {
        var t = _tokens[_pos];
        _pos++;
        return t;
    }

    private Token Expect(TokenType type)
    {
        if (Current.Type != type)
            throw new Exception(
                $"Ошибка: строка {Current.Line}, позиция {Current.Column} — " +
                $"ожидалось {type}, получено {Current.Type} '{Current.Value}'");
        return Consume();
    }

    private int K => _ops.Count;

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

    // СП 3 — конец if/if-else
    private void Sem3()
    {
        var labelAddr = _labelStack.Pop();
        _ops[labelAddr].Value = K;
    }

    // СП 4 — перед условием while
    private void Sem4()
    {
        _labelStack.Push(K);
    }

    // СП 5 — после тела while
    private void Sem5()
    {
        var jfLabelAddr = _labelStack.Pop();
        var loopStart = _labelStack.Pop();
        _ops[jfLabelAddr].Value = K + 2;
        Emit(OpsElement.Label(loopStart));
        Emit(OpsElement.Op(OpCode.OP_J));
    }

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

            // array(n) — особый случай, передаём varIndex в стек до вызова
            if (Current.Type == TokenType.ARRAY)
            {
                Emit(OpsElement.Var(varIndex));
                Consume(); // array
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

    private void ParseWrite()
    {
        Expect(TokenType.WRITE);
        Expect(TokenType.LPAREN);

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

        Expect(TokenType.RPAREN);
        Expect(TokenType.SEMICOLON);
    }
    private void ParseBlock()
    {
        Expect(TokenType.LBRACE);
        ParseStatementList();
        Expect(TokenType.RBRACE);
    }

    private void ParseCondition()
    {
        ParseExpression();
        var op = ParseRelOp();
        ParseExpression();
        Emit(OpsElement.Op(op));
    }

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

    private void ParseExpression()
    {
        ParseTerm();
        ParseExpressionTail();
    }

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

    private void ParseTerm()
    {
        ParseFactor();
        ParseTermTail();
    }

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