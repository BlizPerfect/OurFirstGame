using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;


namespace OurFirstGame
{
    class Program
    {
        public static void ShowMassive(Room room, Dictionary<int, string> dictionary)
        {
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Columns; j++)
                {
                    Console.SetCursorPosition(room.Position.X + j, room.Position.Y + i);
                    Console.Write(dictionary[room.Field[i, j]]);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            //Выделяем область и жмем Ctrl+K, а затем Ctrl+C.
            //Обратное раскомментирование Ctrl - K, а затем Ctrl - U.
            var dictionary = new Dictionary<int, string>
            {
                {0,"."},
                {1," " },
                {2,"╔" },
                {3,"╗"},
                {4,"═" },
                {5,"╚" },
                {6,"╝"},
                {7,"║" },
                {8,"@" },//Player
                {9, "."},//Teleport
                {10,"X" },//Zombie
                {11,"S"}//Snake
            };

            for (var dy = -3; dy <= 3; dy++)
            {
                for (var dx = -3; dx <= 3; dx++)
                {
                    if (dx + dy > 0) continue;
                    else
                    {
                        //Console.WriteLine(dx + ":" + dy);
                    }

                }
            }
            //Console.ReadKey();

            //Основа<
            Floor floor1 = new Floor(10);
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 0, 10));
            floor1.Mobs.Add(new Mob(new Point(3, 3), floor1, 1, 10));


            Player player = new Player(new Point(6, 1), floor1, 0);
            floor1.ShowMap(dictionary);
            //Основа>



            //Room room1 = new Room(10, 5, new Point(0, 0), true);
            //Room room2 = new Room(7, 5, new Point(5, 0), false);
            //Player player = new Player(new Point(1, 1), room1);
            //Random rnd = new Random();
            //var a = rnd.Next(2, room1.Rows - 2);
            //while (a > room2.Rows - 3)
            //{
            //    a -= 1;
            //}
            //room1.Field[a - 1, room1.Columns - 1] = 5;
            //room1.Field[a, room1.Columns - 1] = 0;
            //room1.Field[a + 1, room1.Columns - 1] = 2;
            //room2.Field[a - 1, 0] = 6;
            //room2.Field[a, 0] = 0;
            //room2.Field[a + 1, 0] = 3;
            //ShowMassive(room1, dictionary);
            //ShowMassive(room2, dictionary);

            //bool a = floor1.Intersect(room1, room2);


            //Console.WriteLine(a);
            //floor1.ShowMap(dictionary);

            while (true)
            {
                player.Move(dictionary, player);

                var playerCombatState = player.StartCombat();
                if (playerCombatState.Item1)
                {
                    foreach (var mob in floor1.Mobs)
                    {
                        if (mob.Column == player.Column + playerCombatState.Item2.X && mob.Row == player.Row + playerCombatState.Item2.Y)
                        {
                            var enemy = mob;
                            enemy.HP -= player.Attack;
                            if (enemy.isDead())
                            {
                                enemy.Decomposition(floor1, dictionary);
                                floor1.Mobs.Remove(enemy);
                                break;
                            }
                        }
                    }
                }
                foreach (var enemy in floor1.Mobs)
                {
                    enemy.Move(dictionary, player);
                }
            }
        }
    }
}
