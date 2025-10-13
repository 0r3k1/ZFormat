using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFormat.ZLayer;

namespace ZFormat {
    /// <summary>
    /// Gestiona la colección de capas (ZLayer) y ejecuta el bucle principal de la aplicación.
    /// Es el "motor" o "game loop" para la TUI.
    /// </summary>
    public class ZLayoutManager {

        // Almacenamos las capas en un diccionario, usando un índice para el orden de dibujo (Z-index).
        private readonly List<LayerBase> _layers = new List<LayerBase>();

        // Propiedad para controlar si el bucle principal debe seguir ejecutándose.
        public bool IsRunning { get; set; } = true;

        // Frecuencia del bucle (ej. 30 FPS).
        private const int LOOP_DELAY_MS = 33; // 1000ms / 30 frames = ~33ms

        /// <summary>
        /// Agrega una capa a la lista de gestión.
        /// </summary>
        public void AddLayer(LayerBase layer) {
            _layers.Add(layer);
        }

        // --- NUEVO MÉTODO ---
        /// <summary>
        /// Captura una tecla si hay una disponible y la procesa a través de las capas.
        /// Retorna la tecla presionada o null si no se presionó ninguna tecla.
        /// </summary>
        public ConsoleKeyInfo? PollInput() {
            if(Console.KeyAvailable) {
                // Captura la tecla presionada.
                return Console.ReadKey(true);
            }
            return null;
        }

        /// <summary>
        /// Inicia el bucle principal de la aplicación.
        /// </summary>
        public void Run() {
            IsRunning = true;
            Console.CursorVisible = false; // Ocultar cursor para una experiencia más limpia

            // Limpia la consola y configura colores base una vez
            Console.Clear();
            ZstringFormat.RestoreColors();

            // --- BUCLE PRINCIPAL (Game Loop) ---
            while(IsRunning) {
                // 1. Lógica de Entrada (Input Polling)
                HandleInput();

                // 2. Lógica de Dibujo (Renderizado)
                Draw();

                // 3. Control de Velocidad del Bucle
                Thread.Sleep(LOOP_DELAY_MS);
            }
            // --- FIN BUCLE ---

            // Limpieza al salir
            Console.CursorVisible = true;
            Console.Clear();
            ZstringFormat.RestoreColors();
        }

        /// <summary>
        /// Detiene la ejecución del bucle principal en el siguiente ciclo.
        /// </summary>
        public void Stop() {
            IsRunning = false;
        }

        /// <summary>
        /// Llama al método de actualización en todas las capas.
        /// </summary>
        public void UpdateLayers() {
            // Llama al Update() de todas las capas para manejar lógica de temporizadores, etc.
            foreach(var layer in _layers) {
                layer.Update();
            }
        }

        /// <summary>
        /// Dibuja todas las capas que necesitan ser redibujadas.
        /// </summary>
        public void Draw() {
            // Recorremos y dibujamos las capas.
            foreach(var layer in _layers) {
                if(layer.NeedsRedraw) {
                    layer.Draw();
                }
            }
        }

        /// <summary>
        /// Escanea la entrada del teclado y la envía a las capas para que la manejen.
        /// </summary>
        private void HandleInput() {
            if(Console.KeyAvailable) {
                // Captura la tecla presionada (false = no la muestra en pantalla)
                var keyInfo = Console.ReadKey(true);

                // Opción Global de Salida: ESC siempre detiene la aplicación.
                if(keyInfo.Key == ConsoleKey.Escape) {
                    Stop();
                    return;
                }

                // 1. Enviar la entrada a las capas (en orden inverso, para que la capa más nueva/superior reciba primero)
                // Se detiene la propagación tan pronto como una capa maneja el evento (return true).
                foreach(var layer in _layers.AsEnumerable().Reverse()) {
                    if(layer.HandleInput(keyInfo)) {
                        // El evento fue consumido por esta capa, no lo pasamos a las capas inferiores.
                        return;
                    }
                }
            }
        }
    }
}
