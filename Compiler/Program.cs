using Compilation.Interpreter.Lexer;
using Compilation.Interpreter.Lexer.Models;
using Compilation.Interpreter.Parser;
using Compilation.Interpreter.Interpreter;

void Run(string source)
{
    var lexer = new Lexer(source);
    var tokens = new List<Token>();
    Token token;
    do
    {
        token = lexer.NextToken();
        tokens.Add(token);
    }
    while (token.Type != TokenType.EOF);

    var parser = new Parser(tokens);
    parser.Parse();

    var interpreter = new Interpreter(parser.Ops, parser.VarTable, parser.ConstTable);
    interpreter.Run();
}

// ТЕСТ 1 — Простые формулы и вычисления
Console.WriteLine("=== Тест 1: Формулы ===");
Run("""
a = 10;
b = 3;
write(a + b);
write(a - b);
write(a * b);
write(a / b);
write(sqrt(a));
write(exp(1));
write(log(a));
write("a= ", a);
""");

// ТЕСТ 2 — Условный оператор if-else
Console.WriteLine("=== Тест 2: Условный оператор ===");
Run("""
write("Введите любое число");
read(x);
if (x > 0) {
    write(1);
} else {
    if (x < 0) {
        write(-1);
    } else {
        write(0);
    }
}
""");

// ТЕСТ 3 — Цикл while, сумма от 1 до n
Console.WriteLine("=== Тест 3: Сумма от 1 до n ===");
Run("""
write("Введите любое число n");
read(n);
sum = 0;
i = 1;
while (i <= n) {
    sum = sum + i;
    i = i + 1;
}
write(sum);
""");

// ТЕСТ 4 — Массив: ввод и вывод
Console.WriteLine("=== Тест 4: Ввод и вывод массива ===");
Run("""
n = 5;
M = array(n);
i = 0;
while (i < n) {
    write("Введите элемент массива номер");
    write(i+1);
    read(M[i]);
    i = i + 1;
}
i = 0;
while (i < n) {
    write(M[i]);
    i = i + 1;
}
""");

// ТЕСТ 5 — Пузырьковая сортировка
Console.WriteLine("=== Тест 5: Пузырьковая сортировка ===");
Run("""
n = 5;
M = array(n);
i = 0;
while (i < n) {
    write("Введите элемент массива номер");
    write(i+1);
    read(M[i]);
    i = i + 1;
}
i = 0;
while (i < n) {
    j = 0;
    while (j < n - 1) {
        if (M[j] > M[j + 1]) {
            tmp = M[j];
            M[j] = M[j + 1];
            M[j + 1] = tmp;
        }
        j = j + 1;
    }
    i = i + 1;
}
k = 0;
while (k < n) {
    write(M[k]);
    k = k + 1;
}
""");

// ТЕСТ 6 — Ошибочная программа (лексическая ошибка)
Console.WriteLine("=== Тест 6: Лексическая ошибка ===");
try
{
    Run("""
    x = 10;
    y = x @ 5;
    """);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

// ТЕСТ 7 — Ошибочная программа (синтаксическая ошибка)
Console.WriteLine("=== Тест 7: Синтаксическая ошибка ===");
try
{
    Run("""
    x = 10
    y = x + 5;
    """);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
Console.ReadLine();