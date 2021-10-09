using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace OurFirstGame
{
    class Map
    {
        public int[,] field;
        public int[,] oldField;
        public int rows;
        public int columns;
        public Map()
        {
            //field = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            field = new int[10, 20];
            oldField = new int[10, 20];
            rows = field.GetUpperBound(0) + 1;
            columns = field.GetUpperBound(1) + 1;
        }

    }
    class Player
    {
        public int playerColumn = 1;
        public int playerRow = 1;
    }
    class Program
    {
        public static void CreateBoundaries(Map map)
        {
            for (int i = 0; i < map.rows; i++)
            {
                for (int j = 0; j < map.columns; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            map.field[i, j] = 1;
                        }
                        else if (j == map.columns - 1)
                        {
                            map.field[i, j] = 2;
                        }
                        else
                        {
                            map.field[i, j] = 5;
                        }
                    }
                    else if (i == map.rows - 1)
                    {
                        if (j == 0)
                        {
                            map.field[i, j] = 3;
                        }
                        else if (j == map.columns - 1)
                        {
                            map.field[i, j] = 4;
                        }
                        else
                        {
                            map.field[i, j] = 5;
                        }
                    }
                    else if ((j == 0 || j == map.columns - 1) && (i != 0 || i != map.rows - 1))
                    {
                        map.field[i, j] = 6;
                    }
                }
            }
        }
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
                if (player.playerColumn > 1)
                {
                    player.playerColumn -= 1;
                }
            }
            else if (pressedKey == ConsoleKey.RightArrow)
            {
                if (player.playerColumn < map.columns - 2)
                {
                    player.playerColumn += 1;
                }
            }
            else if (pressedKey.Equals(ConsoleKey.DownArrow))
            {
                if (player.playerRow < map.rows - 2)
                {
                    player.playerRow += 1;
                }
            }
            else if (pressedKey == ConsoleKey.UpArrow)
            {
                if (player.playerRow > 1)
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
            for (int i = 0; i < Map.rows; i++)
            {
                for (int j = 0; j < Map.columns; j++)
                {
                    Console.Write(Map.field[i, j]);
                }
                Console.WriteLine();
            }
            for (int i = 0; i < Map.rows; i++)
            {
                for (int j = 0; j < Map.columns; j++)
                {
                    Console.Write(Map.oldField[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void Redraw(Map map, Dictionary<int, string> mapDictionary)
        {
            for (int x = 0; x < map.rows; x++)
            {
                for (int y = 0; y < map.columns; y++)
                {
                    if (map.field != map.oldField)
                    {
                        Console.SetCursorPosition(y, x);
                        Console.Write(mapDictionary[map.field[x, y]]);
                    }
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
            var player = new Player();
            var map = new Map();
            CreateBoundaries(map);
            PlacePlayer(map, player);
            Redraw(map, mapDictionary);

            while (true)
            {
                MovePlayer(map, player);
                Redraw(map, mapDictionary);
                ShowMassive(map);
                Thread.Sleep(1);
            }

        }
    }
}
