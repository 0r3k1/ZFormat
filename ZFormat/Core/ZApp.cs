using System;
using ZFormat.Layouts;

namespace ZFormat.Core {
    public class ZApp {
        private ZBuffer _buffer;
        private bool _isRunning;
        public string Title { get; set; }

        // El control que actualmente recibe las teclas
        public ZControl? FocusedControl { get; set; }

        // El contenedor principal de toda la app
        public ZPanel RootLayout { get; private set; }

        private List<ZTimer> _timers = new List<ZTimer>();
        public List<ZTimer> Timers { get; } = new List<ZTimer>();

        public event Action OnUpdate;

        public ZApp(string title) {
            Title = title;
            Console.Title = title;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            // Inicializamos el buffer al tamaño de la consola
            _buffer = new ZBuffer(Console.WindowWidth, Console.WindowHeight);

            // Creamos el panel principal que ocupa toda la pantalla
            RootLayout = new ZPanel {
                Width = _buffer.Width,
                Height = _buffer.Height,
                Name = "RootPanel"
            };
        }

        public ZTimer CreateTimer(double intervalMs) {
            var timer = new ZTimer(intervalMs);
            _timers.Add(timer);
            return timer;
        }

        public void Run() {
            _isRunning = true;
            while(_isRunning) {
                // 1. Ejecutar lógicas de fondo (como los Timers)
                OnUpdate?.Invoke();
                Update();

                // 2. Dibujar la pantalla
                Draw();

                // 3. Procesar entrada
                Input();

                // 4. Pausa de ~16ms (Equivale a 60 FPS) para no quemar el CPU
                System.Threading.Thread.Sleep(16);
            }
            Console.ResetColor();
            Console.Clear();
        }

        private void Update() {
            foreach(var timer in _timers) {
                timer.Update();
            }
        }

        private void Draw() {
            _buffer.Clear();
            RootLayout.Render(_buffer);
            _buffer.Render();
        }

        private void Input() {
            if(!Console.KeyAvailable)
                return;

            ZKey zKey = ZKey.ReadKey();

            if(zKey.Key == ConsoleKey.F10) { _isRunning = false; return; }

            // 1. Lógica de Tabulación automática
            if(zKey.IsTab) {
                SwitchFocus();
                return;
            }

            // 2. Si hay un control enfocado, él procesa la tecla
            if(FocusedControl != null && FocusedControl.IsVisible) {
                FocusedControl.HandleInput(zKey);
            }

            OnGlobalKeyPressed?.Invoke(zKey);
        }

        private void SwitchFocus() {
            var focusableControls = GetAllFocusable(RootLayout);
            if(focusableControls.Count == 0)
                return;

            int currentIndex = focusableControls.IndexOf(FocusedControl);
            int nextIndex = (currentIndex + 1) % focusableControls.Count;

            // 1. Quitarle el foco al control actual (si hay alguno)
            if(FocusedControl != null) {
                FocusedControl.HasFocus = false;
            }

            // 2. Asignar el nuevo control y ENCENDER su foco
            FocusedControl = focusableControls[nextIndex];
            FocusedControl.HasFocus = true;
        }

        // Método recursivo para encontrar controles focusables en paneles anidados
        private List<ZControl> GetAllFocusable(ZControl container) {
            var list = new List<ZControl>();

            // 1. Si el contenedor es un Panel, buscamos en sus hijos
            if(container is ZPanel p) {
                foreach(var child in p.Children) {
                    if(child.IsVisible && child.IsFocusable)
                        list.Add(child);

                    // Buscar recursivamente si este hijo tiene más hijos por dentro
                    list.AddRange(GetAllFocusable(child));
                }
            }
            // 2. Si el contenedor es un sistema de pestañas, buscamos en la pestaña ACTIVA
            else if(container is ZFormat.Layouts.ZTabContainer tc) {
                if(tc.Tabs.Count > 0) {
                    var activeContent = tc.Tabs[tc.SelectedIndex].Content;
                    if(activeContent != null) {
                        if(activeContent.IsVisible && activeContent.IsFocusable)
                            list.Add(activeContent);

                        // Buscar recursivamente dentro de esa pestaña
                        list.AddRange(GetAllFocusable(activeContent));
                    }
                }
            }

            return list;
        }

        public event Action<ZKey> OnGlobalKeyPressed;
    }
}