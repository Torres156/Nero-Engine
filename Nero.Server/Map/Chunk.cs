using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server.Map
{
    class Chunk
    {
        public int TileID = 0;                      // ID do gráfico
        public Vector2 Source;                      // Configuração do gráfico
        public ChunkTypes type = ChunkTypes.Normal; // Tipo do chunk
        public Vector2 Position;                    // Posição do chunk
        public byte[] Autotile = new byte[4];       // Configuração do Autotile
    }
}
