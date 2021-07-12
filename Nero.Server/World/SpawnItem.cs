using Nero.Server.World;
using Nero.Server.Core;
using Nero.Server.Map;
using Nero.Server.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nero.Server.Helpers;

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
        long tmrRespawn, tmrHibern, tmrAttack;
        public SpawnStates State { get; private set; } = SpawnStates.Normal;
        Vector2 targetPosition;
        public Character target;
        List<Int2> _cachePath = new List<Int2>();        
        bool isMoving;
        Vector2 offSet;
        int tryCount = 0;        


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
                            targetPosition = ValidPositionMovement();
                            NewPathfind();
                        }
                        return;
                    }

                    if (_cachePath.Count <= 1)
                    {
                        tmrHibern = Environment.TickCount64 + 1000 * Utils.Rand(5, 15);
                        return;
                    }
                    else
                    {
                        var usePath = _cachePath[1];
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
                                pos.y--;
                                break;

                            case Directions.Down:
                                pos.y++;
                                break;

                            case Directions.Left:
                                pos.x--;
                                break;

                            case Directions.Right:
                                pos.x++;
                                break;
                        }

                        if (CanMove(pos))
                        {
                            switch (newDir)
                            {
                                case Directions.Up:
                                    offSet.y = 8;
                                    break;

                                case Directions.Down:
                                    offSet.y = -8;
                                    break;

                                case Directions.Left:
                                    offSet.x = 8;
                                    break;

                                case Directions.Right:
                                    offSet.x = -8;
                                    break;
                            }

                            tryCount = 0;
                            Position = pos;
                            Direction = newDir;
                            if ((Position.ToInt2()).Equals(usePath))
                                _cachePath.RemoveAt(0);
                            isMoving = true;

                            Network.Sender.SpawnMove(SpawnDevice.Instance, this);
                        }
                        else
                        {
                            tryCount++;
                            if (tryCount > 2)
                            {
                                tryCount = 0;
                                tmrHibern = Environment.TickCount64 + 1000 * Utils.Rand(5, 15);
                                return;
                            }

                            NewPathfind();
                        }
                    }

                    break;

                case SpawnStates.Combat:
                    if (target == null)
                    {
                        State = SpawnStates.Normal;
                        tmrHibern = Environment.TickCount64 + 1000 * Utils.Rand(5, 15);
                        return;
                    }

                    if (tmrAttack > 0 && Environment.TickCount64 > tmrAttack)
                        tmrAttack = 0;

                    if (!isMoving)
                    {
                        if (targetPosition != target.Position)
                        {
                            targetPosition = target.Position;
                            NewPathfind();
                        }

                        // Atacar á distancia
                        if (GetNpc().Range > 0)
                            if (Position.Distance(targetPosition) <= GetNpc().Range)
                            {
                                if (CanAttack())
                                {
                                    Directions newDir = Directions.Down;

                                    if (targetPosition.x > Position.x)
                                        newDir = Directions.Right;

                                    if (targetPosition.x < Position.x)
                                        newDir = Directions.Left;

                                    if (targetPosition.y > Position.y)
                                        newDir = Directions.Down;

                                    if (targetPosition.y < Position.y)
                                        newDir = Directions.Up;

                                    Direction = newDir;
                                    tmrAttack = Environment.TickCount64 + (int)(MathF.Pow(1000f, 2) / GetNpc().AttackSpeed);
                                    CombatHelper.NpcRequestAttack(this, target);
                                    Network.Sender.SpawnAttackAnimation(this);
                                }
                                return;
                            }

                        if (_cachePath.Count <= 1) // Atacar Meele
                        {
                            if (CanAttack())
                            {                                
                                Directions newDir = Directions.Down;

                                if (targetPosition.x > Position.x)
                                    newDir = Directions.Right;

                                if (targetPosition.x < Position.x)
                                    newDir = Directions.Left;

                                if (targetPosition.y > Position.y)
                                    newDir = Directions.Down;

                                if (targetPosition.y < Position.y)
                                    newDir = Directions.Up;

                                Direction = newDir;
                                tmrAttack = Environment.TickCount64 + (int)(MathF.Pow(1000f,2) / GetNpc().AttackSpeed);
                                CombatHelper.NpcRequestAttack(this, target);
                                Network.Sender.SpawnAttackAnimation(this);
                            }
                            return;
                        }
                        else
                        {
                            var usePath = _cachePath[1];
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
                                    pos.y--;
                                    break;

                                case Directions.Down:
                                    pos.y++;
                                    break;

                                case Directions.Left:
                                    pos.x--;
                                    break;

                                case Directions.Right:
                                    pos.x++;
                                    break;
                            }


                            if (CanMove(pos))
                            {
                                switch (newDir)
                                {
                                    case Directions.Up:
                                        offSet.y = 8;
                                        break;

                                    case Directions.Down:
                                        offSet.y = -8;
                                        break;

                                    case Directions.Left:
                                        offSet.x = 8;
                                        break;

                                    case Directions.Right:
                                        offSet.x = -8;
                                        break;
                                }
                                tryCount = 0;
                                Position = pos;
                                Direction = newDir;
                                isMoving = true;
                                if ((Position.ToInt2()).Equals(usePath))
                                    _cachePath.RemoveAt(0);

                                Network.Sender.SpawnMove(SpawnDevice.Instance, this);
                            }
                            else
                            {
                                if (tryCount < 3)
                                {
                                    tryCount++;
                                    NewPathfind();
                                }
                            }
                        }
                    }

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

        /// <summary>
        /// Verifica se pode se mover
        /// </summary>
        /// <param name="pos"></param>
        bool CanMove(Vector2 pos)
        {
            foreach (var i in SpawnDevice.Items)
                if (i != this && i.State != SpawnStates.Dead && i.Position.Equals(pos))
                    return false;

            var players = Character.Items.Where(i => i.GetInstance() == SpawnDevice.Instance);
            foreach (var i in players)
                if (i.Position.Equals(pos))
                    return false;

            return true;
        }

        /// <summary>
        /// Pode atacar?
        /// </summary>
        /// <returns></returns>
        bool CanAttack()
        {
            if (tmrAttack > 0)
                return false;
            return true;
        }

        /// <summary>
        ///  Processa o movimento
        /// </summary>
        void ProcessMovement()
        {
            if (!isMoving)
                return;

            var speed = GetNpc().MoveSpeed * Game.FixedTime;

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

        /// <summary>
        /// Encontra um posição válida
        /// </summary>
        /// <returns></returns>
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
        /// Encontra uma posição válida para o movimento
        /// </summary>
        /// <returns></returns>
        Vector2 ValidPositionMovement()
        {
            var map = MapInstance.Items[SpawnDevice.MapID];

        newvalue:;
            var spawnX = Utils.Rand(Math.Max(0, (int)Position.x - 12), Math.Min(map.Size.x * 4, (int)Position.x + 12));
            var spawnY = Utils.Rand(Math.Max(0, (int)Position.y - 12), Math.Min(map.Size.y * 4, (int)Position.y + 12));

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
        /// Encontra um novo caminho
        /// </summary>
        void NewPathfind()
        {
            _cachePath = SpawnDevice.Greedy.CreatePath(this, targetPosition);
            _cachePath.Reverse();
        }

        /// <summary>
        /// Dados do NPC
        /// </summary>
        /// <returns></returns>
        public Npc GetNpc()
            => Npc.Items[NpcID];

        public int IndexOf
            => Array.IndexOf(SpawnDevice.Items, this);

        /// <summary>
        /// Recebe dano
        /// </summary>
        /// <param name="value"></param>
        public void ReceiveDamage(Character player, int value, DamageTypes damageType)
        {
            if (value > HP)
            {
                Death();
            }
            else
            {
                HP -= value;
                target = player;
                State = SpawnStates.Combat;
                // SEND HP UPDATE
            }
            Network.Sender.FloatMessage(SpawnDevice.Instance, $"-{value}", Color.Red, Position * 8 + new Vector2(10, -10));
        }

        /// <summary>
        /// Calcula o dano que irá receber
        /// </summary>
        /// <param name="value"></param>
        public void CalculateDamageReceive(ref int value, DamageTypes damageType, int ignoreResist = 0)
        {
            var resist = damageType == DamageTypes.Physic ? GetNpc().ResistPhysic : GetNpc().ResistMagic;
            resist = Math.Max(resist - ignoreResist, 1);
            value = (int)(MathF.Pow(value, 2) / (resist * 3));
        }

        /// <summary>
        /// Morre o npc
        /// </summary>
        void Death()
        {
            State = SpawnStates.Dead;
            tmrRespawn = Environment.TickCount64 + GetNpc().RespawnTime * 1000;
            Network.Sender.SpawnData(SpawnDevice.Instance, this);
        }

    }
}
