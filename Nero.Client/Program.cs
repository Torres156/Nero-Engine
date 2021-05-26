using Nero;
using System;

namespace Nero.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.Title = "Nero Engine";
            Game.Size = new Vector2(800, 600);
            Game.MinSize = new Vector2(600, 320);
            Game.WindowMaximized = true;
            Game.LoadFont("res/consola.ttf");
            Game.CurrentLanguage = Languages.EN_USA;
            Game.SetStartScene<Scenes.MenuScene>();
            Game.Run();
        }
    }
}
