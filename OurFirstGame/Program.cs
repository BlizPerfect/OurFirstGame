using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace OurFirstGame
{
    class Hud
    {
        public int leftBorder;
        public int rightBorder;
        public int upperBorder;
        public int bottomBorder;

        public void DrawHudBorder(int leftBorder, int rightBorder, int upperBorder, int bottomBorder)
        {
            for (int i = upperBorder; i < bottomBorder; i++)
            {
                for (int j = leftBorder; j < rightBorder; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == upperBorder)
                    {
                        if (j == leftBorder)
                        {
                            Console.Write("╔");
                        }
                        else if (j == rightBorder - 1)
                        {
                            Console.Write("╗");
                        }
                        else
                        {
                            Console.Write("═");
                        }
                    }
                    else if (i == bottomBorder - 1)
                    {
                        if (j == leftBorder)
                        {
                            Console.Write("╚");
                        }
                        else if (j == rightBorder - 1)
                        {
                            Console.Write("╝");
                        }
                        else
                        {
                            Console.Write("═");
                        }
                    }
                    else if ((j == leftBorder || j == rightBorder - 1) && (i != upperBorder || i != bottomBorder - 1))
                    {
                        Console.Write("║");
                    }
                }
            }
        }
    }
    class HudRightLeg : Hud
    {
        public HudRightLeg(Map map)
        {
            leftBorder = Map.columns + 18;
            rightBorder = leftBorder + 12;
            upperBorder = 26;
            bottomBorder = upperBorder + 8;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }
    class HudLeftLeg : Hud
    {
        public HudLeftLeg(Map map)
        {
            leftBorder = Map.columns + 4;
            rightBorder = leftBorder + 12;
            upperBorder = 26;
            bottomBorder = upperBorder + 8;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }
    class HudChest : Hud
    {
        public HudChest(Map map)
        {
            leftBorder = Map.columns + 4;
            rightBorder = leftBorder + 26;
            upperBorder = 10;
            bottomBorder = upperBorder + 16;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }
    class HudRightArm : Hud
    {
        public HudRightArm(Map map)
        {
            leftBorder = Map.columns + 18;
            rightBorder = leftBorder + 12;
            upperBorder = 2;
            bottomBorder = upperBorder + 8;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }

    class HudLeftArm : Hud
    {
        public HudLeftArm(Map map)
        {
            leftBorder = Map.columns + 4;
            rightBorder = leftBorder + 12;
            upperBorder = 2;
            bottomBorder = upperBorder + 8;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }
    class HudAction : Hud
    {
        public HudAction(Map map)
        {
            leftBorder = 0;
            rightBorder = Map.columns + 32;
            upperBorder = Map.rows + 2;
            bottomBorder = upperBorder + 8;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }
    class HudStats : Hud
    {
        public HudStats(Map map, Player player)
        {
            leftBorder = Map.columns + 2;
            rightBorder = leftBorder + 30;
            upperBorder = 0;
            bottomBorder = Map.rows + 2;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
            // DrawPlayerStats(player);
        }

        public void DrawPlayerStats(Player player)
        {
            var i = 1;
            foreach (var stat in player.playerStats)
            {
                Console.SetCursorPosition(leftBorder + 1, upperBorder + i);
                i += 1;
                Console.WriteLine(stat.Key + ": " + stat.Value);
            }
        }
    }

    class HudMap : Hud
    {
        public HudMap(Map map)
        {
            leftBorder = 0;
            rightBorder = Map.columns + 2;
            upperBorder = 0;
            bottomBorder = Map.rows + 2;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
        }
    }
    class Cell
    {
        public int Value;
        public string Type;
        public string VisualRepresentation;

        public void Redraw(int GlobalRow, int GlobalColumn)
        {
            Console.SetCursorPosition(GlobalRow, GlobalColumn);
            Console.Write(VisualRepresentation);
        }
    }

    class CellTopLeftCorner : Cell
    {
        public CellTopLeftCorner()
        {
            Value = 1;
            Type = "WALL";
            VisualRepresentation = "╔";
        }
    }
    class CellTopRightCorner : Cell
    {
        public CellTopRightCorner()
        {
            Value = 2;
            Type = "WALL";
            VisualRepresentation = "╗";
        }
    }

    class CellBottomRightCorner : Cell
    {
        public CellBottomRightCorner()
        {
            Value = 3;
            Type = "WALL";
            VisualRepresentation = "╝";
        }
    }

    class CellBottomLeftCorner : Cell
    {
        public CellBottomLeftCorner()
        {
            Value = 4;
            Type = "WALL";
            VisualRepresentation = "╚";
        }
    }

    class CellHorizontal : Cell
    {
        public CellHorizontal()
        {
            Value = 5;
            Type = "WALL";
            VisualRepresentation = "═";
        }
    }

    class CellVertical : Cell
    {
        public CellVertical()
        {
            Value = 6;
            Type = "WALL";
            VisualRepresentation = "║";
        }
    }

    class CellGate : Cell
    {
        public CellGate()
        {
            Value = 7;
            Type = "Gate";
            VisualRepresentation = "╬";
        }
    }
    class Room
    {
        public int RoomRows;
        public int RoomColumns;
        public int PositionOfRoomRow;
        public int PositionOfRoomColumn;
        public bool IsStart;
        public Cell[,] RoomField;
        public Room(int roomRows, int roomColumns, int positionOfRoomRow, int positionOfRoomColumn, bool startRoom)
        {
            RoomRows = roomRows;
            RoomColumns = roomColumns;
            PositionOfRoomRow = positionOfRoomRow;
            PositionOfRoomColumn = positionOfRoomColumn;
            IsStart = startRoom;
            RoomField = CreateRoomWalls();
            PlaceGate();
            //DrawHudBorder(positionOfRoomColumn, positionOfRoomColumn + roomColumns, positionOfRoomRow, positionOfRoomRow + roomRows);
        }

        public void PlaceGate()
        {
            var i = 0;
            var j = 0;
            Random rnd = new Random();
            if (rnd.Next(100) < 50)// определяем будет ли вход на горизонтальной или вертикальной стене. True - на горизонтальной
            {
                //Горизонтальная стена
                if (rnd.Next(100) < 50)
                {
                    //Верх
                    i = 0;
                }
                else
                {
                    //Низ
                    i = RoomRows - 1;
                }
                j = rnd.Next(1, RoomColumns - 2);
            }
            else
            {
                //Вертикальная стена
                if (rnd.Next(100) < 50)
                {
                    //Лево
                    j = 0;
                }
                else
                {
                    //Право
                    j = RoomColumns - 1;
                }
                i = rnd.Next(1, RoomRows - 2);
            }
            RoomField[i, j] = new CellGate();
            RoomField[i, j].Redraw(j + PositionOfRoomColumn, i + PositionOfRoomRow);
        }

        public Cell[,] CreateRoomWalls()
        {
            var result = new Cell[RoomRows, RoomColumns];
            for (int i = 0; i < RoomRows; i++)
            {
                for (int j = 0; j < RoomColumns; j++)
                {
                    var cell = new Cell();
                    Console.SetCursorPosition(j + PositionOfRoomColumn, i + PositionOfRoomRow);
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            cell = new CellTopLeftCorner();
                        }
                        else if (j == RoomColumns - 1)
                        {
                            cell = new CellTopRightCorner();
                        }
                        else
                        {
                            cell = new CellHorizontal();
                        }
                    }
                    else if (i == RoomRows - 1)
                    {
                        if (j == 0)
                        {
                            cell = new CellBottomLeftCorner();
                        }
                        else if (j == RoomColumns - 1)
                        {
                            cell = new CellBottomRightCorner();
                        }
                        else
                        {
                            cell = new CellHorizontal();
                        }
                    }
                    else if ((j == 0 || j == RoomColumns - 1) && (i != 0 || i != RoomRows - 1))
                    {
                        cell = new CellVertical();
                    }
                    result[i, j] = cell;
                    Console.Write(cell.VisualRepresentation);
                    //Console.Write(cell.Value);
                }
            }
            return result;
        }
    }

    class Floor
    {
        public List<Room> Rooms = new List<Room>();
        public Floor(int roomCount)
        {
            for (var i = 0; i < roomCount; i++)
            {
                if (i == 0)
                {
                    Rooms.Add(new Room(4, 10, 1, 1, true));
                }
                Rooms.Add(new Room(4, 10, 1, 1, false));
            }

        }

    }
    class Map
    {
        public int[,] field;
        public static int rows;
        public static int columns;

        public Map()
        {
            field = new int[38, 148];

            //field = new int[3, 5];
            //oldField = new int[3, 5];
            rows = field.GetUpperBound(0) + 1;
            columns = field.GetUpperBound(1) + 1;
        }


    }
    class Player
    {
        public int playerColumn = 2;
        public int previousPlayerColumn = 2;
        public int playerRow = 2;
        public int previousPlayerRow = 2;
        public int hp;
        public Player()
        {
            hp = 100;
        }
        public Dictionary<string, int> playerStats = new Dictionary<string, int>{
            {"HP", 100},
            {"ARMOR", 0},
        };
        public void PlacePlayer(Map map, Dictionary<int, string> mapDictionary)
        {
            map.field[playerRow, playerColumn] = 7;
            Console.SetCursorPosition(playerColumn + 1, playerRow + 1);
            Console.Write(mapDictionary[map.field[playerRow, playerColumn]]);

        }
        public void MovePlayer(Map map, Dictionary<int, string> mapDictionary)
        {
            previousPlayerColumn = playerColumn;
            previousPlayerRow = playerRow;
            var pressedKey = Console.ReadKey().Key;
            if (pressedKey.Equals(ConsoleKey.LeftArrow))
            {
                if (playerColumn > 0)
                {
                    playerColumn -= 1;
                }
            }
            else if (pressedKey == ConsoleKey.RightArrow)
            {
                if (playerColumn < Map.columns - 1)
                {
                    playerColumn += 1;
                }
            }
            else if (pressedKey.Equals(ConsoleKey.DownArrow))
            {
                if (playerRow < Map.rows - 1)
                {
                    playerRow += 1;
                }
            }
            else if (pressedKey == ConsoleKey.UpArrow)
            {
                if (playerRow > 0)
                {
                    playerRow -= 1;
                }
            }
            Console.SetCursorPosition(playerColumn + 1, playerRow + 1);
            map.field[playerRow, playerColumn] = 7;
            Console.Write(mapDictionary[map.field[playerRow, playerColumn]]);

            Console.SetCursorPosition(previousPlayerColumn + 1, previousPlayerRow + 1);
            map.field[previousPlayerRow, previousPlayerColumn] = 0;
            Console.Write(mapDictionary[map.field[previousPlayerRow, previousPlayerColumn]]);
            Console.SetCursorPosition(0, 0);
        }
    }
    class Program
    {
        public static void ShowMassive(Map Map)
        {
            Console.SetCursorPosition(0, Map.rows + 10);
            for (int i = 0; i < Map.rows; i++)
            {
                for (int j = 0; j < Map.columns; j++)
                {
                    Console.Write(Map.field[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var mapDictionary = new Dictionary<int, string>();

            mapDictionary.Add(0, " ");
            mapDictionary.Add(7, "@");

            var map = new Map();
            var player = new Player();
            var hudStats = new HudStats(map, player);
            //var hudLeftArm = new HudLeftArm(map);
            //var hudRightArm = new HudRightArm(map);
            //var HudChest = new HudChest(map);
            //var hudLeftLeg = new HudLeftLeg(map);
            //var hudRightLeg = new HudRightLeg(map);
            var hudAction = new HudAction(map);
            var hudMap = new HudMap(map);
            Random rnd = new Random();
            //var room1 = new Room(rnd.Next(5, 15), rnd.Next(10, 40), 1, 1, true);
            var floor1 = new Floor(1);
            player.PlacePlayer(map, mapDictionary);

            while (true)
            {
                player.MovePlayer(map, mapDictionary);
                //ShowMassive(map);
                Thread.Sleep(1);
            }
        }
    }
}
