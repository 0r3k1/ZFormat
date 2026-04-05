using System;
using System.Collections.Generic;
using System.Text;

namespace ZFormat.Core {

    /// <summary>
    /// Define cómo se adhiere un control a los bordes de su contenedor padre.
    /// </summary>
    public enum ZDockMode {
        None,   // Posición manual usando X e Y
        Top,    // Se pega arriba y ocupa todo el ancho
        Bottom, // Se pega abajo y ocupa todo el ancho
        Left,   // Se pega a la izquierda y ocupa todo el alto
        Right,  // Se pega a la derecha y ocupa todo el alto
        Fill    // Ocupa todo el espacio restante disponible
    }

    /// <summary>
    /// Clase maestra de la que heredarán todos los componentes de ZFormat.
    /// </summary>
    public abstract class ZControl {
        // --- Propiedades de Identidad y Jerarquía ---
        public string Name { get; set; } = "ZControl";
        public ZControl? Parent { get; set; }

        // --- Posicionamiento y Tamaño ---
        // X e Y son relativos a la esquina superior izquierda del Padre
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // --- Comportamiento de Layout (Estilo Flexbox/Dock) ---
        public ZDockMode Dock { get; set; } = ZDockMode.None;
        public int Margin { get; set; } = 0;  // Espacio exterior
        public int Padding { get; set; } = 0; // Espacio interior

        // --- Estética ---
        public ConsoleColor ForegroundColor { get; set; } = ZTheme.Foreground;
        public ConsoleColor BackgroundColor { get; set; } = ZTheme.Background;
        public bool IsVisible { get; set; } = true;
        

        // --- Capas y Foco ---
        public int ZIndex { get; set; } = 0; // Para ventanas superpuestas
        public bool IsFocusable { get; set; } = false;
        public bool HasFocus { get; set; }

        // --- Métodos Vitales ---

        // Eventos que los controles pueden usar para reaccionar al foco
        public virtual void OnFocusEnter() { HasFocus = true; }
        public virtual void OnFocusLeave() { HasFocus = false; }


        /// <summary>
        /// Cada control hijo implementará aquí su propia lógica de dibujo
        /// </summary>
        public abstract void Render(ZBuffer buffer);

        /// <summary>
        /// Calcula la posición absoluta sumando las coordenadas de todos los padres.
        /// Esto es necesario porque el Buffer es una matriz única de toda la pantalla.
        /// </summary>
        public virtual (int x, int y) GetAbsolutePosition() {
            if(Parent == null)
                return (X, Y);
            var (px, py) = Parent.GetAbsolutePosition();
            return (px + X + Margin, py + Y + Margin);
        }

        // --- Manejo de Eventos de Teclado (Base) ---
        public virtual void OnKeyPressed(ConsoleKeyInfo key) { }


        /// <summary>
        /// Permite que cada control procese las teclas cuando tiene el foco.
        /// </summary>
        public virtual void HandleInput(ZKey key) {
            // Por defecto no hace nada, los hijos lo sobreescribirán
        }
    }
}
