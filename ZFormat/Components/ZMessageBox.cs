using System;
using System.Collections.Generic;
using ZFormat.Core;

namespace ZFormat.Components {
    public static class ZMessageBox {
        public static void Show(ZBuffer buffer, string titulo, string mensaje, ConsoleColor colorBorde = ConsoleColor.Red) {
            int maxTextWidth = 50; // El texto no será más ancho que esto
            List<string> lineasMensaje = WrapText(mensaje, maxTextWidth);

            // Calculamos el ancho de la caja basándonos en la línea más larga o el título
            int maxLineWidth = titulo.Length;
            foreach(var linea in lineasMensaje) {
                if(linea.Length > maxLineWidth)
                    maxLineWidth = linea.Length;
            }

            // Dimensiones finales con padding (4 espacios extra a cada lado)
            int anchoCaja = maxLineWidth + 8;
            int altoCaja = lineasMensaje.Count + 6; // Espacio para bordes, título y botón

            // Centramos la caja en la pantalla
            int x = (buffer.Width - anchoCaja) / 2;
            int y = (buffer.Height - altoCaja) / 2;

            // Protecciones por si la consola es muy pequeña
            if(x < 0)
                x = 0;
            if(y < 0)
                y = 0;

            // 1. Dibujar Sombra
            for(int i = 0; i < altoCaja; i++)
                buffer.DrawString(x + 1, y + 1 + i, new string(' ', anchoCaja), ConsoleColor.Black, ConsoleColor.Black);

            // 2. Dibujar Fondo
            for(int i = 0; i < altoCaja; i++)
                buffer.DrawString(x, y + i, new string(' ', anchoCaja), ConsoleColor.White, ConsoleColor.DarkGray);

            // 3. Dibujar Marco
            string lineaH = new string('═', anchoCaja - 2);
            buffer.DrawString(x, y, "╔" + lineaH + "╗", colorBorde, ConsoleColor.DarkGray);
            for(int i = 1; i < altoCaja - 1; i++) {
                buffer.DrawChar(x, y + i, '║', colorBorde, ConsoleColor.DarkGray);
                buffer.DrawChar(x + anchoCaja - 1, y + i, '║', colorBorde, ConsoleColor.DarkGray);
            }
            buffer.DrawString(x, y + altoCaja - 1, "╚" + lineaH + "╝", colorBorde, ConsoleColor.DarkGray);

            // 4. Dibujar Título Centrado
            buffer.DrawString(x + (anchoCaja - titulo.Length - 2) / 2, y + 1, $" {titulo} ", ConsoleColor.Black, ConsoleColor.Yellow);

            // 5. Dibujar Mensaje (Múltiples líneas centradas)
            for(int i = 0; i < lineasMensaje.Count; i++) {
                string linea = lineasMensaje[i];
                int startX = x + (anchoCaja - linea.Length) / 2;
                buffer.DrawString(startX, y + 3 + i, linea, ConsoleColor.White, ConsoleColor.DarkGray);
            }

            // 6. Dibujar Botón
            string tecla = "[ Presione cualquier tecla ]";
            buffer.DrawString(x + (anchoCaja - tecla.Length) / 2, y + altoCaja - 2, tecla, ConsoleColor.Gray, ConsoleColor.DarkGray);

            // 7. Renderizar y esperar
            buffer.Render();
            Console.ReadKey(true);
        }

        // 💡 Lógica de corte de texto
        private static List<string> WrapText(string text, int width) {
            List<string> lines = new List<string>();

            // 1. Separar primero por los saltos de línea explícitos (\n)
            string[] parrafos = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var parrafo in parrafos) {
                string[] words = parrafo.Split(' ');
                string currentLine = "";

                foreach(var word in words) {
                    if((currentLine + word).Length > width) {
                        if(!string.IsNullOrEmpty(currentLine))
                            lines.Add(currentLine.TrimEnd());
                        currentLine = word + " ";
                    } else {
                        currentLine += word + " ";
                    }
                }
                if(!string.IsNullOrEmpty(currentLine))
                    lines.Add(currentLine.TrimEnd());
            }

            return lines;
        }
    }
}