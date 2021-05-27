using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Network
{
    static class Socket
    {
        // Publics
        public static NetManager Device { get; private set; }   // Dispositivo de conexão


        // Privates
        static EventBasedNetListener listener;  // Lista de eventos


        /// <summary>
        /// Inicializa o dispositivo de conexão
        /// </summary>
        public static void Initialize()
        {
            listener = new EventBasedNetListener();
            Device = new NetManager(listener);
            Device.AutoRecycle = true;
            Device.Start();

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
            Receive.Handle(reader);
        }

        /// <summary>
        /// Processa os eventos
        /// </summary>
        public static void PollEvents()
        {
            Device.PollEvents();
        }

        /// <summary>
        /// Estado de conexão
        /// </summary>
        public static bool IsConnected
            => Device.FirstPeer != null && Device.FirstPeer.ConnectionState == ConnectionState.Connected;

        /// <summary>
        /// Tenta conectar ao servidor
        /// </summary>
        public static void Connect()
        {
            Device.Connect("localhost", 4000, "");
        }
    }
}
