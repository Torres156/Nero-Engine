using LiteNetLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Server.Player
{
    class Account
    {
        #region Static
        public static readonly string Path = "data/account/";
        public static List<Account> Items = new List<Account>();

        /// <summary>
        /// Verifica se existe o diretório
        /// </summary>
        public static void CheckDirectory()
        {
            Console.WriteLine("Checando diretório de contas...");
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        /// <summary>
        /// Verifica se existe a conta
        /// </summary>
        /// <param name="name"></param>
        public static bool Exist(string name)
        {
            var fileName = Path + name.ToLower() + ".json";
            return File.Exists(fileName);
        }

        /// <summary>
        /// Salva a conta
        /// </summary>
        /// <param name="account"></param>
        public static void Save(Account account)
        {
            var fileName = Path + account.Name.ToLower() + ".json";
            JsonHelper.Save(fileName, account);
        }

        /// <summary>
        /// Carrega a conta
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Account Load(string name)
        {
            var fileName = Path + name.ToLower() + ".json";
            Account acc;
            JsonHelper.Load(fileName, out acc);
            return acc;
        }

        /// <summary>
        /// Encontra uma conta
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public static Account Find(NetPeer peer)
            => Items.Find(i => i.peer == peer);

        #endregion

        // Publics
        public string Name = "";                                            // Nome da conta
        public string Password = "";                                        // Senha
        public string[] Characters = new string[Constants.MAX_CHARACTERS];  // Personagens

        [JsonIgnore]
        public NetPeer peer; // Entrada de conexão

        /// <summary>
        /// Construtor
        /// </summary>
        public Account()
        {
            for (int i = 0; i < Characters.Length; i++)
                Characters[i] = "";
        }


    }
}
