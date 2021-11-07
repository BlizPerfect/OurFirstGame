using System;
using System.Collections.Generic;
using System.Drawing;

class Floor
{
    public Random Rnd = new Random();
    public int RoomCount = 10;
    public List<Room> Rooms;

    public Floor()
    {
        //RoomCount = Rnd.Next(6, 11);
        Rooms = new List<Room>();
        CreateFloor();
    }

    public void CreateGate(Room room1, Room room2, int room2ID, bool isHorizontal)
    {
        if (isHorizontal)
        {
            var index = Rnd.Next(2, room1.Rows - 2);
            while (index > room2.Rows - 3)
            {
                index -= 1;
            }
            room1.Field[index - 1, room1.Columns - 1] = 5;
            room1.Field[index, room1.Columns - 1] = 0;
            room1.Field[index + 1, room1.Columns - 1] = 2;
            room2.Field[index - 1, 0] = 6;
            room2.Field[index, 0] = 0;
            room2.Field[index + 1, 0] = 3;
            room1.Gate = new Gate(new Point(room1.Columns - 1, index), room2ID);
            room2.Gate = new Gate(new Point(0, index), room2ID);
        }
        else
        {
            var index = Rnd.Next(2, room1.Columns - 2);
            while (index > room2.Columns - 3)
            {
                index -= 1;
            }
            room1.Field[room1.Rows - 1, index - 1] = 3;
            room1.Field[room1.Rows - 1, index] = 0;
            room1.Field[room1.Rows - 1, index + 1] = 2;
            room2.Field[0, index - 1] = 6;
            room2.Field[0, index] = 0;
            room2.Field[0, index + 1] = 5;
            room1.Gate = new Gate(new Point(index, room1.Rows - 1), room2ID);
            room2.Gate = new Gate(new Point(index, 0), room2ID);
        }
        room1.Field[room1.Gate.GatePosition.Y, room1.Gate.GatePosition.X] = 9;
        room2.Field[room2.Gate.GatePosition.Y, room2.Gate.GatePosition.X] = 9;
    }

    public void CreateFloor()
    {
        int id = 0;
        for (var i = 0; i < RoomCount; i++)
        {
            int rows = Rnd.Next(5, 7);
            int cols = Rnd.Next(15, 20);
            int newColumnPostition;
            int newRowPosition;
            if (Rooms.Count == 0)
            {
                Rooms.Add(new Room(rows, cols, new Point(0, 0), true, i));
            }
            else
            {
                if (Rnd.Next(100) < 50)
                {
                    if (Rooms[i - 1].RightColumn < 90)
                    {
                        newColumnPostition = Rooms[i - 1].Position.X + Rooms[i - 1].Columns;
                        newRowPosition = Rooms[i - 1].Position.Y + Rnd.Next(0, Rooms[i - 1].Rows) / 8;
                        Rooms[i - 1].HorizontalConection = true;
                    }
                    else
                    {
                        newColumnPostition = Rooms[i - 1].Position.X + Rnd.Next(0, Rooms[i - 1].Columns) / 35;
                        newRowPosition = Rooms[i - 1].Position.Y + Rooms[i - 1].Rows;
                        Rooms[i - 1].HorizontalConection = false;
                    }
                }
                else
                {
                    if (Rooms[i - 1].BottomRow < 32)
                    {
                        newColumnPostition = Rooms[i - 1].Position.X + Rnd.Next(0, Rooms[i - 1].Columns) / 35;
                        newRowPosition = Rooms[i - 1].Position.Y + Rooms[i - 1].Rows;
                        Rooms[i - 1].HorizontalConection = false;
                    }
                    else
                    {
                        newColumnPostition = Rooms[i - 1].Position.X + Rooms[i - 1].Columns;
                        newRowPosition = Rooms[i - 1].Position.Y + Rnd.Next(0, Rooms[i - 1].Rows) / 8;
                        Rooms[i - 1].HorizontalConection = true;
                    }
                }
                Rooms.Add(new Room(rows, cols, new Point(newColumnPostition, newRowPosition), false, i));
                if (Rooms.Count > 0 && Rooms.Count < RoomCount + 1)
                {
                    if (Rooms[i - 1].HorizontalConection)
                    {
                        CreateGate(Rooms[i - 1], Rooms[i], i - 1, true);
                    }
                    else
                    {
                        CreateGate(Rooms[i - 1], Rooms[i], i, false);

                    }
                    id += 1;
                }
            }
        }
    }

    public bool Intersect(Room room1, Room room2)
    {
        //a.x room1.Position.X
        //room1.Position.X1 room1.RightColumn
        //a.y room1.Position.Y
        //a.y1 room1.BottomRow
        //b.x room2.Position.X
        //b.x1 room2.RightColumn
        //b.y room2.Position.Y
        //b.y1 room2.BottomRow
        return (
            (((room1.Position.X > room2.Position.X && room1.Position.X < room2.RightColumn) || (room1.RightColumn > room2.Position.X && room1.RightColumn < room2.RightColumn)
      ) && ((room1.Position.Y > room2.Position.Y && room1.Position.Y < room2.BottomRow) || (room1.BottomRow > room2.Position.Y && room1.BottomRow < room2.BottomRow)))
      || (((room2.Position.X > room1.Position.X && room2.Position.X < room1.RightColumn) || (room2.RightColumn > room1.Position.X && room2.RightColumn < room1.RightColumn)
      ) && ((room2.Position.Y > room1.Position.Y && room2.Position.Y < room1.BottomRow) || (room2.BottomRow > room1.Position.Y && room2.BottomRow < room1.BottomRow)
      ))) || ((((room1.Position.X > room2.Position.X && room1.Position.X < room2.RightColumn) || (room1.RightColumn > room2.Position.X && room1.RightColumn < room2.RightColumn)
      ) && ((room2.Position.Y > room1.Position.Y && room2.Position.Y < room1.BottomRow) || (room2.BottomRow > room1.Position.Y && room2.BottomRow < room1.BottomRow)
      )) || (((room2.Position.X > room1.Position.X && room2.Position.X < room1.RightColumn) || (room2.RightColumn > room1.Position.X && room2.RightColumn < room1.RightColumn)
      ) && ((room1.Position.Y > room2.Position.Y && room1.Position.Y < room2.BottomRow) || (room1.BottomRow > room2.Position.Y && room1.BottomRow < room2.BottomRow)
      )));
    }

    public void ShowMap(Dictionary<int, string> dictionary)
    {
        foreach (var room in Rooms)
        {
            //Console.ReadLine();
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Columns; j++)
                {
                    Console.SetCursorPosition(room.Position.X + j, room.Position.Y + i);
                    Console.Write(dictionary[room.Field[i, j]]);
                }
                Console.WriteLine();
            }
        }
    }
}
