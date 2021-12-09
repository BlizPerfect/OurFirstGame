using System;
using System.Collections.Generic;
using System.Drawing;

class Mob : Person
{
    public static List<int> MobsList = new List<int> { 10, 11 };
    public static List<int> MoveSidesList = new List<int> { 0,//Вверх
        1,//Вправо
        2,//Вниз
        3//Влево
         };

    public bool SawPlayer = false;
    public Mob(Point startingPosition, Floor currentFloor, int spawnRoomId, int id)
    {
        Row = startingPosition.Y;
        Column = startingPosition.X;
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
                if (player.Column == Column + dx && player.Row == Row + dy && player.CurrentRoom == CurrentRoom)
                {
                    return true;
                }
            }
        }
        return false;
    }



    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        var rnd = new Random();

        PersonDebug(130, 30);

        //var pressedKey = rnd.Next(0, 4);
        //var pressedKey = 0;
        var pressedKeyMassive = new List<int>();
        if (!SawPlayer)
        {
            SawPlayer = CanSeePlayer(player);
        }
        if (SawPlayer)
        {
            if (player.CurrentRoom == CurrentRoom)
            {
                if (player.PColumn > Column)
                {
                    pressedKeyMassive.Add(1);
                    if (player.PRow > Row)
                    {
                        pressedKeyMassive.Add(2);
                    }
                    else if (player.PRow < Row)
                    {
                        pressedKeyMassive.Add(0);
                    }
                }
                else if (player.PColumn < Column)
                {
                    pressedKeyMassive.Add(3);
                    if (player.PRow > Row)
                    {
                        pressedKeyMassive.Add(2);
                    }
                    else if (player.PRow < Row)
                    {
                        pressedKeyMassive.Add(0);
                    }
                }
                else
                {
                    if (player.PRow > Row)
                    {
                        pressedKeyMassive.Add(2);
                    }
                    else if (player.PRow < Row)
                    {
                        pressedKeyMassive.Add(0);
                    }
                }
            }
            else
            {
                var gate = CurrentRoom.Gates.Find(x => x.NextRoomIndex == player.CurrentRoom.RoomId);
                if (gate.GatePosition.X == Column && gate.GatePosition.Y == Row)
                {
                    if (gate.GatePosition.X == 0)
                    {
                        pressedKeyMassive.Add(3);
                    }
                    else if (gate.GatePosition.X == CurrentRoom.Columns - 1)
                    {
                        pressedKeyMassive.Add(1);
                    }
                    else if (gate.GatePosition.Y == 0)
                    {
                        pressedKeyMassive.Add(0);
                    }
                    else if (gate.GatePosition.Y == CurrentRoom.BottomRow - 1)
                    {
                        pressedKeyMassive.Add(2);
                    }
                }
                else if (gate.GatePosition.X > Column)
                {
                    pressedKeyMassive.Add(1);
                    if (gate.GatePosition.Y > Row)
                    {
                        pressedKeyMassive.Add(2);
                    }
                    else if (gate.GatePosition.Y < Row)
                    {
                        pressedKeyMassive.Add(0);
                    }
                }
                else if (gate.GatePosition.X < Column)
                {
                    pressedKeyMassive.Add(3);
                    if (gate.GatePosition.Y > Row)
                    {
                        pressedKeyMassive.Add(2);
                    }
                    else if (gate.GatePosition.Y < Row)
                    {
                        pressedKeyMassive.Add(0);
                    }
                }
                else
                {
                    if (gate.GatePosition.Y > Row)
                    {
                        pressedKeyMassive.Add(2);
                    }
                    else if (gate.GatePosition.Y < Row)
                    {
                        pressedKeyMassive.Add(0);
                    }
                }
            }
        }
        else
        {
            pressedKeyMassive.Add(rnd.Next(0, 4));
        }
        foreach (var pressedKey in pressedKeyMassive)
        {
            bool test = false;
            var prevRoom = CurrentRoom;
            int previousPlayerRow = Row;
            int previousPlayerCol = Column;
            if (pressedKey == 3)
            {
                if (CheckingLeftWall() && !CheckingPerson(-1, 0))
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
                                Moving(dictionary, i, prevRoom);

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
            else if (pressedKey == 1)
            {
                if (CheckingRightWall() && !CheckingPerson(1, 0))
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
                                Moving(dictionary, i, prevRoom);
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
            else if (pressedKey == 2)
            {
                if (CheckingDownWall() && !CheckingPerson(0, 1))
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
                                Moving(dictionary, i, prevRoom);
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
            else if (pressedKey == 0)
            {
                if (CheckingUpWall() && !CheckingPerson(0, -1))
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
                                Moving(dictionary, i, prevRoom);
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
            CurrentRoom.Field[previousPlayerRow, previousPlayerCol] = 0;

            Console.SetCursorPosition(previousPlayerCol + CurrentRoom.Position.X, previousPlayerRow + CurrentRoom.Position.Y);
            Console.Write(dictionary[CurrentRoom.Field[previousPlayerRow, previousPlayerCol]]);


            CurrentRoom.Field[Row, Column] = Id;

            Console.SetCursorPosition(Column + CurrentRoom.Position.X, Row + CurrentRoom.Position.Y);
            Console.Write(dictionary[CurrentRoom.Field[Row, Column]]);


            Console.SetCursorPosition(130, 8);
            Console.Write("Player global position Row:{0,2}", Row + CurrentRoom.Position.Y);
            Console.SetCursorPosition(130, 9);
            Console.Write("Player global position Column:{0,2}", Column + CurrentRoom.Position.X);

            Console.SetCursorPosition(0, 0);
        }
    }
}
