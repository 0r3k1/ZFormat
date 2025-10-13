using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFormat.ZLayer {
    // Define un delegado (un tipo de función) para el contenido inyectable.
    // El delegado recibe: ancho, alto y padding (relleno) para que la lógica de dibujo se adapte.
    // Usaremos un Action<int, int> simple que recibe (Ancho, Alto).
    // NOTA: Para simplificar, la posición (X, Y) es manejada por el método Draw.
    public delegate void DrawContentDelegate(int width, int height);

    /// <summary>
    /// Clase base abstracta para todos los componentes de diseño (Layers) en la consola.
    /// Define el área de dibujo y permite la inyección de contenido personalizado.
    /// </summary>
    public abstract class LayerBase {

        // --- Propiedades de Posicionamiento y Tamaño ---
        public int StartX { get; protected set; }
        public int StartY { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        // --- Inyección de Contenido ---
        /// <summary>
        /// Delegado que contiene la función de dibujo proporcionada por el usuario final.
        /// Esta función define el contenido visual interno de la capa.
        /// </summary>
        protected DrawContentDelegate ContentDrawer { get; set; }
        
        // --- Control de Redibujo ---
        public bool NeedsRedraw { get; set; } = true;

        /// <summary>
        /// Constructor base. Calcula el ancho de la consola si no se especifica.
        /// </summary>
        /// <param name="startX">Coordenada X de inicio.</param>
        /// <param name="startY">Coordenada Y de inicio.</param>
        /// <param name="height">Alto de la capa.</param>
        /// <param name="width">Ancho de la capa. Si es 0 o negativo, usa Console.WindowWidth.</param>
        public LayerBase(int startX, int startY, int height, int width = 0) {
            this.StartX = startX;
            this.StartY = startY;
            this.Height = height;
            // Si el ancho no se especifica, toma el ancho total de la consola
            this.Width = (width <= 0) ? Console.WindowWidth - startX : width;
        }

        /// <summary>
        /// Inyecta la lógica de dibujo personalizada del contenido dentro del layout.
        /// </summary>
        public void SetContent(DrawContentDelegate contentFunc) {
            ContentDrawer = contentFunc;
            NeedsRedraw = true;
        }

        /// <summary>
        /// Lógica de dibujo principal de la capa, que incluye el borde y el contenido inyectado.
        /// </summary>
        public abstract void Draw();
        
        /// <summary>
        /// Método de manejo de entrada opcional.
        /// </summary>
        public virtual bool HandleInput(ConsoleKeyInfo keyInfo) {
            // Lógica por defecto (la capa no consume el evento)
            return false;
        }

        /// <summary>
        /// Método auxiliar para limpiar el área del layout antes de dibujar.
        /// </summary>
        protected void ClearArea(ConsoleColor backColor = ConsoleColor.Black) {
            ZstringFormat.SetColor(ConsoleColor.White, backColor);
            for (int y = StartY; y < StartY + Height; y++) {
                ZstringFormat.gotoxy(StartX, y);
                Console.Write(new string(' ', Width));
            }
            ZstringFormat.RestoreColors();
        }

        /// <summary>
        /// Método de actualización opcional. Se llama en cada ciclo del Game Loop
        /// y es ideal para animaciones o lógica de temporizadores.
        /// </summary>
        public virtual void Update() {
            // Lógica por defecto: no hacer nada
        }

        /// <summary>
        /// Reestablece las coordenadas y dimensiones de la capa.
        /// Útil para capas cuyo tamaño es determinado por un contenedor.
        /// </summary>
        public void SetBounds(int startX, int startY, int width, int height) {
            StartX = startX;
            StartY = startY;
            Width = width;
            Height = height;
            NeedsRedraw = true; // Forzar un redibujo con las nuevas dimensiones/posición
        }
    }
}
