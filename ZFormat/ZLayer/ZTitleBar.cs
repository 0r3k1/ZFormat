using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFormat.ZLayer {
    public class ZTitleBar:LayerBase {

        public ZTitleBar(int startX, int startY, int height, int width = 0)
            : base(startX, startY, height, width) { }

        public override void Draw() {
            if(!NeedsRedraw)
                return;

            // 1. Limpiar el área antes de dibujar (opcional)
            ClearArea(ConsoleColor.DarkBlue);

            // 2. Posicionar el cursor al inicio del área
            ZstringFormat.gotoxy(StartX, StartY);

            // 3. Dibujar Borde Superior (usando ZTableStyle para consistencia si lo deseas, o simple)
            ZstringFormat.SetColor(ConsoleColor.White, ConsoleColor.DarkBlue);
            Console.Write(ZstringFormat.RepeatChar('═', Width));

            // 4. Mover el cursor al área donde se dibujará el contenido inyectado (una fila abajo)
            // Asumimos que el contenido del título estará en StartY + 1 (si Height > 1)
            int contentY = StartY + 1;
            int contentX = StartX;
            int contentHeight = Height - 2; // Descontamos la línea superior e inferior

            if(contentHeight > 0 && ContentDrawer != null) {
                ZstringFormat.gotoxy(contentX, contentY);
                // 5. Invocar la función de dibujo inyectada por el usuario
                ContentDrawer.Invoke(Width, contentHeight);
            }

            // 6. Dibujar Borde Inferior
            ZstringFormat.gotoxy(StartX, StartY + Height - 1);
            Console.Write(ZstringFormat.RepeatChar('═', Width));

            NeedsRedraw = false;
            ZstringFormat.RestoreColors();
        }
    }
}
