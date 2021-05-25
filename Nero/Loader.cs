using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nero
{
    using NativeTexture = SFML.Graphics.Texture;
    public static class Loader
    {
        public static byte[] LoadData(string filepath)
        {
            if (!File.Exists(filepath))
                throw new Exception($"Arquivo não existe!\n{filepath}");

            var fs = File.OpenRead(filepath);
            var r = new BinaryReader(fs);
            r.ReadString();
            int len = r.ReadInt32();
            byte[] data = r.ReadBytes(len);
            r.Close();
            fs.Close();

            data = MemoryService.Decompress(data);

            return data;
        }

        public static byte[] LoadData(byte[] fileData)
        {
            var fs = new MemoryStream(fileData);
            var r = new BinaryReader(fs);
            r.ReadString();
            int len = r.ReadInt32();
            byte[] data = r.ReadBytes(len);
            r.Close();
            fs.Close();

            data = MemoryService.Decompress(data);

            return data;
        }

        /// <summary>
        /// Carrega a textura
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        internal static NativeTexture LoadNativeTexture(string filepath)
        {            
            var data = LoadData(filepath);
            return new NativeTexture(data);
        }

        internal static LargeTexture LoadLargeTexture(string filepath)
        {            
            var data = LoadData(filepath);

            var img = new Image(data);
            return new LargeTexture(img);
        }

    }
}
