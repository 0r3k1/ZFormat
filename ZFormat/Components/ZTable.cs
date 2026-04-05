using System;
using System.Collections.Generic;
using System.Linq;
using ZFormat.Core;

namespace ZFormat.Components {
    public enum BoundaryType {
        Top,            // Borde superior
        HeaderSeparator, // Separador entre encabezado y datos
        TotalSeparator,  // Línea sobre la fila de totales
        Bottom           // Borde inferior
    }

    public class ZTable:ZControl {
        private string[] _headers;
        private List<string[]> _rows = new List<string[]>();
        private List<int> _colWidths = new List<int>();
        private ZTableStyle _style;

        //propiedades de la tabla
        public string FilterTerm { get; set; } = "";
        public int ScrollIndex { get; set; } = 0;
        public int MaxVisibleRows { get; set; } = 10;
        public int RowsCount { get; set; } = 0;
        public int FilteredRowsCount { get; private set; }
        public int HeadersCount => _headers.Length;

        // Propiedades de estilo 
        public ConsoleColor HeaderColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor HeaderBackColor { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor AlternatingRowColor { get; set; } = ConsoleColor.DarkGray;
        public bool ShowTotalRow { get; set; } = false;
        public string TotalText { get; set; } = " TOTAL ";
        public string GetStatusText() => $" Registros: {FilteredRowsCount}/{RowsCount} | Filtro: '{FilterTerm}' ";

        //propiedades de estado
        public int SelectedIndex { get; set; } = 0; // Fila donde está el cursor
        public bool IsRowSelected { get; set; } = false; // ¿Se presionó Enter?
        public ConsoleColor SelectionColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor SelectionTextColor { get; set; } = ConsoleColor.Black;
        public int SelectedCol { get; set; } = 0; // Columna activa
        public bool IsEditingCell { get; set; } = false; // ¿Estamos escribiendo?
        // Colores para la celda activa
        public ConsoleColor CellFocusBackColor { get; set; } = ConsoleColor.White;
        public ConsoleColor CellFocusForeColor { get; set; } = ConsoleColor.Black;

        public ZTable(string[] headers, TableBorderStyle style = TableBorderStyle.SingleLine) {
            Name = "ZTable";
            _headers = headers;
            _style = ZTableStyle.GetStyle(style);
            // Inicializar anchos con el largo de los encabezados
            foreach(var h in _headers)
                _colWidths.Add(h.Length);
        }

        public void AddRow(string[] rowData) {
            _rows.Add(rowData);
            // Actualizar anchos máximos de columna
            for(int i = 0; i < rowData.Length; i++) {
                if(i < _colWidths.Count && rowData[i].Length > _colWidths[i])
                    _colWidths[i] = rowData[i].Length;
            }
            RowsCount = _rows.Count;
        }

        public override void Render(ZBuffer buffer) {
            if(!IsVisible)
                return;

            // 1. Filtrado de datos
            var filtered = _rows.Where(r =>
                string.IsNullOrEmpty(FilterTerm) ||
                r.Any(cell => cell != null && cell.ToLower().Contains(FilterTerm.ToLower()))
            ).ToList();

            FilteredRowsCount = filtered.Count;

            var (absX, absY) = GetAbsolutePosition();
            int currentY = absY;
            int padding = 1;

            // 2. Dibujar Encabezado
            DrawBoundary(buffer, absX, currentY++, BoundaryType.Top, padding);
            DrawRow(buffer, absX, currentY++, _headers, padding, HeaderColor, HeaderBackColor);
            DrawBoundary(buffer, absX, currentY++, BoundaryType.HeaderSeparator, padding);

            // 3. Dibujar Filas de Datos (Bucle for corregido)
            for(int i = ScrollIndex; i < filtered.Count && (i - ScrollIndex) < MaxVisibleRows; i++) {
                // Colores base
                ConsoleColor rowBack = (i % 2 != 0) ? AlternatingRowColor : BackgroundColor;
                ConsoleColor rowFore = ForegroundColor;

                // 💡 LÓGICA DE SELECCIÓN:
                if(i == SelectedIndex) {
                    // Si la fila es la que tiene el cursor, usamos color Cian (o el que prefieras)
                    rowBack = ConsoleColor.Cyan;
                    rowFore = ConsoleColor.Black;

                    // Si además el usuario presionó ENTER (Modo edición activo)
                    if(IsRowSelected) {
                        // Aquí dibujamos la fila con "Foco de Celda"
                        DrawRowWithCellFocus(buffer, absX, currentY++, filtered[i], padding, i == SelectedIndex);
                        continue; // Saltamos el DrawRow normal
                    }
                }

                DrawRow(buffer, absX, currentY++, filtered[i], padding, rowFore, rowBack);
            }

            // 4. Fila de Totales (Ahora está FUERA del bucle, se dibuja una sola vez al final)
            if(ShowTotalRow) {
                DrawBoundary(buffer, absX, currentY++, BoundaryType.TotalSeparator, padding);

                string[] totalRow = new string[_headers.Length];
                for(int j = 0; j < totalRow.Length; j++)
                    totalRow[j] = "";

                double sum = 0;
                foreach(var r in filtered) {
                    if(double.TryParse(r.Last().Replace("$", "").Replace(",", "").Trim(), out double val))
                        sum += val;
                }

                totalRow[0] = TotalText;
                totalRow[totalRow.Length - 1] = sum.ToString("C2");

                DrawRow(buffer, absX, currentY++, totalRow, padding, ConsoleColor.Green, BackgroundColor);
            }

            // 5. Borde Inferior
            DrawBoundary(buffer, absX, currentY++, BoundaryType.Bottom, padding);

            // Actualizar la altura real ocupada
            this.Height = currentY - absY;
        }

        // Nuevo método para resaltar la celda específica
        private void DrawRowWithCellFocus(ZBuffer buffer, int x, int y, string[] data, int pad, bool isSelectedRow) {
            int currentX = x;
            buffer.DrawChar(currentX++, y, _style.Vertical, ForegroundColor, BackgroundColor);

            for(int i = 0; i < data.Length; i++) {
                string cellValue = (data[i] ?? "").PadRight(_colWidths[i]);

                // Si es la columna activa, cambiamos el color
                ConsoleColor back = (i == SelectedCol) ? CellFocusBackColor : ConsoleColor.DarkGreen;
                ConsoleColor fore = (i == SelectedCol) ? CellFocusForeColor : ConsoleColor.White;

                buffer.DrawString(currentX, y, new string(' ', pad) + cellValue + new string(' ', pad), fore, back);
                currentX += _colWidths[i] + (pad * 2);
                buffer.DrawChar(currentX++, y, _style.Vertical, ForegroundColor, BackgroundColor);
            }
        }

        // Cálculo automático del total de la última columna
        public double GetLastColumnTotal() {
            double total = 0;
            foreach(var row in _rows) {
                // Intentamos limpiar el string de símbolos como 'Q' o ',' antes de sumar
                string val = row.Last().Replace("Q", "").Replace(",", "").Trim();
                if(double.TryParse(val, out double n))
                    total += n;
            }
            return total;
        }

        private void DrawRow(ZBuffer buffer, int x, int y, string[] data, int pad, ConsoleColor fore, ConsoleColor back) {
            int currentX = x;
            buffer.DrawChar(currentX++, y, _style.Vertical, fore, back);

            for(int i = 0; i < data.Length; i++) {
                // 💡 SEGURO EXTRA: Si data[i] es null, usamos "" para que PadRight no falle
                string cellValue = (data[i] ?? "").PadRight(_colWidths[i]);

                buffer.DrawString(currentX, y, new string(' ', pad) + cellValue + new string(' ', pad), fore, back);
                currentX += _colWidths[i] + (pad * 2);
                buffer.DrawChar(currentX++, y, _style.Vertical, fore, back);
            }
        }

        private void DrawBoundary(ZBuffer buffer, int x, int y, BoundaryType type, int pad) {
            // Usamos la lógica de selección de caracteres de tu código original
            char left = _style.TLeft, mid = _style.Cross, right = _style.TRight;

            if(type == BoundaryType.Top) { left = _style.TopLeft; mid = _style.TUp; right = _style.TopRight; } else if(type == BoundaryType.Bottom) { left = _style.BottomLeft; mid = _style.TDown; right = _style.BottomRight; }

            int currentX = x;
            buffer.DrawChar(currentX++, y, left, ForegroundColor, BackgroundColor);

            for(int i = 0; i < _colWidths.Count; i++) {
                int fullWidth = _colWidths[i] + (pad * 2);
                buffer.DrawString(currentX, y, new string(_style.Horizontal, fullWidth), ForegroundColor, BackgroundColor);
                currentX += fullWidth;

                if(i < _colWidths.Count - 1)
                    buffer.DrawChar(currentX++, y, mid, ForegroundColor, BackgroundColor);
            }
            buffer.DrawChar(currentX, y, right, ForegroundColor, BackgroundColor);
        }

        // 1. Método auxiliar para no repetir código de filtrado
        private List<string[]> GetFilteredRows() {
            return _rows.Where(r =>
                string.IsNullOrEmpty(FilterTerm) ||
                r.Any(cell => cell != null && cell.ToLower().Contains(FilterTerm.ToLower()))
            ).ToList();
        }

        // 2. El método GetSelectedRow definitivo (ELIMINA LOS OTROS DOS)
        public string[] GetSelectedRow() {
            var filtered = GetFilteredRows();
            if(SelectedIndex >= 0 && SelectedIndex < filtered.Count) {
                return filtered[SelectedIndex];
            }
            return null;
        }

        public string GetSelectedRowName() {
            var row = GetSelectedRow();
            return row != null ? row[1] : "N/A"; // Asumiendo que el nombre está en la col 1
        }

        // Devuelve una copia de todas las filas registradas
        public List<string[]> GetRows() {
            return new List<string[]>(_rows);
        }

        // Devuelve los encabezados de la tabla
        public string[] GetHeaders() {
            return (string[])_headers.Clone();
        }

        public void MoveUp() {
            if(SelectedIndex > 0) {
                SelectedIndex--;
                if(SelectedIndex < ScrollIndex)
                    ScrollIndex = SelectedIndex;
            }
        }

        public void MoveDown() {
            // IMPORTANTE: Validar contra las filas filtradas, no contra el total
            int totalVisible = GetFilteredRows().Count;
            if(SelectedIndex < totalVisible - 1) {
                SelectedIndex++;
                if(SelectedIndex >= ScrollIndex + MaxVisibleRows)
                    ScrollIndex++;
            }
        }

        public void ClearRows() {
            _rows.Clear();          // 1. Borramos los datos
            SelectedIndex = 0;      // 2. Reseteamos el cursor al inicio
            ScrollIndex = 0;         // 3. Volvemos el scroll arriba de todo
            // FilteredRowsCount = 0;
        }

        public override void HandleInput(ZKey key) {
            if(!HasFocus)
                return;

            if(key.Key == ConsoleKey.UpArrow)
                MoveUp();
            else if(key.Key == ConsoleKey.DownArrow)
                MoveDown();
            else if(key.Key == ConsoleKey.PageUp) {
                for(int i = 0; i < MaxVisibleRows; i++)
                    MoveUp();
            } else if(key.Key == ConsoleKey.PageDown) {
                for(int i = 0; i < MaxVisibleRows; i++)
                    MoveDown();
            }

            if(key.IsEnter) {
                OnRowSelected?.Invoke(GetSelectedRow());
            }
        }

        public event Action<string[]> OnRowSelected;

    }
}