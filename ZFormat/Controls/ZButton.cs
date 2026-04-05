using System;
using System.Collections.Generic;
using System.Text;
using ZFormat.Core;

namespace ZFormat.Controls {
    public class ZButton:ZControl {
        public string Text { get; set; } = "BOTÓN";
        public ConsoleColor FocusColor { get; set; } = ConsoleColor.Green;

        public event Action OnClick;

        public override void Render(ZBuffer buffer) {
            if(!IsVisible)
                return;
            var (absX, absY) = GetAbsolutePosition();

            ConsoleColor back = HasFocus ? FocusColor : ConsoleColor.DarkGray;
            ConsoleColor fore = HasFocus ? ConsoleColor.Black : ConsoleColor.White;

            // Dibujar el cuerpo del botón con un pequeño efecto de sombra
            string btnText = $" [ {Text} ] ";
            buffer.DrawString(absX, absY, btnText, fore, back);

            // Sombra lateral y baja (caracteres ASCII para el efecto DOS)
            buffer.DrawChar(absX + btnText.Length, absY, '▄', ConsoleColor.Black, BackgroundColor);
            buffer.DrawString(absX + 1, absY + 1, new string('▀', btnText.Length), ConsoleColor.Black, BackgroundColor);
        }

        public void PerformClick() {
            OnClick?.Invoke(); // Si alguien se suscribió, ejecuta su función
        }

        public override void HandleInput(ZKey key) {
            if(!HasFocus || !IsVisible)
                return;

            if(key.IsEnter || key.Key == ConsoleKey.Spacebar) {
                PerformClick();
            }
        }
    }
}
