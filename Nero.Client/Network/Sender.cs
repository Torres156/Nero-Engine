using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Network
{
    static class Sender
    {
        enum Packets
        {
            Register,
        }

        /// <summary>
        /// Registrar nova conta
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void Register(string account, string password)
        {
            var buffer = CreateMessage(Packets.Register);
            buffer.Put(account);
            buffer.Put(password);
            SendTo(buffer);
        }

        /// <summary>
        /// Cria um pacote de mensagem
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        static NetDataWriter CreateMessage(Packets packet)
        {
            var buffer = new NetDataWriter();
            buffer.Put((short)packet);
            return buffer;
        }

        /// <summary>
        /// Envia o pacote
        /// </summary>
        /// <param name="buffer"></param>
        static void SendTo(NetDataWriter buffer)
        {
            if (!Socket.IsConnected)
                return;

            Socket.Device.FirstPeer.Send(buffer, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }
    }
}
