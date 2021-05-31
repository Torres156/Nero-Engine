using Nero;
using System;

namespace Nero.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowSettings.Load();

            Game.BackgroundColor = Color.Black;
            Game.Title = "Nero Engine";
            Game.Size = WindowSettings.Instance.WindowSize;
            Game.MinSize = new Vector2(600, 320);
            Game.WindowMaximized = WindowSettings.Instance.WindowMaximize;
            Game.VSync = WindowSettings.Instance.VSync;
            Game.LoadFont("res/consola.ttf");
            Game.CurrentLanguage = WindowSettings.Instance.Language;            
            Game.OnUpdate += Update;
            Game.OnResize += Resize;
            Game.SetStartScene<Scenes.MenuScene>();

            Sound.Volume_Music = (byte)WindowSettings.Instance.Volume_Music;
            Sound.Volume_Sound = (byte)WindowSettings.Instance.Volume_Sound;

            Network.Socket.Initialize();

            Game.Run();

            Network.Socket.Device.Stop();
        }

        static void Update()
        {
            Network.Socket.PollEvents();
        }

        static void Resize()
        {
            var i = WindowState.GetPlacement(Game.Window.SystemHandle);
            if (i.showCmd == WindowState.ShowWindowCommands.Maximized)
                WindowSettings.Instance.WindowMaximize = true;
            else
            {
                WindowSettings.Instance.WindowSize = Game.Size;
                WindowSettings.Instance.WindowMaximize = false;
            }

            WindowSettings.Save();
        }
    }
}
