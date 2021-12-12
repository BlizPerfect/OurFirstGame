using System;
using System.Collections.Generic;
using System.Drawing;

class Player : Person
{
    public Player(Point startingPosition, Floor currentFloor, int spawnRoomId)
    {
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;
        PreviousPosition = Position;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = 8;
        Id = 8;
        POV = 6;
    }

    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        PersonDebug(130, 10);
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
            CurrentRoom.Field[Position.Y, Position.X] = 8;
            CurrentRoom.ChangePlayerPosition(new Point(oldPlayerPostionX, oldPlayerPostionY),
                Position,
                dictionary);
        }
    }


}
