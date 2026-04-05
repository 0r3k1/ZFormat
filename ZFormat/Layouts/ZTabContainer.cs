using System;
using System.Collections.Generic;
using ZFormat.Core;

namespace ZFormat.Layouts {
    public class ZTabContainer:ZControl {
        public List<ZTabItem> Tabs { get; set; } = new List<ZTabItem>();
        public int SelectedIndex { get; set; } = 0;

        // Estilo de la barra de pestañas
        public ConsoleColor ActiveTabColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor InactiveTabColor { get; set; } = ConsoleColor.DarkGray;

        public override void Render(ZBuffer buffer) {
            if(!IsVisible || Tabs.Count == 0)
                return;

            var (absX, absY) = GetAbsolutePosition();
            int currentX = absX;

            // 1. Dibujar Encabezados de Pestañas
            for(int i = 0; i < Tabs.Count; i++) {
                var tab = Tabs[i];
                bool isActive = (i == SelectedIndex);

                string text = $" {tab.Icon} {tab.Title} ";
                ConsoleColor back = isActive ? ActiveTabColor : InactiveTabColor;
                ConsoleColor fore = isActive ? ConsoleColor.Black : ConsoleColor.White;

                // Dibujar la "pestaña"
                buffer.DrawString(currentX, absY, text, fore, back);

                // Dibujar un pequeño separador visual
                currentX += text.Length;
                buffer.DrawChar(currentX++, absY, ' ', ForegroundColor, BackgroundColor);
            }

            // 2. Dibujar línea decorativa debajo de los encabezados
            buffer.DrawString(absX, absY + 1, new string('═', Width), ActiveTabColor, BackgroundColor);

            // 3. Renderizar el Contenido de la pestaña activa
            var activeContent = Tabs[SelectedIndex].Content;
            if(activeContent != null) {
                // Ajustamos la posición del contenido para que aparezca debajo de la barra
                activeContent.X = this.X;
                activeContent.Y = this.Y + 2;
                activeContent.Width = this.Width;
                activeContent.Render(buffer);
            }
        }

        public void NextTab() => SelectedIndex = (SelectedIndex + 1) % Tabs.Count;
        public void PreviousTab() => SelectedIndex = (SelectedIndex - 1 + Tabs.Count) % Tabs.Count;
    }
}