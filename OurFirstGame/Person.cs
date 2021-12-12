using System;
using System.Collections.Generic;
using System.Drawing;

abstract class Person
{
    public Point Position;
    public Point GlobalPosition;
    public Floor CurrentFloor;
    public Room CurrentRoom;

    public int Id;
    public int HP = 200;
    public int Attack = 100;
    public int POV;

    public void ChangeCurrentRoom(Room newCurrentRoom)
    {
        CurrentRoom = newCurrentRoom;
    }

    public void Teleporting(int x, int y, Dictionary<int, string> dictionary)
    {
        var gate = CurrentRoom.Gates.Find(x => x.GatePosition.Equals(Position));

        var nextGate = CurrentFloor.Rooms[gate.NextRoomIndex].Gates.Find(x => x.GateId == gate.NextGateId);

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

    public bool isDead()
    {
        return HP <= 0;
    }
    public void Decomposition(Floor floor, Dictionary<int, string> dictionary)
    {
        //floor.Rooms[CurrentRoom.RoomId].Field[Row, Position.X] = 0;
        CurrentRoom.ReDrawRoom(dictionary);
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

    public abstract void Move(Dictionary<int, string> dictionary);

    public bool CheckingPerson(int x, int y)
    {
        return (CheckingPlayer(x, y) || CheckingEnemy(x, y));
    }
    public bool CheckingEnemy(int x, int y)
    {
        if (CurrentRoom.Position.X <= Position.X + x || 0 > Position.X + x)
        {
            x = 0;
        }
        else if (CurrentRoom.Rows <= Position.Y + y || 0 > Position.Y + y)
        {
            y = 0;
        }
        return Mob.MobsList.Contains(CurrentRoom.Field[Position.Y + y, Position.X + x]);
    }
    public bool CheckingPlayer(int x, int y)
    {
        return CurrentRoom.Field[Position.Y + y, Position.X + x] == 8;
    }

    public bool CheckingRightWall()
    {
        return (Position.X < CurrentRoom.Columns - 2 && (Position.Y < CurrentRoom.Rows - 1 && Position.Y > 0));
    }

    public bool CheckingLeftWall()
    {
        return (Position.X > 1 && (Position.Y < CurrentRoom.Rows - 1 && Position.Y > 0));
    }

    public bool CheckingDownWall()
    {
        return (Position.Y < CurrentRoom.Rows - 2 && (Position.X > 0 && Position.X < CurrentRoom.Columns - 1));
    }
    public bool CheckingUpWall()
    {
        return (Position.Y > 1 && (Position.X > 0 && Position.X < CurrentRoom.Columns - 1));
    }
    public bool CheckingLeftTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Position.Y && CurrentRoom.Gates[i].GatePosition.X == Position.X - 1);
    }
    public bool CheckingRightTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Position.Y && CurrentRoom.Gates[i].GatePosition.X == Position.X + 1);
    }
    public bool CheckingDownTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Position.Y + 1 && CurrentRoom.Gates[i].GatePosition.X == Position.X);
    }
    public bool CheckingUpTeleport(int i)
    {
        return (CurrentRoom.Gates[i].GatePosition.Y == Position.Y - 1 && CurrentRoom.Gates[i].GatePosition.X == Position.X);
    }

    public bool CheckArrayLimits(int dx, int dy)
    {
        return (Position.X + dx >= 0 &&
            Position.X + dx < CurrentRoom.Columns &&
            Position.Y + dy >= 0 &&
            Position.Y + dy < CurrentRoom.Rows);
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

    public bool CheckGate(int x, int y)
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
