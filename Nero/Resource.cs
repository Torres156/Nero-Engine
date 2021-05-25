using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero
{
    public sealed class Resource
    {
        List<ResourceData> data;
        public string filePath = "";

        public int Count
            => data.Count;
        

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="filePath"></param>
        public Resource(string filePath)
        {            
            this.filePath = filePath;
            data = new List<ResourceData>();
        }

        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public void Load()
        {
            if (!File.Exists(filePath))
                return;

            data = new List<ResourceData>();


            byte[] d = File.ReadAllBytes(filePath);

            // Descomprimir
            d = MemoryService.Decompress(d);

            using(var r = new BinaryReader(new MemoryStream(d)))
            {
                int l = r.ReadInt32();
                if (l > 0)
                {
                    for (int i = 0; i < l; i++)
                    {
                        var res = new ResourceData();
                        res.FileName = r.ReadString();
                        int limg = r.ReadInt32();
                        res.imgData = r.ReadBytes(limg);

                        data.Add(res);
                    }
                }
            }
        }

        /// <summary>
        /// Salva os recursos
        /// </summary>
        public void Save()
        {
            byte[] d = { };

            using (var w = new BinaryWriter(new MemoryStream()))
            {
                w.Write(data.Count);
                if (data.Count > 0)
                    for(int i = 0; i < data.Count; i++)
                    {
                        w.Write(data[i].FileName);
                        w.Write(data[i].imgData.Length);
                        w.Write(data[i].imgData);
                    }

                d = (w.BaseStream as MemoryStream).ToArray();
            }

            // Comprimir
            d = MemoryService.Compress(d);

            File.WriteAllBytes(filePath, d);
        }

        /// <summary>
        /// Pega uma textura do recurso
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Texture GetTexture(int index)
        {
            if (index >= data.Count)
                return null;

            var d = data[index].imgData;
            return new Texture(d);
        }

        /// <summary>
        /// Adiciona um novo recurso
        /// </summary>
        /// <param name="data"></param>
        public void Add(string fileName, byte[] data)
        {
            var d = new ResourceData();
            d.FileName = fileName;
            d.imgData = data;
            this.data.Add(d);
        }
    }

    public sealed class ResourceData
    {
        public string FileName = "";
        public byte[] imgData = { };


        /// <summary>
        /// Construtor
        /// </summary>
        internal ResourceData()
        {}
    }
}
