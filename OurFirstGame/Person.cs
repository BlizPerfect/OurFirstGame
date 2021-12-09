using System;
using System.Collections.Generic;

abstract class Person
{
    public int Row;
    public int Column;
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


    public bool isDead()
    {
        return HP <= 0;
    }
    public void Decomposition(Floor floor, Dictionary<int, string> dictionary)
    {
        floor.Rooms[CurrentRoom.RoomId].Field[Row, Column] = 0;
        CurrentRoom.ReDrawRoom(dictionary);
    }

    public void PersonDebug(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write("Current room ID: " + CurrentRoom.RoomId);
        Console.SetCursorPosition(x, y + 1);
        Console.Write("Person Row:{0,2}", Row);
        Console.SetCursorPosition(x, y + 2);
        Console.Write("Person Column:{0,2}", Column);
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

    public bool CheckingPerson(int x, int y)
    {
        return (CheckingEnemy(x, y) || CheckingPlayer(x, y));
    }
    public bool CheckingEnemy(int x, int y)
    {
        if (CurrentRoom.Columns <= Column + x || 0 > Column + x)
        {
            x = 0;
        }
        else if (CurrentRoom.Rows <= Row + y || 0 > Row + y)
        {
            y = 0;
        }
        return Mob.MobsList.Contains(CurrentRoom.Field[Row + y, Column + x]);
    }
    public bool CheckingPlayer(int x, int y)
    {
        return CurrentRoom.Field[Row + y, Column + x] == 8;
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
