using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nero.Server.Core
{
    static class ServerCore
    {
        public static bool Running = false;

        public static void ServerLoop()
        {
            Running = true;

            long timerDelay = 0;
            while(Running)
            {
                if (Environment.TickCount64 > timerDelay)
                {
                    Network.Socket.PollEvents();

                    
                    timerDelay = Environment.TickCount64 + 1;
                }
                Thread.Sleep(1);
            }
        }
    }
}
