using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFormat {

    // Enumera los tipos de línea de borde que la función DrawBoundary necesita renderizar.
    public enum BoundaryType {
        Top,            // Borde superior (usa esquinas TopLeft/TopRight y cruces TUp).
        HeaderSeparator, // Separador entre el encabezado y el cuerpo de datos (usa cruces Cross y TLeft/TRight).
        TotalSeparator,  // Línea sobre la fila de totales (lógica especial para celdas fusionadas).
        Bottom           // Borde inferior (usa esquinas BottomLeft/BottomRight y cruces TDown).
    }

    /// <summary>
    /// Componente principal para generar y dibujar una tabla estilizada en la consola.
    /// </summary>
    public class ZTableConsole {
        private string[] _header;
        private List<string[]> _rows = new List<string[]>();
        private List<int> _colWidths = new List<int>(); // Ancho máximo real de cada columna.
        private ZTableStyle _style;
        private bool _isFirstDraw = true;

        // --- NUEVAS PROPIEDADES DE POSICIONAMIENTO ---
        /// <summary>Posición X (columna) de inicio para el dibujo de la tabla.</summary>
        public int RenderX { get; set; } = -1;
        /// <summary>Posición Y (fila) de inicio para el dibujo de la tabla. Se incrementa automáticamente.</summary>
        public int RenderY { get; set; } = -1;

        // --- PROPIEDADES DE ESTILO EXISTENTES ---
        public bool ShowTotalRow { get; set; } = false;
        public string TotalText { get; set; } = " Total ";
        public ConsoleColor HeaderBackColor { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor HeaderColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor AlternatingRowColor { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor RowBackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor RowForeColor { get; set; } = ConsoleColor.White;
        public ConsoleColor TotalRowBackColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor TotalRowForeColor { get; set; } = ConsoleColor.Black;
        public int RowCount => _rows.Count;

        // Constructor: Inicializa la tabla con encabezados y define el estilo de borde.
        public ZTableConsole(string[] headers, TableBorderStyle style = TableBorderStyle.SingleLine) {
            _header = headers ?? throw new ArgumentNullException(nameof(headers), "Los encabezados no pueden ser nulos.");
            _colWidths.AddRange(_header.Select(h => h.Length).ToArray());
            _style = ZTableStyle.GetStyle(style);
        }

        public void AddRow(string[] rowData) {
            if(rowData == null || rowData.Length != _header.Length) {
                throw new ArgumentException("El número de elementos en la fila no coincide con el número de encabezados.");
            }
            _rows.Add(rowData);
            for(int i = 0; i < rowData.Length; i++) {
                if(rowData[i].Length > _colWidths[i]) {
                    _colWidths[i] = rowData[i].Length;
                }
            }
        }

        /// <summary>
        /// Calcula la suma de la **última columna** y dibuja la fila de totales con celda fusionada.
        /// </summary>
        private void DrawTotalRow(int padding, ConsoleColor foreColor, ConsoleColor backColor) {
            int numColumns = _colWidths.Count;
            if(numColumns <= 1)
                return;

            // ... (Lógica de cálculo de total sin cambios)
            long totalValue = 0;
            int totalColumnIndex = numColumns - 1;
            foreach(var row in _rows) {
                if(long.TryParse(row[totalColumnIndex].Trim(), out long val)) {
                    totalValue += val;
                }
            }

            // 2. Dibujar la fila

            // --- POSICIONAMIENTO X/Y ---
            if(RenderX >= 0) {
                ZstringFormat.gotoxy(RenderX, RenderY);
            }
            RenderY++; // Incrementa Y para la siguiente línea
            // --- FIN POSICIONAMIENTO ---

            Console.Write(_style.Vertical); // Borde vertical izquierdo

            // 2a. Celda fusionada (" Total ") - Cubre las primeras N-1 columnas.
            int mergedCellTotalWidth = 0;
            for(int i = 0; i < totalColumnIndex; i++) {
                mergedCellTotalWidth += _colWidths[i] + (padding * 2) + 1;
            }
            mergedCellTotalWidth -= 1;

            string totalTextPadded = TotalText.PadRight(mergedCellTotalWidth);

            ZstringFormat.WriteColor(totalTextPadded, foreColor, backColor);

            Console.Write(_style.Vertical); // Separador entre "Total" y el valor

            // 2b. Celda del Valor Final Sumado (Alineado a la derecha)
            string sumValue = totalValue.ToString();
            int lastColWidth = _colWidths[totalColumnIndex];

            string cellContent =
                ZstringFormat.RepeatChar(' ', padding) +
                sumValue.PadLeft(lastColWidth) + // Alineación a la derecha.
                ZstringFormat.RepeatChar(' ', padding);

            ZstringFormat.WriteColor(cellContent, foreColor, backColor);

            Console.Write(_style.Vertical); // Borde vertical derecho final
            Console.WriteLine();
        }


        /// <summary>
        /// Método principal para dibujar la tabla completa en la consola.
        /// </summary>
        public void Render() {
            const int padding = 2; // Relleno de espacios a los lados del contenido.

            // --- Lógica para determinar el inicio de Y ---
            // Si el usuario no definió RenderY (o lo dejó en -1), tomamos la posición actual del cursor.
            if(RenderY < 0) {
                RenderY = Console.CursorTop;
            }
            // Si el usuario no definió RenderX (o lo dejó en -1), tomamos la posición actual del cursor.
            if(RenderX < 0) {
                RenderX = Console.CursorLeft;
            }
            // --- Fin Lógica de inicio ---

            _isFirstDraw = true;

            // 1. Borde Superior
            DrawBoundary(padding, BoundaryType.Top);

            // 2. Fila de Encabezados
            DrawRow(_header, padding, HeaderColor, HeaderBackColor);

            // 3. Separador entre Encabezado y Datos
            _isFirstDraw = false;
            DrawBoundary(padding, BoundaryType.HeaderSeparator);

            // 4. Filas de Datos (con colores alternos para mejor lectura)
            for(int i = 0; i < _rows.Count; i++) {
                var row = _rows[i];
                ConsoleColor currentBackColor = (i % 2 == 0) ? RowBackColor : AlternatingRowColor;
                DrawRow(row, padding, RowForeColor, currentBackColor);
            }

            // 5. Lógica condicional de Totales y Borde Inferior
            if(ShowTotalRow && _colWidths.Count > 0) {
                DrawBoundary(padding, BoundaryType.TotalSeparator);
                DrawTotalRow(padding, TotalRowForeColor, TotalRowBackColor);
                DrawBoundary(padding, BoundaryType.Bottom);
            } else {
                DrawBoundary(padding, BoundaryType.Bottom);
            }

            // Resetea RenderY a -1 para que la próxima llamada a Render() tome la nueva posición del cursor
            // o para forzar al usuario a establecer una posición de nuevo.
            RenderY = -1;
            RenderX = -1;
        }


        // Método auxiliar: Dibuja las líneas horizontales de la tabla (Top, Bottom, Separators)
        private void DrawBoundary(int padding, BoundaryType type) {
            // ... (Configuración de piezas de borde sin cambios)
            char leftCorner = _style.TLeft;
            char cross = _style.Cross;
            char rightCorner = _style.TRight;
            char tdown = _style.TDown;

            switch(type) {
                case BoundaryType.Top:
                leftCorner = _style.TopLeft;
                cross = _style.TUp;
                rightCorner = _style.TopRight;
                break;
                case BoundaryType.HeaderSeparator:
                leftCorner = _style.TLeft;
                cross = _style.Cross;
                rightCorner = _style.TRight;
                break;
                case BoundaryType.TotalSeparator:
                leftCorner = _style.TLeft;
                cross = _style.Cross;
                rightCorner = _style.TRight;
                break;
                case BoundaryType.Bottom:
                leftCorner = _style.BottomLeft;
                cross = _style.TDown; // Usamos TDown para el cruce normal de pie de tabla.
                rightCorner = _style.BottomRight;
                break;
            }

            // --- POSICIONAMIENTO X/Y ---
            if(RenderX >= 0) {
                ZstringFormat.gotoxy(RenderX, RenderY);
            }
            RenderY++; // Incrementa Y para la siguiente línea
            // --- FIN POSICIONAMIENTO ---

            // 2. DIBUJAR LA LÍNEA
            Console.Write(leftCorner);

            for(int i = 0; i < _colWidths.Count; i++) {
                int totalWidth = _colWidths[i] + (padding * 2);

                // 2.1. Línea horizontal (repetir el carácter de línea horizontal)
                Console.Write(ZstringFormat.RepeatChar(_style.Horizontal, totalWidth));

                // 2.2. Intersección/Cruce (solo si NO es la última columna)
                if(i < _colWidths.Count - 1) {
                    if(type == BoundaryType.TotalSeparator) {
                        if(i < _colWidths.Count - 2) {
                            Console.Write(tdown);
                        }
                        if(i == _colWidths.Count - 2) {
                            Console.Write(cross);
                        }
                    } else if(type != BoundaryType.Bottom) {
                        Console.Write(cross);
                    }
                    if(type == BoundaryType.Bottom) {
                        if(i == _colWidths.Count - 2 && ShowTotalRow) {
                            if(i == _colWidths.Count - 2 && (_colWidths.Count % 2) == 0) {
                                Console.Write($"{_style.Horizontal}{_style.Horizontal}{tdown}");
                            }else {
                                Console.Write($"{_style.Horizontal}{tdown}");
                            }
                        } else if(!ShowTotalRow) {
                            Console.Write(tdown);
                        }
                    }
                }
            }

            // 3. Esquina derecha final
            Console.Write(rightCorner);
            Console.WriteLine();
        }

        // Método auxiliar: Dibuja una fila de contenido (Header o Row data).
        private void DrawRow(string[] data, int padding, ConsoleColor foreColor, ConsoleColor backColor) {

            // --- POSICIONAMIENTO X/Y ---
            if(RenderX >= 0) {
                ZstringFormat.gotoxy(RenderX, RenderY);
            }
            RenderY++; // Incrementa Y para la siguiente línea
                       // --- FIN POSICIONAMIENTO ---

            Console.Write(_style.Vertical); // Borde vertical izquierdo

            for(int i = 0; i < data.Length; i++) {

                string text = data[i];
                string paddedString = text.PadRight(_colWidths[i]);

                ZstringFormat.RepeatWriteColor(" ", foreColor, backColor, padding);
                ZstringFormat.WriteColor(paddedString, foreColor, backColor);
                ZstringFormat.RepeatWriteColor(" ", foreColor, backColor, padding);

                Console.Write(_style.Vertical);
            }
            Console.WriteLine();
        }
    }
}