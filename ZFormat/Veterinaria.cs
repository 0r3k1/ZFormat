using System;
using System.Linq;
using System.Threading;

namespace ZFormat {

    // NOTA: Asumimos que ZTableConsole y TableBorderStyle están definidos en tu proyecto.

    public static class Veterinaria {

        // Colores de la paleta: Usaremos azul oscuro/claro y cian.
        private static readonly ConsoleColor ColorFondoPrincipal = ConsoleColor.DarkBlue;
        private static readonly ConsoleColor ColorBarraLateral = ConsoleColor.Blue;
        private static readonly ConsoleColor ColorTextoPrincipal = ConsoleColor.White;
        private static readonly ConsoleColor ColorTextoLateral = ConsoleColor.Cyan;
        private static readonly ConsoleColor ColorCabecera = ConsoleColor.Blue;

        // Dimensiones del Layout
        private static int AnchoTotal => ZstringFormat.MaxConsoleWidth;
        private static int AltoTotal = 30;
        private static int AnchoLateral = 25; // Ancho fijo para el menú lateral
        private static int AltoCabecera = 3;
        private static int AltoPie = 1;

        // Coordenadas del Área Principal de Contenido
        private static int ContenidoX => AnchoLateral + 1;
        private static int ContenidoY => AltoCabecera;
        private static int ContenidoW => AnchoTotal - AnchoLateral - 1;
        private static int ContenidoH => AltoTotal - AltoCabecera - AltoPie;

        // Opciones del menú lateral
        private static string[] MenuOpciones = new string[] {
            "Pacientes (P)",
            "Citas (C)",
            "Registro (R)",
            "Reportes (T)"
        };

        // --- FUNCIÓN PRINCIPAL DE INTERFAZ ---
        public static void ShowInterface() {
            Console.Clear();

            // 1. Dibujar el Layout Completo
            DrawLayout();

            // 2. Establecer colores por defecto para el contenido principal
            ZstringFormat.SetColor(ColorTextoPrincipal, ColorFondoPrincipal);

            string opcionActual = "INICIO";
            bool running = true;

            while(running) {

                // Muestra la barra de título del área principal y el contenido
                DisplayPrincipalContent(opcionActual);

                // Muestra el menú lateral y captura la opción
                string input = DisplayAndCaptureLateralMenu();

                // Lógica de navegación
                switch(input.ToUpper()) {
                    case "1":
                    case "P":
                    opcionActual = "PACIENTES";
                    break;
                    case "2":
                    case "C":
                    opcionActual = "CITAS";
                    break;
                    case "3":
                    case "R":
                    opcionActual = "REGISTRO";
                    break;
                    case "4":
                    case "T":
                    opcionActual = "REPORTES";
                    break;
                    case "0":
                    running = false; // Salir
                    break;
                    default:
                    // Mensaje de error temporal en el pie
                    DisplayFooter($"❌ Opción inválida '{input}'. Intente nuevamente.", ConsoleColor.Red, ConsoleColor.Black);
                    Thread.Sleep(500);
                    break;
                }
            }

            // Mensaje de salida
            Console.Clear();
            ZstringFormat.RestoreColors();
            Console.WriteLine("Saliendo de la Interfaz 'Pandita Médico'.");
        }

        // --- FUNCIONES DE DIBUJO ---

        /// <summary>
        /// Dibuja el marco y las secciones de la interfaz.
        /// </summary>
        private static void DrawLayout() {
            // Se asegura de que la consola tenga el tamaño correcto si es posible
            if(Console.WindowHeight < AltoTotal)
                AltoTotal = Console.WindowHeight;

            // Fondo general (lo pintamos primero para evitar sobreescritura)
            ZstringFormat.printLayoutInCoord(ColorFondoPrincipal, 0, 0, AnchoTotal, AltoTotal);

            // 1. Cabecera (Barra de Título)
            ZstringFormat.printLayoutInCoord(ColorCabecera, 0, 0, AnchoTotal, AltoCabecera);
            ZstringFormat.gotoxy(0, 1);
            ZstringFormat.WriteColor(ZstringFormat.centeredString("PANDITA MÉDICO - Sistema de Gestión Veterinaria 🐾", AnchoTotal), ColorTextoPrincipal, ColorCabecera);
            ZstringFormat.gotoxy(0, 2);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('=', AnchoTotal), ColorCabecera, ColorCabecera); // Línea divisoria

            // 2. Barra Lateral (Menú de Navegación)
            ZstringFormat.printLayoutInCoord(ColorBarraLateral, 0, AltoCabecera, AnchoLateral, AltoTotal - AltoCabecera - AltoPie);
            ZstringFormat.gotoxy(0, AltoCabecera);
            ZstringFormat.WriteColor(ZstringFormat.centeredString(" N A V E G A C I Ó N \n", AnchoLateral), ColorTextoPrincipal, ColorBarraLateral);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('-', AnchoLateral-3), ColorTextoLateral, ColorBarraLateral); // Separador

            // 3. Pie de página
            DisplayFooter("Use [0] para salir de la interfaz.", ConsoleColor.White, ConsoleColor.DarkGray);
        }

        /// <summary>
        /// Dibuja y gestiona el menú lateral.
        /// </summary>
        /// <returns>La opción ingresada por el usuario (número o letra).</returns>
        private static string DisplayAndCaptureLateralMenu() {
            int yPos = AltoCabecera + 2;

            // Limpia el área del menú lateral para repintar
            ZstringFormat.printLayoutInCoord(ColorBarraLateral, 0, AltoCabecera + 2, AnchoLateral, MenuOpciones.Length + 4);

            // Dibuja las opciones
            ZstringFormat.SetColor(ColorTextoLateral, ColorBarraLateral);
            for(int i = 0; i < MenuOpciones.Length; i++) {
                ZstringFormat.gotoxy(2, yPos + i);
                Console.Write($"[{i + 1}] {MenuOpciones[i]}");
            }

            // Dibuja la opción de salir
            ZstringFormat.SetColor(ConsoleColor.Red, ColorBarraLateral);
            ZstringFormat.gotoxy(2, yPos + MenuOpciones.Length + 1);
            Console.Write("[0] Salir");

            // Posiciona el cursor para la entrada del usuario
            ZstringFormat.SetColor(ConsoleColor.Yellow, ColorBarraLateral);
            ZstringFormat.gotoxy(2, yPos + MenuOpciones.Length + 3);
            Console.Write("Opción: ");

            // Captura la entrada. Usamos ReadLine simple ya que el error se maneja en el loop principal.
            string input = Console.ReadLine();

            return input;
        }

        /// <summary>
        /// Muestra el contenido dinámico en el área principal y su submenú.
        /// </summary>
        private static void DisplayPrincipalContent(string seccion) {
            // Limpia el área de contenido principal
            ZstringFormat.printLayoutInCoord(ColorFondoPrincipal, ContenidoX, ContenidoY, ContenidoW, ContenidoH);

            // Título de la sección principal
            ZstringFormat.gotoxy(ContenidoX + 2, ContenidoY + 1);
            ZstringFormat.WriteColor(ZstringFormat.leftString($"SECCIÓN: {seccion}", ContenidoW - 4), ColorTextoPrincipal, ColorFondoPrincipal);

            // Separador de submenú
            ZstringFormat.gotoxy(ContenidoX + 2, ContenidoY + 2);
            ZstringFormat.WriteColor(ZstringFormat.RepeatChar('—', ContenidoW - 4), ConsoleColor.Gray, ColorFondoPrincipal);

            // Lógica de submenú y datos
            string[] subMenu = { "Ver Lista", "Agregar Nuevo" };

            switch(seccion) {
                case "PACIENTES":
                DisplaySubMenu(subMenu, ContenidoX + 2, ContenidoY + 3);
                DisplayDataExample(ContenidoX + 2, ContenidoY + 5, "Pacientes Registrados");
                break;
                case "CITAS":
                DisplaySubMenu(subMenu, ContenidoX + 2, ContenidoY + 3);
                DisplayDataExample(ContenidoX + 2, ContenidoY + 5, "Citas Pendientes");
                break;
                case "REGISTRO":
                DisplaySubMenu(new string[] { "Ver Historial", "Buscar por ID" }, ContenidoX + 2, ContenidoY + 3);
                ZstringFormat.gotoxy(ContenidoX + 2, ContenidoY + 5);
                Console.WriteLine("Aquí va la lógica de búsqueda y visualización de registros.");
                break;
                case "REPORTES":
                DisplaySubMenu(new string[] { "Generar PDF", "Ver Estadísticas" }, ContenidoX + 2, ContenidoY + 3);
                ZstringFormat.gotoxy(ContenidoX + 2, ContenidoY + 5);
                Console.WriteLine("Aquí se muestran gráficos o resúmenes estadísticos.");
                break;
                default:
                ZstringFormat.gotoxy(ContenidoX + 2, ContenidoY + 3);
                Console.WriteLine("Bienvenido. Seleccione una opción en la barra lateral.");
                break;
            }
        }

        /// <summary>
        /// Dibuja el submenú en el área principal.
        /// </summary>
        private static void DisplaySubMenu(string[] opciones, int x, int y) {
            ZstringFormat.gotoxy(x, y);
            ZstringFormat.WriteColor("SUB-MENÚ: ", ConsoleColor.Yellow, ColorFondoPrincipal);
            for(int i = 0; i < opciones.Length; i++) {
                ZstringFormat.WriteColor($"[{i + 1}] {opciones[i]}   ", ConsoleColor.Green, ColorFondoPrincipal);
            }
        }

        /// <summary>
        /// Dibuja una tabla de ejemplo en el área principal usando ZTableConsole.
        /// </summary>
        private static void DisplayDataExample(int x, int y, string title) {
            ZstringFormat.gotoxy(x, y);
            ZstringFormat.WriteColor($"--- {title} ---", ConsoleColor.Cyan, ColorFondoPrincipal);

            // Creamos una tabla simple de ejemplo
            ZTableConsole tabla = new ZTableConsole(
                new string[] { "ID", "Nombre", "Especie", "Última Cita" },
                TableBorderStyle.SingleLine
            );

            // 1. CONFIGURACIÓN DE POSICIÓN
            // Le indicamos a la tabla donde debe empezar a dibujarse.
            // Empezaremos DOS líneas después del título (y + 2). La tabla se encargará del resto.
            tabla.RenderX = x;
            tabla.RenderY = y + 2;

            // 2. CONFIGURACIÓN DE ESTILO
            tabla.HeaderColor = ConsoleColor.Yellow;
            tabla.HeaderBackColor = ColorFondoPrincipal;
            tabla.AlternatingRowColor = ConsoleColor.DarkGray; // Color alterno para las filas impares
            tabla.RowBackColor = ColorFondoPrincipal; // Color de fondo para filas pares (negro en este caso)

            tabla.AddRow(new string[] { "1", "Max", "Perro", "2025-09-10" });
            tabla.AddRow(new string[] { "2", "Lola", "Gato", "2025-08-25" });
            tabla.AddRow(new string[] { "3", "Pipo", "Pájaro", "2025-10-01" });

            // NO se necesita ZstringFormat.gotoxy() aquí.

            // 3. Renderiza la tabla (se dibujará usando tabla.RenderX/Y)
            tabla.Render();

            // Reposiciona el cursor al final del área de la tabla para evitar conflicto con el menú lateral
            // Nota: Hemos añadido más offset (2 líneas adicionales) porque el Render() ahora posiciona el cursor.
            ZstringFormat.gotoxy(x, y + tabla.RowCount + 6);
        }

        /// <summary>
        /// Muestra un mensaje en la barra de pie de página.
        /// </summary>
        private static void DisplayFooter(string message, ConsoleColor fore, ConsoleColor back) {
            int yFooter = AltoTotal - AltoPie;
            ZstringFormat.printLayoutInCoord(back, 0, yFooter, AnchoTotal, AltoPie);
            ZstringFormat.gotoxy(1, yFooter);
            ZstringFormat.WriteColor(message, fore, back);
            // Vuelve a la posición de contenido para evitar pintar sobre el pie
            ZstringFormat.gotoxy(ContenidoX + 2, ContenidoY + 1);
        }
    }
}