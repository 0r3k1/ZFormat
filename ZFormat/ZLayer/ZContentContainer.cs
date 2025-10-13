using System;

namespace ZFormat.ZLayer {

    public class ZContentContainer:LayerBase {

        private LayerBase _activeContent = null;
        private readonly ConsoleColor _backColor = ConsoleColor.Black;

        /// <summary>
        /// Constructor para el contenedor de contenido principal.
        /// Ocupa el área restante de la consola.
        /// </summary>
        public ZContentContainer(int startX, int startY, int width, int height)
            : base(startX, startY, height, width) {

            NeedsRedraw = true; // Dibujar el fondo al inicio.
        }

        /// <summary>
        /// Establece la capa de contenido que se mostrará y tomará el foco.
        /// </summary>
        public void SetContent(LayerBase newContent) {
            // 1. Opcional: Notificar a la capa anterior que va a desaparecer (si es necesario).
            // (Para esta demo, no es necesario, pero es buena práctica)

            // 2. Establecer el nuevo contenido.
            _activeContent = newContent;

            // 3. Posicionar y redimensionar la nueva capa para que encaje en el contenedor.
            _activeContent.SetBounds(StartX, StartY, Width, Height);
            _activeContent.NeedsRedraw = true;

            // 4. Marcar el contenedor para redibujar su fondo (si el nuevo contenido no lo cubre todo).
            NeedsRedraw = true;
        }

        public override void Update() {
            // Permitir que el contenido activo también se actualice (ej. si tiene temporizadores).
            _activeContent?.Update();
        }

        public override void Draw() {
            if(NeedsRedraw) {
                // Dibujar un fondo limpio para el contenedor.
                ClearArea(_backColor);
                NeedsRedraw = false;
            }

            // Pedir a la capa de contenido que se dibuje
            _activeContent?.Draw();
        }

        public override bool HandleInput(ConsoleKeyInfo keyInfo) {
            // El contenedor simplemente pasa la entrada al contenido activo.
            if(_activeContent != null) {
                return _activeContent.HandleInput(keyInfo);
            }
            return false;
        }
    }
}