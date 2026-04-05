using ZFormat.Core;

namespace ZFormat.Layouts {
    public class ZForm:ZPanel {
        public int Spacing { get; set; } = 2; // Espacio entre controles
        public int Padding { get; set; } = 1; // Margen interno

        private int _nextY = 0;

        public void AddField(ZControl control) {
            control.X = Padding;
            control.Y = _nextY;

            // Calculamos la posición del siguiente control
            _nextY += 1 + Spacing;

            this.AddChild(control);

            // Ajustamos el alto del formulario automáticamente
            this.Height = _nextY;
        }
    }
}