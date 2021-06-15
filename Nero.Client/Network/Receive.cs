using LiteNetLib.Utils;
using Nero.Client.Helpers;
using Nero.Client.Map;
using Nero.Client.Player;
using Nero.Client.World;
using Nero.Client.World.Chat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Network
{
    static class Receive
    {
        enum Packets
        {
            Alert, ChangeToSelectCharacter, UpdateClass,
            UpdateCharacters, ChangeToGameplay, UpdateMyCharacter,
            UpdateCharacterPosition, CheckMapRevision, MapData,
            CharacterData, RemoveCharacter, MoveCharacter,
            ChatText, ChatTextSystem,
        }

        /// <summary>
        /// Recebe e direciona os pacotes
        /// </summary>
        /// <param name="buffer"></param>
        public static void Handle(NetDataReader buffer)
        {
            var packet = (Packets)buffer.GetShort();

            switch(packet)
            {
                case Packets.Alert: Alert(buffer); break;
                case Packets.ChangeToSelectCharacter: ChangeToSelectCharacter(buffer); break;
                case Packets.UpdateClass: UpdateClass(buffer); break;
                case Packets.UpdateCharacters: UpdateCharacters(buffer); break;
                case Packets.ChangeToGameplay: ChangeToGameplay(buffer); break;
                case Packets.UpdateMyCharacter: UpdateMyCharacter(buffer); break;
                case Packets.UpdateCharacterPosition: UpdateCharacterPosition(buffer); break;
                case Packets.CheckMapRevision: CheckMapRevision(buffer); break;
                case Packets.MapData: MapData(buffer); break;
                case Packets.CharacterData: CharacterData(buffer); break;
                case Packets.RemoveCharacter: RemoveCharacter(buffer); break;
                case Packets.MoveCharacter: MoveCharacter(buffer); break;
                case Packets.ChatText: ChatText(buffer); break;
                case Packets.ChatTextSystem: ChatTextSystem(buffer); break;
            }
        }


        /// <summary>
        /// Mensagem para o chat
        /// </summary>
        /// <param name="buffer"></param>
        static void ChatTextSystem(NetDataReader buffer)
        {
            ChatHelper.Add(ChatTypes.System, buffer.GetString(), new Color(buffer.GetUInt()));
        }

        /// <summary>
        /// Mensagem para o chat
        /// </summary>
        /// <param name="buffer"></param>
        static void ChatText(NetDataReader buffer)
        {
            ChatHelper.Add(ChatTypes.Normal, buffer.GetString(), new Color(buffer.GetUInt()));
        }

        /// <summary>
        /// Movimento do personagem
        /// </summary>
        /// <param name="buffer"></param>
        static void MoveCharacter(NetDataReader buffer)
        {
            var name = buffer.GetString();
            var dir = (Directions)buffer.GetByte();
            var pos = buffer.GetVector2();

            var find = Character.Find(name);
            if (find.Equals(null)) 
                return;

            find.Direction = dir;
            find.Position = pos;
            find.Moving = true;
            switch(dir)
            {
                case Directions.Up:
                    find.OffSet.y += 32;
                    break;
                case Directions.Down:
                    find.OffSet.y -= 32;
                    break;
                case Directions.Left:
                    find.OffSet.x += 32;
                    break;
                case Directions.Right:
                    find.OffSet.x -= 32;
                    break;
            }
        }

        /// <summary>
        /// Remove um personagem do mapa
        /// </summary>
        /// <param name="buffer"></param>
        static void RemoveCharacter(NetDataReader buffer)
        {
            var name = buffer.GetString();

            var find = Character.Find(name);
            if (find != null && find != Character.My)
                Character.Items.Remove(find);
        }

        /// <summary>
        /// Dados de personagem
        /// </summary>
        /// <param name="buffer"></param>
        static void CharacterData(NetDataReader buffer)
        {
            var name = buffer.GetString();

            var find = Character.Find(name);
            if (find == null)
            {
                find = new Character();
                Character.Items.Add(find);
            }

            find.Name = name;
            find.ClassID = buffer.GetInt();
            find.SpriteID = buffer.GetInt();
            find.Level = buffer.GetInt();
            find.Position = buffer.GetVector2();
            find.AccessLevel = (AccessLevels)buffer.GetByte();
        }

        /// <summary>
        /// Dados do mapa
        /// </summary>
        /// <param name="buffer"></param>
        static void MapData(NetDataReader buffer)
        {
            var mapID = buffer.GetInt();
            Character.My.MapID = mapID;

            var m = JsonConvert.DeserializeObject<MapInstance>(buffer.GetString());
            for (int i = 0; i < (int)Layers.count; i++)
            {
                m.Layer[i].SetMap(m, false);
                for (int x = 0; x <= m.Size.x; x++)
                    for (int y = 0; y <= m.Size.y; y++)
                        m.Layer[i].chunks[x, y]?.SetLayer(m.Layer[i]);
            }
            MapInstance.Current = m;
            MapInstance.Save();
        }

        /// <summary>
        /// Checa a revisão do mapa
        /// </summary>
        /// <param name="buffer"></param>
        static void CheckMapRevision(NetDataReader buffer)
        {
            var id = buffer.GetInt();
            var rev = buffer.GetInt();

            var m = MapInstance.Load(id);
            if (m.Revision != rev)
            {
                m = null;
                Sender.MapAnswer(true);
            }
            else
            {
                MapInstance.Current = m;                
                Sender.MapAnswer(false);
            }

            // Clear Player cache
            Character.Items.Clear();
        }

        /// <summary>
        /// Atualiza a posição
        /// </summary>
        /// <param name="buffer"></param>
        static void UpdateCharacterPosition(NetDataReader buffer)
        {
            var name = buffer.GetString();
            var mapID = buffer.GetInt();
            var pos = buffer.GetVector2();
            var dir = (Directions)buffer.GetByte();

            var player = Character.Find(name);
            if (player == null) return;

            player.MapID = mapID;
            player.Position = pos;
            player.OffSet = Vector2.Zero;
            player.Direction = dir;
        }

        /// <summary>
        /// Atualiza meu personagem
        /// </summary>
        /// <param name="buffer"></param>
        static void UpdateMyCharacter(NetDataReader buffer)
        {
            if (Character.My == null)
                Character.My = new Character();

            var c = Character.My;
            c.Name = buffer.GetString();
            c.ClassID = buffer.GetInt();
            c.SpriteID = buffer.GetInt();
            c.Level = buffer.GetInt();
            c.Position = buffer.GetVector2();
            c.AccessLevel = (AccessLevels)buffer.GetByte();
        }

        /// <summary>
        /// Muda para a cena de gameplay
        /// </summary>
        /// <param name="buffer"></param>
        static void ChangeToGameplay(NetDataReader buffer)
        {
            GlobalResources.LoadAllResources();
            Game.SetScene<Scenes.GameplayScene>();            
        }

        /// <summary>
        /// Atualiza os personagens
        /// </summary>
        /// <param name="buffer"></param>
        static void UpdateCharacters(NetDataReader buffer)
        {
            if (CharacterPreview.Items == null)
                CharacterPreview.Items = new List<CharacterPreview>();
            else
                CharacterPreview.Items.Clear();

            for(int i = 0; i < Constants.MAX_CHARACTERS; i++)
            {
                var name = buffer.GetString();
                if (name.Length > 0)
                {
                    var c = new CharacterPreview();
                    c.Name = name;
                    c.SpriteID = buffer.GetInt();
                    CharacterPreview.Items.Add(c);
                }
                else
                    CharacterPreview.Items.Add(null);
            }
        }

        /// <summary>
        /// Recebe as classes
        /// </summary>
        /// <param name="buffer"></param>
        static void UpdateClass(NetDataReader buffer)
        {
            // Limpa as classes
            if (CharacterClass.Items == null)
                CharacterClass.Items = new List<CharacterClass>();
            else
                CharacterClass.Items.Clear();

            int count = buffer.GetInt();
            for(int i = 0; i < count; i++)
            {
                var c = new CharacterClass();
                c.Name = buffer.GetStringArray();
                c.Description = buffer.GetStringArray();
                c.StatPrimary = buffer.GetIntArray();
                c.MaleSprite = buffer.GetIntArray();
                c.FemaleSprite = buffer.GetIntArray();
                CharacterClass.Items.Add(c);
            }
        }

        /// <summary>
        /// Muda a cena para seleção de personagem
        /// </summary>
        /// <param name="buffer"></param>
        static void ChangeToSelectCharacter(NetDataReader buffer)
        {
            Game.SetScene<Scenes.SelectCharacterScene>();
        }

        /// <summary>
        /// Recebe um alerta
        /// </summary>
        /// <param name="buffer"></param>
        static void Alert(NetDataReader buffer)
        {
            var text = buffer.GetStringArray();
            Game.GetScene().Alert(text[(int)Game.CurrentLanguage]);
        }

    }
}
