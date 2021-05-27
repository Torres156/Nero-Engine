using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server.Network
{
    static class Sender
    {
        enum Packets
        {
            Alert, ChangeToSelectCharacter,
        }

        /// <summary>
        /// Muda a cena para seleção de personagens
        /// </summary>
        /// <param name="peer"></param>
        public static void ChangeToSelectCharacter(NetPeer peer)
        {
            SendTo(peer, Create(Packets.ChangeToSelectCharacter));
        }

        /// <summary>
        /// Envia um alerta
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="text"></param>
        public static void Alert(NetPeer peer, string text)
        {
            var buffer = Create(Packets.Alert);
            buffer.Put(text);
            SendTo(peer, buffer);
        }

        /// <summary>
        /// Cria um pacote
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        static NetDataWriter Create(Packets packet)
        {
            var buffer = new NetDataWriter();
            buffer.Put((short)packet);
            return buffer;
        }

        /// <summary>
        /// Envia o pacote
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void SendTo(NetPeer peer, NetDataWriter buffer)
        {
            if (peer == null || peer.ConnectionState != ConnectionState.Connected)
                return;

            peer.Send(buffer, DeliveryMethod.ReliableOrdered);
        }

    }
}
