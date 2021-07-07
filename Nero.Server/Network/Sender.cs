using LiteNetLib;
using LiteNetLib.Utils;
using Nero.Server.Map;
using Nero.Server.Player;
using Nero.Server.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nero.Server.Network
{
    static class Sender
    {
        enum Packets
        {
            Alert, ChangeToSelectCharacter, UpdateClass,
            UpdateCharacters, ChangeToGameplay, UpdateMyCharacter,
            UpdateCharacterPosition, CheckMapRevision, MapData,
            CharacterData, RemoveCharacter, MoveCharacter,
            ChatText, ChatTextSystem, UpdateNpc, RequestSpawnFactory,
            SpawnData, PrepareSpawn, SpawnMove,
        }

        /// <summary>
        /// Movimento do spawn
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="spawn"></param>
        public static void SpawnMove(absInstance instance, SpawnItem spawn)
        {
            var buffer = Create(Packets.SpawnMove);
            buffer.Put(spawn.IndexOf);
            buffer.Put((byte)spawn.Direction);
            buffer.Put(spawn.Position);
            SendToInstance(instance, buffer);
        }

        /// <summary>
        /// Prepara todos os spawns do mapa
        /// </summary>
        /// <param name="peer"></param>
        public static void PrepareSpawn(NetPeer peer)
        {
            var buffer = Create(Packets.PrepareSpawn);
            var player = Character.Find(peer);
            buffer.Put(player.GetInstance().Spawn.Items.Length);
            SendTo(peer, buffer);
        }
        
        /// <summary>
        /// Envia dados de todos os spawns
        /// </summary>
        /// <param name="peer"></param>
        public static void SpawnDataAll(NetPeer peer)
        {
            var player = Character.Find(peer);

            foreach (var i in player.GetInstance().Spawn.Items)
                SpawnData(player, i);
        }

        /// <summary>
        /// Envia dados do spawn
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="spawnItem"></param>
        public static void SpawnData(Character player, SpawnItem spawnItem)
        {
            SendTo(player, SpawnDataPacket(player.GetInstance(), spawnItem));
        }

        /// <summary>
        /// Envia dados do spawn
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="spawnItem"></param>
        public static void SpawnData(absInstance instance, SpawnItem spawnItem)
        {
            SendToInstance(instance, SpawnDataPacket(instance, spawnItem));
        }

        /// <summary>
        /// Pacote de dados do spawn
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="spawnItem"></param>
        /// <returns></returns>
        static NetDataWriter SpawnDataPacket(absInstance instance, SpawnItem spawnItem)
        {
            var buffer = Create(Packets.SpawnData);
            buffer.Put(Array.IndexOf(instance.Spawn.Items, spawnItem));
            buffer.Put(spawnItem.NpcID);
            buffer.Put(spawnItem.HP);
            buffer.Put((byte)spawnItem.Direction);
            buffer.Put(spawnItem.Position);
            return buffer;
        }

        /// <summary>
        /// Requesita a produção de spawn
        /// </summary>
        public static void RequestSpawnFactory(NetPeer peer)
        {
            var player = Character.Find(peer);
            var buffer = Create(Packets.RequestSpawnFactory);
            var fac = SpawnFactory.Factories[player.MapID];

            buffer.Put(fac.Items.Count);
            foreach(var i in fac.Items)
            {
                buffer.Put(i.NpcID);
                buffer.Put(i.BlockMove);
                buffer.Put((byte)i.Direction);
                buffer.Put(i.UsePositionSpawn);
                buffer.Put(i.Position);
            }
            SendTo(peer, buffer);
        }

        /// <summary>
        /// Atualiza todos os npcs
        /// </summary>
        /// <param name="peer"></param>
        public static void UpdateNpcAll(NetPeer peer)
        {
            for (int i = 0; i < Constants.MAX_NPCS; i++)
                UpdateNpc(peer, i);
        }

        /// <summary>
        /// Atualiza o npc para todos
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateNpc(int id)
        {
            var buffer = Create(Packets.UpdateNpc);
            buffer.Put(id);
            buffer.Put(JsonConvert.SerializeObject(Npc.Items[id]));
            SendToAll(buffer);
        }

        /// <summary>
        /// Atualiza o npc
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateNpc(NetPeer peer, int id)
        {
            var buffer = Create(Packets.UpdateNpc);
            buffer.Put(id);
            buffer.Put(JsonConvert.SerializeObject(Npc.Items[id]));
            SendTo(peer, buffer);
        }

        /// <summary>
        /// Envia a mensagem ao jogador
        /// </summary>
        /// <param name="player"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void ChatTextSystem(Character player, string text, Color color)
        {
            var buffer = Create(Packets.ChatTextSystem);
            buffer.Put(text);
            buffer.Put(color.ToInteger());
            SendTo(player, buffer);
        }

        /// <summary>
        /// Envia mensagem para todos
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void ChatTextToAll(string text, Color color)
        {
            var buffer = Create(Packets.ChatText);
            buffer.Put(text);
            buffer.Put(color.ToInteger());
            SendToAll(buffer);
        }

        /// <summary>
        /// Envia a mensagem aos jogadores na instancia
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void ChatTextToInstance(absInstance instance, string text, Color color)
        {
            var buffer = Create(Packets.ChatText);
            buffer.Put(text);
            buffer.Put(color.ToInteger());
            SendToInstance(instance, buffer);
        }

        /// <summary>
        /// Envia a mensagem aos jogadores na instancia
        /// </summary>
        /// <param name="player"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void ChatTextToInstance(Character player, string text, Color color)
            => ChatTextToInstance(player.GetInstance(), text, color);

        /// <summary>
        /// Envia a mensagem ao jogador
        /// </summary>
        /// <param name="player"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void ChatText(Character player, string text, Color color)
        {
            var buffer = Create(Packets.ChatText);
            buffer.Put(text);
            buffer.Put(color.ToInteger());
            SendTo(player, buffer);
        }

        /// <summary>
        /// Movimenta o jogador
        /// </summary>
        /// <param name="player"></param>
        public static void MoveCharacter(Character player)
        {
            var buffer = Create(Packets.MoveCharacter);
            buffer.Put(player.Name);
            buffer.Put((byte)player.Direction);
            buffer.Put(player.Position);
            SendToInstanceBut(player, buffer);
        }

        /// <summary>
        /// Remove o personagem da instância
        /// </summary>
        /// <param name="player"></param>
        public static void RemoveCharacter(Character player)
        {
            var buffer = Create(Packets.RemoveCharacter);
            buffer.Put(player.Name);
            SendToInstanceBut(player, buffer);
        }

        /// <summary>
        /// Envia dados de todos os personagens para mim
        /// </summary>
        /// <param name="peer"></param>
        public static void CharacterDataAllForMe(NetPeer peer)
        {
            var player = Character.Find(peer);

            foreach(var i in Character.Items)
                if (i != player && i.GetInstance().Equals(player.GetInstance()))
                {
                    var buffer = Create(Packets.CharacterData);
                    UpdateCharacterPacket(i, buffer);
                    SendTo(peer, buffer);
                }
        }

        /// <summary>
        /// Envia meu dados para todos os personagens na instancia
        /// </summary>
        /// <param name="player"></param>
        public static void CharacterDataToInstance(Character player)
        {
            var buffer = Create(Packets.CharacterData);            
            UpdateCharacterPacket(player, buffer);
            SendToInstanceBut(player, buffer);
        }

        /// <summary>
        /// Envia o mapa para todos presentes no mesmo
        /// </summary>
        /// <param name="peer"></param>
        public static void MapDataBut(NetPeer peer)
        {
            var player = Character.Find(peer);
            var buffer = Create(Packets.MapData);
            buffer.Put(player.MapID);
            buffer.Put(JsonConvert.SerializeObject(MapInstance.Items[player.MapID]));
            SendToInstanceBut(player, buffer);
        }

        /// <summary>
        /// Envia o mapa
        /// </summary>
        /// <param name="peer"></param>
        public static void MapData(NetPeer peer)
        {
            var player = Character.Find(peer);
            var buffer = Create(Packets.MapData);
            buffer.Put(player.MapID);
            buffer.Put(JsonConvert.SerializeObject(MapInstance.Items[player.MapID]));
            SendTo(player, buffer);
        }

        /// <summary>
        /// Verifica a revisão do mapa
        /// </summary>
        /// <param name="player"></param>
        public static void CheckMapRevision(Character player)
        {
            var buffer = Create(Packets.CheckMapRevision);
            buffer.Put(player.MapID);
            buffer.Put(MapInstance.Items[player.MapID].Revision);
            SendTo(player, buffer);
        }

        /// <summary>
        /// Atualiza a posição do personagem
        /// </summary>
        /// <param name="c"></param>
        public static void UpdateCharacterPosition(Character player)
        {
            var buffer = Create(Packets.UpdateCharacterPosition);
            buffer.Put(player.Name);
            buffer.Put(player.MapID);
            buffer.Put(player.Position);
            buffer.Put((byte)player.Direction);
            SendToInstance(player, buffer);
        }

        /// <summary>
        /// Dados de personagem
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="buffer"></param>
        static void UpdateCharacterPacket(Character c, NetDataWriter buffer)
        {
            buffer.Put(c.Name);
            buffer.Put(c.ClassID);            
            buffer.Put(c.SpriteID);
            buffer.Put(c.Level);
            buffer.Put(c.Position);
            buffer.Put((byte)c.AccessLevel);
        }

        /// <summary>
        /// Atualiza meu personagem
        /// </summary>
        /// <param name="peer"></param>
        public static void UpdateMyCharacter(NetPeer peer)
        {
            var buffer = Create(Packets.UpdateMyCharacter);
            var c = Character.Find(peer);

            UpdateCharacterPacket(c, buffer);

            SendTo(peer, buffer);
        }

        /// <summary>
        /// Muda a cena para gameplay
        /// </summary>
        /// <param name="peer"></param>
        public static void ChangeToGameplay(NetPeer peer)
        {
            SendTo(peer, Create(Packets.ChangeToGameplay));
        }

        /// <summary>
        /// Envia os personagens da conta
        /// </summary>
        /// <param name="peer"></param>
        public static void UpdateCharacters(NetPeer peer)
        {
            var acc = Account.Find(peer);
            var buffer = Create(Packets.UpdateCharacters);

            foreach(var i in acc.Characters)
            {
                buffer.Put(i);
                if (i.Length > 0)
                {
                    var c = Character.Load(i);
                    buffer.Put(c.SpriteID);                    
                }
            }

            SendTo(peer, buffer);
        }

        /// <summary>
        /// Atualiza as classes
        /// </summary>
        /// <param name="peer"></param>
        public static void UpdateClass(NetPeer peer)
        {
            var buffer = Create(Packets.UpdateClass);
            buffer.Put(CharacterClass.Items.Count);
            foreach(var i in CharacterClass.Items)
            {
                buffer.PutArray(i.Name);
                buffer.PutArray(i.Description);
                buffer.PutArray(i.StatPrimary);
                buffer.PutArray(i.MaleSprite);
                buffer.PutArray(i.FemaleSprite);
            }
            SendTo(peer, buffer);
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
        public static void Alert(NetPeer peer, params string[] text)
        {
            var buffer = Create(Packets.Alert);
            buffer.PutArray(text);
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

        /// <summary>
        /// Envia um pacote
        /// </summary>
        /// <param name="player"></param>
        /// <param name="buffer"></param>
        static void SendTo(Character player, NetDataWriter buffer)
            => SendTo(player.peer, buffer);        

        /// <summary>
        /// Envia o pacote para a Instancia
        /// </summary>
        static void SendToInstance(Character player, NetDataWriter buffer)
        {
            var lst = Character.Items.Where(i => i.GetInstance() == player.GetInstance()).ToList();

            foreach (var i in lst)
                SendTo(i.peer, buffer);
        }

        /// <summary>
        /// Envia o pacote para a Instancia
        /// </summary>
        static void SendToInstance(absInstance instance, NetDataWriter buffer)
        {
            var lst = Character.Items.Where(i => i.GetInstance() == instance).ToList();

            foreach (var i in lst)
                SendTo(i.peer, buffer);
        }

        /// <summary>
        /// Envia o pacote para a Instancia, mas sem para si
        /// </summary>
        static void SendToInstanceBut(Character controller, NetDataWriter buffer)
        {
            var lst = Character.Items.Where(i => i != controller && i.GetInstance() == controller.GetInstance()).ToList();

            foreach (var i in lst)
                SendTo(i.peer, buffer);
        }

        /// <summary>
        /// Envia o pacote para todos
        /// </summary>
        /// <param name="buffer"></param>
        static void SendToAll(NetDataWriter buffer)
        {
            Socket.Device.SendToAll(buffer, DeliveryMethod.ReliableOrdered);
        }
    }
}
