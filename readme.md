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
| 31 | `STRING` | `"hello"` | Строковый литерал |
| 32 | `EOF` | — | Конец ввода |

---

## 2. Таблица переходов конечного автомата

### Классы символов

| Обозначение | Описание |
|-------------|----------|
| `<б>` | Буква: a–z, A–Z |
| `<ц>` | Цифра: 0–9 |
| `<.>` | Точка: `.` |
| `<пр>` | Пробел, табуляция, перевод строки |
| `<">` | Кавычка: `"` |
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
| `Q` | Строковый литерал (внутри кавычек) |
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

| Состояние ↓ \ Вход → | `<б>` | `<ц>` | `.` | `<пр>` | `<">` | `+` | `-` | `*` | `/` | `=` | `!` | `<` | `>` | `(` | `)` | `{` | `}` | `[` | `]` | `;` | `,` | `⊥` | `other` |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `S` | I | N | ERR | S | Q | Z | Z | Z | Z | A | C | E | H | Z | Z | Z | Z | Z | Z | Z | Z | Z | ERR |
| `I` | I | I | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `N` | Z* | N | F | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `F` | Z* | F | ERR | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `Q` | Q | Q | Q | Q | Z | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | Q | ERR | Q |
| `A` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | B | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `B` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `C` | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | D | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR | ERR |
| `D` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `E` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | G | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `G` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `H` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | K | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |
| `K` | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* | Z* |

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
| `S` | `"` | `Q` | 0 | Начало строки — открывающую кавычку пропустить |
| `S` | `+` `-` `*` `/` `(` `)` `{` `}` `[` `]` `;` `,` | `Z` | 2 | Односимвольная лексема — накопить и вернуть |
| `S` | `=` | `A` | 1 | Начало `=` или `==` — накопить |
| `S` | `!` | `C` | 1 | Начало `!=` — накопить |
| `S` | `<` | `E` | 1 | Начало `<` или `<=` — накопить |
| `S` | `>` | `H` | 1 | Начало `>` или `>=` — накопить |
| `Q` | любой (кроме `"` и EOF) | `Q` | 1 | Продолжение строки — накопить символ |
| `Q` | `"` | `Z` | 2 | Конец строки — закрывающую кавычку пропустить, зафиксировать STRING |
| `Q` | EOF | `ERR` | 6 | Незакрытая строка — ошибка |
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
write_statement → WRITE LPAREN write_arg { COMMA write_arg } RPAREN SEMICOLON

write_arg → expression
write_arg → STRING
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

write_statement → WRITE LPAREN write_arg { COMMA write_arg } RPAREN SEMICOLON

write_arg → expression
write_arg → STRING

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

---

# Нестрогая нормальная форма Грейбах (ННФГ)

## Алгоритм преобразования

Нестрогая нормальная форма Грейбах (ННФГ) требует, чтобы **каждая правая часть правила либо начиналась с терминального символа, либо была пустой (ε)**.

Для преобразования используется замена: в каждом правиле вида `A → B α`, где B — нетерминал, заменяем B на все его правые части. Повторяем до тех пор, пока все правила не будут соответствовать ННФГ.

В нашей грамматике (после устранения левой рекурсии) нетерминальные символы в начале правых частей встречаются в правилах:

```
statement     → assignment | if_statement | while_statement | ...
assignment    → variable ASSIGN expression SEMICOLON
variable      → ID | ID LBRACKET expression RBRACKET
condition     → expression rel_op expression
expression    → term expression'
term          → factor term'
factor        → variable | ...
function_call → function_name LPAREN expression RPAREN
```

Раскрываем каждое такое правило, подставляя правые части нетерминала.

---

## Итоговая грамматика в ННФГ

```text
program → statement_list

statement_list → statement statement_list
statement_list → ε

statement → ID statement_tail
statement → ID LBRACKET expression RBRACKET ASSIGN expression SEMICOLON
statement → IF LPAREN condition RPAREN block else_part
statement → WHILE LPAREN condition RPAREN block
statement → READ LPAREN ID read_tail RPAREN SEMICOLON
statement → WRITE LPAREN write_arg { COMMA write_arg } RPAREN SEMICOLON
statement → LBRACE statement_list RBRACE

statement_tail → ASSIGN expression SEMICOLON
statement_tail → LBRACKET expression RBRACKET ASSIGN expression SEMICOLON

block → LBRACE statement_list RBRACE

else_part → ELSE LBRACE statement_list RBRACE
else_part → ε

read_tail → LBRACKET expression RBRACKET
read_tail → ε

condition → ID condition_tail
condition → NUMBER rel_op expression
condition → LPAREN expression RPAREN rel_op expression
condition → SQRT LPAREN expression RPAREN rel_op expression
condition → EXP LPAREN expression RPAREN rel_op expression
condition → LOG LPAREN expression RPAREN rel_op expression
condition → ARRAY LPAREN expression RPAREN rel_op expression

condition_tail → LBRACKET expression RBRACKET rel_op expression
condition_tail → rel_op expression

rel_op → LT
rel_op → GT
rel_op → LE
rel_op → GE
rel_op → EQ
rel_op → NE

expression  → ID expression_var_tail
expression  → NUMBER term' expression'
expression  → LPAREN expression RPAREN term' expression'
expression  → SQRT LPAREN expression RPAREN term' expression'
expression  → EXP LPAREN expression RPAREN term' expression'
expression  → LOG LPAREN expression RPAREN term' expression'
expression  → ARRAY LPAREN expression RPAREN term' expression'

expression_var_tail → LBRACKET expression RBRACKET term' expression'
expression_var_tail → term' expression'

expression' → PLUS ID expression_var_tail
expression' → PLUS NUMBER term' expression'
expression' → PLUS LPAREN expression RPAREN term' expression'
expression' → PLUS SQRT LPAREN expression RPAREN term' expression'
expression' → PLUS EXP LPAREN expression RPAREN term' expression'
expression' → PLUS LOG LPAREN expression RPAREN term' expression'
expression' → PLUS ARRAY LPAREN expression RPAREN term' expression'
expression' → MINUS ID expression_var_tail
expression' → MINUS NUMBER term' expression'
expression' → MINUS LPAREN expression RPAREN term' expression'
expression' → MINUS SQRT LPAREN expression RPAREN term' expression'
expression' → MINUS EXP LPAREN expression RPAREN term' expression'
expression' → MINUS LOG LPAREN expression RPAREN term' expression'
expression' → MINUS ARRAY LPAREN expression RPAREN term' expression'
expression' → ε

term' → MUL ID term_var_tail
term' → MUL NUMBER term'
term' → MUL LPAREN expression RPAREN term'
term' → MUL SQRT LPAREN expression RPAREN term'
term' → MUL EXP LPAREN expression RPAREN term'
term' → MUL LOG LPAREN expression RPAREN term'
term' → MUL ARRAY LPAREN expression RPAREN term'
term' → DIV ID term_var_tail
term' → DIV NUMBER term'
term' → DIV LPAREN expression RPAREN term'
term' → DIV SQRT LPAREN expression RPAREN term'
term' → DIV EXP LPAREN expression RPAREN term'
term' → DIV LOG LPAREN expression RPAREN term'
term' → DIV ARRAY LPAREN expression RPAREN term'
term' → ε

term_var_tail → LBRACKET expression RBRACKET term'
term_var_tail → term'
```
---

# Семантические действия для генерации ОПС

Генерация ОПС выполняется одновременно с работой LL(1)-анализатора. Для каждого правила грамматики задана последовательность семантических действий — по одному на каждый символ правой части. Действия хранятся во втором магазине и выполняются синхронно с основным магазином анализатора.

## Обозначения семантических действий

| Символ | Описание |
|--------|----------|
| `□` | Пустое действие |
| `a` | Записать в ОПС ссылку на переменную |
| `k` | Записать в ОПС ссылку на константу |
| `ks` | Записать в ОПС ссылку на строковую константу |
| `+` `-` `*` `/` | Записать в ОПС соответствующую арифметическую операцию |
| `:=` | Записать в ОПС операцию присваивания |
| `–'` | Записать в ОПС операцию унарного минуса |
| `i` | Записать в ОПС операцию индексирования массива |
| `<` `>` `<=` `>=` `==` `!=` | Записать в ОПС операцию сравнения |
| `r` | Записать в ОПС операцию ввода |
| `w` | Записать в ОПС операцию вывода числа |
| `ws` | Записать в ОПС операцию вывода строки |
| `wln` | Записать в ОПС операцию перевода строки |
| `1`–`5` | Выполнить соответствующую семантическую программу |

## Семантические программы 1–5

Используют счётчик `k` (номер текущего элемента ОПС) и магазин меток.

**Программа 1** — вызывается после условия `if`:
1. В магазин меток записывается `k`.
2. В ОПС записывается пустое место под будущую метку перехода при false.
3. В ОПС записывается операция `jf`.

**Программа 2** — вызывается в начале `else`:
1. Заполняется пустое место от программы 1 значением `k + 2`.
2. В магазин меток записывается `k`.
3. В ОПС записывается пустое место под метку безусловного перехода.
4. В ОПС записывается операция `j`.

**Программа 3** — вызывается в конце `if` или `if-else`:
1. Заполняется пустое место от программы 2 значением `k`.

**Программа 4** — вызывается перед условием `while`:
1. В магазин меток записывается `k` — адрес начала цикла.

**Программа 5** — вызывается после тела `while`:
1. Заполняется пустое место от программы 1 значением `k + 2`.
2. В ОПС записывается адрес начала цикла из магазина меток.
3. В ОПС записывается операция `j`.

## Таблица семантических действий

### Присваивание

| Нетерминал | Правая часть | Сем. действия |
|------------|-------------|--------------|
| `statement` | `ID statement_tail` | `a □` |
| `statement_tail` | `ASSIGN expression SEMICOLON` | `□ □ :=` |
| `statement_tail` | `LBRACKET expression RBRACKET ASSIGN expression SEMICOLON` | `□ □ i □ □ :=` |

### Выражения и термы

| Нетерминал | Правая часть | Сем. действия |
|------------|-------------|--------------|
| `S` | `(S)VU` | `□ □ □ □ □` |
| `S` | `aHVU` | `a □ □ □` |
| `S` | `kVU` | `k □ □` |
| `S` | `–GVU` | `□ □ –' □` |
| `U` | `+TU` | `□ □ +` |
| `U` | `–TU` | `□ □ –` |
| `U` | `λ` | — |
| `T` | `(S)V` | `□ □ □ □` |
| `T` | `aHV` | `a □ □` |
| `T` | `kV` | `k □` |
| `T` | `–GV` | `□ □ –'` |
| `V` | `*FV` | `□ □ *` |
| `V` | `/FV` | `□ □ /` |
| `V` | `λ` | — |
| `F` | `(S)` | `□ □ □` |
| `F` | `aH` | `a □` |
| `F` | `k` | `k` |
| `G` | `(S)` | `□ □ □` |
| `G` | `aH` | `a □` |
| `G` | `k` | `k` |
| `H` | `[SK` | `□ □ □` |
| `H` | `λ` | — |
| `K` | `]` | `i` |

### Условие

| Нетерминал | Правая часть | Сем. действия |
|------------|-------------|--------------|
| `condition` | `(S)VUD` | `□ □ □ □ □ □` |
| `condition` | `aHVUD` | `a □ □ □ □` |
| `condition` | `kVUD` | `k □ □ □` |
| `rel_op` | `LT` | `<` |
| `rel_op` | `GT` | `>` |
| `rel_op` | `LE` | `<=` |
| `rel_op` | `GE` | `>=` |
| `rel_op` | `EQ` | `==` |
| `rel_op` | `NE` | `!=` |

### Условный оператор if

| Нетерминал | Правая часть | Сем. действия |
|------------|-------------|--------------|
| `if_statement` | `IF LPAREN condition RPAREN block else_part` | `□ □ □ 1 □ □ 3` |
| `else_part` | `ELSE block` | `2 □` |
| `else_part` | `ε` | — |

### Цикл while

| Нетерминал | Правая часть | Сем. действия |
|------------|-------------|--------------|
| `while_statement` | `WHILE LPAREN condition RPAREN block` | `4 □ □ □ 1 □ 5` |

### Ввод и вывод

| Нетерминал | Правая часть | Сем. действия |
|------------|-------------|--------------|
| `read_statement` | `READ LPAREN ID RPAREN SEMICOLON` | `□ □ a r □` |
| `read_statement` | `READ LPAREN ID LBRACKET expression RBRACKET RPAREN SEMICOLON` | `□ □ a □ □ i r □` |
| `write_statement` | `WRITE LPAREN write_arg { COMMA write_arg } RPAREN SEMICOLON` | `□ □ □ { □ □ } wln □` |
| `write_arg` | `expression` | `w` |
| `write_arg` | `STRING` | `ks ws` |

### Составной оператор (блок)

| Правая часть | Сем. действия |
|-------------|--------------|
| `LBRACE statement_list RBRACE` | `□ □ □` |
| `statement statement_list` | `□ □` |
| `ε` | — |

---

# Список операций ОПС

| № | Операция | Обозначение | Арность |
|---|----------|-------------|---------|
| 1 | Сложение | `+` | 2 |
| 2 | Вычитание | `–` | 2 |
| 3 | Умножение | `*` | 2 |
| 4 | Деление | `/` | 2 |
| 5 | Унарный минус | `–'` | 1 |
| 6 | Присваивание | `:=` | 2 |
| 7 | Индексирование массива | `i` | 2 |
| 8 | Меньше | `<` | 2 |
| 9 | Больше | `>` | 2 |
| 10 | Меньше или равно | `<=` | 2 |
| 11 | Больше или равно | `>=` | 2 |
| 12 | Равно | `==` | 2 |
| 13 | Не равно | `!=` | 2 |
| 14 | Условный переход по false | `jf` | 2 |
| 15 | Безусловный переход | `j` | 1 |
| 16 | Ввод | `r` | 1 |
| 17 | Вывод числа | `w` | 1 |
| 18 | Вывод строки | `ws` | 1 |
| 19 | Перевод строки | `wln` | 0 |
| 20 | Квадратный корень | `sqrt` | 1 |
| 21 | Экспонента | `exp` | 1 |
| 22 | Натуральный логарифм | `log` | 1 |

---

# Формат ОПС

ОПС — линейный массив элементов. Каждый элемент состоит из двух полей: тип и значение.

## Поле `type`

| Значение | Описание |
|----------|----------|
| `TYPE_VAR` | Ссылка на переменную в таблице переменных |
| `TYPE_CONST` | Ссылка на константу в таблице констант |
| `TYPE_STR_CONST` | Ссылка на строку в таблице строковых констант |
| `TYPE_LABEL` | Метка — номер элемента ОПС (адрес перехода) |
| `TYPE_OP` | Операция |

## Поле `value`

| `type` | Содержимое `value` | Пример |
|--------|-------------------|--------|
| `TYPE_VAR` | Индекс в таблице переменных | `2` |
| `TYPE_CONST` | Индекс в таблице числовых констант | `0` |
| `TYPE_STR_CONST` | Индекс в таблице строковых констант | `0` |
| `TYPE_LABEL` | Номер элемента ОПС | `7` |
| `TYPE_OP` | Код операции | `OP_ADD`, `OP_JF`, `OP_I` |

## Виды содержимого в магазине интерпретатора

| Вид | Описание |
|-----|----------|
| Ссылка на переменную | Адрес в таблице переменных |
| Ссылка на константу | Адрес в таблице констант |
| Числовое значение | Результат арифметической операции |
| Ссылка на массив | Адрес паспорта массива |
| Ссылка на элемент массива | Вычисленный адрес `M + d*k` |

---

## Примеры генерации ОПС

### Пример 1. Присваивание с формулой

```
x = a + b * 2;
```

ОПС: `x  a  b  2  *  +  :=`

| Индекс | Тип | Значение |
|--------|-----|----------|
| 0 | `TYPE_VAR` | ссылка на `x` |
| 1 | `TYPE_VAR` | ссылка на `a` |
| 2 | `TYPE_VAR` | ссылка на `b` |
| 3 | `TYPE_CONST` | ссылка на `2` |
| 4 | `TYPE_OP` | `OP_MUL` |
| 5 | `TYPE_OP` | `OP_ADD` |
| 6 | `TYPE_OP` | `OP_ASSIGN` |

### Пример 2. Индексирование массива

```
M[k] = a * L[k];
```

ОПС: `M  k  i  a  L  k  i  *  :=`

### Пример 3. Условный оператор if-else

```
if (a > b) { a = b; } else { b = a; }
```

ОПС: `a  b  >  m1  jf  a  b  :=  m2  j  b  a  :=`

| Индекс | Тип | Значение |
|--------|-----|----------|
| 0 | `TYPE_VAR` | `a` |
| 1 | `TYPE_VAR` | `b` |
| 2 | `TYPE_OP` | `OP_GT` |
| 3 | `TYPE_LABEL` | `10` (m1) |
| 4 | `TYPE_OP` | `OP_JF` |
| 5 | `TYPE_VAR` | `a` |
| 6 | `TYPE_VAR` | `b` |
| 7 | `TYPE_OP` | `OP_ASSIGN` |
| 8 | `TYPE_LABEL` | `12` (m2) |
| 9 | `TYPE_OP` | `OP_J` |
| 10 | `TYPE_VAR` | `b` |
| 11 | `TYPE_VAR` | `a` |
| 12 | `TYPE_OP` | `OP_ASSIGN` |

### Пример 4. Цикл while

```
while (a > b) { a = b; }
```

ОПС: `a  b  >  m1  jf  a  b  :=  m0  j`

| Индекс | Тип | Значение |
|--------|-----|----------|
| 0 | `TYPE_VAR` | `a` (← m0) |
| 1 | `TYPE_VAR` | `b` |
| 2 | `TYPE_OP` | `OP_GT` |
| 3 | `TYPE_LABEL` | `9` (m1) |
| 4 | `TYPE_OP` | `OP_JF` |
| 5 | `TYPE_VAR` | `a` |
| 6 | `TYPE_VAR` | `b` |
| 7 | `TYPE_OP` | `OP_ASSIGN` |
| 8 | `TYPE_LABEL` | `0` (m0) |
| 9 | `TYPE_OP` | `OP_J` |

### Пример 5. Ввод и вывод с массивом

```
read(a); read(M[a]); write(M[a] * a);
```

ОПС: `a  r  M  a  i  r  M  a  i  a  *  w`

