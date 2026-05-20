Make these code changes?
readme.md

md
# Лексический анализатор

## 1. Список лексем

| № | Лексема | Пример | Описание |
|---|---------|--------|----------|
| 1 | `ID` | `a`, `abc`, `x1` | Идентификатор |
| 2 | `NUMBER` | `123`, `12.34` | Число (целое или вещественное) |
| 3 | `PLUS` | `+` | Сложение |
| 4 | `MINUS` | `-` | Вычитание |
| 5 | `MUL` | `*` | Умножение |
| 6 | `DIV` | `/` | Деление |
| 7 | `ASSIGN` | `=` | Присваивание |
| 8 | `SEMICOLON` | `;` | Точка с запятой |
| 9 | `COMMA` | `,` | Запятая |
| 10 | `LPAREN` | `(` | Левая круглая скобка |
| 11 | `RPAREN` | `)` | Правая круглая скобка |
| 12 | `LBRACE` | `{` | Левая фигурная скобка |
| 13 | `RBRACE` | `}` | Правая фигурная скобка |
| 14 | `LBRACKET` | `[` | Левая квадратная скобка |
| 15 | `RBRACKET` | `]` | Правая квадратная скобка |
| 16 | `LT` | `<` | Меньше |
| 17 | `GT` | `>` | Больше |
| 18 | `LE` | `<=` | Меньше или равно |
| 19 | `GE` | `>=` | Больше или равно |
| 20 | `EQ` | `==` | Равно |
| 21 | `NE` | `!=` | Не равно |
| 22 | `IF` | `if` | Ключевое слово if |
| 23 | `ELSE` | `else` | Ключевое слово else |
| 24 | `WHILE` | `while` | Ключевое слово while |
| 25 | `READ` | `read` | Ключевое слово read |
| 26 | `WRITE` | `write` | Ключевое слово write |
| 27 | `SQRT` | `sqrt` | Функция квадратного корня |
| 28 | `EXP` | `exp` | Функция экспоненты |
| 29 | `LOG` | `log` | Функция логарифма |
| 30 | `ARRAY` | `array` | Ключевое слово array |
| 31 | `EOF` | — | Конец ввода |

---

## 2. Таблица переходов конечного автомата

### Классы символов

| Обозначение | Описание |
|-------------|----------|
| `<б>` | Буква: a–z, A–Z |
| `<ц>` | Цифра: 0–9 |
| `<.>` | Точка: `.` |
| `<пр>` | Пробел, табуляция, перевод строки |
| `+` `-` `*` `/` `=` `!` `<` `>` `(` `)` `{` `}` `[` `]` `;` `,` | Сами символы |
| `⊥` | Конец ввода (EOF) |
| `other` | Прочие символы (ошибка) |

### Состояния автомата

| Состояние | Описание |
|-----------|----------|
| `S` | Старт |
| `I` | Идентификатор |
| `N` | Число (целая часть) |
| `F` | Число (дробная часть) |
| `A` | Символ `=` |
| `B` | Символ `==` |
| `C` | Символ `!` |
| `D` | Символ `!=` |
| `E` | Символ `<` |
| `G` | Символ `<=` |
| `H` | Символ `>` |
| `K` | Символ `>=` |
| `Z` | Завершение (финальное состояние) |
| `ERR` | Ошибка |

### Таблица переходов

| Состояние ↓ \ Вход → | `<б>` | `<ц>` | `.` | `<пр>` | `+` | `-` | `*` | `/` | `=` | `!` | `<` | `>` | `(` | `)` | `{` | `}` | `[` | `]` | `;` | `,` | `⊥` | `other` |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `S` | I | N | ERR | S | Z | Z | Z | Z | A | C | E | H | Z | Z | Z | Z | Z | Z | Z | Z | Z | ERR |
| `I` | I | I | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `N` | Z* | N | F | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `F` | Z* | F | ERR | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `A` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | B | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `B` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `C` | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | D | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR |
| `D` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `E` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | G | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `G` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `H` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | K | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `K` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |

> `Z*` — финальное состояние с возвратом текущего символа во входную ленту (символ принадлежит следующей лексеме)

---

## 3. Семантические программы

| № | Название | Описание действия |
|---|----------|-------------------|
| 0 | **Нет действия** | Просто переход в новое состояние, ничего не делать |
| 1 | **Накопить символ** | Добавить текущий символ к буферу накапливаемой лексемы |
| 2 | **Накопить и вернуть** | Добавить текущий символ к буферу, зафиксировать лексему и вернуть её |
| 3 | **Вернуть без накопления** | Зафиксировать лексему из буфера, текущий символ вернуть во входную ленту |
| 4 | **Определить тип: ID или ключевое слово** | Буфер содержит идентификатор — проверить таблицу ключевых слов |
| 5 | **Зафиксировать NUMBER** | Буфер содержит число — вернуть лексему `NUMBER` (2) |
| 6 | **Ошибка** | Недопустимый символ в данном состоянии — выдать ошибку |
| 7 | **Пропустить пробел** | Текущий символ — пробел / табуляция / перевод строки; если `\n` — увеличить номер строки |

### Привязка семантических программ к переходам

| Из состояния | Вход | В состояние | Сем. программа | Смысл |
|---|---|---|---|---|
| `S` | `<б>` | `I` | 1 | Начало идентификатора — накопить символ |
| `S` | `<ц>` | `N` | 1 | Начало числа — накопить символ |
| `S` | `<пр>` | `S` | 7 | Пропустить пробел |
| `S` | `+` `-` `*` `/` `(` `)` `{` `}` `[` `]` `;` `,` | `Z` | 2 | Односимвольная лексема — накопить и вернуть |
| `S` | `=` | `A` | 1 | Начало `=` или `==` — накопить |
| `S` | `!` | `C` | 1 | Начало `!=` — накопить |
| `S` | `<` | `E` | 1 | Начало `<` или `<=` — накопить |
| `S` | `>` | `H` | 1 | Начало `>` или `>=` — накопить |
| `I` | `<б>` `<ц>` | `I` | 1 | Продолжение идентификатора — накопить |
| `I` | любой другой | `Z*` | 3 + 4 | Конец идентификатора — вернуть символ, определить тип |
| `N` | `<ц>` | `N` | 1 | Продолжение целого числа — накопить |
| `N` | `.` | `F` | 1 | Начало дробной части — накопить |
| `N` | любой другой | `Z*` | 3 + 5 | Конец числа — вернуть символ, зафиксировать NUMBER |
| `F` | `<ц>` | `F` | 1 | Продолжение дробной части — накопить |
| `F` | любой другой | `Z*` | 3 + 5 | Конец вещественного числа — вернуть символ, зафиксировать NUMBER |
| `A` | `=` | `B` | 2 | `=` + `=` → лексема `==` — накопить и вернуть |
| `A` | другой | `Z*` | 3 | Просто `=` — вернуть символ, зафиксировать `ASSIGN` |
| `C` | `=` | `D` | 2 | `!` + `=` → лексема `!=` — накопить и вернуть |
| `C` | другой | `ERR` | 6 | `!` без `=` — ошибка |
| `E` | `=` | `G` | 2 | `<` + `=` → лексема `<=` — накопить и вернуть |
| `E` | другой | `Z*` | 3 | Просто `<` — вернуть символ, зафиксировать `LT` |
| `H` | `=` | `K` | 2 | `>` + `=` → лексема `>=` — накопить и вернуть |
| `H` | другой | `Z*` | 3 | Просто `>` — вернуть символ, зафиксировать `GT` |

---

# КС-грамматика языка

Исходная контекстно-свободная грамматика (до преобразований).

## Программа

```text
program → statement_list
Список операторов
Text
statement_list → statement statement_list
statement_list → ε
Оператор
Text
statement → assignment
statement → if_statement
statement → while_statement
statement → read_statement
statement → write_statement
statement → block
Составной оператор (блок)
Text
block → LBRACE statement_list RBRACE
Присваивание
Text
assignment → variable ASSIGN expression SEMICOLON
Переменная
Text
variable → ID
variable → ID LBRACKET expression RBRACKET
Условный оператор
Text
if_statement → IF LPAREN condition RPAREN block else_part

else_part → ELSE block
else_part → ε
Оператор цикла
Text
while_statement → WHILE LPAREN condition RPAREN block
Оператор ввода
Text
read_statement → READ LPAREN variable RPAREN SEMICOLON
Оператор вывода
Text
write_statement → WRITE LPAREN expression RPAREN SEMICOLON
Условие
Text
condition → expression rel_op expression
Операции сравнения
Text
rel_op → LT
rel_op → GT
rel_op → LE
rel_op → GE
rel_op → EQ
rel_op → NE
Выражение
Text
expression → expression PLUS term
expression → expression MINUS term
expression → term
Терм
Text
term → term MUL factor
term → term DIV factor
term → factor
Множитель
Text
factor → NUMBER
factor → variable
factor → LPAREN expression RPAREN
factor → function_call
Вызов функции
Text
function_call → function_name LPAREN expression RPAREN

function_name → SQRT
function_name → EXP
function_name → LOG
function_name → ARRAY
КС-грамматика без левой рекурсии
Преобразование грамматики путем замены леворекурсивных правил вида A → A α | β на A → β A' и A' → α A' | ε.

Для выражений:

Text
expression → term expression'
expression' → PLUS term expression'
expression' → MINUS term expression'
expression' → ε
Для термов:

Text
term → factor term'
term' → MUL factor term'
term' → DIV factor term'
term' → ε
Итоговая грамматика без левой рекурсии:

Text
program → statement_list

statement_list → statement statement_list | ε

statement → assignment
statement → if_statement
statement → while_statement
statement → read_statement
statement → write_statement
statement → block

assignment → variable ASSIGN expression SEMICOLON

variable → ID
variable → ID LBRACKET expression RBRACKET

if_statement → IF LPAREN condition RPAREN block else_part

else_part → ELSE block | ε

while_statement → WHILE LPAREN condition RPAREN block

read_statement → READ LPAREN variable RPAREN SEMICOLON

write_statement → WRITE LPAREN expression RPAREN SEMICOLON

block → LBRACE statement_list RBRACE

condition → expression rel_op expression

rel_op → LT | GT | LE | GE | EQ | NE

expression → term expression'

expression' → PLUS term expression' | MINUS term expression' | ε

term → factor term'

term' → MUL factor term' | DIV factor term' | ε

factor → NUMBER
factor → variable
factor → LPAREN expression RPAREN
factor → function_call

function_call → function_name LPAREN expression RPAREN

function_name → SQRT | EXP | LOG | ARRAY
Нормальная форма Грейбах
Преобразованная грамматика, в которой все правила начинаются с терминала или имеют вид A → ε.

Используется для детерминированного синтаксического анализа (LL(1)-парсер, рекурсивный спуск).

Преобразование
Нетерминалы, начинающие правые части, заменяются на их порождающие правила.

Раскрытие statement_list:

Text
statement_list → assignment statement_list
statement_list → if_statement statement_list
statement_list → while_statement statement_list
statement_list → read_statement statement_list
statement_list → write_statement statement_list
statement_list → block statement_list
statement_list → ε
Раскрытие statement через операторы:

Text
statement → assignment | if_statement | while_statement | 
            read_statement | write_statement | block
Раскрытие variable в других правилах и терминализация assignment:

Text
assignment → ID ASSIGN expression SEMICOLON
assignment → ID LBRACKET expression RBRACKET ASSIGN expression SEMICOLON
Раскрытие expression в правилах:

Text
expression → NUMBER expression'
expression → ID expression'
expression → ID LBRACKET expression RBRACKET expression'
expression → LPAREN expression RPAREN expression'
expression → SQRT LPAREN expression RPAREN expression'
expression → EXP LPAREN expression RPAREN expression'
expression → LOG LPAREN expression RPAREN expression'
expression → ARRAY LPAREN expression RPAREN expression'

expression' → PLUS expression expression'
expression' → MINUS expression expression'
expression' → ε
Условия и условные операторы:

Text
if_statement → IF LPAREN expression rel_op expression RPAREN block else_part
while_statement → WHILE LPAREN expression rel_op expression RPAREN block
condition → expression rel_op expression
Все операции сравнения:

Text
rel_op → LT | GT | LE | GE | EQ | NE
Итоговая грамматика в нормальной форме Грейбах
Text
program → ID statement_list
program → NUMBER statement_list
program → IF statement_list
program → WHILE statement_list
program → READ statement_list
program → WRITE statement_list
program → LBRACE statement_list
program → SQRT statement_list
program → EXP statement_list
program → LOG statement_list
program → ARRAY statement_list

statement_list → ID statement_list
statement_list → IF statement_list
statement_list → WHILE statement_list
statement_list → READ statement_list
statement_list → WRITE statement_list
statement_list → LBRACE statement_list
statement_list → SQRT statement_list
statement_list → EXP statement_list
statement_list → LOG statement_list
statement_list → ARRAY statement_list
statement_list → ε

assignment → ID ASSIGN expression SEMICOLON
assignment → ID LBRACKET expression RBRACKET ASSIGN expression SEMICOLON

variable → ID
variable → ID LBRACKET expression RBRACKET

if_statement → IF LPAREN expression rel_op expression RPAREN LBRACE statement_list RBRACE else_part

else_part → ELSE LBRACE statement_list RBRACE
else_part → ε

while_statement → WHILE LPAREN expression rel_op expression RPAREN LBRACE statement_list RBRACE

read_statement → READ LPAREN ID RPAREN SEMICOLON
read_statement → READ LPAREN ID LBRACKET expression RBRACKET RPAREN SEMICOLON

write_statement → WRITE LPAREN expression RPAREN SEMICOLON

block → LBRACE statement_list RBRACE

expression → NUMBER expression'
expression → ID expression'
expression → ID LBRACKET expression RBRACKET expression'
expression → LPAREN expression RPAREN expression'
expression → SQRT LPAREN expression RPAREN expression'
expression → EXP LPAREN expression RPAREN expression'
expression → LOG LPAREN expression RPAREN expression'
expression → ARRAY LPAREN expression RPAREN expression'

expression' → PLUS expression expression'
expression' → MINUS expression expression'
expression' → ε

term → NUMBER term'
term → ID term'
term → ID LBRACKET expression RBRACKET term'
term → LPAREN expression RPAREN term'
term → SQRT LPAREN expression RPAREN term'
term → EXP LPAREN expression RPAREN term'
term → LOG LPAREN expression RPAREN term'
term → ARRAY LPAREN expression RPAREN term'

term' → MUL term term'
term' → DIV term term'
term' → ε

factor → NUMBER
factor → ID
factor → ID LBRACKET expression RBRACKET
factor → LPAREN expression RPAREN
factor → SQRT LPAREN expression RPAREN
factor → EXP LPAREN expression RPAREN
factor → LOG LPAREN expression RPAREN
factor → ARRAY LPAREN expression RPAREN

function_call → SQRT LPAREN expression RPAREN
function_call → EXP LPAREN expression RPAREN
function_call → LOG LPAREN expression RPAREN
function_call → ARRAY LPAREN expression RPAREN

function_name → SQRT | EXP | LOG | ARRAY

rel_op → LT | GT | LE | GE | EQ | NE

condition → expression rel_op expression
