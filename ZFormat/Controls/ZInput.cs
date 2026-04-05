using System;
using System.Collections.Generic;
using System.Text;
using ZFormat.Core;

namespace ZFormat.Controls {
    public class ZInput:ZControl {
        public string Label { get; set; } = "Buscar:";
        public string Value { get; set; } = "";
        public ConsoleColor InputColor { get; set; } = ConsoleColor.White;
        public string Placeholder { get; set; } = "";
        public bool IsPassword { get; set; } = false;
        public char PasswordChar { get; set; } = '*';

        public ZInput() { Height = 1; }

        public override void Render(ZBuffer buffer) {
            if(!IsVisible)
                return;

            var (absX, absY) = GetAbsolutePosition();

            // 1. Dibujar el Label
            buffer.DrawString(absX, absY, Label, ForegroundColor, BackgroundColor);
            int inputX = absX + Label.Length + 1;

            // 💡 Calculamos el ancho real de la caja de texto (Ancho Total - Label)
            int inputFieldWidth = Width - Label.Length - 1;
            if(inputFieldWidth < 1)
                inputFieldWidth = 1;

            string textToDisplay = Value;
            ConsoleColor textColor = ForegroundColor;

            if(string.IsNullOrEmpty(Value)) {
                textToDisplay = Placeholder;
                textColor = ConsoleColor.DarkGray;
            } else if(IsPassword) {
                textToDisplay = new string(PasswordChar, Value.Length);
            }

            // 2. Evitamos que el texto visual supere el ancho de la caja
            if(textToDisplay.Length > inputFieldWidth) {
                textToDisplay = textToDisplay.Substring(0, inputFieldWidth);
            }

            // 3. Dibujar el fondo del campo y el texto
            string fieldBackground = new string(' ', inputFieldWidth);
            buffer.DrawString(inputX, absY, fieldBackground, textColor, ConsoleColor.DarkBlue);
            buffer.DrawString(inputX, absY, textToDisplay, textColor, ConsoleColor.DarkBlue);

            // 4. Dibujar un "cursor" visual si tiene el foco
            if(HasFocus) {
                int cursorPosition = inputX + Math.Min(Value.Length, inputFieldWidth - 1);
                buffer.DrawChar(cursorPosition, absY, '_', ConsoleColor.White, ConsoleColor.DarkBlue);
            }
        }

        public override void HandleInput(ZKey key) {
            if(!HasFocus)
                return;

            if(key.IsBackspace) {
                if(Value.Length > 0)
                    Value = Value.Substring(0, Value.Length - 1);
            } else if(!key.IsNavigation && !key.IsAction && key.KeyChar != '\0') {
                // 💡 Ahora el máximo de caracteres es el correcto
                int maxChars = Width - Label.Length - 1;
                if(Value.Length < maxChars) {
                    Value += key.KeyChar;
                }
            }
            OnValueChanged?.Invoke(Value);
        }

        public event Action<string> OnValueChanged;
    }
}
