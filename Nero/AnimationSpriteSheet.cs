using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nero
{
    internal struct AnimationDataSpriteSheet
    {
        public byte[] Data;
        public int Lenght;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Data"></param>
        public AnimationDataSpriteSheet(byte[] Data)
        {
            this.Data = Data;
            Lenght = Data.Length;
        }
    }

    internal class AnimationSpriteSheet
    {
        /// <summary>
        /// Quantidade de Frames no Eixo X
        /// </summary>
        public int X = 1;

        /// <summary>
        /// Quantidade de Frames no Eixo Y
        /// </summary>
        public int Y = 1;

        /// <summary>
        /// Dados de Frame
        /// </summary>
        public List<AnimationDataSpriteSheet> Data;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="FileName"></param>
        public AnimationSpriteSheet(string FileName)
        {
            Data = new List<AnimationDataSpriteSheet>();
            Open(FileName);
        }

        public AnimationSpriteSheet(byte[] data)
        {
            Data = new List<AnimationDataSpriteSheet>();
            int W = 0, H = 0;

            var r = new BinaryReader(new MemoryStream(data));
            X = r.ReadInt16();
            Y = r.ReadInt16();
            W = r.ReadInt32();
            H = r.ReadInt32();

            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int l = r.ReadInt32();
                var b = r.ReadBytes(l);
                Data.Add(new AnimationDataSpriteSheet(b));
            }

            r.Close();
        }

        /// <summary>
        /// Abre o Arquivo
        /// </summary>
        /// <param name="FileName"></param>
        void Open(string FileName)
        {
            int W = 0, H = 0;
                        
            var r = new BinaryReader(File.OpenRead(FileName));
            X = r.ReadInt16();
            Y = r.ReadInt16();
            W = r.ReadInt32();
            H = r.ReadInt32();

            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int l = r.ReadInt32();
                var b = r.ReadBytes(l);
                Data.Add(new AnimationDataSpriteSheet(b));
            }

            r.Close();
        }
    }
}
