using System;
using System.Collections.Generic;
using System.Text;

namespace ZFormat.Core {
    /// <summary>
    /// Representa un único carácter en la pantalla con sus colores.
    /// Usamos un struct (tipo de valor) para que la matriz en memoria sea muy rápida.
    /// </summary>
    public struct ZPixel{
        public char Character { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public ZPixel(char character, ConsoleColor foreground, ConsoleColor background) {
            Character = character;
            Foreground = foreground;
            Background = background;
        }
    }

    /// <summary>
    /// El lienzo en memoria donde todos los controles dibujarán antes de mostrarse.
    /// </summary>
    public class ZBuffer{

        private ZPixel[,] _grid;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ConsoleColor DefaultBackground { get; set; }

        public ZBuffer(int width, int height, ConsoleColor defaultBg = ConsoleColor.Black) {
            Width = width;
            Height = height;
            DefaultBackground = defaultBg;
            _grid = new ZPixel[width, height];
            Clear();
        }

        // Limpia el lienzo llenándolo de espacios vacíos
        public void Clear() {
            for(int y = 0; y < Height; y++) {
                for(int x = 0; x < Width; x++) {
                    _grid[x, y] = new ZPixel(' ', ConsoleColor.White, DefaultBackground);
                }
            }
        }

        // Dibuja un solo píxel en memoria (protegido contra desbordamientos)
        public void SetPixel(int x, int y, char c, ConsoleColor fg, ConsoleColor bg) {
            if(x >= 0 && x < Width && y >= 0 && y < Height) {
                _grid[x, y] = new ZPixel(c, fg, bg);
            }
        }

        // Dibuja una cadena de texto en memoria
        public void DrawString(int x, int y, string text, ConsoleColor fg, ConsoleColor bg) {
            if(y >= 0 && y < Height) {
                for(int i = 0; i < text.Length; i++) {
                    SetPixel(x + i, y, text[i], fg, bg);
                }
            }
        }

        // Este es el método mágico que vuelca la memoria a la consola real
        public void Render() {
            ConsoleColor currentFg = Console.ForegroundColor;
            ConsoleColor currentBg = Console.BackgroundColor;

            for(int y = 0; y < Height; y++) {
                // 1. Forzamos el cursor al inicio de la línea exacta
                Console.SetCursorPosition(0, y);

                for(int x = 0; x < Width; x++) {
                    // 2. Evitamos dibujar el último píxel de la esquina inferior derecha
                    if(x == Width - 1 && y == Height - 1)
                        continue;

                    ZPixel pixel = _grid[x, y];

                    if(pixel.Foreground != currentFg) {
                        Console.ForegroundColor = pixel.Foreground;
                        currentFg = pixel.Foreground;
                    }
                    if(pixel.Background != currentBg) {
                        Console.BackgroundColor = pixel.Background;
                        currentBg = pixel.Background;
                    }

                    Console.Write(pixel.Character);
                }
            }

            Console.ResetColor();
        }

        public void DrawChar(int x, int y, char c, ConsoleColor fg, ConsoleColor bg) {
            SetPixel(x, y, c, fg, bg);
        }
    }
}
