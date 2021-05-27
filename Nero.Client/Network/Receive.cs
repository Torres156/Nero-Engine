using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Network
{
    static class Receive
    {
        enum Packets
        {
            Alert, ChangeToSelectCharacter
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
