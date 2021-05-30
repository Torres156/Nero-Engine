using LiteNetLib.Utils;
using Nero.Client.Player;
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
            Game.GetScene().Alert(buffer.GetString());
        }

    }
}
