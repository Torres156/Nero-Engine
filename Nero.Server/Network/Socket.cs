using LiteNetLib;
using Nero.Server.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server.Network
{
    static class Socket
    {
        // Publics
        public static NetManager Device { get; private set; }   // Dispositivo de conexão
        public static int PORT = 4000;                          // Porta de entrada

        // Privates
        static EventBasedNetListener listener;  // Lista de eventos
        
        /// <summary>
        /// Inicializa o dispositivo de conexão
        /// </summary>
        public static void Initialize()
        {
            Console.WriteLine("Inicializando conexão...");
            listener = new EventBasedNetListener();
            Device = new NetManager(listener);
            Device.AutoRecycle = true;
            Device.Start(PORT);
            Console.WriteLine($"Servidor aberto na porta {PORT}.");

            listener.ConnectionRequestEvent += Listener_ConnectionRequestEvent;
            listener.PeerConnectedEvent += Listener_PeerConnectedEvent;
            listener.PeerDisconnectedEvent += Listener_PeerDisconnectedEvent;
            listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
        }

        /// <summary>
        /// Recebe os pacotes
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="reader"></param>
        /// <param name="deliveryMethod"></param>
        private static void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            Receive.Handle(peer, reader);
        }

        /// <summary>
        /// Entrada conexão desconectada
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="disconnectInfo"></param>
        private static void Listener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"A entrada de conexão <{peer.EndPoint.Address.ToString()}> se desconectou!");

            var findAcc = Account.Find(peer);
            if (findAcc != null)
            {
                Account.Save(findAcc);
                Account.Items.Remove(findAcc);
            }
        }

        /// <summary>
        /// Nova entrada de conexão
        /// </summary>
        /// <param name="peer"></param>
        private static void Listener_PeerConnectedEvent(NetPeer peer)
        {
            Console.WriteLine($"Nova entrada de conexão <{peer.EndPoint.Address.ToString()}>");
        }

        /// <summary>
        /// Pedido para entrada de conexão
        /// </summary>
        /// <param name="request"></param>
        private static void Listener_ConnectionRequestEvent(ConnectionRequest request)
        {
            if (Device.ConnectedPeersCount < 100)
                request.Accept();
            else
                request.Reject();
        }

        /// <summary>
        /// Processa os eventos
        /// </summary>
        public static void PollEvents()
        {
            Device.PollEvents();
        }
    }
}
