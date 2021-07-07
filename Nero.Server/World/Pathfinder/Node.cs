using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.World.Pathfinder
{
    class Node
    {
        public Node Parent { get; set; }
        public bool InOpenList { get; set; }
        public bool InClosedList { get; set; }
        public float DistanceToGoal { get; set; }
        public float DistanceTraveled { get; set; }
        private Node[] _neighbors;
        public Vector2 Position { get; private set; }
        public bool Walkable { get; set; }


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="position"></param>
        public Node(Vector2 position)
        {
            this.Position = position;
        }

        public void SetNeighbors(Node[] neighbors)
        {
            _neighbors = neighbors;
        }

        public Node[] GetNeighbors()
        {
            return _neighbors;
        }
    }
}
