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
            
            // Inicializa o dispositivo de conexão
            Network.Socket.Initialize();

            // Verifica os diretórios
            Account.CheckDirectory();
            Character.CheckDirectory();
            CharacterClass.CheckDirectory();

            Console.WriteLine("");

            // Inicializa
            CharacterClass.Initialize();
            
            Console.WriteLine("");
            Console.WriteLine("Servidor iniciado!");
            Console.WriteLine("");
            System.Threading.Tasks.Task.Run(new Action(ConsoleCore.ConsoleLoop));
            ServerCore.ServerLoop();
            
        }
    }
}
