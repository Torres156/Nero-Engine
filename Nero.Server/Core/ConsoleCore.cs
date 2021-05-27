using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server.Core
{
    static class ConsoleCore
    {
        public static void ConsoleLoop()
        {
        startLoop:;
            Console.WriteLine("Digite um comando:");
            string cmd = Console.ReadLine();

            string[] commands = cmd.Split();
            if (commands.Length == 0) goto startLoop;

            switch (commands[0].Trim().ToLower())
            {
                case "exit":
                    ServerCore.Running = false;
                    break;
            }

            goto startLoop;
        }
    }
}
