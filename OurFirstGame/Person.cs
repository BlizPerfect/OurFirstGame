using System;
using System.Collections.Generic;
using System.Drawing;

abstract class Person
{
    public Point Position;
    public Point PreviousPosition;
    public Floor CurrentFloor;
    public Room CurrentRoom;


    public int Id;
    public int HP;
    public int Attack;
    public int Armor;
    public int Dexterity;
    public int POV;

    public void ChangeCurrentRoom(Room newCurrentRoom)
    {
        CurrentRoom = newCurrentRoom;
    }

    public void Teleporting(int x, int y, Dictionary<int, string> dictionary)
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

    public bool isDead()
    {
        return HP <= 0;
    }
    public void Decomposition(Dictionary<int, string> dictionary)
    {
        CurrentFloor.Rooms[CurrentRoom.RoomId].Field[Position.Y, Position.X] = 0;
        CurrentRoom.ReDrawOneCell(Position.X, Position.Y, dictionary);
    }

    public void PersonDebug(int x, int y)
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

    public abstract void Move(Dictionary<int, string> dictionary, Player player);

    public bool CheckArrayLimits(int dx, int dy)
    {
        return (Position.X + dx >= 0 &&
            Position.X + dx < CurrentRoom.Columns &&
            Position.Y + dy >= 0 &&
            Position.Y + dy < CurrentRoom.Rows);
    }

    public bool CheckGateExit(Room nextRoom, Gate nextGate)
    {
        return !Floor.Enemies.Contains(nextRoom.Field[nextGate.GatePosition.Y, nextGate.GatePosition.X]) &&
            nextRoom.Field[nextGate.GatePosition.Y, nextGate.GatePosition.X] != 8;
    }

    public bool CheckArrayEnemies(int x, int y)
    {
        return !Floor.Enemies.Contains(CurrentRoom.Field[y, x]);
    }
    public bool CheckArrayPlayer(int x, int y)
    {
        return CurrentRoom.Field[y, x] == 8;
    }
    public bool CheckArrayWalls(int x, int y)
    {
        return !Floor.Walls.Contains(CurrentRoom.Field[y, x]);
    }
    public bool CheckArrayWallsExtended(int x, int y)
    {
        return !Floor.WallsExtended.Contains(CurrentRoom.Field[y, x]);
    }

    public bool CheckGatePlacement(int x, int y)
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
