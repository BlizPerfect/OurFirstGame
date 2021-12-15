﻿using System;
using System.Collections.Generic;
using System.Drawing;

class Floor
{
    public int FloorLevel;
    public Random Rnd = new Random();
    public int RoomCount;
    public List<Room> Rooms;
    public List<Mob> Mobs = new List<Mob>();
    public List<Mob> Graveyard = new List<Mob>();
    public List<Chest> Chests = new List<Chest>();

    public Ladder Ladder;

    public static List<int> Walls = new List<int> { 2, 3, 4, 5, 6, 7 };
    public static List<int> WallsExtended = new List<int> { 2, 3, 4, 5, 6, 7, 98, 99 };
    public static List<int> Enemies = new List<int> { 10, 11, 12, 13, 14, 15 };

    public Floor(int roomCount, int floorLevel)
    {
        FloorLevel = floorLevel;
        RoomCount = roomCount;
        Rooms = new List<Room>();
        CreateFloor();
        PlaceLadder();
        PlaceMobs();
        PlaceLoot();
    }

    public void PlaceLoot()
    {
        foreach (var room in Rooms)
        {
            if (room.RoomId == 0)
            {
                continue;
            }
            if (Rnd.Next(100) < 33)
            {
                var tempPosition = new Point(Rnd.Next(1, room.Columns - 2), Rnd.Next(1, room.Rows - 2));
                while (room.Field[tempPosition.Y, tempPosition.X] == 1)
                {
                    tempPosition = new Point(Rnd.Next(1, room.Columns - 2), Rnd.Next(1, room.Rows - 2));
                }
                Chests.Add(new Chest(tempPosition, room.RoomId, this));
            }
        }
    }

    private void PlaceMobs()
    {
        foreach (var room in Rooms)
        {
            if (room.RoomId == 0)
            {
                continue;
            }
            if (Rnd.Next(100) < 66)
            {
                var chance = Rnd.Next(100);
                var tempPosition = new Point(Rnd.Next(1, room.Columns - 2), Rnd.Next(1, room.Rows - 2));
                while (room.Field[tempPosition.Y, tempPosition.X] == 1)
                {
                    tempPosition = new Point(Rnd.Next(1, room.Columns - 2), Rnd.Next(1, room.Rows - 2));
                }
                if (chance + FloorLevel < Death.Chance)
                {
                    Mobs.Add(new Death(tempPosition, this, room.RoomId, 12));
                }
                else if (chance + FloorLevel < Knight.Chance)
                {
                    Mobs.Add(new Knight(tempPosition, this, room.RoomId, 14));
                }
                else if (chance + FloorLevel < Alligator.Chance)
                {
                    Mobs.Add(new Alligator(tempPosition, this, room.RoomId, 13));
                }
                else if (chance + FloorLevel < Snake.Chance)
                {
                    Mobs.Add(new Snake(tempPosition, this, room.RoomId, 11));
                }
                else
                {
                    Mobs.Add(new Zombie(tempPosition, this, room.RoomId, 10));
                }
            }
        }
    }

    public void PlaceLadder()
    {
        Rooms[RoomCount - 1].Field[2, 5] = 99;
        Ladder = new Ladder(new Point(5, 2), RoomCount - 1);
    }

    public void CreateGate(Room room1, Room room2, bool isHorizontal, int gateIndex)
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
            room1.Gates.Add(new Gate(new Point(room1.Columns - 1, index), room2.RoomId, gateIndex, gateIndex));
            room2.Gates.Add(new Gate(new Point(0, index), room1.RoomId, gateIndex, gateIndex));
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
            room1.Gates.Add(new Gate(new Point(index, room1.Rows - 1), room2.RoomId, gateIndex, gateIndex));
            room2.Gates.Add(new Gate(new Point(index, 0), room1.RoomId, gateIndex, gateIndex));

        }
        room1.Field[room1.Gates[room1.Gates.Count - 1].GatePosition.Y, room1.Gates[room1.Gates.Count - 1].GatePosition.X] = 9;
        room2.Field[room2.Gates[room2.Gates.Count - 1].GatePosition.Y, room2.Gates[room2.Gates.Count - 1].GatePosition.X] = 9;
    }

    public void CreateFloor()
    {
        for (var i = 0; i < RoomCount; i++)
        {
            int rows = Rnd.Next(5, 7);
            int cols = Rnd.Next(15, 20);
            int newColumnPostition;
            int newRowPosition;
            if (Rooms.Count == 0)
            {
                Rooms.Add(new Room(rows, cols, new Point(0, 0), i));
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
                Rooms.Add(new Room(rows, cols, new Point(newColumnPostition, newRowPosition), i));
                if (Rooms.Count > 0 && Rooms.Count < RoomCount + 1)
                {
                    if (Rooms[i - 1].HorizontalConection)
                    {
                        CreateGate(Rooms[i - 1], Rooms[i], true, i);
                    }
                    else
                    {
                        CreateGate(Rooms[i - 1], Rooms[i], false, i);
                    }
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
            room.ReDrawRoom(dictionary);
        }
    }

    public void ChangePersonPosition(Person person, int dx, int dy)
    {
        var oldX = person.Position.X - dx;
        var oldY = person.Position.Y - dy;
        person.CurrentRoom.Field[oldX, oldY] = 0;
        person.CurrentRoom.Field[person.Position.X, person.Position.Y] = 8;
    }

}
