using Nero.Server.World;
using Nero.Server.Map;
using Nero.SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Nero.Server.Player;

namespace Nero.Server.Core
{
    static class ServerCore
    {
        public static bool Running = false;
        public static float DeltaTime;

        public static void ServerLoop()
        {
            Running = true;

            long timerDelay = 0;
            Clock clock = new Clock();
            while (Running)
            {
                if (Environment.TickCount64 > timerDelay)
                {
                    DeltaTime = clock.Restart().AsSeconds();
                    Network.Socket.PollEvents();

                    for (int i = 0; i < Constants.MAX_MAPS; i++)
                        MapInstance.Items[i].Update();

                    foreach (var i in Character.Items)
                        i.Update();

                    timerDelay = Environment.TickCount64 + 1;
                }
                Thread.Sleep(1);
            }
        }
    }
}
