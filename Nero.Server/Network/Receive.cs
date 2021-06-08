using LiteNetLib;
using LiteNetLib.Utils;
using Nero.Server.Helpers;
using Nero.Server.Map;
using Nero.Server.Player;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nero.Server.Network
{
    static class Receive
    {
        enum Packets
        {
            Register, Login, CreateCharacter, UseCharacter, MapAnswer, MapSave,
            MoveCharacter,
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
                case Packets.Login: Login(peer, buffer); break;
                case Packets.CreateCharacter: CreateCharacter(peer, buffer); break;
                case Packets.UseCharacter: UseCharacter(peer, buffer); break;
                case Packets.MapAnswer: MapAnswer(peer, buffer); break;
                case Packets.MapSave: MapSave(peer, buffer); break;
                case Packets.MoveCharacter: MoveCharacter(peer, buffer); break;
            }
        }

        /// <summary>
        /// Movimento do personagem
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void MoveCharacter(NetPeer peer, NetDataReader buffer)
        {
            var direction = (Directions)buffer.GetByte();
            var pos = buffer.GetVector2();
            var player = Character.Find(peer);

            if (!player.Position.Equals(pos))
            {
                Sender.UpdateCharacterPosition(player);
                return;
            }

            MoveHelper.Move(player, direction);
        }

        /// <summary>
        /// Salva o mapa
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void MapSave(NetPeer peer, NetDataReader buffer)
        {
            var json = buffer.GetString();

            var player = Character.Find(peer);
            MapInstance.Items[player.MapID] = JsonConvert.DeserializeObject<MapInstance>(json);
            MapInstance.Save(player.MapID);
            Sender.MapDataBut(peer);
        }

        /// <summary>
        /// Resposta da revisão de mapa
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void MapAnswer(NetPeer peer, NetDataReader buffer)
        {
            var result = buffer.GetBool();

            if (result)            
                Sender.MapData(peer);

            // Envia dados
            Sender.CharacterDataToInstance(Character.Find(peer));
            Sender.CharacterDataAllForMe(peer);
        }

        /// <summary>
        /// Usa um personagem
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void UseCharacter(NetPeer peer, NetDataReader buffer)
        {
            var slot = buffer.GetInt();
            var acc = Account.Find(peer);

            // Carrega o personagem
            var controller = Character.Load(acc.Characters[slot]);
            controller.peer = peer;
            controller.account = acc;
            Character.Items.Add(controller);

            PlayerHelper.Join(controller);
        }

        /// <summary>
        /// Cria um novo personagem
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void CreateCharacter(NetPeer peer, NetDataReader buffer)
        {
            var slot = buffer.GetInt();
            var name = buffer.GetString();
            var classID = buffer.GetInt();
            var spriteID = buffer.GetInt();

            // Verifica se já está em uso
            if (Character.Exist(name))
            {
                Sender.Alert(peer, $"O nome {name} não está disponivel!", $"The name {name} is not available!");
                return;
            }

            // Cria o personagem
            var c = new Character();
            c.Name = name;
            c.ClassID = classID;
            c.MapID = CharacterClass.Items[classID].MapID;
            c.Position = CharacterClass.Items[classID].StartPosition;
            c.StatPrimary = CharacterClass.Items[classID].StatPrimary;
            c.SpriteID = spriteID;
            Character.Save(c);

            var acc = Account.Find(peer);
            acc.Characters[slot] = name;
            Account.Save(acc);

            Sender.Alert(peer, "Personagem criado com sucesso!", "Character created successfully!");
            Sender.UpdateCharacters(peer);
            Sender.ChangeToSelectCharacter(peer);
        }

        /// <summary>
        /// Entra em uma conta
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void Login(NetPeer peer, NetDataReader buffer)
        {
            var accName = buffer.GetString();
            var password = buffer.GetString();

            // Verifica se existe a conta
            if (!Account.Exist(accName))
            {
                Sender.Alert(peer, $"A conta {accName} não foi encontrada!");
                return;
            }

            // Carrega a conta
            var acc = Account.Load(accName);

            // Verifica as senhas
            if (password != acc.Password)
            {
                Sender.Alert(peer, "Senha incorreta!", "Incorrect password!");
                return;
            }

            // Verifica se está em uso            
            if (Account.Items.Any(i => i.Name.ToLower().Equals(accName.ToLower())))
            {
                var fAcc = Account.Items.Find(i => i.Name.ToLower().Equals(accName.ToLower()));
                Sender.Alert(peer, "A conta já está em uso, reporte caso não seja você!", "The account is already in use, report if it's not you!");
                fAcc.peer?.Disconnect();
                return;
            }

            acc.peer = peer;            
            Account.Items.Add(acc);

            // Verifica se os personagens ainda existem
            bool isUpdate = false;
            for(int i = 0; i < Constants.MAX_CHARACTERS; i++)
                if (acc.Characters[i].Length > 0 && !Character.Exist(acc.Characters[i]))
                {
                    acc.Characters[i] = "";
                    isUpdate = true;
                }

            // Salva a conta atualizada!
            if (isUpdate)
                Account.Save(acc);

            // Troca a cena            
            Sender.UpdateClass(peer);
            Sender.UpdateCharacters(peer);
            Sender.ChangeToSelectCharacter(peer);
        }

        /// <summary>
        /// Registra uma nova conta
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="buffer"></param>
        static void Register(NetPeer peer, NetDataReader buffer)
        {
            var accName = buffer.GetString();
            var accPwd = buffer.GetString();

            // Verifica se já existe a conta
            if (Account.Exist(accName))
            {
                Sender.Alert(peer, $"O nome {accName} não está disponivel!", $"The name {accName} is not available!");
                return;
            }

            // Cria a conta
            var acc = new Account();
            acc.Name = accName;
            acc.Password = accPwd;
            Account.Save(acc);

            Sender.Alert(peer, $"A conta {accName} foi criada com sucesso!", $"The account {accName} has been successfully created!");
        }
    }
}
