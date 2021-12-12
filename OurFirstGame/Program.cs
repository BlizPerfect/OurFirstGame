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
        static void Main(string[] args)
        {
            //ctrl+shift+/
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

            //Основа<
            Floor floor1 = new Floor(10);
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 1, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 2, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 3, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 4, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 5, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 6, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 7, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 8, 10));
            floor1.Mobs.Add(new Mob(new Point(1, 1), floor1, 9, 10));

            Player player = new Player(new Point(1, 1), floor1, 0);

            floor1.ShowMap(dictionary);
            //Основа>

            while (true)
            {
                player.Move(dictionary, player);
                foreach (var enemy in floor1.Mobs)
                {
                    enemy.Move(dictionary, player);
                }
            }
        }
    }
}
