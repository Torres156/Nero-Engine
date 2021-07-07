using Nero.Client.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.World.Pathfinder
{
    class AStar
    {
        const int MAX_TILE = 15;
        
        private List<Node> openList = new List<Node>();        
        private List<Node> closedList = new List<Node>();
        Node[,] _searchNodes;

        Spawn spawnDevice;
        
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="spawnDevice"></param>
        public AStar(Spawn spawnDevice)
        {
            this.spawnDevice = spawnDevice;
        }

        /// <summary>
        /// Returns an estimate of the distance between two points. (H)
        /// </summary>
        private float Heuristic(Vector2 point1, Vector2 point2)
        {
            return Math.Abs(point1.x - point2.x) +
                   Math.Abs(point1.y - point2.y);
        }

        /// <summary>
        /// Resets the state of the search nodes.
        /// </summary>
        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();
            var map = spawnDevice.GetMap();

            for (int x = 0; x <= map.Size.x; x++)
            {
                for (int y = 0; y <= map.Size.y; y++)
                {
                    Node node = _searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Returns the node with the smallest distance to goal.
        /// </summary>
        private Node FindBestNode()
        {
            Node currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal.
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint)
        {
            InitilizeSearchNodes();
            Vector2 normStartPoint = startPoint;
            Vector2 normEndPoint = endPoint;
            var map = spawnDevice.GetMap();

            if (normStartPoint.x < 0 || normStartPoint.y < 0
                || normEndPoint.x < 0 || normEndPoint.y < 0)
                return new List<Vector2>();

            if (normStartPoint.x > map.Size.x || normStartPoint.y > map.Size.y
                || normEndPoint.x > map.Size.x || normEndPoint.y > map.Size.y)
                return new List<Vector2>();

            // Only try to find a path if the start and end points are different.
            if (normStartPoint.x == normEndPoint.x && normStartPoint.y == normEndPoint.y)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience.
            Node startNode = _searchNodes[(int)normStartPoint.x, (int)normStartPoint.y];
            Node endNode = _searchNodes[(int)normEndPoint.x, (int)normEndPoint.y];

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(normStartPoint, normEndPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Setp 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                Node currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                {
                    break;
                }

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////

                if (currentNode.Position.x == endNode.Position.x && currentNode.Position.y == endNode.Position.y)
                {
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.GetNeighbors().Length; i++)
                {
                    Node neighbor = currentNode.GetNeighbors()[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    // An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbor.Position, normEndPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value 
                        //     we just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + 
                        //     the estimated distance between the neighbouring node and
                        //     goal node.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active 
                        //     Node.
                        neighbor.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }

        
        /// <summary>
        /// Use the parent field of the search nodes to trace
        /// a path from the end node to the start node.
        /// </summary>
        private List<Vector2> FindFinalPath(Node startNode, Node endNode)
        {
            closedList.Add(endNode);

            Node parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path.
            while (parentTile != startNode)
            {

                closedList.Add(parentTile);

                parentTile = parentTile.Parent;

            }

            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                // Add the path, as well as transform the positions from tile-space to world-space.
                finalPath.Add(new Vector2(closedList[i].Position.x,
                                          closedList[i].Position.y));
            }

            return finalPath;
        }

        private void InitilizeSearchNodes()
        {
            var map = spawnDevice.GetMap();
            _searchNodes = new Node[map.Size.x + 1, map.Size.y + 1];

            for (int x = 0; x <= map.Size.x; x++)
            {
                for (int y = 0; y <= map.Size.y; y++)
                {
                    var node = new Node(new Vector2(x, y))
                    {
                        Walkable = true
                    };

                    var atr = map.Attributes[x, y];
                    if (atr.Any(i => i.Type == Server.Map.AttributeTypes.Block || i.Type == Server.Map.AttributeTypes.Warp))
                        node.Walkable = false;

                    _searchNodes[x, y] = node;
                }
            }

            // Loop back through and add the neighbors of the nodes.
            for (int x = 0; x < _searchNodes.GetLength(0); x++)
            {
                for (int y = 0; y < _searchNodes.GetLength(1); y++)
                {
                    Node[] neighbors = new Node[4];

                    Vector2[] neighborPositions = new Vector2[]
                    {
                        new Vector2(x, y - 1),
                        new Vector2(x, y + 1),
                        new Vector2(x - 1, y),
                        new Vector2(x + 1, y)
                    };

                    for (int i = 0; i < neighborPositions.Length; i++)
                    {
                        if (neighborPositions[i].x < 0 || neighborPositions[i].x >= _searchNodes.GetLength(0))
                        {
                            continue;
                        }

                        if (neighborPositions[i].y < 0 || neighborPositions[i].y >= _searchNodes.GetLength(1))
                        {
                            continue;
                        }
                        if (_searchNodes[(int)neighborPositions[i].x, (int)neighborPositions[i].y] == null)
                            continue;

                        if (_searchNodes[(int)neighborPositions[i].x, (int)neighborPositions[i].y].Walkable)
                            neighbors[i] = _searchNodes[(int)neighborPositions[i].x, (int)neighborPositions[i].y];

                    }

                    _searchNodes[x, y].SetNeighbors(neighbors);
                }
            }

        }

    }
}

