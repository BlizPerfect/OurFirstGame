using System;
using System.Collections.Generic;
using System.Drawing;


/// <summary>
/// Абстрактная сущность существо
/// </summary>
abstract class Person
{
    public Point Position; // Позиция существа в комнате
    public Point PreviousPosition; // Предыдущая позиция существа в комнате
    public Floor CurrentFloor; // Текущий этаж для существа
    public Room CurrentRoom; // Текущая комната, в котором существо находится

    public int Id; // Идентификатор существа
    public int HP; // Здровье существа
    public int Attack; // Атака существа
    public int Armor; // Броня существа
    public int Dexterity; // Шанс существа уклонениться от атаки
    public int POV; // Дальность зрения существа

    /// <summary>
    /// Смена текущей комнаты
    /// </summary>
    /// <returns>void</returns>
    protected void ChangeCurrentRoom(Room newCurrentRoom)
    {
        CurrentRoom = newCurrentRoom;
    }

    /// <summary>
    /// Перемещение персонажа между комнатами
    /// </summary>
    /// <returns>void</returns>
    protected void Teleporting(int x, int y, Dictionary<int, string> dictionary)
    {
        var gate = CurrentRoom.Gates.Find(x => x.GatePosition.Equals(Position));
        var nextGate = CurrentFloor.Rooms[gate.NextRoomIndex].Gates.Find(x => x.GateId == gate.NextGateId);
        var nextRoom = CurrentFloor.Rooms[gate.NextRoomIndex];
        if (CheckGateExit(nextRoom, nextGate))
        {
            PreviousPosition = Position;

            Position.X = nextGate.GatePosition.X;
            Position.Y = nextGate.GatePosition.Y;

            CurrentRoom.Field[y, x] = 0;
            CurrentRoom.ReDrawOneCell(x, y, dictionary);
            ChangeCurrentRoom(CurrentFloor.Rooms[gate.NextRoomIndex]);
            CurrentRoom.Field[Position.Y, Position.X] = Id;
            CurrentRoom.ChangePlayerPosition(new Point(Position.X, Position.Y),
                Position,
                dictionary);
        }
    }

    /// <summary>
    /// Проверка, мертво ли существо
    /// </summary>
    /// <returns>bool</returns>
    public bool isDead()
    {
        return HP <= 0;
    }

    /// <summary>
    /// Удаление существа с экрана
    /// </summary>
    /// <returns>void</returns>
    public void Decomposition(Dictionary<int, string> dictionary)
    {
        CurrentFloor.Rooms[CurrentRoom.RoomId].Field[Position.Y, Position.X] = 0;
        CurrentRoom.ReDrawOneCell(Position.X, Position.Y, dictionary);
    }

    /// <summary>
    /// Использовалось при разработке, в данный момент не нужно
    /// </summary>
    /// <returns>void</returns>
    private void PersonDebug(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write("Current room ID: " + CurrentRoom.RoomId);
        Console.SetCursorPosition(x, y + 1);
        Console.Write("Person Row:{0,2}", Position.Y);
        Console.SetCursorPosition(x, y + 2);
        Console.Write("Person Position.X:{0,2}", Position.X);
        var ind = y + 2;
        foreach (var nextRoomId in CurrentRoom.Gates)
        {
            ind += 1;
            Console.SetCursorPosition(x, ind);
            Console.Write("{0,2}", nextRoomId.NextRoomIndex);
        }
        Console.SetCursorPosition(0, 0);
    }

    /// <summary>
    /// Передвижение существа
    /// </summary>
    /// <returns>void</returns>
    public abstract void Move(Dictionary<int, string> dictionary, Player player);

    /// <summary>
    /// Проверка, находтся ли существа в пределах комнаты
    /// </summary>
    /// <returns>bool</returns>
    protected bool CheckArrayLimits(int dx, int dy)
    {
        return (Position.X + dx >= 0 &&
            Position.X + dx < CurrentRoom.Columns &&
            Position.Y + dy >= 0 &&
            Position.Y + dy < CurrentRoom.Rows);
    }

    /// <summary>
    /// Проверка, находтся ли существа в пределах комнаты
    /// </summary>
    /// <returns>bool</returns>
    private bool CheckGateExit(Room nextRoom, Gate nextGate)
    {
        return !Floor.Enemies.Contains(nextRoom.Field[nextGate.GatePosition.Y, nextGate.GatePosition.X]) &&
            nextRoom.Field[nextGate.GatePosition.Y, nextGate.GatePosition.X] != 8;
    }

    /// <summary>
    /// Проверка, не находится ли по координатам x и y враг
    /// </summary>
    /// <returns>bool</returns>
    protected bool CheckArrayEnemies(int x, int y)
    {
        return !Floor.Enemies.Contains(CurrentRoom.Field[y, x]);
    }

    /// <summary>
    /// Проверка, не находится ли по координатам x и y игрок
    /// </summary>
    /// <returns>bool</returns>
    protected bool CheckArrayPlayer(int x, int y)
    {
        return CurrentRoom.Field[y, x] == 8;
    }

    /// <summary>
    /// Проверка, не находится ли по координатам x и y стена
    /// </summary>
    /// <returns>bool</returns>
    protected bool CheckArrayWalls(int x, int y)
    {
        return !Floor.Walls.Contains(CurrentRoom.Field[y, x]);
    }

    /// <summary>
    /// Проверка, не находится ли по координатам x и y стена
    /// </summary>
    /// <returns>bool</returns>
    public bool CheckArrayWallsExtended(int x, int y)
    {
        return !Floor.WallsExtended.Contains(CurrentRoom.Field[y, x]);
    }

    /// <summary>
    /// Проверка, не находится ли по координатам x и y врата
    /// </summary>
    /// <returns>bool</returns>
    protected bool CheckGatePlacement(int x, int y)
    {
        foreach (var gate in CurrentRoom.Gates)
        {
            if (gate.GatePosition.X == x && gate.GatePosition.Y == y)
            {
                return true;
            }
        }
        return false;
    }
}
