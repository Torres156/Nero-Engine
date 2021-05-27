using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server.Network
{
    static class Receive
    {
        enum Packets
        {
            Register,
        }

        /// <summary>
        /// Recebe e direciona os pacotes
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        public static void Handle(NetPeer peer, NetDataReader buffer)
        {
            var packet = (Packets)buffer.GetShort();

            switch(packet)
            {
                case Packets.Register: Register(peer, buffer); break;

                    
            }
        }

        /// <summary>
        /// Registra uma nova conta
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void Register(NetPeer peer, NetDataReader buffer)
        {

        }
    }
}
