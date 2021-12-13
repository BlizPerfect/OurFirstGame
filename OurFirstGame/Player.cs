using System;
using System.Collections.Generic;
using System.Drawing;

class Player : Person
{
    public int Score = 0;
    public Player(Point startingPosition, Floor currentFloor, int spawnRoomId)
    {
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;
        PreviousPosition = Position;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        Id = 8;
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = Id;
        POV = 6;

        Attack = 5;
        HP = 15;
        Dexterity = 50;
        Armor = 2;
    }

    public void Hud(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write("HP:{0,7}", HP);
        Console.SetCursorPosition(x, y + 1);
        Console.Write("ATTACK:{0,3}", Attack);
        Console.SetCursorPosition(x, y + 2);
        Console.Write("ARMOR:{0,4}", Armor);
        Console.SetCursorPosition(130, 40);
        Console.Write("SCORE:{0,4}", Score);
        Console.SetCursorPosition(0, 0);
    }

    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        //PersonDebug(130, 10);
        Hud(130, 10);
        var pressedKey = Console.ReadKey().Key;

        var oldPlayerPostionX = Position.X;
        var oldPlayerPostionY = Position.Y;

        var probPlayerPostionX = Position.X;
        var probPlayerPostionY = Position.Y;

        if (pressedKey.Equals(ConsoleKey.LeftArrow))
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
        else if (pressedKey.Equals(ConsoleKey.RightArrow))
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
        else if (pressedKey.Equals(ConsoleKey.DownArrow))
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
        else if (pressedKey.Equals(ConsoleKey.UpArrow))
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

        if (CheckArrayEnemies(probPlayerPostionX, probPlayerPostionY) &&
            CheckArrayWalls(probPlayerPostionX, probPlayerPostionY))
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
    public void Test(Dictionary<int, string> dictionary)
    {
        for (var dy = -1 * POV; dy <= POV; dy++)
        {
            for (var dx = -1 * POV; dx <= POV; dx++)
            {
                if (CheckArrayLimits(dx, dy))
                {
                    CurrentRoom.ReDrawOneCell(Position.X + dx, Position.Y + dy, dictionary);
                }
            }
        }
    }

    public bool CheckLaddePlacement()
    {
        if (CurrentFloor.Ladder.Position.X == Position.X &&
            CurrentFloor.Ladder.Position.Y == Position.Y &&
            CurrentRoom.RoomId == CurrentFloor.Ladder.RoomIndex)
        {
            return true;
        }
        return false;
    }

    public void ChangeCurrentFloor(Floor newFloor)
    {
        CurrentFloor = newFloor;
        CurrentRoom = newFloor.Rooms[0];
        newFloor.Rooms[0].Field[1, 1] = 8;
        Position.X = 1;
        Position.Y = 1;
        PreviousPosition = Position;
    }
}
