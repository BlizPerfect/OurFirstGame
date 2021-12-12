using System;
using System.Collections.Generic;
using System.Drawing;

class Mob : Person
{
    public static List<int> MobsList = new List<int> { 10, 11 };
    public static List<int> MoveSidesList = new List<int> { 0,//Вверх
        1,//Вправо
        2,//Вниз
        3//Влево
         };

    public bool SawPlayer = false;
    public Mob(Point startingPosition, Floor currentFloor, int spawnRoomId, int id)
    {
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;

        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = 10;
        Id = id;
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

    public override void Move(Dictionary<int, string> dictionary)
    {
        // 0
        //3 1
        // 2
        var rnd = new Random();
        var pressedKey = rnd.Next(0, 4);

        var oldPlayerPostionX = Position.X;
        var oldPlayerPostionY = Position.Y;

        var probPlayerPostionX = Position.X;
        var probPlayerPostionY = Position.Y;

        if (pressedKey.Equals(3))
        {
            if (CheckArrayLimits(-1, 0))
            {
                probPlayerPostionX -= 1;
            }
            else if (CheckGate(Position.X, Position.Y))
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
            else if (CheckGate(Position.X, Position.Y))
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
            else if (CheckGate(Position.X, Position.Y))
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
            else if (CheckGate(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }

        if (!CheckArrayPlayer(probPlayerPostionX, probPlayerPostionY) && CheckArrayEnemies(probPlayerPostionX, probPlayerPostionY) &&
            CheckArrayWalls(probPlayerPostionX, probPlayerPostionY))
        {
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