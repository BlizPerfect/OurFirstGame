using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Сущность врага.
/// </summary>
class Mob : Person
{
    private bool SawPlayer = false; // Видел ли враг игрока

    public string Name { protected set; get; } // Имя врага
    public string NameForHitAndMiss { protected set; get; } // Имя врага в нужном падеже для промаха по нему
    public string NameForBlock { protected set; get; } // Имя врага в нужном падеже для полного блокирования урона по нему
    public bool Male { protected set; get; } // Является ли враг "мужчиной"
    public int Points { protected set; get; } // Кол-во очков за победу над врагом

    public Mob(Point startingPosition, Floor currentFloor, int spawnRoomId, int id)
    {
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;
        PreviousPosition = Position;
        Id = id;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = Id;
        POV = 5;
    }

    /// <summary>
    /// Проверка, видит ли враг игрока
    /// </summary>
    /// <returns>bool</returns>
    private bool CanSeePlayer(Player player)
    {
        for (var dy = -1 * POV; dy <= POV; dy++)
        {
            for (var dx = -1 * POV; dx <= POV; dx++)
            {
                if (player.Position.X == Position.X + dx && player.Position.Y == Position.Y + dy && player.CurrentRoom == CurrentRoom)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Проверка, может ли враг атаковать игрока
    /// </summary>
    /// <returns>bool</returns>
    public bool CanAttackPlayer(Player player)
    {
        for (var dy = -1; dy <= 1; dy++)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                if (player.Position.X == Position.X + dx && player.Position.Y == Position.Y + dy && player.CurrentRoom == CurrentRoom)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Передвижение врага
    /// </summary>
    /// <returns>void</returns>
    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        //8 0 5
        //3 x 1
        //7 2 6
        var rnd = new Random();
        var pressedKey = -1;

        var oldPlayerPostionX = Position.X;
        var oldPlayerPostionY = Position.Y;

        var probPlayerPostionX = Position.X;
        var probPlayerPostionY = Position.Y;

        if (!SawPlayer)
        {
            pressedKey = rnd.Next(0, 4);
            SawPlayer = CanSeePlayer(player);
        }
        else if (CurrentRoom == player.CurrentRoom)
        {
            if (player.PreviousPosition.X == Position.X & player.PreviousPosition.Y < Position.Y)
            {
                pressedKey = 0;
            }
            else if (player.PreviousPosition.X > Position.X & player.PreviousPosition.Y < Position.Y)
            {
                pressedKey = 5;
            }
            else if (player.PreviousPosition.X > Position.X & player.PreviousPosition.Y == Position.Y)
            {
                pressedKey = 1;
            }
            else if (player.PreviousPosition.X > Position.X & player.PreviousPosition.Y > Position.Y)
            {
                pressedKey = 6;
            }
            else if (player.PreviousPosition.X == Position.X & player.PreviousPosition.Y > Position.Y)
            {
                pressedKey = 2;
            }
            else if (player.PreviousPosition.X < Position.X & player.PreviousPosition.Y > Position.Y)
            {
                pressedKey = 7;
            }
            else if (player.PreviousPosition.X < Position.X & player.PreviousPosition.Y == Position.Y)
            {
                pressedKey = 3;
            }
            else if (player.PreviousPosition.X < Position.X & player.PreviousPosition.Y < Position.Y)
            {
                pressedKey = 8;
            }
            else
            {
                //pass
            }
        }
        else if (SawPlayer && Math.Abs(CurrentRoom.RoomId - player.CurrentRoom.RoomId) == 1)
        {
            var gate = CurrentRoom.Gates.Find(x => x.NextRoomIndex == player.CurrentRoom.RoomId);
            if (gate.GatePosition.X == Position.X && gate.GatePosition.Y == Position.Y)
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
            if (gate.GatePosition.X == Position.X && gate.GatePosition.Y < Position.Y)
            {
                pressedKey = 0;
            }
            else if (gate.GatePosition.X > Position.X && gate.GatePosition.Y < Position.Y)
            {
                pressedKey = 5;
            }
            else if (gate.GatePosition.X > Position.X && gate.GatePosition.Y == Position.Y)
            {
                pressedKey = 1;
            }
            else if (gate.GatePosition.X > Position.X && gate.GatePosition.Y > Position.Y)
            {
                pressedKey = 6;
            }
            else if (gate.GatePosition.X == Position.X && gate.GatePosition.Y > Position.Y)
            {
                pressedKey = 2;
            }
            else if (gate.GatePosition.X < Position.X && gate.GatePosition.Y > Position.Y)
            {
                pressedKey = 7;
            }
            else if (gate.GatePosition.X < Position.X && gate.GatePosition.Y == Position.Y)
            {
                pressedKey = 3;
            }
            else if (gate.GatePosition.X < Position.X && gate.GatePosition.Y < Position.Y)
            {
                pressedKey = 8;
            }
            else
            {
                //pass
            }
        }
        else if (Math.Abs(CurrentRoom.RoomId - player.CurrentRoom.RoomId) > 1)
        {
            SawPlayer = false;
        }

        if (pressedKey.Equals(3))
        {
            if (CheckArrayLimits(-1, 0))
            {
                probPlayerPostionX -= 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(1))
        {
            if (CheckArrayLimits(1, 0))
            {
                probPlayerPostionX += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(2))
        {
            if (CheckArrayLimits(0, 1))
            {
                probPlayerPostionY += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(0))
        {
            if (CheckArrayLimits(0, -1))
            {
                probPlayerPostionY -= 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(5))
        {
            if (CheckArrayLimits(1, -1))
            {
                probPlayerPostionX += 1;
                probPlayerPostionY += -1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(6))
        {
            if (CheckArrayLimits(1, 1))
            {
                probPlayerPostionX += 1;
                probPlayerPostionY += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(7))
        {
            if (CheckArrayLimits(-1, 1))
            {
                probPlayerPostionX += -1;
                probPlayerPostionY += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(8))
        {
            if (CheckArrayLimits(-1, -1))
            {
                probPlayerPostionX += -1;
                probPlayerPostionY += -1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }

        if (!CheckArrayPlayer(probPlayerPostionX, probPlayerPostionY) && CheckArrayEnemies(probPlayerPostionX, probPlayerPostionY) &&
        CheckArrayWallsExtended(probPlayerPostionX, probPlayerPostionY))
        {
            PreviousPosition = Position;
            Position.X = probPlayerPostionX;
            Position.Y = probPlayerPostionY;
            CurrentRoom.Field[oldPlayerPostionY, oldPlayerPostionX] = 0;
            CurrentRoom.Field[Position.Y, Position.X] = Id;
            CurrentRoom.ChangePlayerPosition(new Point(oldPlayerPostionX, oldPlayerPostionY),
                Position,
                dictionary);
        }
    }
}


/// <summary>
/// Враг Зомби
/// </summary>
class Zombie : Mob
{
    public Zombie(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 3;
        HP = 5;
        Dexterity = 10;
        Armor = 0;
        Name = "Зомби";
        NameForHitAndMiss = Name;
        NameForBlock = Name;
        Male = true;
        Points = 1;
    }
}

/// <summary>
/// Враг Змея
/// </summary>
class Snake : Mob
{
    public readonly static int Chance = 55; // Шанс появления данного врага
    public Snake(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 3;
        HP = 1;
        Dexterity = 33;
        Armor = 0;
        Name = "Змея";
        NameForHitAndMiss = "Змее";
        NameForBlock = "Змеи";
        Male = false;
        Points = 10;
    }
}

/// <summary>
/// Враг С.М.Е.Р.Т.Ь
/// </summary>
class Death : Mob
{
    public readonly static int Chance = -9; // Шанс появления данного врага
    public Death(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 20;
        HP = 30;
        Dexterity = 50;
        Armor = 12;
        Name = "C.M.E.P.T.Ь";
        NameForHitAndMiss = "C.М.Е.Р.Т.И";
        NameForBlock = NameForHitAndMiss;
        Male = false;
        Points = 1000;
    }
}

/// <summary>
/// Враг Аллигатор
/// </summary>
class Alligator : Mob
{
    public readonly static int Chance = 45; // Шанс появления данного врага
    public Alligator(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 4;
        HP = 5;
        Dexterity = 10;
        Armor = 3;
        Name = "Аллигатор";
        NameForHitAndMiss = "Аллигатору";
        NameForBlock = "Аллигатора";
        Male = true;
        Points = 20;
    }
}

/// <summary>
/// Враг рыцарь
/// </summary>
class Knight : Mob
{
    public readonly static int Chance = 35; // Шанс появления данного врага
    public Knight(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 5;
        HP = 10;
        Dexterity = 0;
        Armor = 2;
        Name = "Рыцарь";
        NameForHitAndMiss = "Рыцарю";
        NameForBlock = "Рыцаря";
        Male = true;
        Points = 50;
    }
}

/// <summary>
/// Враг гоблин
/// </summary>
class Goblin : Mob
{
    public readonly static int Chance = 40; // Шанс появления данного врага
    public Goblin(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 3;
        HP = 8;
        Dexterity = 10;
        Armor = 1;
        Name = "Гоблин";
        NameForHitAndMiss = "Гоблину";
        NameForBlock = "Гоблина";
        Male = true;
        Points = 15;
    }
}