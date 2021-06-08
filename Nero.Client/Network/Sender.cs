using LiteNetLib.Utils;
using Nero.Client.Map;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Network
{
    static class Sender
    {
        enum Packets
        {
            Register, Login, CreateCharacter, UseCharacter, MapAnswer, MapSave,
        }

        /// <summary>
        /// Salva o mapa
        /// </summary>
        public static void MapSave()
        {
            var buffer = Create(Packets.MapSave);
            buffer.Put(JsonConvert.SerializeObject(MapInstance.Current));
            SendTo(buffer);
        }

        /// <summary>
        /// Resposta da revisão de mapa
        /// </summary>
        /// <param name="answer"></param>
        public static void MapAnswer(bool answer)
        {
            var buffer = Create(Packets.MapAnswer);
            buffer.Put(answer);
            SendTo(buffer);
        }

        /// <summary>
        /// Usa um personagem
        /// </summary>
        /// <param name="slot"></param>
        public static void UseCharacter(int slot)
        {
            var buffer = Create(Packets.UseCharacter);
            buffer.Put(slot);
            SendTo(buffer);
        }

        /// <summary>
        /// Cria um novo personagem
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="name"></param>
        /// <param name="classID"></param>
        /// <param name="sprite"></param>
        public static void CreateCharacter(int slot, string name, int classID, int sprite)
        {
            var buffer = Create(Packets.CreateCharacter);
            buffer.Put(slot);
            buffer.Put(name);
            buffer.Put(classID);
            buffer.Put(sprite);

            SendTo(buffer);
        }

        /// <summary>
        /// Entra na conta
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void Login(string account, string password)
        {
            var buffer = Create(Packets.Login);
            buffer.Put(account);
            buffer.Put(password);
            SendTo(buffer);
        }

        /// <summary>
        /// Registrar nova conta
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void Register(string account, string password)
        {
            var buffer = Create(Packets.Register);
            buffer.Put(account);
            buffer.Put(password);
            SendTo(buffer);
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
        /// <param name="buffer"></param>
        static void SendTo(NetDataWriter buffer)
        {
            if (!Socket.IsConnected)
                return;

            Socket.Device.FirstPeer.Send(buffer, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }
    }
}
