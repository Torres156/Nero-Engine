using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World.Pathfinder
{
    class Greedy
    {
        const int MAX_LOOPS = 100;

        bool[,] grid, gridCache;
        List<CellNode> OpenNodes = new List<CellNode>();
        List<CellNode> CloseNodes = new List<CellNode>();
        Spawn spawnDevice;
        Int2 Size;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="grid"></param>
        public Greedy(Spawn spawnDevice)
        {
            this.spawnDevice = spawnDevice;

            // Tamanho da grade
            var map = spawnDevice.GetMap();
            Size = (map.Size + Int2.One) * 4;

            // Cria a grade
            grid = new bool[Size.x, Size.y];
            gridCache = new bool[Size.x, Size.y];

            // Cria um cache dos bloqueios
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    gridCache[x, y] = map.Attributes[x / 4, y / 4].Any(i => i.Type == Map.AttributeTypes.Block || i.Type == Map.AttributeTypes.Warp);
        }

        /// <summary>
        /// Reseta o grid
        /// </summary>
        void ResetGrid(SpawnItem spawn)
        {
            grid = (bool[,])gridCache.Clone();

            foreach (var i in spawnDevice.Items)
                if (i != spawn && i.State != SpawnStates.Dead)
                    grid[(int)i.Position.x, (int)i.Position.y] = true;
        }

        /// <summary>
        /// Cria o caminho
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public List<Int2> CreatePath(SpawnItem spawn, Int2 endPoint)
        {
            ResetGrid(spawn);
            Int2 startPoint = spawn.Position.ToInt2();
            if (startPoint.Distance(endPoint) > 30)
                return new List<Int2>(); // Sem caminho

            OpenNodes.Clear();
            OpenNodes.Add(new CellNode(startPoint)
            {
                H = Manhattan(startPoint, endPoint),
                F = 0
            });
            CloseNodes.Clear();

            int processLoop = 0;
            while (OpenNodes.Count > 0)
            {
                if (processLoop > MAX_LOOPS)
                    return new List<Int2>(); // Evitar loop infinito

                var current = FindBestNode();
                if (current == null || OpenNodes.Count == 0)
                    return new List<Int2>(); // Não encontrado

                if (current.Position.Equals(endPoint))
                    return GetPath(endPoint);

                Int2[] pointNeighbors =
                {
                    new Int2(current.Position.x - 1, current.Position.y),
                    new Int2(current.Position.x + 1, current.Position.y),
                    new Int2(current.Position.x, current.Position.y - 1),
                    new Int2(current.Position.x, current.Position.y + 1),

                    // Diagonais
                    //new Point(current.Position.X + 1, current.Position.Y + 1),
                    //new Point(current.Position.X - 1, current.Position.Y + 1),
                    //new Point(current.Position.X + 1, current.Position.Y - 1),
                    //new Point(current.Position.X - 1, current.Position.Y - 1),
                };

                foreach (var p in pointNeighbors)
                {
                    if (p.x < 0 || p.x >= Size.x ||
                        p.y < 0 || p.y >= Size.y)
                        continue;

                    if (CloseNodes.Any(i => i.Position == p) || OpenNodes.Any(i => i.Position == p))
                        continue;

                    if (grid[p.x, p.y])
                        continue;

                    var node = new CellNode(p)
                    {
                        H = Manhattan(p, endPoint),
                        F = current.F + 1,
                        G = Manhattan(p, startPoint),
                        Parent = current,
                    };
                    OpenNodes.Add(node);
                }

                current.Closed = true;
                CloseNodes.Add(current);
                OpenNodes.Remove(current);
                processLoop++;
            }

            return new List<Int2>(); // No path
        }

        CellNode FindBestNode()
        {
            int H = int.MaxValue;
            CellNode current = null;
            foreach (var i in OpenNodes)
                if (i.H < H)
                {
                    current = i;
                    H = i.H;
                }
            return current;
        }

        /// <summary>
        /// Coloca o grid
        /// </summary>
        /// <param name="grid"></param>
        public void SetGrid(bool[,] grid)
        {
            this.grid = grid;
        }

        List<Int2> GetPath(Int2 endPoint)
        {
            var list = new List<Int2>();
            if (CloseNodes.Count == 0)
                return list;
            var current = CloseNodes.Last();

            while (current != null)
            {
                list.Add(current.Position);
                current = current.Parent;
            }

            return list;
        }

        public List<Int2> GetAllPoints()
        {
            var list = new List<Int2>();
            foreach (var i in CloseNodes)
                list.Add(i.Position);
            return list;
        }

        int Manhattan(Int2 p1, Int2 p2)
            => Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
    }
}
