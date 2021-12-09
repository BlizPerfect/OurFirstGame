using System;
using System.Collections.Generic;
using System.Drawing;

class Player : Person
{
    public Player(Point startingPosition, Floor currentFloor, int spawnRoomId)
    {
        Row = startingPosition.Y;
        Column = startingPosition.X;
        GlobalPosition = startingPosition;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = 8;
        Id = 8;
        POV = 6;
    }

    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        PersonDebug(130, 10);
        bool test = false;
        var pressedKey = Console.ReadKey().Key;
        var prevRoom = CurrentRoom;

        PRow = Row;
        PColumn = Column;
        if (pressedKey.Equals(ConsoleKey.LeftArrow))
        {
            if (CheckingLeftWall() && !CheckingPerson(-1, 0))
            {
                Column -= 1;
            }
            else
            {
                if (Row != 0 && Row != CurrentRoom.Rows - 1)
                {
                    for (var i = 0; i < CurrentRoom.Gates.Count; i++)
                    {
                        if (CurrentRoom.Gates[i].GatePosition.Y == Row && CurrentRoom.Gates[i].GatePosition.X == Column)
                        {
                            test = true;
                            Moving(dictionary, i, prevRoom);
                        }
                        else if (CheckingLeftTeleport(i))
                        {
                            Column -= 1;
                        }
                        if (test)
                        {
                            break;
                        }
                    }
                }

            }
        }
        else if (pressedKey == ConsoleKey.RightArrow)
        {
            if (CheckingRightWall() && !CheckingPerson(1, 0))
            {
                Column += 1;
            }
            else
            {
                if (Row != 0 && Row != CurrentRoom.Rows - 1)
                {
                    for (var i = 0; i < CurrentRoom.Gates.Count; i++)
                    {
                        if (CurrentRoom.Gates[i].GatePosition.Y == Row && CurrentRoom.Gates[i].GatePosition.X == Column)
                        {
                            test = true;
                            Moving(dictionary, i, prevRoom);
                        }
                        else if (CheckingRightTeleport(i))
                        {
                            Column += 1;
                        }
                        if (test)
                        {
                            break;
                        }
                    }
                }
            }
        }
        else if (pressedKey.Equals(ConsoleKey.DownArrow))
        {
            if (CheckingDownWall() && !CheckingPerson(0, 1))
            {
                Row += 1;
            }
            else
            {
                if (Column != 0 && Column != CurrentRoom.Columns - 1)
                {
                    for (var i = 0; i < CurrentRoom.Gates.Count; i++)
                    {
                        if (CurrentRoom.Gates[i].GatePosition.Y == Row && CurrentRoom.Gates[i].GatePosition.X == Column)
                        {
                            test = true;
                            Moving(dictionary, i, prevRoom);
                        }
                        else if (CheckingDownTeleport(i))
                        {
                            Row += 1;
                        }
                        if (test)
                        {
                            break;
                        }
                    }
                }
            }
        }
        else if (pressedKey == ConsoleKey.UpArrow)
        {
            if (CheckingUpWall() && !CheckingPerson(0, -1))
            {
                Row -= 1;
            }
            else
            {
                if (Column != 0 && Column != CurrentRoom.Columns - 1)
                {
                    for (var i = 0; i < CurrentRoom.Gates.Count; i++)
                    {
                        if (CurrentRoom.Gates[i].GatePosition.Y == Row && CurrentRoom.Gates[i].GatePosition.X == Column)
                        {
                            test = true;
                            Moving(dictionary, i, prevRoom);
                        }
                        else if (CheckingUpTeleport(i))
                        {
                            Row -= 1;
                        }
                        if (test)
                        {
                            break;
                        }
                    }
                }
            }
        }
        Console.SetCursorPosition(PColumn + CurrentRoom.Position.X, PRow + CurrentRoom.Position.Y);
        CurrentRoom.Field[PRow, PColumn] = 0;
        Console.Write(dictionary[CurrentRoom.Field[PRow, PColumn]]);

        Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
        CurrentRoom.Field[Row, Column] = Id;
        Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

        Console.SetCursorPosition(130, 8);
        Console.Write("Player global position Row:{0,2}", Row + CurrentRoom.Position.Y);
        Console.SetCursorPosition(130, 9);
        Console.Write("Player global position Column:{0,2}", Column + CurrentRoom.Position.X);

        Console.SetCursorPosition(0, 0);
    }

    public (bool, Point) StartCombat()
    {
        for (var dy = -1; dy <= 1; dy++)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                if (CheckingEnemy(dx, dy))
                {
                    return (true, new Point(dx, dy));
                }
            }
        }
        return (false, new Point(-1, -1));
    }
}
