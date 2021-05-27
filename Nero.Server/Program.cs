using Nero.Server.Core;
using Nero.Server.Player;
using System;

namespace Nero.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Servidor Nero Engine";
            
            // Inicializa o dispositivo de conex�o
            Network.Socket.Initialize();

            // Verifica os diret�rios
            Account.CheckDirectory();

            System.Threading.Tasks.Task.Run(new Action(ConsoleCore.ConsoleLoop));
            ServerCore.ServerLoop();
            
        }
    }
}
