public enum CharClass
{
    Letter,        // <б> в readme — любая буква (a-z, A-Z и Unicode)
    Digit,         // <ц> — цифра 0-9
    Dot,           // <.> — точка, нужна для вещественных чисел (3.14)
    WhiteSpace,    // <пр> — пробел, \t, \r, \n
    Quote,         // <"> — двойная кавычка, начало/конец строкового литерала
    Plus,          // +
    Minus,         // -
    Star,          // *
    Slash,         // /
    Equal,         // =  (может быть ASSIGN или начало ==)
    Exclamation,   // !  (только в составе !=)
    Less,          // <  (может быть LT или начало <=)
    Greater,       // >  (может быть GT или начало >=)
    LeftParen,     // (
    RightParen,    // )
    LeftBrace,     // {
    RightBrace,    // }
    LeftBracket,   // [
    RightBracket,  // ]
    Semicolon,     // ;
    Comma,         // ,
    EOF,           // \0 — конец входной строки
    Other          // всё остальное → колонка other в таблице → ERR
}