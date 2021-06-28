using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero
{
    public static class JsonHelper
    {
        /// <summary>
        /// Salva um objeto em Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void Save<T>(string filePath, T obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Carrega um objeto em Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        public static void Load<T>(string filePath, out T obj)
        {
            if (!File.Exists(filePath))
                throw new Exception(filePath + "\nArquivo não encontrado!");

            var json = File.ReadAllText(filePath);
            obj = JsonConvert.DeserializeObject<T>(json);            
        }

        /// <summary>
        /// Carrega um objeto em Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception(filePath + "\nArquivo não encontrado!");

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
