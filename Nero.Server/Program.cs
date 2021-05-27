using Nero.Server.Helpers;
using System;

namespace Nero.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Servidor Nero Engine";
            Network.Socket.Initialize();

            System.Threading.Tasks.Task.Run(new Action(ConsoleHelper.ConsoleLoop));
            ServerHelper.ServerLoop();
            
        }
    }
}
