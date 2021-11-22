using System;
using System.Collections.Generic;
using System.Drawing;

class Player
{
    public int Row;
    public int Column;
    public Floor CurrentFloor;
    public Room CurrentRoom;

    public Player(Point startingPosition, Floor currentFloor)
    {
        Row = startingPosition.Y;
        Column = startingPosition.X;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[0];
    }

    public void PlayerDebug()
    {
        Console.SetCursorPosition(130, 10);
        Console.Write("Current room ID: " + CurrentRoom.RoomId);
        Console.SetCursorPosition(130, 11);
        Console.Write("Player Row:{0,2}", Row);
        Console.SetCursorPosition(130, 12);
        Console.Write("Player Column:{0,2}", Column);
        var ind = 12;
        foreach (var nextRoomId in CurrentRoom.Gates)
        {
            ind += 1;
            Console.SetCursorPosition(130, ind);
            Console.Write("{0,2}", nextRoomId.NextRoomIndex);
        }
        Console.SetCursorPosition(0, 0);
    }

    public void ChangeCurrentRoom(Room newCurrentRoom)
    {
        CurrentRoom = newCurrentRoom;
    }

    public void Move(Dictionary<int, string> dictionary)
    {
        PlayerDebug();
        bool test = false;
        var pressedKey = Console.ReadKey().Key;
        var prevRoom = CurrentRoom;
        int previousPlayerRow = Row;
        int previousPlayerCol = Column;

        if (pressedKey.Equals(ConsoleKey.LeftArrow))
        {
            if (CheckingLeftWall())
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
                            Console.SetCursorPosition(130, 6);
                            Console.Write("i:{0,2}", prevRoom.Gates[i].NextGateId);

                            CurrentRoom.Field[Row, Column] = 0;
                            Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
                            Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

                            Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
                            CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;
                            Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);

                            ChangeCurrentRoom(CurrentFloor.Rooms[CurrentRoom.Gates[i].NextRoomIndex]);

                            Row = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.Y;
                            Column = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.X;

                            CurrentRoom.Field[Row, Column] = 8;

                            previousPlayerRow = Row;
                            previousPlayerCol = Column;
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
            if (CheckingRightWall())
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

                            CurrentRoom.Field[Row, Column] = 0;
                            Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
                            Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

                            Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
                            CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;
                            Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);

                            ChangeCurrentRoom(CurrentFloor.Rooms[CurrentRoom.Gates[i].NextRoomIndex]);

                            Row = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.Y;
                            Column = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.X;

                            CurrentRoom.Field[Row, Column] = 8;

                            previousPlayerRow = Row;
                            previousPlayerCol = Column;
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
            if (CheckingDownWall())
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

                            CurrentRoom.Field[Row, Column] = 0;
                            Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
                            Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

                            Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
                            CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;
                            Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);

                            ChangeCurrentRoom(CurrentFloor.Rooms[CurrentRoom.Gates[i].NextRoomIndex]);

                            Row = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.Y;
                            Column = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.X;

                            CurrentRoom.Field[Row, Column] = 8;

                            previousPlayerRow = Row;
                            previousPlayerCol = Column;
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
            if (CheckingUpWall())
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

                            CurrentRoom.Field[Row, Column] = 0;
                            Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
                            Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

                            Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
                            CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;
                            Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);

                            ChangeCurrentRoom(CurrentFloor.Rooms[CurrentRoom.Gates[i].NextRoomIndex]);

                            Row = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.Y;
                            Column = CurrentRoom.Gates[prevRoom.Gates[i].NextGateId].GatePosition.X;

                            CurrentRoom.Field[Row, Column] = 8;

                            previousPlayerRow = Row;
                            previousPlayerCol = Column;
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
        //После реализации всех телепортов убрать этот блок
        Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
        CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;
        Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);

        Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
        CurrentRoom.Field[Row, Column] = 8;
        Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

        Console.SetCursorPosition(130, 8);
        Console.Write("Player global position Row:{0,2}", Row + CurrentRoom.Position.Y);
        Console.SetCursorPosition(130, 9);
        Console.Write("Player global position Column:{0,2}", Column + CurrentRoom.Position.X);

        Console.SetCursorPosition(0, 0);

    }
    public bool CheckingRightWall()
    {
        return (Column < CurrentRoom.Columns - 2 && (Row < CurrentRoom.Rows - 1 && Row > 0));
    }
    public bool CheckingLeftWall()
    {
        return (Column > 1 && (Row < CurrentRoom.Rows - 1 && Row > 0));
    }
    public bool CheckingDownWall()
    {
        return (Row < CurrentRoom.Rows - 2 && (Column > 0 && Column < CurrentRoom.Columns - 1));
    }
    public bool CheckingUpWall()
    {
        return (Row > 1 && (Column > 0 && Column < CurrentRoom.Columns - 1));
    }
    public bool CheckingLeftTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Row && CurrentRoom.Gates[i].GatePosition.X == Column - 1);
    }
    public bool CheckingRightTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Row && CurrentRoom.Gates[i].GatePosition.X == Column + 1);
    }
    public bool CheckingDownTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Row + 1 && CurrentRoom.Gates[i].GatePosition.X == Column);
    }
    public bool CheckingUpTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Row - 1 && CurrentRoom.Gates[i].GatePosition.X == Column);
    }
}
