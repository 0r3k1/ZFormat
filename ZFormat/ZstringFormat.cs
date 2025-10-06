using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; // Necesario para Thread.Sleep

namespace ZFormat {
    public class ZstringFormat {

        /// <summary>
        /// Propiedad estática que obtiene el ancho máximo actual de la ventana de la consola (Console.WindowWidth).
        /// Reemplaza el valor fijo de 80 en todas las funciones de formato, haciéndolas adaptables.
        /// </summary>
        public static int MaxConsoleWidth => Console.WindowWidth;

        /// <summary>
        /// Mueve el cursor de la consola a una coordenada específica (columna/fila).
        /// </summary>
        /// <param name="x">Posición horizontal (columna).</param>
        /// <param name="y">Posición vertical (fila).</param>
        public static void gotoxy(int x, int y) {
            // Nota: Necesario para dibujar en puntos exactos de la consola, como los bordes.
            Console.SetCursorPosition(x, y);
        }

        /// <summary>
        /// Pinta todo el fondo de la consola con un color específico (inicia en 0, 0).
        /// </summary>
        /// <param name="color">El color de fondo a usar para el layout.</param>
        /// <param name="w">El ancho del bloque (caracteres). Usa MaxConsoleWidth por defecto.</param>
        /// <param name="h">La altura del bloque (filas).</param>
        public static void printLayout(ConsoleColor color, int w = -1, int h = 25) {
            // Si no se especifica ancho (w = -1), usa el ancho máximo de la consola.
            if(w == -1)
                w = MaxConsoleWidth;

            Console.BackgroundColor = color;
            // Recorre la altura, pinta una línea de espacios y usa gotoxy para volver al inicio de cada fila.
            for(int i = 0; i < h; i++) {
                gotoxy(0, i);
                Console.Write(new string(' ', w));
            }
        }

        /// <summary>
        /// Pinta un bloque rectangular de color en coordenadas específicas. Útil para dibujar paneles o fondos de menú.
        /// </summary>
        /// <param name="color">El color de fondo a usar para el layout.</param>
        /// <param name="x">Columna de inicio.</param>
        /// <param name="y">Fila de inicio.</param>
        /// <param name="w">Ancho del bloque. Usa MaxConsoleWidth por defecto.</param>
        /// <param name="h">Altura del bloque.</param>
        public static void printLayoutInCoord(ConsoleColor color, int x = 0, int y = 0, int w = -1, int h = 25) {
            // Si no se especifica ancho (w = -1), usa el ancho máximo de la consola.
            if(w == -1)
                w = MaxConsoleWidth;

            Console.BackgroundColor = color;
            for(int i = 0; i < h; i++) {
                gotoxy(x, y + i);
                Console.Write(new string(' ', w));
            }
            // Devolver el cursor a la posición por defecto para continuar escribiendo.
            gotoxy(0, 0);
        }

        /// <summary>
        /// Dibuja un encabezado azul con el título de la aplicación y derechos de autor en la parte superior.
        /// El formato se adapta al ancho de la consola.
        /// </summary>
        /// <param name="txt">El texto principal que se centrará en el encabezado.</param>
        public static void encavezado(string txt) {
            int w = MaxConsoleWidth;

            // Dibuja el bloque de fondo azul para el encabezado (4 líneas de alto).
            printLayoutInCoord(ConsoleColor.Blue, 0, 0, w, 4);

            gotoxy(0, 1);
            Console.WriteLine(centeredString("Cristobal Rodas", w));
            Console.WriteLine(centeredString(txt, w)); // Título del módulo/pantalla actual.
            Console.WriteLine(centeredString(".: Todos los derechos son de libre uso :.", w));
        }

        /// <summary>
        /// Simula una barra de carga animada (estilo 'DOS') que va del 0% al 100%.
        /// Utiliza gotoxy para dibujar la barra en un solo lugar y Thread.Sleep para la animación.
        /// </summary>
        /// <param name="x">Columna de inicio.</param>
        /// <param name="y">Fila de inicio.</param>
        /// <param name="tam">Longitud de la barra (máximo de caracteres para el relleno).</param>
        /// <param name="espera">Velocidad (ms) entre ticks.</param>
        /// <param name="sprite">El carácter de relleno usado para la barra de progreso (default: '*').</param>
        public static void cargando(int x, int y, int tam = 73, int espera = 110, char sprite = '*') {
            int estado = 0;
            // Caracteres de animación de Shell/DOS
            string carga = "|/-\\";

            // Guardar colores y cursor iniciales para restaurar al terminar
            ConsoleColor originalFore = Console.ForegroundColor;
            ConsoleColor originalBack = Console.BackgroundColor;

            // Prepara la posición inicial del texto y el color de la barra (ej: gris sobre negro)
            SetColor(ConsoleColor.Gray, ConsoleColor.Black);
            gotoxy(x, y);

            // Dibuja el marco y el texto de progreso inicial (0%)
            // Nota: Se usa ' ' como relleno inicial para evitar errores de gotoxy antes del bucle.
            Console.Write($" 0%{carga[0]}[{new string(' ', tam)}]");

            for(int i = 0; i <= 100; i++) {
                // Calcula el tamaño actual del relleno de la barra
                int barraLength = (i * tam) / 100;
                // Caracter de relleno de la barra
                string barra = new string(sprite, barraLength);

                // Muestra el porcentaje y el caracter animado
                gotoxy(x, y);
                // leftString asegura que el porcentaje (1% a 100%) siempre ocupe 3 espacios.
                Console.Write($"{leftString(i.ToString(), 3)}%{carga[estado]}");

                // Dibuja la barra de progreso [********** ]
                // Se usa Console.Write para evitar el salto de línea.
                Console.Write($"[{barra}{new string(' ', tam - barraLength)}]");

                // Actualiza el caracter animado
                estado++;
                if(estado == carga.Length)
                    estado = 0;

                // Espera para la animación
                Thread.Sleep(espera);
            }

            // Restaura la posición del cursor después de terminar (salto de línea implícito)
            RestoreColors();
            gotoxy(0, y + 1);
        }

        /// <summary>
        /// Centra una cadena de texto agregando relleno de espacios a ambos lados.
        /// </summary>
        /// <param name="s">La cadena a centrar.</param>
        /// <param name="width">El ancho total (por defecto MaxConsoleWidth).</param>
        /// <returns>La cadena centrada.</returns>
        public static string centeredString(string s, int width = -1) {
            if(width == -1)
                width = MaxConsoleWidth;

            if(s.Length >= width)
                return s;

            // Calcula el padding necesario.
            int leftPadding = (width - s.Length) / 2;
            int rightPadding = width - s.Length - leftPadding;

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }

        /// <summary>
        /// Alinea una cadena a la izquierda rellenando el espacio sobrante a la derecha.
        /// </summary>
        /// <param name="s">La cadena a alinear.</param>
        /// <param name="width">El ancho total deseado (por defecto MaxConsoleWidth).</param>
        /// <returns>La cadena alineada a la izquierda.</returns>
        public static string leftString(string s, int width = -1) {
            if(width == -1)
                width = MaxConsoleWidth;

            // Si la cadena es más larga que el ancho, la trunca.
            if(s.Length > width)
                return s.Substring(0, width);

            int rightPadding = width - s.Length;
            return s + new string(' ', rightPadding);
        }

        /// <summary>
        /// Alinea una cadena a la derecha rellenando el espacio sobrante a la izquierda.
        /// </summary>
        /// <param name="s">La cadena a alinear.</param>
        /// <param name="width">El ancho total deseado (por defecto MaxConsoleWidth).</param>
        /// <returns>La cadena alineada a la derecha.</returns>
        public static string rightString(string s, int width = -1) {
            if(width == -1)
                width = MaxConsoleWidth;

            if(s.Length > width)
                return s.Substring(0, width);

            int leftPadding = width - s.Length;
            return new string(' ', leftPadding) + s;
        }

        /// <summary>
        /// Genera una cadena repitiendo un carácter un número específico de veces.
        /// </summary>
        /// <param name="c">El carácter a repetir.</param>
        /// <param name="val">El conteo de repeticiones (por defecto MaxConsoleWidth).</param>
        /// <returns>La cadena resultante.</returns>
        public static string RepeatChar(char c, int val = -1) {
            if(val == -1)
                val = MaxConsoleWidth;

            // Si la consola es muy pequeña, evitamos valores negativos.
            if(val <= 0)
                return string.Empty;

            // Usa el constructor de string (new string(char, count)) que es más eficiente.
            return new string(c, val);
        }

        /// <summary>
        /// Dibuja un menú simple con opciones numeradas en la consola.
        /// </summary>
        /// <param name="encavezado">El título del menú.</param>
        /// <param name="lista">Array de strings con las opciones.</param>
        public static void imprimirMenu(string encavezado, string[] lista) {
            int w = MaxConsoleWidth;

            // Dibuja un fondo limpio para el área del menú.
            printLayoutInCoord(ConsoleColor.Black, 0, 7, w, lista.Length + 4);

            gotoxy(0, 7);
            Console.Write(centeredString($".:{encavezado}:.", w));

            // Lista las opciones, numerándolas desde 1.
            for(int i = 0; i < lista.Length; i++) {
                gotoxy(0, 8 + i);
                Console.WriteLine($"{i + 1}) {lista[i]}");
            }

            gotoxy(0, 8 + lista.Length); // Posiciona el cursor al final del menú
        }

        /// <summary>
        /// Pide y valida la opción numérica del menú elegida por el usuario.
        /// </summary>
        /// <param name="numMax">El valor máximo de opción válido.</param>
        /// <param name="numMin">El valor mínimo de opción válido (por defecto 0, para salir).</param>
        /// <returns>El número de la opción elegida.</returns>
        public static int capturarop(int numMax, int numMin = 0) {
            int op = -1; // Inicializamos en un valor inválido

            Console.WriteLine("0) salir");
            Console.Write("op: ");
            do {
                // Usamos TryParse para evitar errores si el usuario no ingresa un número.
                string input = Console.ReadLine();
                if(!int.TryParse(input, out op)) {
                    op = -1; // Forzar que el bucle se repita si la entrada no es un entero
                }

                if(op < numMin || op > numMax) {
                    Console.WriteLine($"el numero {op} es incorrecto verifique y buelva a intentar");
                    Console.Write("op: "); // Volver a solicitar la entrada
                }
            } while(op < numMin || op > numMax);
            return op;
        }

        /// <summary>
        /// Establece los colores de la consola.
        /// </summary>
        /// <param name="fore">Color de texto.</param>
        /// <param name="back">Color de fondo.</param>
        public static void SetColor(ConsoleColor fore, ConsoleColor back) {
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;
        }

        /// <summary>
        /// Devuelve los colores de la consola a los valores por defecto.
        /// </summary>
        public static void RestoreColors() {
            Console.ResetColor();
        }

        /// <summary>
        /// Escribe una cadena de texto usando colores específicos y restaura los colores anteriores. ¡Es vital para evitar el cambio global de color!
        /// </summary>
        /// <param name="s">La cadena de texto a escribir.</param>
        /// <param name="fore">Color de texto.</param>
        /// <param name="back">Color de fondo.</param>
        public static void WriteColor(string s, ConsoleColor fore, ConsoleColor back = ConsoleColor.Black) {
            // Guardar los colores actuales antes de cambiarlos.
            ConsoleColor originalFore = Console.ForegroundColor;
            ConsoleColor originalBack = Console.BackgroundColor;

            SetColor(fore, back);
            Console.Write(s);

            // Restaurar los colores guardados.
            SetColor(originalFore, originalBack);
        }

        /// <summary>
        /// Repite una cadena de texto varias veces, imprimiéndola con colores temporales. Útil para padding.
        /// </summary>
        /// <param name="s">La cadena de texto a repetir e imprimir.</param>
        /// <param name="fore">Color de texto.</param>
        /// <param name="back">Color de fondo.</param>
        /// <param name="repeat">Número de veces a repetir.</param>
        public static void RepeatWriteColor(string s, ConsoleColor fore, ConsoleColor back = ConsoleColor.Black, int repeat = 2) {

            for(int i = 0; i < repeat; i++) {
                WriteColor(s, fore, back);
            }
        }

        ///////////// mensajes /////////////////////

        /// <summary>
        /// Muestra un mensaje de error en rojo y espera una tecla.
        /// </summary>
        /// <param name="message">El mensaje de error a mostrar.</param>
        public static void ShowError(string message) {
            WriteColor($"\n[ ❌ ERROR ] {message}\n", ConsoleColor.Red, ConsoleColor.Black);
            Pause();
        }

        /// <summary>
        /// Muestra un mensaje de éxito en verde y espera una tecla.
        /// </summary>
        /// <param name="message">El mensaje de éxito a mostrar.</param>
        public static void ShowSuccess(string message) {
            WriteColor($"\n[ ✅ ÉXITO ] {message}\n", ConsoleColor.Green, ConsoleColor.Black);
            Pause();
        }

        /// <summary>
        /// Pausa el flujo de ejecución hasta que el usuario presione cualquier tecla.
        /// </summary>
        public static void Pause() {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            // 'true' oculta la tecla presionada
            Console.ReadKey(true);
        }
    }
}