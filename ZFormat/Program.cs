using System;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZFormat {
    internal class Program {

        static void Main(string[] args) {
            Console.Title = "ZFormat - Utilidades de Consola Avanzadas";

            // Intentar establecer un tamaño de ventana.
            try {
                Console.SetWindowSize(120, 35);
            } catch(Exception) {
                // Manejar errores de tamaño en consolas limitadas.
            }

            while(ShowMainMenu()) {
                // El bucle continúa mientras ShowMainMenu devuelva true
            }
            Console.Clear();
            Console.WriteLine("Saliendo del programa ZFormat. ¡Adiós!");
            Thread.Sleep(1000); // Pequeña pausa antes de cerrar
        }

        // --- LÓGICA DEL MENÚ PRINCIPAL ---
        // --- LÓGICA DEL MENÚ PRINCIPAL ---
        static bool ShowMainMenu() {
            Console.Clear();
            int width = ZstringFormat.MaxConsoleWidth;
            string title = "ZFormat - Menú de Demostración de Utilidades de Formato de Consola";

            // Títulos que se adaptan dinámicamente al ancho de la consola
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.DarkMagenta, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.centeredString($"=== {title} ===", width), ConsoleColor.Yellow, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.DarkMagenta, ConsoleColor.Black);

            Console.WriteLine("\nSelecciona una funcionalidad para ver su descripción y ejemplos de uso:");
            Console.WriteLine(ZstringFormat.RepeatChar('-', width));

            // Opciones del menú
            ZstringFormat.WriteColor("  [1] ", ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine("ZTableConsole - Generación de Tablas con Estilo y Totales.");

            ZstringFormat.WriteColor("  [2] ", ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine("ZstringFormat - Colores y Alineación de Cadenas.");

            ZstringFormat.WriteColor("  [3] ", ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine("Demostrar todos los estilos de ZTableStyle (Bordes).");

            ZstringFormat.WriteColor("  [4] ", ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine("Layouts, Menús Interactivos y Barras de Carga Animadas.");

            ZstringFormat.WriteColor("  [5] ", ConsoleColor.Cyan, ConsoleColor.Black); // NUEVA OPCIÓN
            Console.WriteLine("Mini App Demo: Pandita Médico (Interfaz Layout).");

            ZstringFormat.WriteColor("\n  [0] ", ConsoleColor.Red, ConsoleColor.Black);
            Console.WriteLine("Salir del programa.");

            Console.WriteLine("\n" + ZstringFormat.RepeatChar('-', width));
            Console.Write("Tu opción: ");
            string input = Console.ReadLine();

            Console.Clear();

            switch(input) {
                case "1":
                DemonstrateZTableConsole();
                break;
                case "2":
                DemonstrateZStringFormat();
                break;
                case "3":
                DemonstrateAllTableStyles();
                break;
                case "4":
                DemonstrateMenusAndLoadingBars();
                break;
                case "5":
                Veterinaria.ShowInterface(); // LLAMADA A LA NUEVA INTERFAZ
                break;
                case "0":
                return false; // Salir del bucle principal
                default:
                ZstringFormat.ShowError("Opción inválida.");
                break;
            }
            return true; // Continuar en el bucle principal
        }

        // --- FUNCIÓN DE DEMOSTRACIÓN COMPUESTA ---
        static void DemonstrateMenusAndLoadingBars() {
            DemonstrateLayoutsAndMenus();

            // Después del menú, limpiamos para la barra de carga
            Console.Clear();
            DemonstrateLoadingBars();

            WaitForUser();
        }

        // --- FUNCIÓN DE DEMOSTRACIÓN: LAYOUTS Y MENÚS ---
        static void DemonstrateLayoutsAndMenus() {
            int width = ZstringFormat.MaxConsoleWidth;

            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.DarkGreen, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.centeredString("=== 4.1. Layouts y Menús Interactivos ===", width), ConsoleColor.White, ConsoleColor.DarkGreen);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.DarkGreen, ConsoleColor.Black);

            Console.WriteLine("\nDescripción: Demostración de las utilidades de `printLayoutInCoord` y `encavezado`.");

            Console.WriteLine("\n--- EJEMPLO 1: Encabezado Dinámico y Bloque de Color ---\n");

            // 1. Dibujar el encabezado (utiliza printLayoutInCoord internamente)
            ZstringFormat.encavezado("DEMOSTRACIÓN DE ZSTRINGFORMAT");

            // 2. Dibujar un bloque de color en coordenadas específicas (simulando un panel lateral)
            int blockWidth = width / 4;
            // Aseguramos que la posición y el ancho no causen un desbordamiento.
            if(width > 20)
                ZstringFormat.printLayoutInCoord(ConsoleColor.DarkBlue, width - blockWidth, 5, blockWidth, 10);

            ZstringFormat.gotoxy(0, 5);
            Console.WriteLine("El encabezado azul superior se generó con `encavezado()`.");
            Console.WriteLine("El bloque azul oscuro de la derecha se generó con `printLayoutInCoord()`.");
            Console.WriteLine("El cursor vuelve a la fila 5 para seguir escribiendo.");

            // --- EJEMPLO 2: Menú simple con capturarop ---
            Console.WriteLine("\n--- EJEMPLO 2: Menú con ImprimirMenu y CapturarOp ---\n");

            string[] menuOpciones = {
                "Opción 1: Sumar",
                "Opción 2: Restar",
                "Opción 3: Multiplicar",
            };

            // La función imprime el menú y luego pide la opción
            ZstringFormat.imprimirMenu("CALCULADORA SIMPLE", menuOpciones);

            // Nota: Aquí se usa la función capturarop para esperar la entrada
            int op = ZstringFormat.capturarop(menuOpciones.Length);

            Console.WriteLine($"\nSeleccionaste la opción: {op}.");
        }

        // --- FUNCIÓN DE DEMOSTRACIÓN: BARRAS DE CARGA ---
        static void DemonstrateLoadingBars() {
            int width = ZstringFormat.MaxConsoleWidth;

            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.DarkYellow, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.centeredString("=== 4.2. Barras de Carga Animadas ===", width), ConsoleColor.White, ConsoleColor.DarkYellow);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.DarkYellow, ConsoleColor.Black);

            Console.WriteLine("\nDescripción: La función 'cargando' ahora permite especificar el carácter de relleno ('sprite').");
            Console.WriteLine("Muestra el porcentaje, un spinner de Shell/Mac ('|/-\\') y la barra de progreso.");


            int barLength = width > 78 ? 70 : width - 8; // Ajusta el tamaño al ancho de la consola
            int startX = 5;

            // --- EJEMPLO 1: Sprite por defecto ('*') ---
            Console.WriteLine("\n--- EJEMPLO 1: Sprite por defecto (*), Velocidad: 50ms ---");
            ZstringFormat.cargando(startX, Console.CursorTop, barLength, 50); // Usa '*' por defecto

            // --- EJEMPLO 2: Sprite Bloque (Simulador DOS/Windows) ---
            Console.WriteLine("\n--- EJEMPLO 2: Sprite Bloque (█), Velocidad: 15ms ---");
            ZstringFormat.cargando(startX, Console.CursorTop, barLength, 15, '█');

            // --- EJEMPLO 3: Sprite Hash (Simulador ASCII/Retro) ---
            Console.WriteLine("\n--- EJEMPLO 3: Sprite Hash (#), Velocidad: 100ms ---");
            ZstringFormat.cargando(startX, Console.CursorTop, barLength, 100, '#');

            Console.WriteLine("\nBarras de carga completadas.");
        }

        // --- FUNCIÓN DE DEMOSTRACIÓN DE ZTABLECONSOLE (Con Totales) ---
        static void DemonstrateZTableConsole() {
            int width = ZstringFormat.MaxConsoleWidth;

            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Cyan, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.centeredString("=== 1. ZTableConsole - Tablas con Estilo y Fila de Total ===", width), ConsoleColor.White, ConsoleColor.DarkBlue);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Cyan, ConsoleColor.Black);

            Console.WriteLine("\nDescripción: Permite generar tablas con bordes estilizados (línea simple, doble, ASCII, etc.) ");
            Console.WriteLine("y calcula automáticamente los anchos de columna. Soporta colores alternos en filas y fusión de celdas para el total.");

            Console.WriteLine("\n--- EJEMPLO 1: Facturación con Totales y Estilo Doble ---\n");

            // 1. Crear la tabla con encabezados y estilo Doble
            ZTableConsole miTabla = new ZTableConsole(
                new string[] { "ID", "Nombre del Producto", "Cantidad", "Costo Total" },
                TableBorderStyle.DoubleLine // Estilo de línea doble
            );

            // 2. Configuración de Totales
            miTabla.ShowTotalRow = true;
            miTabla.TotalText = " TOTAL de Venta (Unidades) "; // El texto ocupa las N-1 celdas

            // 3. Agregar filas de datos. El total se calculará sobre la última columna.
            miTabla.AddRow(new string[] { "1", "Azucar Refinada", "2", "10" });
            miTabla.AddRow(new string[] { "2", "Frijoles Negros", "1", "5" });
            miTabla.AddRow(new string[] { "3", "Crema Fresca", "1", "12" });
            miTabla.AddRow(new string[] { "4", "Arroz Grano Fino", "5", "25" });
            miTabla.AddRow(new string[] { "5", "Pan de Molde", "3", "15" });

            // 4. Renderizar la tabla
            miTabla.Render();

            Console.WriteLine("\nNotas del ejemplo:");
            Console.WriteLine(" - El ancho de la columna 'Nombre del Producto' se ajustó a 'Arroz Grano Fino' automáticamente.");
            Console.WriteLine(" - La fila de Total fusiona las primeras tres celdas y suma la última: 10+5+12+25+15 = 67.");

            WaitForUser();
        }

        // --- FUNCIÓN DE DEMOSTRACIÓN DE ZSTRINGFORMAT (Colores y Repetición) ---
        static void DemonstrateZStringFormat() {
            int width = ZstringFormat.MaxConsoleWidth;

            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Yellow, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.centeredString("=== 2. ZstringFormat - Manipulación y Color de Cadenas ===", width), ConsoleColor.White, ConsoleColor.DarkRed);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Yellow, ConsoleColor.Black);

            Console.WriteLine("\nDescripción: Proporciona métodos estáticos para escribir texto con colores específicos ");
            Console.WriteLine("de primer plano y fondo en la consola, y para repetir caracteres eficientemente.");

            Console.WriteLine("\n--- EJEMPLO 1: Escritura con Colores ---\n");

            Console.Write("Texto normal. ");
            ZstringFormat.WriteColor("¡Texto en Rojo ", ConsoleColor.Red, ConsoleColor.Black);
            ZstringFormat.WriteColor("y Amarillo de Fondo!", ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine(" Continúa el texto normal.");

            Console.Write("Esto es un encabezado: ");
            ZstringFormat.WriteColor(" MI REPORTE ", ConsoleColor.White, ConsoleColor.DarkGreen);
            Console.WriteLine();

            Console.WriteLine("\n--- EJEMPLO 2: Repetición de Caracteres (Simulación de Barras) ---\n");

            string barChar = "█";
            int length = 50;
            Console.Write("Progreso 25%: ");
            ZstringFormat.RepeatWriteColor(barChar, ConsoleColor.Green, ConsoleColor.DarkGreen, length / 4);
            Console.WriteLine();

            Console.Write("Progreso 75%: ");
            ZstringFormat.RepeatWriteColor(barChar, ConsoleColor.Yellow, ConsoleColor.DarkYellow, (length * 3) / 4);
            Console.WriteLine();

            Console.Write("Línea divisoria: ");
            ZstringFormat.RepeatWriteColor("—", ConsoleColor.Gray, ConsoleColor.Black, length);
            Console.WriteLine();

            WaitForUser();
        }

        // --- FUNCIÓN DE DEMOSTRACIÓN DE TODOS LOS ESTILOS DE TABLA ---
        static void DemonstrateAllTableStyles() {
            int width = ZstringFormat.MaxConsoleWidth;

            // --- Configuración de Paginación ---
            const int TablasPorPagina = 2;
            var styles = Enum.GetValues(typeof(TableBorderStyle)).Cast<TableBorderStyle>().ToArray();
            int totalStyles = styles.Length;

            // --- Encabezado General ---
            Console.Clear(); // Limpia antes de empezar
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Magenta, ConsoleColor.Black);
            ZstringFormat.WriteColor(ZstringFormat.centeredString("=== 3. ZTableStyle - Demostración de Todos los Estilos de Borde ===", width), ConsoleColor.White, ConsoleColor.DarkCyan);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Magenta, ConsoleColor.Black);
            Console.WriteLine("\nSe demuestra la misma tabla de datos renderizada con cada estilo de borde disponible:");

            // --- Bucle de Paginación ---
            for(int i = 0; i < totalStyles; i++) {
                var style = styles[i];

                // 1. Dibuja el encabezado de la tabla actual
                ZstringFormat.WriteColor($"\n--- Estilo: {style} ({i + 1}/{totalStyles}) ---", ConsoleColor.Yellow, ConsoleColor.DarkBlue);
                Console.WriteLine();

                // 2. Crea y configura la tabla
                ZTableConsole miTabla = new ZTableConsole(
                    new string[] { "Columna 1", "Columna 2 (Largo)", "Valor" },
                    style
                );

                // Opcional: Centrar la tabla en el espacio disponible después del menú lateral (si estuviera en Veterinaria.cs)
                // Por ahora, se dibuja donde está el cursor (RenderX/Y no es necesario si la tabla se dibuja secuencialmente)

                miTabla.ShowTotalRow = true;
                miTabla.TotalText = $"Total {style}";

                miTabla.AddRow(new string[] { "A", "Texto corto", "100" });
                miTabla.AddRow(new string[] { "B", "Un texto muy largo", "200" });

                // 3. Renderiza la tabla
                miTabla.Render();

                // 4. Lógica de Pausa y Paginación
                // Condición de pausa: Si se ha llegado al límite por página O si es la última tabla
                if((i + 1) % TablasPorPagina == 0 || i == totalStyles - 1) {

                    // Pausa y espera la acción del usuario
                    if(i < totalStyles - 1) {
                        // Mensaje si hay más páginas
                        ZstringFormat.WriteColor("\nPresione ENTER para ver las siguientes tablas...", ConsoleColor.Green, ConsoleColor.Black);
                        WaitForUser();
                        Console.Clear();

                        // Vuelve a dibujar el encabezado principal en la nueva pantalla
                        ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Magenta, ConsoleColor.Black);
                        ZstringFormat.WriteColor(ZstringFormat.centeredString("=== 3. ZTableStyle - Demostración de Todos los Estilos de Borde ===", width), ConsoleColor.White, ConsoleColor.DarkCyan);
                        ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', width), ConsoleColor.Magenta, ConsoleColor.Black);
                        Console.WriteLine("\nContinuación de la demostración de estilos de borde:");
                    } else {
                        // Mensaje final
                        ZstringFormat.WriteColor("\n--- Fin de la demostración de estilos de tabla ---", ConsoleColor.Yellow, ConsoleColor.DarkBlue);
                        WaitForUser();
                    }
                }
            }
        }

        // --- UTILIDAD AUXILIAR ---
        static void WaitForUser() {
            Console.WriteLine("\n\nPresiona ENTER para volver al menú principal...");
            Console.ReadLine();
        }
    }
}