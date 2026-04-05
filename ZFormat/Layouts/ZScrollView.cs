using System;
using System.Linq;
using ZFormat.Core;

namespace ZFormat.Layouts {
    public class ZScrollView:ZPanel {
        public int ScrollY { get; private set; } = 0;
        public int MaxScroll { get; private set; } = 0;

        public ZScrollView() {
            Name = "ZScrollView";
            IsFocusable = true; // Para poder recibir las flechas del teclado
        }

        // Se llama antes de dibujar para saber qué tan largo es el contenido
        public void CalcularLimites() {
            if(Children.Count > 0) {
                // Buscamos cuál es el elemento que está más abajo
                int contenidoAlto = Children.Max(c => c.Y + c.Height);
                MaxScroll = Math.Max(0, contenidoAlto - this.Height + 2); // +2 para margen extra
            } else {
                MaxScroll = 0;
            }
        }

        public override void Render(ZBuffer buffer) {
            if(!IsVisible)
                return;

            CalcularLimites();
            var (absX, absY) = GetAbsolutePosition();

            // 1. Limpiamos el fondo del área visible
            for(int y = 0; y < Height; y++) {
                for(int x = 0; x < Width; x++) {
                    buffer.SetPixel(absX + x, absY + y, ' ', ForegroundColor, BackgroundColor);
                }
            }

            // 2. MAGIA: Dibujamos a los hijos desplazados hacia arriba
            foreach(var child in Children.OrderBy(c => c.ZIndex)) {
                int originalY = child.Y; // Guardamos su posición original
                child.Y -= ScrollY;      // Lo desplazamos virtualmente

                child.Render(buffer);    // Lo dibujamos

                child.Y = originalY;     // Lo regresamos a la normalidad
            }

            // 3. Dibujar la barra de Scroll Visual
            if(MaxScroll > 0) {
                int trackHeight = Height;
                // Calculamos la posición del pulgar con una regla de 3
                int thumbY = (int)((ScrollY / (float)MaxScroll) * (trackHeight - 1));

                for(int i = 0; i < trackHeight; i++) {
                    char c = (i == thumbY) ? '█' : '│';
                    buffer.SetPixel(absX + Width - 1, absY + i, c, ConsoleColor.DarkCyan, BackgroundColor);
                }
            }
        }

        // Atrapamos las flechas Arriba/Abajo para mover el scroll
        public void ScrollUp() {
            if(ScrollY > 0)
                ScrollY--;
        }

        public void ScrollDown() {
            CalcularLimites();
            if(ScrollY < MaxScroll)
                ScrollY++;
        }
    }
}