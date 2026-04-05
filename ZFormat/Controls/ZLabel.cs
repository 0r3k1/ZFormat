using System;
using System.Collections.Generic;
using System.Text;
using ZFormat.Core;

namespace ZFormat.Controls {
    public enum ZTextAlign {
        Left,
        Center,
        Right
    }

    public class ZLabel:ZControl {
        public string Text { get; set; } = string.Empty;
        public ZTextAlign TextAlign { get; set; } = ZTextAlign.Left;

        public ZLabel(string text = "") {
            Name = "ZLabel";
            Text = text;
            Height = 1;
            Width = 0; // 💡 Lo dejamos en 0 para que sea dinámico
        }

        public override void Render(ZBuffer buffer) {
            if(!IsVisible || string.IsNullOrEmpty(Text))
                return;

            var (absX, absY) = GetAbsolutePosition();

            
            // 💡 Si el usuario no definió un Width manual, calculamos 
            // cuánto espacio queda disponible en el contenedor (Parent).
            int effectiveWidth = Width;
            if(effectiveWidth <= 0) {
                effectiveWidth = (Parent != null) ? Parent.Width - X - Padding : buffer.Width - absX;
            }

            // 1. Dividir el texto usando el ancho efectivo
            List<string> lines = WrapText(Text, effectiveWidth);

            // 2. Ajustar la altura del control al número de líneas reales
            if(this.Height != lines.Count) {
                this.Height = lines.Count;
            }

            // 3. Dibujar cada línea
            for(int i = 0; i < lines.Count; i++) {
                string currentLine = lines[i];
                int startX = absX;

                switch(TextAlign) {
                    case ZTextAlign.Center:
                    startX = absX + (effectiveWidth - currentLine.Length) / 2;
                    break;
                    case ZTextAlign.Right:
                    startX = absX + (effectiveWidth - currentLine.Length);
                    break;
                }

                buffer.DrawString(startX, absY + i, currentLine, ForegroundColor, BackgroundColor);
            }
        }

        private List<string> WrapText(string text, int width) {
            if(width <= 0)
                return new List<string> { text };

            List<string> lines = new List<string>();
            string[] parrafos = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var parrafo in parrafos) {
                string[] words = parrafo.Split(' ');
                string currentLine = "";

                foreach(var word in words) {
                    string safeWord = word.Length > width ? word.Substring(0, width) : word;

                    if((currentLine + safeWord).Length > width) {
                        if(!string.IsNullOrEmpty(currentLine))
                            lines.Add(currentLine.TrimEnd());
                        currentLine = safeWord + " ";
                    } else {
                        currentLine += safeWord + " ";
                    }
                }
                if(!string.IsNullOrEmpty(currentLine))
                    lines.Add(currentLine.TrimEnd());
            }

            return lines;
        }
    }
}
