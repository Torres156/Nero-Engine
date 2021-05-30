using LiteNetLib;
using LiteNetLib.Utils;
using Nero.Server.Player;
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
            Register, Login,
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
            }
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
                Sender.Alert(peer, "Senha incorreta!");
                return;
            }

            // Verifica se está em uso            
            if (Account.Items.Any(i => i.Name.ToLower().Equals(accName.ToLower())))
            {
                var fAcc = Account.Items.Find(i => i.Name.ToLower().Equals(accName.ToLower()));
                Sender.Alert(peer, "A conta já está em uso, reporte caso não seja você!");
                fAcc.peer?.Disconnect();
                return;
            }

            acc.peer = peer;            
            Account.Items.Add(acc);

            // Verifica se os personagens existem

            // Troca a cena
            Sender.ChangeToSelectCharacter(peer);
            Sender.UpdateClass(peer);
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
                Sender.Alert(peer, $"O nome {accName} não está disponivel!");
                return;
            }

            // Cria a conta
            var acc = new Account();
            acc.Name = accName;
            acc.Password = accPwd;
            Account.Save(acc);

            Sender.Alert(peer, $"A conta {accName} foi criada com sucesso!");
        }
    }
}
