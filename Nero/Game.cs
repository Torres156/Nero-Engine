using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using Nero.Control;
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;

    public sealed class Game
    {
        public static bool Running = false;                                                 // Estado do jogo
        public static int FPS = 0;                                                          // FPS atual
        public static float DeltaTime;                                                      // Tempo do delta
        public static Vector2 MousePosition;                                                // Posição do Mouse
        public static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "/";   // Diretório do jogo
        public static RenderWindow Window;                                                  // Dispositivo gráfico para janela
        

        static System.Windows.Forms.Form form;                                              // Janela do jogo


        // Configurações
        public static Vector2 Size = new Vector2(1024, 600);        // Tamanho da janela
        public static Vector2 MinSize = new Vector2(640, 360);      // Tamanho mínimo para a janela
        public static bool FPS_visible = false;                     // Esconde ou mostra o FPS
        public static Vector2 FPS_position = new Vector2(10, 10);   // Posição do FPS
        public static View DefaultView { get; private set; }        // Definição para Resolução
        public static Color BackgroundColor = Color.CornflowerBlue; // Cor do fundo da janela
        public static bool Button_Maximize = false;                 // Botão de maximizar a janela
        public static string Title = "Nero Library Game";           // Titulo da janela
        public static bool VSync = false;                           // Modo V-Sync
        public static bool Fullscreen = false;                      // Modo tela cheia
                

        // Cena
        static SceneBase scene = null;      // Cena atual
        static SceneBase nextscene = null;  // Proxima cena
        static Type StartScene = null;      // Cena inicial


        #region Controladores de janela
        private const int SW_MAXIMIZE = 3;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        
        // Action delegates
        public static Action OnUpdate = delegate { };       // Evento para atualizar o jogo
        public static Action OnResources = delegate { };    // Evento para carregar os recursos
        public static Action OnResize = delegate { };       // Evento de redimensionar a janela
        public static Action OnClosed = delegate { };       // Evento de fechar o jogo
                        
        
        /// <summary>
        /// Borda da janela
        /// </summary>
        public static System.Windows.Forms.FormBorderStyle WindowBorder
        {
            get
            {
                var frm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.SystemHandle);
                return frm.FormBorderStyle;
            }
            set
            {
                var frm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.SystemHandle);
                frm.FormBorderStyle = value;
            }
        }

        /// <summary>
        /// Roda o jogo
        /// </summary>
        public static void Run(System.Windows.Forms.FormBorderStyle BorderStyle, bool maximized = false)
        {
            OnResources.Invoke();
            CreateWindow(BorderStyle);

            if (maximized )
            {
                var i = Window.SystemHandle;
                ShowWindow(i, SW_MAXIMIZE);
            }

            HandleEvents();

            if (StartScene != null)
            {
                scene = (SceneBase)Activator.CreateInstance(StartScene);
                scene.FadeOff();
                scene.LoadContent();
            }

            Running = true;
            form?.Show();
            GameLoop();
        }

        /// <summary>
        /// Loop do jogo
        /// </summary>
        static void GameLoop()
        {
            long timer_delay = 0;
            long timer_animation = 0;
            var clock = new Clock();
            var pressed = new MouseButtonEvent();
            bool ispressed = false;
            long timer_fps = 0;
            int count_fps = 0;
            

            while (Running)
            {
                if (Environment.TickCount64 > timer_delay)
                {
                    // Delta Time
                    DeltaTime = clock.Restart().AsSeconds();

                    if (Environment.TickCount64 > timer_animation)
                    {
                        Control.TextBox.s_animation = !Control.TextBox.s_animation;
                        timer_animation = Environment.TickCount64 + 250;
                    }

                    // Atualizações
                    scene?.Update();
                    OnUpdate.Invoke();
                    Sound.ProcessSounds();


                    // Dispara os eventos da janela
                    System.Windows.Forms.Application.DoEvents();
                    Window.DispatchEvents();

                    if (Window.HasFocus())
                    {
                        #region Mouse Events
                        var mousepos = Mouse.GetPosition(Window);
                        MousePosition = (Vector2)mousepos;
                        scene?.MouseMoved(MousePosition);

                        if (Mouse.IsButtonPressed(Mouse.Button.Left))
                        {
                            pressed.Button = Mouse.Button.Left;
                            pressed.X = mousepos.X;
                            pressed.Y = mousepos.Y;
                            ispressed = true;

                            // Pressed Event
                            scene?.MousePressed(pressed);
                        }
                        else if (pressed.Button == Mouse.Button.Left && ispressed)
                        {
                            TextBox.Focus = null;
                            scene?.MouseReleased(pressed);
                            ispressed = false;
                        }

                        if (Mouse.IsButtonPressed(Mouse.Button.Right))
                        {
                            pressed.Button = Mouse.Button.Right;
                            pressed.X = mousepos.X;
                            pressed.Y = mousepos.Y;
                            ispressed = true;

                            // Pressed Event
                            scene?.MousePressed(pressed);
                        }
                        else if (pressed.Button == Mouse.Button.Right && ispressed)
                        {
                            TextBox.Focus = null;
                            scene?.MouseReleased(pressed);
                            ispressed = false;
                        }
                        #endregion
                    }
                    if (form.Cursor != currentCursor)
                        form.Cursor = currentCursor;
                    Window.Clear(BackgroundColor);
                    

                    // Desenha a cena
                    scene?.Draw(Window, RenderStates.Default);

                    if (FPS_visible)
                        Renderer.DrawText(Window, $"FPS: {FPS}  Delta: {DeltaTime}", 12, FPS_position, Color.White, 1, new Color(0, 0, 0, 100));


                    Window.Display();
             

                    count_fps++;
                    if (Environment.TickCount64 > timer_fps)
                    {
                        FPS = count_fps;
                        count_fps = 0;
                        timer_fps = Environment.TickCount64 + 1000;
                    }

                    timer_delay = Environment.TickCount64 + 1;
                }


            }
            scene?.UnloadContent();
        }

        /// <summary>
        /// Cria a janela
        /// </summary>
        static void CreateWindow(System.Windows.Forms.FormBorderStyle borderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle)
        {
            form = new System.Windows.Forms.Form();
            form.ClientSize = new System.Drawing.Size((int)Size.x, (int)Size.y);
            form.MaximizeBox = Button_Maximize;
            form.Text = Title;
            form.MinimumSize = new System.Drawing.Size((int)MinSize.x, (int)MinSize.y);
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            if (!Fullscreen)
            {
                form.FormBorderStyle = borderStyle;
                Window = new RenderWindow(form.Handle, new ContextSettings(32, 8, 8));                
            }
            else
            {
                var video = VideoMode.DesktopMode;
                form.ClientSize = new System.Drawing.Size((int)video.Width, (int)video.Height);
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Window = new RenderWindow(form.Handle, new ContextSettings(32, 8, 8));                
            }
            DefaultView = Window.DefaultView;
            Window.SetActive(false);
            Window.SetFramerateLimit(0);
            Window.SetVerticalSyncEnabled(VSync);
        }

        /// <summary>
        /// Evento da janela
        /// </summary>
        static void HandleEvents()
        {
            Window.Closed += Window_Closed;
            Window.MouseWheelScrolled += Window_MouseWheelScrolled;
            Window.TextEntered += Window_TextEntered;
            Window.KeyPressed += Window_KeyPressed;
            Window.KeyReleased += Window_KeyReleased;
            Window.Resized += Window_Resized;

            form.FormClosing += Form_FormClosing;
        }

        private static void Form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            OnClosed?.Invoke();
            form.Hide();
            Running = false;
        }

        private static void Window_Resized(object sender, SizeEventArgs e)
        {
            Size = (Vector2)Window.Size;            
            DefaultView = new View(new FloatRect(0, 0, Size.x, Size.y));
            Window.SetView(DefaultView);
            OnResize?.Invoke();

            if (scene != null)
                scene.Resize();
        }

        private static void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            if (scene != null)
                scene.KeyReleased(e);
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (scene != null)
                scene.KeyPressed(e);
        }

        /// <summary>
        /// Carrega a font
        /// </summary>
        /// <param name="filename"></param>
        public static void LoadFont(string filename)
        {
            var f = new Font(filename);
            Renderer.gameFont = f;
            Renderer._text = new Text("", Renderer.gameFont);
        }

        public static void LoadShadow(string filename)
        {
            var t = new Texture(filename);
            Renderer.Shadow = t;
        }

        private static void Window_TextEntered(object sender, TextEventArgs e)
        {
            if (scene != null) scene.TextEntered(e);
        }

        private static void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (scene != null) scene.MouseScrolled(e);
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
            Running = false;
        }

        public static void SetScene<T>() where T : SceneBase
        {
            if (scene != null)
            {
                nextscene = (SceneBase)Activator.CreateInstance(typeof(T));
                scene?.FadeOn();
                //  scene?.UnloadContent();
            }
            else
            {
                scene = (SceneBase)Activator.CreateInstance(typeof(T));
                scene.FadeOff();
                scene.LoadContent();
            }
        }

        public static void SetScene<T>(params object[] args) where T : SceneBase
        {
            if (scene != null)
            {
                nextscene = (SceneBase)Activator.CreateInstance(typeof(T), args);
                scene?.FadeOn();
                // scene?.UnloadContent();
            }
            else
            {
                scene = (SceneBase)Activator.CreateInstance(typeof(T), args);
                scene.FadeOff();
                scene.LoadContent();
            }
        }

        public static void SetStartScene<T>() where T : SceneBase
        {
            StartScene = typeof(T);
        }

        public static T GetScene<T>() where T : SceneBase
            => (T)scene;

        public static SceneBase GetScene()
            => scene;

        internal static void NextScene()
        {
            if (nextscene != null && nextscene != scene)
            {
                scene?.UnloadContent();
                scene = nextscene;
                scene.FadeOff();
                scene.LoadContent();
                GC.Collect();
            }
        }

        public static void SetSize(Vector2 value)
            => Size = value;

        public static bool HasFocus()
            => Window.HasFocus();

        public static RenderWindow GetWindow()
            => Window;

        public static void ToggleFullscreen()
        {
            //if (!isFullscreen)
            //{
            //    Window.Close();
            //    Window = new RenderWindow(VideoMode.DesktopMode, Title, Styles.Fullscreen);
            //    Window.SetVerticalSyncEnabled(VSync);
            //    HandleEvents();
            //    isFullscreen = true;
            //    _size = Size;
            //    Size = (Vector2)Window.Size;
            //}
            //else
            //{
            //    Size = _size;
            //    Window.Close();
            //    CreateWindow();
            //    HandleEvents();
            //    isFullscreen = false;
            //}

            if (scene != null)
                scene.Resize();
        }

        public static System.Windows.Forms.Form GetForm()
            => form;

        public static void SetCursor(System.Windows.Forms.Cursor value)
        {
            if (currentCursor != value)
                currentCursor = value;
        }
        static System.Windows.Forms.Cursor currentCursor;
    }
}
