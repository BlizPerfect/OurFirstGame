using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

/// <summary>
/// Сущность игрока
/// </summary>
class Player : Person
{
    public int Score { set; get; } // Счёт игрока
    public int MedicineCount { set; get; } // Кол-во апетчек у игрока

    public string WeaponVisual = "/\\\n" + "// \\\n" + "|| |\n" + "|| |\n" + "|| |\n" + "|| |\n" + "|| |\n" +
        "|| |\n" + "__ || | __\n" + "/___||_|___\\\n" + "ww\n" + "MM\n" + "_MM_\n" + "(&<>&)\n" + "~~~~";

    public string ArmorVisual = "\\_________________/\n" + "|       | |       |\n" + "|       | |       |\n" +
        "|       | |       |\n" + "|_______| |_______|\n" + "|________ ________|\n" + "|       | |       |\n" +
        "|       | |       |\n" + "\\       | |       /\n" + "\\      | |      /\n" + "\\     | |     /\n" +
        "\\    | |    /\n" + "\\   | |   /\n" + "\\  | |  /\n" + "\\ | | /\n" + "\\| |/\n" + "\\_/";

    public string test = "";

    public Player(Point startingPosition, Floor currentFloor, int spawnRoomId)
    {
        Score = 0;
        MedicineCount = 0;
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;
        PreviousPosition = Position;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        Id = 8;
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = Id;
        POV = 6;

        Attack = 3;
        HP = 15;
        Dexterity = 33;
        Armor = 2;
    }

    /// <summary>
    /// Отрисовка HUD
    /// </summary>
    /// <returns>void</returns>
    private void Hud(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write("PLAYER STATS:");

        Console.SetCursorPosition(x, y + 1);
        Console.Write("{0,3}{1,8}{2,32}{3,4}", "HP:", HP, "HEALS:", MedicineCount);
        Console.SetCursorPosition(x, y + 2);
        Console.Write("{0,3}{1,4}{2,32}{3,4}", "ATTACK:", Attack, "ARMOR:", Armor);
        Console.SetCursorPosition(x, y + 4);
        Console.Write("{0,0}{1,43}", "Ваш верный меч:", "Ваш неприступный щит:");
        DrawItem(x, y + 6, WeaponVisual);
        DrawItem(x + 37, y + 6, ArmorVisual);
        Console.SetCursorPosition(x, y + 35);
        Console.Write("Score: {0,3}", Score);
        Console.SetCursorPosition(0, 0);
    }

    /// <summary>
    /// Отрисовка предмета
    /// </summary>
    /// <returns>void</returns>
    private void DrawItem(int x, int y, string item)
    {
        var splittedWeaponString = item.Split("\n");
        var max = 0;
        foreach (var e in splittedWeaponString)
        {
            if (e.Length > max)
            {
                max = e.Length;
            }
        }
        foreach (var e in splittedWeaponString)
        {
            Console.SetCursorPosition(x + (max - e.Length) / 2, y);
            Console.Write(e);
            y += 1;
        }
    }

    /// <summary>
    /// Очистка визуала предыдущего приедмета
    /// </summary>
    /// <returns>void</returns>
    private void ClearItem(int x, int y)
    {
        for (var i = 0; i < 30; i++)
        {
            for (var j = 0; j < 30; j++)
            {
                Console.SetCursorPosition(x + j, y + i);
                Console.Write(" ");
            }
        }
    }

    /// <summary>
    /// Передвижение игрока
    /// </summary>
    /// <returns>void</returns>
    public override void Move(Dictionary<int, string> dictionary, Player player)
    {
        Hud(110, 1);
        var pressedKey = Console.ReadKey().Key;

        var oldPlayerPostionX = Position.X;
        var oldPlayerPostionY = Position.Y;

        var probPlayerPostionX = Position.X;
        var probPlayerPostionY = Position.Y;

        if (pressedKey.Equals(ConsoleKey.LeftArrow))
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
        else if (pressedKey.Equals(ConsoleKey.RightArrow))
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
        else if (pressedKey.Equals(ConsoleKey.DownArrow))
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
        else if (pressedKey.Equals(ConsoleKey.UpArrow))
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

        if (CheckArrayEnemies(probPlayerPostionX, probPlayerPostionY) &&
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


    /// <summary>
    /// Проверка, не находится ли игрок на лестнице
    /// </summary>
    /// <returns>bool</returns>
    public bool CheckLaddePlacement()
    {
        if (CurrentFloor.Ladder.Position.X == Position.X &&
            CurrentFloor.Ladder.Position.Y == Position.Y &&
            CurrentRoom.RoomId == CurrentFloor.Ladder.RoomIndex)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Проверка, не находится ли игрок на Сундуке
    /// </summary>
    /// <returns>bool</returns>
    public bool CheckChestPlacement()
    {
        foreach (var chest in CurrentFloor.Chests)
        {
            if (chest.Position.X == Position.X &&
            chest.Position.Y == Position.Y &&
            chest.CurrentRoom.RoomId == CurrentRoom.RoomId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Выбор нужного сундука
    /// </summary>
    /// <returns>Chest</returns>
    public Chest PickChest()
    {
        var chest = CurrentFloor.Chests.First(x => x.CurrentRoom.RoomId == CurrentRoom.RoomId);
        return CurrentFloor.Chests.Find(x => CheckChestPlacement());
    }

    /// <summary>
    /// Смена этажа
    /// </summary>
    /// <returns>void</returns>
    public void ChangeCurrentFloor(Floor newFloor)
    {
        CurrentFloor = newFloor;
        CurrentRoom = newFloor.Rooms[0];
        newFloor.Rooms[0].Field[1, 1] = 8;
        Position.X = 1;
        Position.Y = 1;
        PreviousPosition = Position;
    }

    /// <summary>
    /// Открытие сундука и получение предмета из него
    /// </summary>
    /// <returns>void</returns>
    public void OpenChest(Chest chest)
    {
        chest.PutItemIntoChest(this);
        if (chest.Item.Type.Equals(ItemTypers.Weapon))
        {
            Attack = chest.Item.Value;
            WeaponVisual = chest.Item.Visual;
            ClearItem(110, 6);
        }
        else if (chest.Item.Type.Equals(ItemTypers.Armor))
        {
            Armor = chest.Item.Value;
            ArmorVisual = chest.Item.Visual;
            ClearItem(147, 6);
        }
        else
        {
            MedicineCount += 1;
        }
    }
}
