using System;
using System.Text;
using System.Threading;
using ZFormat.ZLayer;
using ZFormat.PanditaMedic;

namespace ZFormat {

    // --- Enum para gestionar el foco ---
    enum FocusState {
        Menu,
        Content
    }

    internal class Program {


        // -----------------------------------------------------------------------------
        // 1. LÓGICA DE DIBUJO DEL TÍTULO (Movida a la Clase Program)
        // -----------------------------------------------------------------------------
        static void DrawMyTitle(int width, int height) {
            string appName = "Sistema de Farmacia ZFormat PanditaMedic V1.0";
            ZstringFormat.gotoxy(0, 1);
            ZstringFormat.WriteColor(
                ZstringFormat.centeredString(appName, width),
                ConsoleColor.Yellow,
                ConsoleColor.DarkBlue
            );
        }

        // -----------------------------------------------------------------------------
        // 2. MÉTODO PRINCIPAL (GAME LOOP)
        // -----------------------------------------------------------------------------
        static void Main(string[] args) {

            // Ajuste de Codificación (para los caracteres especiales)
            Console.OutputEncoding = Encoding.UTF8;

            // --- CONSTANTES DE LAYOUT ---
            const int TitleBarHeight = 3;
            const int StatusBarHeight = 1;
            const int SideMenuWidth = 25;

            // 1. CALCULAR DIMENSIONES
            int availableHeight = Console.WindowHeight - TitleBarHeight - StatusBarHeight;
            int contentAreaWidth = Console.WindowWidth - SideMenuWidth;

            // 2. INICIALIZACIÓN DEL MOTOR
            var appManager = new ZLayoutManager();

            // 3. DEFINICIÓN DE CAPAS ESTATICAS (Layout)
            var barraTitulo = new ZTitleBar(0, 0, TitleBarHeight);
            var menuOpciones = new string[] { "Clientes", "Inventario", "Reportes", "Configuración", "Ayuda" }; // 👈 Opciones Finales
            var menuLateral = new ZSideMenu(menuOpciones, 0, TitleBarHeight, SideMenuWidth, availableHeight);
            var statusBar = new ZStatusBar("ZStatusBar: Listo para la acción.", StatusBarHeight);
            var contentContainer = new ZContentContainer(
                startX: SideMenuWidth,
                startY: TitleBarHeight,
                width: contentAreaWidth,
                height: availableHeight
            );

            // 4. DEFINICIÓN DE VISTAS DINÁMICAS (PanditaMedic)
            // 💡 Vista de Bienvenida (Estado Inicial)
            var welcomeView = new WelcomeView();
            var clientesView = new ClientsView();
            // ... Aquí irían InventoryView, ReportsView, etc. ...

            // 5. ENSAMBLAJE
            barraTitulo.SetContent(DrawMyTitle);
            appManager.AddLayer(barraTitulo);
            appManager.AddLayer(statusBar);
            appManager.AddLayer(menuLateral);
            appManager.AddLayer(contentContainer);

            // Estado inicial: Mostrar la vista de bienvenida
            FocusState currentFocus = FocusState.Menu;
            contentContainer.SetContent(welcomeView); // 👈 Vista de Bienvenida al inicio

            // --- AJUSTES INICIALES ---
            Console.Clear();
            ZstringFormat.RestoreColors();
            Console.CursorVisible = false;

            // --- BUCLE PRINCIPAL (Game Loop) ---
            int loopDelayMs = 33;

            while(appManager.IsRunning) {

                // 1. MANEJO DE EVENTOS (Input)
                ConsoleKeyInfo? keyInfo = appManager.PollInput();

                if(keyInfo.HasValue) {
                    ConsoleKey key = keyInfo.Value.Key;
                    bool eventConsumed = false;

                    // Lógica de Manejo de Foco y Salida
                    if(key == ConsoleKey.Tab) {
                        currentFocus = (currentFocus == FocusState.Menu) ? FocusState.Content : FocusState.Menu;
                        statusBar.SetStatus($"Foco cambiado a: {currentFocus}", ConsoleColor.DarkMagenta, ConsoleColor.White, 1500);
                        eventConsumed = true;
                    } else if(key == ConsoleKey.Escape || key == ConsoleKey.Q) {
                        appManager.IsRunning = false;
                        eventConsumed = true;
                    }

                    if(!eventConsumed) {
                        if(currentFocus == FocusState.Menu) {
                            eventConsumed = menuLateral.HandleInput(keyInfo.Value);

                            if(key == ConsoleKey.Enter) {
                                int selectedIndex = menuLateral.GetSelectedIndex();

                                // >>> LÓGICA DE NAVEGACIÓN DE VISTAS (Switching) <<<
                                LayerBase newView = selectedIndex switch {
                                    0 => clientesView, // "Clientes"
                                    _ => welcomeView,  // Cualquier otra opción, por ahora muestra la bienvenida
                                };

                                contentContainer.SetContent(newView);
                                statusBar.SetStatus($"Módulo {menuOpciones[selectedIndex]} cargado. Foco movido a Contenido.",
                                    ConsoleColor.DarkBlue, ConsoleColor.White, 2500);

                                currentFocus = FocusState.Content;
                                eventConsumed = true;
                            }

                        } else if(currentFocus == FocusState.Content) {
                            // Si el contenido no consume el evento, podemos hacer que ENTER o ESC devuelvan el foco al menú
                            eventConsumed = contentContainer.HandleInput(keyInfo.Value);

                            // Si el contenido no consumió la tecla (ej. no fue una acción CRUD)
                            if(!eventConsumed && (key == ConsoleKey.Escape || key == ConsoleKey.Q)) {
                                currentFocus = FocusState.Menu;
                                statusBar.SetStatus("Foco devuelto al Menú Principal.", ConsoleColor.DarkMagenta, ConsoleColor.White, 1500);
                                eventConsumed = true;
                            }
                        }
                    }
                }

                // 2. ACTUALIZAR LÓGICA DE LA APLICACIÓN (Update)
                appManager.UpdateLayers();

                // 3. DIBUJAR (Render)
                appManager.Draw();

                // 4. CONTROL DE TIEMPO
                Thread.Sleep(loopDelayMs);
            }
            // --- FIN BUCLE ---

            // Limpieza final
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Aplicación Farmacia PanditaMedic finalizada. Presiona cualquier tecla para cerrar la consola...");
            Console.ReadKey(true);
        }
    }
}