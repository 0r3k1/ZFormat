using System;

namespace ZFormat.ZLayer {

    public class ZStatusBar:LayerBase {

        // --- Estado del Mensaje Actual ---
        private string _currentMessage = "ZStatusBar: Idle";
        private ConsoleColor _backColor = ConsoleColor.DarkGray;
        private ConsoleColor _foreColor = ConsoleColor.White;

        // --- Estado por Defecto (Reset) ---
        private string _defaultMessage = "ZStatusBar: Idle";
        private ConsoleColor _defaultBackColor = ConsoleColor.DarkGray;
        private ConsoleColor _defaultForeColor = ConsoleColor.White;

        // --- Lógica de Timer (Reset) ---
        // DateTime.MinValue indica que no hay un restablecimiento pendiente.
        private DateTime _resetTime = DateTime.MinValue;

        /// <summary>
        /// Constructor para la barra de estado inferior.
        /// </summary>
        /// <param name="defaultMessage">Mensaje por defecto.</param>
        /// <param name="height">Altura de la capa (default 1).</param>
        public ZStatusBar(string defaultMessage = "ZStatusBar: Idle", int height = 1)
            : base(0, Console.WindowHeight - height, height) {

            _defaultMessage = defaultMessage;
            _currentMessage = defaultMessage;

            // Colores por defecto para un estado "Idle" claro
            _defaultBackColor = ConsoleColor.DarkGray;
            _defaultForeColor = ConsoleColor.White;
            _backColor = _defaultBackColor;
            _foreColor = _defaultForeColor;
        }

        /// <summary>
        /// Actualiza el mensaje de estado, colores y opcionalmente setea un delay para el reset.
        /// </summary>
        /// <param name="message">Mensaje a mostrar.</param>
        /// <param name="backColor">Color de fondo.</param>
        /// <param name="foreColor">Color de texto.</param>
        /// <param name="delayMs">Tiempo en milisegundos antes de restablecer al mensaje por defecto.</param>
        public void SetStatus(string message, ConsoleColor backColor = ConsoleColor.DarkGreen, ConsoleColor foreColor = ConsoleColor.White, int delayMs = 0) {

            // Si el mensaje o el color cambian, forzamos un redibujo.
            if(_currentMessage != message || _backColor != backColor) {
                _currentMessage = message;
                _backColor = backColor;
                _foreColor = foreColor;
                NeedsRedraw = true;
            }

            // Configurar el temporizador de restablecimiento
            if(delayMs > 0) {
                _resetTime = DateTime.Now.AddMilliseconds(delayMs);
            } else {
                _resetTime = DateTime.MinValue; // Deshabilitar reset
            }
        }

        /// <summary>
        /// Lógica de actualización ejecutada en cada ciclo del Game Loop.
        /// Verifica si es tiempo de restablecer el mensaje.
        /// </summary>
        public override void Update() {
            // Solo procesa si hay un tiempo de restablecimiento configurado Y ese tiempo ya pasó.
            if(_resetTime != DateTime.MinValue && DateTime.Now >= _resetTime) {

                // Restablecer el estado a los valores por defecto
                _currentMessage = _defaultMessage;
                _backColor = _defaultBackColor;
                _foreColor = _defaultForeColor;

                NeedsRedraw = true;
                _resetTime = DateTime.MinValue; // Deshabilitar el temporizador
            }
        }

        // ... (Draw y HandleInput se mantienen igual) ...
        public override void Draw() {
            if(!NeedsRedraw)
                return;

            // 1. Limpiar y dibujar el fondo
            ClearArea(_backColor);

            // 2. Posicionar el cursor al inicio de la barra (StartX, StartY)
            ZstringFormat.gotoxy(StartX, StartY);

            // 3. Escribir el mensaje (alineado a la izquierda)
            string displayMessage = ZstringFormat.leftString($" {_currentMessage}", Width);

            ZstringFormat.WriteColor(
                displayMessage,
                _foreColor,
                _backColor
            );

            NeedsRedraw = false;
            ZstringFormat.RestoreColors();
        }

        // ... (HandleInput se mantiene igual) ...
        public override bool HandleInput(ConsoleKeyInfo keyInfo) {
            return false;
        }
    }
}