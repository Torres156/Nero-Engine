using LiteNetLib.Utils;
using Nero.Client.Map;
using Nero.Client.Player;
using Nero.Client.World;
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
            MoveCharacter, ChatSpeak, OnGame, SaveNpc, RequestSpawnFactory, UpdateSpawnFactory,
            RequestAttack, ChangeDirection,
        }

        /// <summary>
        /// Altera a direção
        /// </summary>
        public static void ChangeDirection()
        {
            var buffer = Create(Packets.ChangeDirection);
            buffer.Put((byte)Character.My.Direction);
            SendTo(buffer);
        }

        /// <summary>
        /// Requer um ataque
        /// </summary>
        public static void RequestAttack()
        {
            SendTo(Create(Packets.RequestAttack));
        }

        /// <summary>
        /// Atualiza a produção de spawn
        /// </summary>
        public static void UpdateSpawnFactory()
        {
            var buffer = Create(Packets.UpdateSpawnFactory);            
            
            buffer.Put(SpawnFactory.Items.Count);
            foreach (var i in SpawnFactory.Items)
            {
                buffer.Put(i.NpcID);
                buffer.Put(i.BlockMove);
                buffer.Put((byte)i.Direction);
                buffer.Put(i.UsePositionSpawn);
                buffer.Put(i.Position);
            }
            SendTo(buffer);
        }

        /// <summary>
        /// Requesita a produção de spawn
        /// </summary>
        public static void RequestSpawnFactory()
            => SendTo(Create(Packets.RequestSpawnFactory));

        /// <summary>
        /// Salva o npc
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="newNpc"></param>
        public static void SaveNpc(int ID)
        {
            var buffer = Create(Packets.SaveNpc);
            buffer.Put(ID);
            buffer.Put(JsonConvert.SerializeObject(Npc.Items[ID]));
            SendTo(buffer);
        }

        /// <summary>
        /// Estou no game
        /// </summary>
        public static void OnGame()
        {
            SendTo(Create(Packets.OnGame));
        }

        /// <summary>
        /// Fala no chat
        /// </summary>
        /// <param name="text"></param>
        public static void ChatSpeak(string text)
        {
            var buffer = Create(Packets.ChatSpeak);
            buffer.Put(text);
            SendTo(buffer);
        }

        /// <summary>
        /// Movimenta o personagem
        /// </summary>
        /// <param name="direction"></param>
        public static void MoveCharacter(Directions direction)
        {
            var buffer = Create(Packets.MoveCharacter);
            buffer.Put((byte)direction);
            buffer.Put(Character.My.Position);            
            SendTo(buffer);
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
