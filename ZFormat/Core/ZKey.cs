namespace ZFormat.Core {
    public class ZKey {
        public ConsoleKey Key { get; }
        public char KeyChar { get; }
        public bool Modifiers { get; }

        public ZKey(ConsoleKeyInfo info) {
            Key = info.Key;
            KeyChar = info.KeyChar;
            // Por hacer, expandir para detectar Shift, Alt, Ctrl si fuera necesario
        }

        // Propiedades semánticas
        public bool IsEnter => Key == ConsoleKey.Enter;
        public bool IsEscape => Key == ConsoleKey.Escape;
        public bool IsTab => Key == ConsoleKey.Tab;
        public bool IsBackspace => Key == ConsoleKey.Backspace;
        public bool IsArrow => Key == ConsoleKey.UpArrow || Key == ConsoleKey.DownArrow ||
                               Key == ConsoleKey.LeftArrow || Key == ConsoleKey.RightArrow;

        public bool IsNavigation => IsArrow || IsTab;
        public bool IsAction => IsEnter || IsEscape;

        public static ZKey ReadKey() {
            return new ZKey(Console.ReadKey(true));
        }
    }
}