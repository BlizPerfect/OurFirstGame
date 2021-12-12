using System;
using System.Collections.Generic;
using System.Drawing;

class Mob : Person
{
    public bool SawPlayer = false;

    public Mob(Point startingPosition, Floor currentFloor, int spawnRoomId, int id)
    {
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;
        PreviousPosition = Position;

        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = 10;
        Id = id;
        POV = 5;
    }

    public bool CanSeePlayer(Player player)
    {
        for (var dy = -1 * POV; dy <= POV; dy++)
        {
            for (var dx = -1 * POV; dx <= POV; dx++)
            {
                if (player.Position.X == Position.X + dx && player.Position.Y == Position.Y + dy && player.CurrentRoom == CurrentRoom)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        //8 0 5
        //3 x 1
        //7 2 6
        var rnd = new Random();
        var pressedKey = -1;

        var oldPlayerPostionX = Position.X;
        var oldPlayerPostionY = Position.Y;

        var probPlayerPostionX = Position.X;
        var probPlayerPostionY = Position.Y;

        if (!SawPlayer)
        {
            pressedKey = rnd.Next(0, 4);
            SawPlayer = CanSeePlayer(player);
        }
        else if (CurrentRoom == player.CurrentRoom)
        {
            if (player.PreviousPosition.X == Position.X & player.PreviousPosition.Y < Position.Y)
            {
                pressedKey = 0;
            }
            else if (player.PreviousPosition.X > Position.X & player.PreviousPosition.Y < Position.Y)
            {
                pressedKey = 5;
            }
            else if (player.PreviousPosition.X > Position.X & player.PreviousPosition.Y == Position.Y)
            {
                pressedKey = 1;
            }
            else if (player.PreviousPosition.X > Position.X & player.PreviousPosition.Y > Position.Y)
            {
                pressedKey = 6;
            }
            else if (player.PreviousPosition.X == Position.X & player.PreviousPosition.Y > Position.Y)
            {
                pressedKey = 2;
            }
            else if (player.PreviousPosition.X < Position.X & player.PreviousPosition.Y > Position.Y)
            {
                pressedKey = 7;
            }
            else if (player.PreviousPosition.X < Position.X & player.PreviousPosition.Y == Position.Y)
            {
                pressedKey = 3;
            }
            else if (player.PreviousPosition.X < Position.X & player.PreviousPosition.Y < Position.Y)
            {
                pressedKey = 8;
            }
            else
            {
                //pass
            }
        }
        else if (SawPlayer && Math.Abs(CurrentRoom.RoomId - player.CurrentRoom.RoomId) == 1)
        {
            var gate = CurrentRoom.Gates.Find(x => x.NextRoomIndex == player.CurrentRoom.RoomId);
            if (gate.GatePosition.X == Position.X && gate.GatePosition.Y == Position.Y)
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
            if (gate.GatePosition.X == Position.X && gate.GatePosition.Y < Position.Y)
            {
                pressedKey = 0;
            }
            else if (gate.GatePosition.X > Position.X && gate.GatePosition.Y < Position.Y)
            {
                pressedKey = 5;
            }
            else if (gate.GatePosition.X > Position.X && gate.GatePosition.Y == Position.Y)
            {
                pressedKey = 1;
            }
            else if (gate.GatePosition.X > Position.X && gate.GatePosition.Y > Position.Y)
            {
                pressedKey = 6;
            }
            else if (gate.GatePosition.X == Position.X && gate.GatePosition.Y > Position.Y)
            {
                pressedKey = 2;
            }
            else if (gate.GatePosition.X < Position.X && gate.GatePosition.Y > Position.Y)
            {
                pressedKey = 7;
            }
            else if (gate.GatePosition.X < Position.X && gate.GatePosition.Y == Position.Y)
            {
                pressedKey = 3;
            }
            else if (gate.GatePosition.X < Position.X && gate.GatePosition.Y < Position.Y)
            {
                pressedKey = 8;
            }
            else
            {
                //pass
            }
        }
        else if (Math.Abs(CurrentRoom.RoomId - player.CurrentRoom.RoomId) > 1)
        {
            SawPlayer = false;
        }

        if (pressedKey.Equals(3))
        {
            if (CheckArrayLimits(-1, 0))
            {
                probPlayerPostionX -= 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(1))
        {
            if (CheckArrayLimits(1, 0))
            {
                probPlayerPostionX += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(2))
        {
            if (CheckArrayLimits(0, 1))
            {
                probPlayerPostionY += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(0))
        {
            if (CheckArrayLimits(0, -1))
            {
                probPlayerPostionY -= 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(5))
        {
            if (CheckArrayLimits(1, -1))
            {
                probPlayerPostionX += 1;
                probPlayerPostionY += -1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(6))
        {
            if (CheckArrayLimits(1, 1))
            {
                probPlayerPostionX += 1;
                probPlayerPostionY += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(7))
        {
            if (CheckArrayLimits(-1, 1))
            {
                probPlayerPostionX += -1;
                probPlayerPostionY += 1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }
        else if (pressedKey.Equals(8))
        {
            if (CheckArrayLimits(-1, -1))
            {
                probPlayerPostionX += -1;
                probPlayerPostionY += -1;
            }
            else if (CheckGatePlacement(Position.X, Position.Y))
            {
                Teleporting(oldPlayerPostionX, oldPlayerPostionY, dictionary);
                return;
            }
        }

        if (!CheckArrayPlayer(probPlayerPostionX, probPlayerPostionY) && CheckArrayEnemies(probPlayerPostionX, probPlayerPostionY) &&
        CheckArrayWalls(probPlayerPostionX, probPlayerPostionY))
        {
            PreviousPosition = Position;
            Position.X = probPlayerPostionX;
            Position.Y = probPlayerPostionY;
            CurrentRoom.Field[oldPlayerPostionY, oldPlayerPostionX] = 0;
            CurrentRoom.Field[Position.Y, Position.X] = Id;
            CurrentRoom.ChangePlayerPosition(new Point(oldPlayerPostionX, oldPlayerPostionY),
                Position,
                dictionary);
        }
    }
}