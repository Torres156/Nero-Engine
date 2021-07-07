using Nero.Client.World;
using Nero.Server.Core;
using Nero.Server.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World
{
    class SpawnItem
    {
        public int NpcID { get; set; }
        public Vector2 Position { get; set; }
        public long HP { get; set; }
        public Directions Direction { get; set; }


        // Server Only
        public SpawnFactoryItem FactoryItem { get; private set; }
        public Spawn SpawnDevice { get; private set; }
        long tmrRespawn, tmrHibern;
        SpawnStates State = SpawnStates.Normal;
        Vector2 targetPosition;
        List<Vector2> _cachePath = new List<Vector2>();
        bool isMoving;
        Vector2 offSet;


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="factoryItem"></param>
        public SpawnItem(Spawn SpawnDevice, SpawnFactoryItem factoryItem)
        {
            this.FactoryItem = factoryItem;
            this.SpawnDevice = SpawnDevice;
            NpcID = factoryItem.NpcID;
        }

        /// <summary>
        /// Atualiza o spawn
        /// </summary>
        public void Update()
        {
            ProcessMovement();

            if (isMoving)
                return;

            switch (State)
            {
                case SpawnStates.Normal:
                    // Modo hibernado
                    if (tmrHibern > 0)
                    {
                        if (Environment.TickCount64 > tmrHibern)
                        {
                            tmrHibern = 0;
                            targetPosition = ValidPosition();
                            _cachePath = SpawnDevice.AStar.FindPath(Position / 4, targetPosition / 4);
                            _cachePath.Reverse();
                        }
                        return;
                    }


                    if (_cachePath.Count == 0)
                    {
                        tmrHibern = Environment.TickCount64 + 1000 * Utils.Rand(5, 15);
                    }
                    else
                    {
                        var usePath = _cachePath[0] * 4;
                        Directions newDir = Directions.Down;
                        var pos = Position;
                        if (usePath.x > Position.x)                        
                            newDir = Directions.Right;                            
                        
                        if (usePath.x < Position.x)                        
                            newDir = Directions.Left;                            
                        
                        if (usePath.y > Position.y)                        
                            newDir = Directions.Down;                            
                        
                        if (usePath.y < Position.y)                        
                            newDir = Directions.Up;                         

                        switch (newDir)
                        {
                            case Directions.Up:
                                offSet.y = 8;
                                pos.y--;
                                break;

                            case Directions.Down:
                                offSet.y = -8;
                                pos.y++;
                                break;

                            case Directions.Left:
                                offSet.x = 8;
                                pos.x--;
                                break;

                            case Directions.Right:
                                offSet.x = -8;
                                pos.x++;
                                break;
                        }

                        Position = pos;
                        Direction = newDir;
                        if ((Position.ToInt2() / 4).Equals(usePath.ToInt2() / 4))
                            _cachePath.RemoveAt(0);
                        isMoving = true;

                        Network.Sender.SpawnMove(SpawnDevice.Instance, this);
                    }

                    break;

                case SpawnStates.Combat:
                    break;

                case SpawnStates.Dead:
                    if (Environment.TickCount64 > tmrRespawn)
                    {
                        Respawn();
                        Network.Sender.SpawnData(SpawnDevice.Instance, this);
                    }
                    break;
            }
        }

        void ProcessMovement()
        {
            if (!isMoving)
                return;

            var speed = GetNpc().MoveSpeed * ServerCore.DeltaTime;

            if (offSet.x > 0)
            {
                offSet.x -= speed;
                if (offSet.x < 0)
                    offSet.x = 0;
            }

            if (offSet.x < 0)
            {
                offSet.x += speed;
                if (offSet.x > 0)
                    offSet.x = 0;
            }

            if (offSet.y > 0)
            {
                offSet.y -= speed;
                if (offSet.y < 0)
                    offSet.y = 0;
            }

            if (offSet.y < 0)
            {
                offSet.y += speed;
                if (offSet.y > 0)
                    offSet.y = 0;
            }

            if (offSet.Equals(Vector2.Zero))
                isMoving = false;
        }

        /// <summary>
        /// Invoca um spawn
        /// </summary>
        public void Respawn()
        {
            State = SpawnStates.Normal;
            Direction = FactoryItem.BlockMove ? FactoryItem.Direction : (Directions)Utils.Rand(0, 3);

            if (FactoryItem.UsePositionSpawn)
                Position = FactoryItem.Position;
            else
                Position = ValidPosition();


            var npc = GetNpc();
            HP = npc.HP;
            tmrHibern = Environment.TickCount64 + 1000 * Utils.Rand(5, 15);
        }

        Vector2 ValidPosition()
        {
            var map = MapInstance.Items[SpawnDevice.MapID];

        newvalue:;
            var spawnX = Utils.Rand(0, map.Size.x * 4);
            var spawnY = Utils.Rand(0, map.Size.y * 4);

            // Verifica se há algum atributo no mapa que não pode ter npc
            var attr = map.Attributes[spawnX / 4, spawnY / 4];
            if (attr.Any(i => i.Type == AttributeTypes.Block || i.Type == AttributeTypes.Warp))
                goto newvalue;

            // Verifica se já não há outro spawn na mesma posição
            if (SpawnDevice.Items.Any(i => i != this && i != null && i.Position.Equals(new Vector2(spawnX, spawnY))))
                goto newvalue;

            return new Vector2(spawnX, spawnY);
        }

        /// <summary>
        /// Dados do NPC
        /// </summary>
        /// <returns></returns>
        public Npc GetNpc()
            => Npc.Items[NpcID];

        public int IndexOf
            => Array.IndexOf(SpawnDevice.Items, this);
    }
}
