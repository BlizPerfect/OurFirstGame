using System;
using System.Drawing;

enum ItemTypers
{
    Weapon,
    Armor,
    Medicine
}

class Item
{
    public ItemTypers Type;
    public string Name;
    public string Visual;
    public int Value;

    public static string[] FirstWeaponPhrase = new string[]
    {
        "Длинный ",
        "Короткий ",
        "Изогнутый ",
        "Двуручный ",
        "Ультра ",
        "Сломанный ",
        "Кристаличсекий ",
        "Серебрянный ",
        "Золотой ",
        "Дервянный ",
        "Тёмный ",
        "Обсидиановый ",
        "Драконий ",
        "Демонический ",
    };
    public static string[] SecondPhrase = new string[]
    {
        " Хаоса",
        " Огня",
        " Льда",
        " Императора",
        " Драконаборца",
        " Бездны",
        " Рыцаря",
        " Тьмы",
        " Странника",
        " Скверны",
    };
    public static string[] FirstArmorPhrase = new string[]
    {
        "Тяжёлый ",
        "Лёгкий ",
        "Усиленный ",
        "Облегчённый ",
        "Утяжелённый ",
        "Деревянный ",
        "Кристалический ",
        "Серебрянный ",
        "Золотой ",
        "Тёмный ",
        "Обсидиановый ",
        "Драконий ",
        "Демонический ",
    };
    //в высоту - 22 ед, в ширину-20
    public static string[] WeaponVisual = new string[]
    {
        "(**)\n" +
        "IIII\n" + "####\n" + "HHHH\n" + "HHHH\n" + "####\n" + "___IIII___\n" + " .-`_._\" * *\"_._`-.\n" + "|/``  .`\\/`.  ``\\|\n" +
        "`     }    {     '\n" + ") () (\n" + "( :: )\n" + "| :: |\n" + "| )( |\n" + "| || |\n" + "| || |\n" + "| || |\n" + "| || |\n" +
        "| || |\n" + "( () )\n" + "\\  /\n" + "\\/",
        "/^\\\n" + "|\\ /|\n" + "| | |\n" + "| | |\n" + "| | |\n" + "| | |\n" + "| | |\n" +
        "|||\n" + "|||\n" + "|||\n" + "..  |||  ..\n" + "`\\\\=====//'\n" + "(=)\n" + "(=)\n" + "(=)\n" + "\\U/"
    };
    public static string[] ArmorVisual = new string[]
    {
        "\\_ _/\n" + "] --__________-- [\n" + "|       ||       |\n" + "\\       ||       /\n" +
        "[      ||      ]\n" + "|______||______|\n" + "|------..------|\n" + "]      ||      [\n" +
        "\\     ||     /\n" + "[    ||    ]\n" + "\\    ||    /\n" + "[   ||   ]\n" + "\\__||__/\n",
         "_________________________\n" + "|<><><>     |  |    <><><>|\n" + "|<>         |  |        <>|\n" +
        "|   (______<\\-/> ______)  |\n" + "|  /_.-=-.\\| \" |/.-=-._\\  |\n" + "|   /_    \\(o_o)/    _\\   |\n" +
        "|    /_  /\\/ ^ \\/\\  _\\    |\n" + "|      \\/ | / \\ | \\/      |\n" + "|_______ /((( )))\\ _______|\n" +
        "|      __\\ \\___/ /__      |\n" + "|--- (((---'   '---))) ---|\n" + "|           |  |          |\n" +
        ":           |  |          :\n" + "\\<>        |  |       <>/\n" + " \\<>       |  |      <>/\n" +
        "\\<>      |  |     <>/\n" + "`\\<>    |  |   <>/'\n" + " `\\<>  |  |  <>/'\n" + " `\\<>|  |<>/'\n" +
        " `-.  .-`\n" + " '--'\n"
    };

    public void GenerateName()
    {
        var rnd = new Random();
        if (Type.Equals(ItemTypers.Weapon))
        {
            Name = FirstWeaponPhrase[rnd.Next(0, Item.FirstWeaponPhrase.Length)] + "Меч" + SecondPhrase[rnd.Next(0, Item.SecondPhrase.Length)];
        }
        else if (Type.Equals(ItemTypers.Armor))
        {
            Name = FirstArmorPhrase[rnd.Next(0, Item.FirstArmorPhrase.Length)] + "щит" + SecondPhrase[rnd.Next(0, Item.SecondPhrase.Length)];
        }
        else
        {
            Name = "Аптечку";
        }
    }
    public void GenerateStats(Player player)
    {
        var rnd = new Random();
        if (Type.Equals(ItemTypers.Weapon))
        {
            Value = player.Attack + rnd.Next(1, 4);
        }
        else if (Type.Equals(ItemTypers.Armor))
        {
            Value = player.Armor + rnd.Next(1, 4);
        }
    }
    public void GenerateVisual()
    {
        var rnd = new Random();
        if (Type.Equals(ItemTypers.Weapon))
        {
            Visual = WeaponVisual[rnd.Next(0, WeaponVisual.Length)];
        }
        else if (Type.Equals(ItemTypers.Armor))
        {
            Visual = WeaponVisual[rnd.Next(0, WeaponVisual.Length)];
        }
    }
}

class Medicine : Item
{
    public Medicine(Player player)
    {
        Type = ItemTypers.Medicine;
        GenerateName();
    }
}


class Weapon : Item
{
    public Weapon(Player player)
    {
        Type = ItemTypers.Weapon;
        GenerateName();
        GenerateStats(player);
        GenerateVisual();
    }
}
class Armor : Item
{
    public Armor(Player player)
    {
        Type = ItemTypers.Armor;
        GenerateName();
        GenerateStats(player);
        GenerateVisual();
    }
}


class Chest
{
    public Item Item;
    public Point Position;
    public Floor CurrentFloor;
    public Room CurrentRoom;
    public Chest(Point position, int roomIndex, Floor floor)
    {
        Position = position;
        CurrentFloor = floor;
        CurrentRoom = CurrentFloor.Rooms[roomIndex];
        CurrentFloor.Rooms[roomIndex].Field[Position.Y, Position.X] = 98;
    }
    public void PutItemIntoChest(Player player)
    {
        var rnd = new Random();
        if (rnd.Next(100) < 20)
        {
            Item = new Medicine(player);
        }
        else
        {
            if (rnd.Next(100) < 50)
            {
                Item = new Weapon(player);
            }
            else
            {
                Item = new Armor(player);
            }
        }
    }
}


