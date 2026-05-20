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

> `Z*` — финальное состояние с возвратом текущего символа во входную ленту (символ принадлежит следующей лексеме).

---

## 3. Семантические программы

| № | Название | Описание действия |
|---|----------|-------------------|
| 0 | **Нет действия** | Просто переход в новое состояние, ничего не делать |
| 1 | **Накопить символ** | Добавить текущий символ к буферу накапливаемой лексемы |
| 2 | **Накопить и вернуть** | Добавить текущий символ к буферу, зафиксировать лексему и вернуть её |
| 3 | **Вернуть без накопления** | Зафиксировать лексему из буфера, текущий символ вернуть во входную ленту (он принадлежит следующей лексеме) |
| 4 | **Определить тип: ID или ключевое слово** | Буфер содержит идентификатор — проверить таблицу ключевых слов; если найдено — вернуть соответствующий номер лексемы (22–30), иначе вернуть `ID` (1) |
| 5 | **Зафиксировать NUMBER** | Буфер содержит число — вернуть лексему `NUMBER` (2) |
| 6 | **Ошибка** | Недопустимый символ в данном состоянии — выдать сообщение об ошибке с номером строки и позицией |
| 7 | **Пропустить пробел** | Текущий символ — пробел / табуляция / перевод строки; буфер не трогать; если `\n` — увеличить счётчик строки |

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
| `I` | любой другой | `Z*` | 3 + 4 | Конец идентификатора — вернуть символ, определить тип (ID или ключевое слово) |
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

Исходная контекстно-свободная грамматика. 

## Программа

```text
program → statement_list
```

## Список операторов

```text
statement_list → statement statement_list
statement_list → ε
```

## Оператор

```text
statement → assignment
statement → if_statement
statement → while_statement
statement → read_statement
statement → write_statement
statement → block
```

## Составной оператор (блок)

```text
block → LBRACE statement_list RBRACE
```

## Присваивание

```text
assignment → variable ASSIGN expression SEMICOLON
```

## Переменная

```text
variable → ID
variable → ID LBRACKET expression RBRACKET
```

## Условный оператор

```text
if_statement → IF LPAREN condition RPAREN block else_part

else_part → ELSE block
else_part → ε
```

## Оператор цикла

```text
while_statement → WHILE LPAREN condition RPAREN block
```

## Оператор ввода

```text
read_statement → READ LPAREN variable RPAREN SEMICOLON
```

## Оператор вывода

```text
write_statement → WRITE LPAREN expression RPAREN SEMICOLON
```

## Условие

```text
condition → expression rel_op expression
```

## Операции сравнения

```text
rel_op → LT
rel_op → GT
rel_op → LE
rel_op → GE
rel_op → EQ
rel_op → NE
```
## Выражение 

```text
expression → expression PLUS term
expression → expression MINUS term
expression → term
```

## Терм 

```text
term → term MUL factor
term → term DIV factor
term → factor
```

## Множитель

```text
factor → NUMBER
factor → variable
factor → LPAREN expression RPAREN
factor → function_call
```

## Вызов функции

```text
function_call → function_name LPAREN expression RPAREN

function_name → SQRT
function_name → EXP
function_name → LOG
function_name → ARRAY
```

---

# Устранение левой рекурсии

Левая рекурсия возникает когда правило начинается само с себя — парсер уходит в бесконечный цикл ещё до чтения хоть одного токена. В данной грамматике она обнаружена в двух нетерминалах: `expression` и `term`.

## Алгоритм

Правила вида:

```text
A → A α₁
A → A α₂
A → β        ← база (не начинается с A)
```

Заменяются на:

```text
A  → β A'
A' → α₁ A'
A' → α₂ A'
A' → ε
```

Вводится новый нетерминал `A'`, который берёт на себя повторяющийся хвост. База `β` идёт первой — рекурсии больше нет.

---

## Устранение в `expression`

В исходной грамматике:

```text
expression → expression PLUS term    ← левая рекурсия
expression → expression MINUS term   ← левая рекурсия
expression → term                    ← база (β = term)
```

Применяем алгоритм — база `term` выносится вперёд, хвосты `PLUS term` и `MINUS term` уходят в `expression'`:

```text
expression  → term expression'
expression' → PLUS term expression'
expression' → MINUS term expression'
expression' → ε
```

---

## Устранение в `term`

В исходной грамматике:

```text
term → term MUL factor    ← левая рекурсия
term → term DIV factor    ← левая рекурсия
term → factor             ← база (β = factor)
```

Применяем алгоритм — база `factor` выносится вперёд, хвосты `MUL factor` и `DIV factor` уходят в `term'`:

```text
term  → factor term'
term' → MUL factor term'
term' → DIV factor term'
term' → ε
```

---

# Итоговая грамматика без левой рекурсии

```text
program → statement_list

statement_list → statement statement_list
statement_list → ε

statement → assignment
statement → if_statement
statement → while_statement
statement → read_statement
statement → write_statement
statement → block

block → LBRACE statement_list RBRACE

assignment → variable ASSIGN expression SEMICOLON

variable → ID
variable → ID LBRACKET expression RBRACKET

if_statement → IF LPAREN condition RPAREN block else_part

else_part → ELSE block
else_part → ε

while_statement → WHILE LPAREN condition RPAREN block

read_statement → READ LPAREN variable RPAREN SEMICOLON

write_statement → WRITE LPAREN expression RPAREN SEMICOLON

condition → expression rel_op expression

rel_op → LT | GT | LE | GE | EQ | NE

expression  → term expression'
expression' → PLUS term expression'
expression' → MINUS term expression'
expression' → ε

term  → factor term'
term' → MUL factor term'
term' → DIV factor term'
term' → ε

factor → NUMBER
factor → variable
factor → LPAREN expression RPAREN
factor → function_call

function_call → function_name LPAREN expression RPAREN

function_name → SQRT | EXP | LOG | ARRAY
```
