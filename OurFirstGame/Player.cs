using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

class Player : Person
{
    public int Score = 0;
    public int MedicineCount = 0;

    public string WeaponVisual = "/\\\n" + "// \\\n" + "|| |\n" + "|| |\n" + "|| |\n" + "|| |\n" + "|| |\n" +
        "|| |\n" + "__ || | __\n" + "/___||_|___\\\n" + "ww\n" + "MM\n" + "_MM_\n" + "(&<>&)\n" + "~~~~";

    public string ArmorVisual = "\\_________________/\n" + "|       | |       |\n" + "|       | |       |\n" +
        "|       | |       |\n" + "|_______| |_______|\n" + "|________ ________|\n" + "|       | |       |\n" +
        "|       | |       |\n" + "\\       | |       /\n" + "\\      | |      /\n" + "\\     | |     /\n" +
        "\\    | |    /\n" + "\\   | |   /\n" + "\\  | |  /\n" + "\\ | | /\n" + "\\| |/\n" + "\\_/";

    public string test = "";

    public Player(Point startingPosition, Floor currentFloor, int spawnRoomId)
    {
        Position.Y = startingPosition.Y;
        Position.X = startingPosition.X;
        PreviousPosition = Position;
        CurrentFloor = currentFloor;
        CurrentRoom = CurrentFloor.Rooms[spawnRoomId];
        Id = 8;
        currentFloor.Rooms[spawnRoomId].Field[startingPosition.Y, startingPosition.X] = Id;
        POV = 6;

        Attack = 5;
        HP = 15;
        Dexterity = 33;
        Armor = 2;
    }

    public void Hud(int x, int y)
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
        Console.SetCursorPosition(0, 0);
    }

    public void DrawItem(int x, int y, string item)
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
    public void ClearItem(int x, int y)
    {
        for (var i = 0; i < 22; i++)
        {
            for (var j = 0; j < 20; j++)
            {
                Console.SetCursorPosition(x + j, y + i);
                Console.Write(" ");
            }
        }
    }

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
    public Chest PickChest()
    {
        var chest = CurrentFloor.Chests.First(x => x.CurrentRoom.RoomId == CurrentRoom.RoomId);
        return CurrentFloor.Chests.Find(x => CheckChestPlacement());
    }

    public void ChangeCurrentFloor(Floor newFloor)
    {
        CurrentFloor = newFloor;
        CurrentRoom = newFloor.Rooms[0];
        newFloor.Rooms[0].Field[1, 1] = 8;
        Position.X = 1;
        Position.Y = 1;
        PreviousPosition = Position;
    }

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
