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
            //ctrl+shift+/ - комментирование
            var randomazer = new Random();
            var broadcaster = new Broadcaster();
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
                {10,"Z" },//Zombie
                {11,"S"},//Snake
                {12,"D"},//D.E.A.T.H
                {13,"A"},//Аlligator
                {14,"K"},//Knight
                {15,"G"},//Goblin
                {98,"C" },//Chest
                {99,"X"}//NextFloor
            };

            //Основа<
            var deep = 1;
            Floor floor = new Floor(randomazer.Next(8, 11), 50);

            Player player = new Player(new Point(1, 1), floor, 0);

            floor.ShowMap(dictionary);
            //Основа>

            while (!player.isDead())
            {
                player.Move(dictionary, player);
                if (player.CheckLaddePlacement())
                {
                    deep += 1;
                    floor = new Floor(randomazer.Next(6, 11), floor.FloorLevel - 10);
                    player.ChangeCurrentFloor(floor);
                    Console.Clear();
                    floor.ShowMap(dictionary);
                    continue;
                }
                if (player.CheckChestPlacement())
                {
                    var chest = player.PickChest();
                    player.OpenChest(chest);
                    if (chest.Item.Type.Equals(ItemTypers.Weapon))
                    {
                        broadcaster.PlayerSwapWeapon(chest);
                    }
                    else if (chest.Item.Type.Equals(ItemTypers.Armor))
                    {
                        broadcaster.PlayerSwapArmor(chest);
                    }
                    else
                    {
                        broadcaster.PlayerPickMedicine();
                    }
                    floor.Rooms[player.CurrentRoom.RoomId].Field[chest.Position.Y, chest.Position.X] = 1;
                    floor.Chests.Remove(chest);
                }
                foreach (var enemy in floor.Mobs)
                {
                    enemy.Move(dictionary, player);
                    if (enemy.CanAttackPlayer(player))
                    {
                        if (randomazer.Next(100) < enemy.Dexterity)
                        {
                            broadcaster.PlayerMiss(enemy);
                        }
                        else
                        {
                            var damage = enemy.Armor - player.Attack;
                            if (damage < 0)
                            {
                                enemy.HP += damage;
                                broadcaster.PlayerHit(enemy, player);
                            }
                            else
                            {
                                broadcaster.PlayerHitBlock(enemy, player);
                            }
                        }
                        if (enemy.isDead())
                        {
                            player.Score += enemy.Points;
                            broadcaster.EnemyIsDead(enemy);
                            floor.Graveyard.Add(enemy);
                        }
                        else
                        {
                            if (randomazer.Next(100) < player.Dexterity)
                            {
                                broadcaster.EnemyMiss(enemy);
                            }
                            else
                            {
                                var damage = player.Armor - enemy.Attack;
                                if (damage < 0)
                                {
                                    player.HP += damage;
                                    broadcaster.EnemyHit(enemy, player);
                                }
                                else
                                {
                                    broadcaster.EnemyHitBlock(enemy);
                                }
                            }
                        }
                    }
                }
                foreach (var corpse in floor.Graveyard)
                {
                    floor.Mobs.Remove(corpse);
                    corpse.Decomposition(dictionary);
                }
                floor.Graveyard = new List<Mob>();
                if (player.isDead())
                {
                    if (player.MedicineCount > 0)
                    {
                        player.HP = 15;
                        broadcaster.PlayerHeal();
                    }
                    else
                    {
                        broadcaster.PlayerLastBreath();
                    }
                }
            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Ваше приключение окончено...");
            Console.WriteLine("Ваш итоговый счёт: " + (player.Score * 1.15 * deep));
        }
    }
}
