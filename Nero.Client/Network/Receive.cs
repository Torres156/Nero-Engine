using LiteNetLib.Utils;
using Nero.Client.Player;
using Nero.Client.World;
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
            }
        }

        /// <summary>
        /// Atualiza meu personagem
        /// </summary>
        /// <param name="buffer"></param>
        static void UpdateMyCharacter(NetDataReader buffer)
        {
            if (Character.My == null)
                Character.My = new Character();

            ref var c = ref Character.My;
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
