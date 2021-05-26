using Nero;
using System;

namespace NIC.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.Title = "NIC Engine";
            Game.Size = new Vector2(800, 600);
            Game.MinSize = new Vector2(600, 320);
            Game.WindowMaximized = true;
            Game.LoadFont("res/consola.ttf");
            Game.SetStartScene<Scenes.MenuScene>();
            Game.Run();
        }
    }
}
