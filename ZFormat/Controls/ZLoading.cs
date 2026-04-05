using System;
using System.Collections.Generic;
using System.Text;
using ZFormat.Core;

namespace ZFormat.Controls {
    public static class ZLoading {
        public static void Ejecutar(ZBuffer buffer, int x, int y, int tam = 30, int espera = 30) {
            string anim = "|/-\\";
            int estado = 0;

            for(int i = 0; i <= 100; i++) {
                buffer.Clear(); // Limpiamos para la animación

                int barraLength = (i * tam) / 100;
                string barra = new string('█', barraLength); // Usamos bloque sólido para que se vea más moderno
                string fondo = new string('░', tam - barraLength);

                string porcentaje = $"{i,3}% {anim[estado]}";

                // Dibujamos en el buffer
                buffer.DrawString(x, y, " PROCESANDO DATOS DE ZARITAPIXEL... ", ConsoleColor.Yellow, ConsoleColor.Black);
                buffer.DrawString(x, y + 2, $"{porcentaje} [{barra}{fondo}]", ConsoleColor.Cyan, ConsoleColor.Black);

                buffer.Render(); // Renderizado físico a consola

                estado = (estado + 1) % anim.Length;
                System.Threading.Thread.Sleep(espera);
            }
        }
    }
}
