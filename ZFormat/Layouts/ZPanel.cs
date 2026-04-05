using System;
using System.Collections.Generic;
using System.Linq;
using ZFormat.Core;

namespace ZFormat.Layouts {
    public class ZPanel:ZControl {
        public List<ZControl> Children { get; private set; } = new List<ZControl>();

        public ZPanel() {
            Name = "ZPanel";
        }

        public void AddChild(ZControl child) {
            child.Parent = this;
            Children.Add(child);
            // Cada vez que añadimos alguien, recalculamos posiciones
            UpdateLayout();
        }

        /// <summary>
        /// El corazón del diseño líquido. Calcula el tamaño y posición de cada hijo
        /// basándose en su propiedad Dock.
        /// </summary>
        public void UpdateLayout() {
            // El área de dibujo inicial es el tamaño total del panel menos el Padding
            int availableX = Padding;
            int availableY = Padding;
            int availableWidth = Width - (Padding * 2);
            int availableHeight = Height - (Padding * 2);

            // Ordenamos: Primero procesamos los bordes (Top, Bottom, Left, Right) 
            // y al final el Fill.
            var sortedChildren = Children.OrderBy(c => c.Dock == ZDockMode.Fill ? 1 : 0);

            foreach(var child in sortedChildren) {
                if(!child.IsVisible)
                    continue;

                switch(child.Dock) {
                    case ZDockMode.Top:
                    child.X = availableX;
                    child.Y = availableY;
                    child.Width = availableWidth;
                    // El hijo mantiene su Height, pero nosotros bajamos el "techo"
                    availableY += child.Height + child.Margin;
                    availableHeight -= (child.Height + child.Margin);
                    break;

                    case ZDockMode.Bottom:
                    child.X = availableX;
                    child.Width = availableWidth;
                    child.Y = (availableY + availableHeight) - child.Height;
                    // Subimos el "piso" disponible
                    availableHeight -= (child.Height + child.Margin);
                    break;

                    case ZDockMode.Left:
                    child.X = availableX;
                    child.Y = availableY;
                    child.Height = availableHeight; // Toma todo el alto disponible
                    availableX += child.Width + child.Margin;
                    availableWidth -= (child.Width + child.Margin);
                    break;

                    case ZDockMode.Right:
                    child.Y = availableY;
                    child.Height = availableHeight; // Toma todo el alto disponible
                    child.X = (availableX + availableWidth) - child.Width;
                    availableWidth -= (child.Width + child.Margin);
                    break;

                    case ZDockMode.Fill:
                    // El hijo Fill toma TODO el espacio que sobró después de los bordes
                    child.X = availableX;
                    child.Y = availableY;
                    child.Width = availableWidth;
                    child.Height = availableHeight;
                    break;

                    case ZDockMode.None:
                    // Si no tiene Dock, respeta su X e Y manuales
                    break;
                }

                // Si el hijo es otro Panel, tiene que actualizar a sus propios hijos
                if(child is ZPanel childPanel) {
                    childPanel.UpdateLayout();
                }
            }
        }

        public override void Render(ZBuffer buffer) {
            if(!IsVisible)
                return;

            // 1. Dibujar el fondo del panel (si es necesario)
            var (absX, absY) = GetAbsolutePosition();
            for(int y = 0; y < Height; y++) {
                for(int x = 0; x < Width; x++) {
                    buffer.SetPixel(absX + x, absY + y, ' ', ForegroundColor, BackgroundColor);
                }
            }

            // 2. Mandar a dibujar a los hijos en orden de ZIndex
            foreach(var child in Children.OrderBy(c => c.ZIndex)) {
                child.Render(buffer);
            }
        }

        public void ClearChildren() {  Children.Clear(); }
    }
}