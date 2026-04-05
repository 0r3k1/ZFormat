using System;
using ZFormat.Core;
using ZFormat.Controls;
using ZFormat.Layouts;

namespace PanditaCoffe {
    class Program {
        // Variables globales para acceder a ellas fácilmente
        static ZApp app;
        static ZPanel panelLogin;
        static ZPanel panelDashboard;
        static ZLabel lblUsuarioLogueado;
        static ZList menuLateral;

        static void Main() {
            // 1. Configurar Tema y App
            ZTheme.SetTheme("terminal-azul"); // Un tema azulado queda bien para sistemas
            app = new ZApp("Pandita Coffe - ERP");

            // 2. Construir las dos pantallas
            ConstruirLogin();
            ConstruirDashboard();

            // 3. Añadir ambas pantallas al Root (El Dashboard nace oculto)
            app.RootLayout.AddChild(panelDashboard);
            app.RootLayout.AddChild(panelLogin);

            app.Run();
        }

        static void ConstruirLogin() {
            int w = 45, h = 12; // Lo hicimos un poquito más ancho (42) para que respire
            panelLogin = new ZPanel {
                X = (app.RootLayout.Width - w) / 2,
                Y = (app.RootLayout.Height - h) / 2,
                Width = w,
                Height = h,
                BackgroundColor = ConsoleColor.DarkGray,
                ZIndex = 10
            };

            // Al darle un Width=28 evitamos que el fondo gris se desborde creando ese pixel extra
            panelLogin.AddChild(new ZLabel("☕ PANDITA COFFE - ACCESO ☕") { X = 7, Y = 1, Width = 27, ForegroundColor = ConsoleColor.Yellow, BackgroundColor = ConsoleColor.DarkGray });

            // Inputs más simétricos (Width = 34)
            var txtUser = new ZInput { Label = "Usuario: ", X = 4, Y = 4, Width = 34, IsFocusable = true, BackgroundColor = ConsoleColor.DarkGray };
            var txtPass = new ZInput { Label = "Clave:   ", X = 4, Y = 6, Width = 34, IsFocusable = true, IsPassword = true, BackgroundColor = ConsoleColor.DarkGray };

            var btnIngresar = new ZButton { Text = "INGRESAR", X = 14, Y = 9, IsFocusable = true, BackgroundColor = ConsoleColor.DarkGray };

            btnIngresar.OnClick += () => {
                if(txtUser.Value == "admin" && txtPass.Value == "123") {
                    panelLogin.IsVisible = false;
                    panelDashboard.IsVisible = true;
                    lblUsuarioLogueado.Text = $"👤 {txtUser.Value.ToUpper()}";

                    // Forzamos el redibujado de la pantalla principal en color negro
                    app.RootLayout.BackgroundColor = ConsoleColor.Black;
                    app.RootLayout.UpdateLayout();

                    app.FocusedControl = menuLateral;
                    menuLateral.HasFocus = true; // Encendemos el foco visual del menú
                } else {
                    btnIngresar.Text = "DATOS INCORRECTOS";
                    btnIngresar.FocusColor = ConsoleColor.Red;
                }
            };

            panelLogin.AddChild(txtUser);
            panelLogin.AddChild(txtPass);
            panelLogin.AddChild(btnIngresar);

            // ¡LA MAGIA DEL FOCO INICIAL!
            app.FocusedControl = txtUser;
            txtUser.HasFocus = true; // Encendemos el cursor desde el inicio. Ya no tienes que dar TAB.
        }

        static void ConstruirDashboard() {
            // El dashboard principal ahora es negro
            panelDashboard = new ZPanel { Dock = ZDockMode.Fill, IsVisible = false, ZIndex = 1, BackgroundColor = ConsoleColor.Black };

            // --- HEADER (Azul) ---
            var header = new ZPanel { Dock = ZDockMode.Top, Height = 3, BackgroundColor = ConsoleColor.DarkBlue };

            lblUsuarioLogueado = new ZLabel("👤 Invitado") { X = 2, Y = 1, ForegroundColor = ConsoleColor.Yellow, BackgroundColor = ConsoleColor.DarkBlue };
            var lblTitulo = new ZLabel("☕ PANDITA COFFE ERP") { X = app.RootLayout.Width / 2 - 10, Y = 1, ForegroundColor = ConsoleColor.White, BackgroundColor = ConsoleColor.DarkBlue };
            var lblReloj = new ZLabel("00:00:00") { X = app.RootLayout.Width - 12, Y = 1, ForegroundColor = ConsoleColor.Cyan, BackgroundColor = ConsoleColor.DarkBlue };

            header.AddChild(lblUsuarioLogueado);
            header.AddChild(lblTitulo);
            header.AddChild(lblReloj);

            var timerReloj = app.CreateTimer(1000);
            timerReloj.OnTick += () => lblReloj.Text = DateTime.Now.ToString("HH:mm:ss");
            timerReloj.Start();
            app.OnUpdate += () => timerReloj.Update();

            // --- FOOTER (Gris) ---
            var footer = new ZPanel { Dock = ZDockMode.Bottom, Height = 1, BackgroundColor = ConsoleColor.DarkGray };
            footer.AddChild(new ZLabel(" [TAB] Navegar | [ENTER] Seleccionar | [↑↓] Mover Menú | [F10] Salir ") { X = 2, Y = 0, ForegroundColor = ConsoleColor.Black, BackgroundColor = ConsoleColor.DarkGray });

            // --- BODY (Negro) ---
            var body = new ZPanel { Dock = ZDockMode.Fill, BackgroundColor = ConsoleColor.Black };

            // Menú Lateral (Violeta = DarkMagenta)
            var sidebar = new ZPanel { Dock = ZDockMode.Left, Width = 20, BackgroundColor = ConsoleColor.DarkMagenta };
            menuLateral = new ZList { X = 0, Y = 1, Width = 20, Height = 15, IsFocusable = true, SelectedBackColor = ConsoleColor.White, SelectedForeColor = ConsoleColor.Black, BackgroundColor = ConsoleColor.DarkMagenta };

            menuLateral.Items.Add(new ZListItem("Usuarios", "👤"));
            menuLateral.Items.Add(new ZListItem("Ventas", "💰"));
            menuLateral.Items.Add(new ZListItem("Pedidos", "🛒"));
            menuLateral.Items.Add(new ZListItem("Inventario", "📦"));
            menuLateral.Items.Add(new ZListItem("Clientes", "🤝"));
            menuLateral.Items.Add(new ZListItem("Ayuda", "💡"));
            menuLateral.Items.Add(new ZListItem("Nosotros", "⭐"));

            sidebar.AddChild(menuLateral);

            // Contenido Principal (Negro)
            var contenido = new ZPanel { Dock = ZDockMode.Fill, BackgroundColor = ConsoleColor.Black };
            var lblBienvenida = new ZLabel("Seleccione una opción del menú lateral para comenzar.") { X = 5, Y = 5, BackgroundColor = ConsoleColor.Black };
            contenido.AddChild(lblBienvenida);

            menuLateral.OnItemSelected += (item) => {
                lblBienvenida.Text = $"Has seleccionado el módulo de: {item.Text.ToUpper()}";
            };

            body.AddChild(sidebar);
            body.AddChild(contenido);

            panelDashboard.AddChild(header);
            panelDashboard.AddChild(footer);
            panelDashboard.AddChild(body);
        }
    }
}