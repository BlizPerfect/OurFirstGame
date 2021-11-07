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

    public void ChangeCurrentRoom(Room newCurrentRoom)
    {
        CurrentRoom = newCurrentRoom;
    }

    public void Move(Dictionary<int, string> dictionary)
    {
        var pressedKey = Console.ReadKey().Key;
        int previousPlayerRow = Row;
        int previousPlayerCol = Column;
        if (pressedKey.Equals(ConsoleKey.LeftArrow))
        {
            if (Column > 1)
            {
                Column -= 1;
            }
        }
        else if (pressedKey == ConsoleKey.RightArrow)
        {
            if (CurrentRoom.Gate.GatePosition.Y == Row && CurrentRoom.Gate.GatePosition.X == Column)
            {
                ChangeCurrentRoom(CurrentFloor.Rooms[CurrentRoom.RoomId + 1]);
                Row = CurrentRoom.Gate.GatePosition.Y;
                Column = CurrentRoom.Gate.GatePosition.X;
                previousPlayerRow = Row;
                previousPlayerCol = Column;
            }
            else if (Column < CurrentRoom.Columns - 2 && CurrentRoom.Field[Row, Column] != 9)
            {
                Column += 1;
            }
            else if (CurrentRoom.Field[Row, Column + 1] == 9)
            {
                Column += 1;
            }
        }
        else if (pressedKey.Equals(ConsoleKey.DownArrow))
        {
            if (Row < CurrentRoom.Rows - 2)
            {
                Row += 1;
            }
        }
        else if (pressedKey == ConsoleKey.UpArrow)
        {
            if (Row > 1)
            {
                Row -= 1;
            }
        }
        if (Column != previousPlayerCol || Row != previousPlayerRow)
        {
            Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
            CurrentRoom.Field[Row, Column] = 8;
            Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);

            Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
            CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;
            Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);
            Console.SetCursorPosition(0, 0);
        }
    }
}
