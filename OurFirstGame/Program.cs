using System;
using System.Collections.Generic;
using System.Drawing;
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
    class HudAction : Hud
    {
        public HudAction(Map map)
        {
            leftBorder = 0;
            rightBorder = Map.columns + 22;
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
            rightBorder = leftBorder + 20;
            upperBorder = 0;
            bottomBorder = Map.rows + 2;
            DrawHudBorder(leftBorder, rightBorder, upperBorder, bottomBorder);
            DrawPlayerStats(player);
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
    class Map
    {
        public int[,] field;
        public int[,] oldField;
        public static int rows;
        public static int columns;

        public Map()
        {
            field = new int[28, 98];
            oldField = new int[28, 98];

            //field = new int[3, 5];
            //oldField = new int[3, 5];
            rows = field.GetUpperBound(0) + 1;
            columns = field.GetUpperBound(1) + 1;
        }
        public void Redraw(Map map, Dictionary<int, string> mapDictionary)
        {
            for (int x = 0; x < Map.rows; x++)
            {
                for (int y = 0; y < Map.columns; y++)
                {
                    if (map.field != map.oldField)
                    {
                        Console.SetCursorPosition(y + 1, x + 1);
                        Console.Write(mapDictionary[map.field[x, y]]);
                    }
                }
                Console.WriteLine();
            }
        }
    }
    class Player
    {
        public int playerColumn = 0;
        public int playerRow = 0;
        public int hp;
        public Player()
        {
            hp = 100;
        }
        public Dictionary<string, int> playerStats = new Dictionary<string, int>{
            {"HP", 100},
            {"ARMOR", 0},
        };

    }
    class Program
    {
        public static void PlacePlayer(Map map, Player player)
        {
            map.field[player.playerRow, player.playerColumn] = 7;
        }

        public static void MovePlayer(Map map, Player player)
        {
            var oldPlayerColumn = player.playerColumn;
            var oldPlayerRow = player.playerRow;
            var pressedKey = Console.ReadKey().Key;
            if (pressedKey.Equals(ConsoleKey.LeftArrow))
            {
                if (player.playerColumn > 0)
                {
                    player.playerColumn -= 1;
                }
            }
            else if (pressedKey == ConsoleKey.RightArrow)
            {
                if (player.playerColumn < Map.columns - 1)
                {
                    player.playerColumn += 1;
                }
            }
            else if (pressedKey.Equals(ConsoleKey.DownArrow))
            {
                if (player.playerRow < Map.rows - 1)
                {
                    player.playerRow += 1;
                }
            }
            else if (pressedKey == ConsoleKey.UpArrow)
            {
                if (player.playerRow > 0)
                {
                    player.playerRow -= 1;
                }
            }
            map.oldField = (int[,])map.field.Clone();
            map.field[oldPlayerRow, oldPlayerColumn] = 0;
            map.field[player.playerRow, player.playerColumn] = 7;
        }
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
            mapDictionary.Add(1, "╔");
            mapDictionary.Add(2, "╗");
            mapDictionary.Add(3, "╚");
            mapDictionary.Add(4, "╝");
            mapDictionary.Add(5, "═");
            mapDictionary.Add(6, "║");
            mapDictionary.Add(7, "@");
            var map = new Map();
            var player = new Player();
            var hudStats = new HudStats(map, player);
            var hudAction = new HudAction(map);
            var hudMap = new HudMap(map);


            PlacePlayer(map, player);
            map.Redraw(map, mapDictionary);
            while (true)
            {
                MovePlayer(map, player);
                map.Redraw(map, mapDictionary);
                //ShowMassive(map);
                Thread.Sleep(1);
            }

        }
    }
}
