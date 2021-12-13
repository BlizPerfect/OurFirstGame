using System;
using System.Collections.Generic;
using System.Drawing;

class Mob : Person
{
    public bool SawPlayer = false;

    public string Name;
    public string NameForHitAndMiss;
    public string NameForBlock;
    public bool Male;
    public int Points;

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

    public bool CanSeePlayer(Player player)
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
class Snake : Mob
{
    public static int Chance = 55;
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
class Death : Mob
{
    public static int Chance = -9;
    public Death(Point startingPosition, Floor currentFloor, int spawnRoomId, int id) : base(startingPosition, currentFloor, spawnRoomId, id)
    {
        Attack = 15;
        HP = 30;
        Dexterity = 50;
        Armor = 10;
        Name = "C.M.E.P.T.Ь";
        NameForHitAndMiss = "C.М.Е.Р.Т.И";
        NameForBlock = NameForHitAndMiss;
        Male = false;
        Points = 1000;
    }
}
class Alligator : Mob
{
    public static int Chance = 45;
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
class Knight : Mob
{
    public static int Chance = 35;
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
class Goblin : Mob
{
    public static int Chance = 40;
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