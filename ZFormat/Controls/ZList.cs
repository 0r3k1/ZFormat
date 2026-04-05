using System;
using System.Collections.Generic;
using System.Text;
using ZFormat.Core;

namespace ZFormat.Controls {
    public enum ZListType { Bullet, Numbered, Square, None }

    public class ZListItem {
        public string Text { get; set; }
        public string Icon { get; set; } = "";
        public object Tag { get; set; } // Para guardar IDs o acciones asociadas

        public ZListItem(string text, string icon = "", object tag = null) {
            Text = text;
            Icon = icon;
            Tag = tag;
        }
    }

    public class ZList:ZControl {
        public List<ZListItem> Items { get; set; } = new List<ZListItem>();
        public int SelectedIndex { get; set; } = 0;
        public ZListType Type { get; set; } = ZListType.Bullet;

        public ConsoleColor SelectedBackColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor SelectedForeColor { get; set; } = ConsoleColor.Black;

        public override void Render(ZBuffer buffer) {
            if(!IsVisible)
                return;
            var (absX, absY) = GetAbsolutePosition();

            // 1. Pintar TODO el rectángulo del fondo primero
            for(int y = 0; y < Height; y++) {
                for(int x = 0; x < Width; x++) {
                    buffer.DrawChar(absX + x, absY + y, ' ', ForegroundColor, BackgroundColor);
                }
            }

            for(int i = 0; i < Items.Count; i++) {
                var item = Items[i];
                string prefix = GetPrefix(i);
                string content = $" {item.Icon} {item.Text} ".TrimStart();
                string line = $"{prefix} {content}";

                // Colores si el elemento está seleccionado
                ConsoleColor back = (i == SelectedIndex && HasFocus) ? SelectedBackColor : BackgroundColor;
                ConsoleColor fore = (i == SelectedIndex && HasFocus) ? SelectedForeColor : ForegroundColor;

                // Dibujar fondo de la línea seleccionada (ancho completo del control)
                buffer.DrawString(absX, absY + i, new string(' ', Width), fore, back);
                buffer.DrawString(absX, absY + i, line, fore, back);
            }
        }

        private string GetPrefix(int index) {
            return Type switch {
                ZListType.Bullet => " •",
                ZListType.Numbered => $" {index + 1}.",
                ZListType.Square => " ■",
                _ => ""
            };
        }

        public override void HandleInput(ZKey key) {
            if(!HasFocus || !IsVisible || Items.Count == 0)
                return;

            if(key.Key == ConsoleKey.UpArrow)
                MoveUp();
            else if(key.Key == ConsoleKey.DownArrow)
                MoveDown();

            if(key.IsEnter) {
                OnItemSelected?.Invoke(GetSelected());
            }
        }

        public event Action<ZListItem> OnItemSelected;

        // Métodos para facilitar la navegación
        public void MoveDown() => SelectedIndex = (SelectedIndex + 1) % Items.Count;
        public void MoveUp() => SelectedIndex = (SelectedIndex - 1 + Items.Count) % Items.Count;
        public ZListItem GetSelected() => Items.Count > 0 ? Items[SelectedIndex] : null;
    }
}
