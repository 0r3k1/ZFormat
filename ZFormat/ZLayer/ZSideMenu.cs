using System;
using System.Collections.Generic;

namespace ZFormat.ZLayer {

    public class ZSideMenu:LayerBase {

        // --- Datos y Estado del Menú ---
        private readonly string[] _menuItems;
        private int _selectedIndex = 0;
        private readonly ConsoleColor _selectedColor = ConsoleColor.Yellow;
        private readonly ConsoleColor _defaultColor = ConsoleColor.White;
        private readonly ConsoleColor _menuBackColor = ConsoleColor.DarkCyan;
        private readonly char _selectorChar = (char)26; // Carácter de flecha

        /// <summary>
        /// Constructor para la barra de menú lateral.
        /// </summary>
        /// <param name="items">Lista de opciones del menú.</param>
        /// <param name="startX">Posición X de inicio (ej. 0).</param>
        /// <param name="startY">Posición Y de inicio (ej. 3, debajo del título).</param>
        /// <param name="width">Ancho de la columna del menú.</param>
        public ZSideMenu(string[] items, int startX, int startY, int width, int height)
    : base(startX, startY, height, width) { // Acepta la altura calculada

            _menuItems = items;

            // Forzamos el primer dibujo.
            NeedsRedraw = true;
        }

        // --- Lógica de Dibujo (Draw) ---
        public override void Draw() {
            if(!NeedsRedraw)
                return;

            // 1. Limpia y dibuja el fondo del área del menú
            ClearArea(_menuBackColor);

            // 2. Título (Opcional)
            ZstringFormat.gotoxy(StartX, StartY);
            ZstringFormat.WriteColor(
                ZstringFormat.centeredString(":: MENÚ PRINCIPAL ::", Width),
                ConsoleColor.Black, _menuBackColor);

            // 3. Dibujar las opciones del menú
            for(int i = 0; i < _menuItems.Length; i++) {
                // Coordenada Y de la opción (startY + 2 es la primera opción)
                int currentY = StartY + 2 + i;

                // Determina si esta opción está seleccionada
                bool isSelected = (_selectedIndex == i);

                // Color de texto: Amarillo si seleccionado, Blanco si no
                ConsoleColor foreColor = isSelected ? _selectedColor : _defaultColor;
                ConsoleColor backColor = isSelected ? ConsoleColor.DarkRed : _menuBackColor;

                ZstringFormat.gotoxy(StartX, currentY);

                // Dibujar el selector y el texto con el color correspondiente
                string text = isSelected ? $"{_selectorChar} {_menuItems[i]}" : $"  {_menuItems[i]}";

                ZstringFormat.WriteColor(
                    ZstringFormat.leftString(text, Width), // Asegura que se rellene el fondo en todo el ancho
                    foreColor, backColor
                );
            }

            // 4. Dibujar la opción Salir (siempre al final)
            ZstringFormat.gotoxy(StartX, StartY + Height - 1);
            ZstringFormat.WriteColor(
                ZstringFormat.centeredString(" (ENTER o Esq)", Width),
                ConsoleColor.DarkGray, _menuBackColor);

            NeedsRedraw = false;
        }

        // --- Lógica de Entrada (HandleInput) ---
        public override bool HandleInput(ConsoleKeyInfo keyInfo) {

            int oldIndex = _selectedIndex;

            if(keyInfo.Key == ConsoleKey.DownArrow) {
                _selectedIndex++;
            } else if(keyInfo.Key == ConsoleKey.UpArrow) {
                _selectedIndex--;
            } else {
                // Si no es una flecha, no consumimos el evento, el loop principal lo verá.
                return false;
            }

            // Lógica de bucle:
            if(_selectedIndex >= _menuItems.Length)
                _selectedIndex = 0;
            else if(_selectedIndex < 0)
                _selectedIndex = _menuItems.Length - 1;

            // Si el índice ha cambiado, el evento es consumido y necesitamos redibujar
            if(_selectedIndex != oldIndex) {
                NeedsRedraw = true;
                return true; // Evento consumido: la capa ha actualizado su estado.
            }

            return true; // Flecha presionada, pero no causó un cambio (ej. ya estaba en el límite).
        }

        /// <summary>
        /// Expone el índice actual seleccionado del menú para que el loop principal
        /// pueda tomar decisiones de lógica de aplicación.
        /// </summary>
        public int GetSelectedIndex() {
            return _selectedIndex;
        }
    }
}