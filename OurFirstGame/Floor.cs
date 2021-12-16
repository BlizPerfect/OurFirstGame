using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Сущность этажа.
/// </summary>
class Floor
{
    public int FloorLevel { private set; get; } // Текущая сложность уровня
    private Random Rnd = new Random();
    public int RoomCount; // Число комнат на уровне
    public List<Room> Rooms { private set; get; } // Список комнат уровня
    public List<Mob> Mobs { private set; get; } // Список врагов на уровне
    public List<Mob> Graveyard { private set; get; } // Список мобов, которые умерли, а следовательно должны быть удалены из Mobs
    public List<Chest> Chests { private set; get; } // Список сундуков на уровне

    public Ladder Ladder { private set; get; } // Сущность лестницы на уровне

    public readonly static List<int> Walls = new List<int> { 2, 3, 4, 5, 6, 7 }; //Список обозначений стен

    //Расширенный список обозначений стен
    public readonly static List<int> WallsExtended = new List<int> { 2, 3, 4, 5, 6, 7, 98, 99 };

    //Список обозначений врагов
    public readonly static List<int> Enemies = new List<int> { 10, 11, 12, 13, 14, 15 };

    public Floor(int roomCount, int floorLevel)
    {
        FloorLevel = floorLevel;
        RoomCount = roomCount;
        Rooms = new List<Room>();
        Graveyard = new List<Mob>();
        Mobs = new List<Mob>();
        Chests = new List<Chest>();
        CreateFloor();
        PlaceLadder();
        PlaceMobs(66);
        PlaceLoot(20);
    }

    /// <summary>
    /// Очистка Graveyard.
    /// </summary>
    /// <returns>void</returns>
    public void RefreshGraveyard()
    {
        Graveyard = new List<Mob>();
    }

    /// <summary>
    /// Расположение, с шаносом равным chance, сундуков в каждой комнате уровня.
    /// </summary>
    /// <returns>void</returns>
    private void PlaceLoot(int chance)
    {
        foreach (var room in Rooms)
        {
            if (room.RoomId == 0)
            {
                continue;
            }
            if (Rnd.Next(100) < chance)
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

    /// <summary>
    /// Расположение, с шаносом равным mainChance, врагов в каждой комнате уровня.
    /// </summary>
    /// <returns>void</returns>
    private void PlaceMobs(int mainChance)
    {
        foreach (var room in Rooms)
        {
            if (room.RoomId == 0)
            {
                continue;
            }
            if (Rnd.Next(100) < mainChance)
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
                else if (chance + FloorLevel < Goblin.Chance)
                {
                    Mobs.Add(new Goblin(tempPosition, this, room.RoomId, 15));
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

    /// <summary>
    /// Расположение лестницы в последней комнате уровня.
    /// </summary>
    /// <returns>void</returns>
    private void PlaceLadder()
    {
        Rooms[RoomCount - 1].Field[2, 5] = 99;
        Ladder = new Ladder(new Point(5, 2), RoomCount - 1);
    }

    /// <summary>
    /// Расположение переходов между комнатами внутри уровня.
    /// </summary>
    /// <returns>void</returns>
    private void CreateGate(Room room1, Room room2, bool isHorizontal, int gateIndex)
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

    /// <summary>
    /// Полное создание географии уровня.
    /// </summary>
    /// <returns>void</returns>
    private void CreateFloor()
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

    /// <summary>
    /// Полная отрисовка уровня.
    /// </summary>
    /// <returns>void</returns>
    public void ShowMap(Dictionary<int, string> dictionary)
    {
        foreach (var room in Rooms)
        {
            room.ReDrawRoom(dictionary);
        }
    }
}
