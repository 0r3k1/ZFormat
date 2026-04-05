using System;

namespace ZFormat.Core {
    public static class ZTheme {
        // Colores base de la interfaz
        public static ConsoleColor Background { get; set; } = ConsoleColor.Black;
        public static ConsoleColor Foreground { get; set; } = ConsoleColor.White;

        // Colores para elementos con foco o seleccionados
        public static ConsoleColor Accent { get; set; } = ConsoleColor.Cyan;
        public static ConsoleColor AccentForeground { get; set; } = ConsoleColor.Black;

        // Colores para estados secundarios (Placeholders, bordes desactivados)
        public static ConsoleColor Secondary { get; set; } = ConsoleColor.DarkGray;

        // Método para cambiar el estilo completo
        public static void SetTheme(string name) {
            switch(name.ToLower()) {
                case "retro":
                Background = ConsoleColor.Black;
                Foreground = ConsoleColor.Green;
                Accent = ConsoleColor.DarkGreen;
                AccentForeground = ConsoleColor.White;
                break;
                case "terminal-azul":
                Background = ConsoleColor.DarkBlue;
                Foreground = ConsoleColor.White;
                Accent = ConsoleColor.Yellow;
                AccentForeground = ConsoleColor.Black;
                break;
                case "dracula":
                Background = ConsoleColor.Black;
                Foreground = ConsoleColor.Magenta;
                Accent = ConsoleColor.DarkCyan;
                AccentForeground = ConsoleColor.White;
                break;
                default: // Modo Oscuro Estándar
                Background = ConsoleColor.Black;
                Foreground = ConsoleColor.White;
                Accent = ConsoleColor.Cyan;
                AccentForeground = ConsoleColor.Black;
                break;
            }
        }
    }
}