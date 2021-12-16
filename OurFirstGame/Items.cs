using System;
using System.Drawing;

/// <summary>
/// Перечисление возможных типов предметов
/// </summary>
enum ItemTypers
{
    Weapon,
    Armor,
    Medicine
}

/// <summary>
/// Сущность предмета
/// </summary>
class Item
{
    public ItemTypers Type { protected set; get; } // Тип предмета
    public string Name { protected set; get; } // Имя предмета
    public string Visual { protected set; get; } // Визуальное отображение предмета
    public int Value { protected set; get; } // Числовое значение предмета

    //Список первых слов для создания имени меча
    public readonly static string[] FirstWeaponPhrase = new string[]
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

    //Список вторых фраз для создания предмета
    public readonly static string[] SecondPhrase = new string[]
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

    //Список первых слов для создания имени щита
    public readonly static string[] FirstArmorPhrase = new string[]
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

    //Список визуального представления мечей. Ориентировочный максимальный разем - в высоту - 22 ед, в ширину-20.
    //Превышать рекомендуемый размер очень не приветствуется, но можно на 3-4 ед.
    public readonly static string[] WeaponVisual = new string[]
    {
        "(**)\n" +
        "IIII\n" + "####\n" + "HHHH\n" + "HHHH\n" + "####\n" + "___IIII___\n" + " .-`_._\" * *\"_._`-.\n" + "|/``  .`\\/`.  ``\\|\n" +
        "`     }    {     '\n" + ") () (\n" + "( :: )\n" + "| :: |\n" + "| )( |\n" + "| || |\n" + "| || |\n" + "| || |\n" + "| || |\n" +
        "| || |\n" + "( () )\n" + "\\  /\n" + "\\/",

        "/^\\\n" + "|\\ /|\n" + "| | |\n" + "| | |\n" + "| | |\n" + "| | |\n" + "| | |\n" +
        "|||\n" + "|||\n" + "|||\n" + "..  |||  ..\n" + "`\\\\=====//'\n" + "(=)\n" + "(=)\n" + "(=)\n" + "\\U/"
    };

    //Список визуального представления щитов. Ориентировочный максимальный разем - в высоту - 22 ед, в ширину-20.
    //Превышать рекомендуемый размер очень не приветствуется, но можно на 3-4 ед.
    public readonly static string[] ArmorVisual = new string[]
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

    /// <summary>
    /// Генерирование имени для предмета.
    /// </summary>
    /// <returns>void</returns>
    protected void GenerateName()
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

    /// <summary>
    /// Генерирование статов для предмета.
    /// </summary>
    /// <returns>void</returns>
    protected void GenerateStats(Player player)
    {
        var rnd = new Random();
        if (Type.Equals(ItemTypers.Weapon))
        {
            Value = player.Attack + rnd.Next(1, 3);
        }
        else if (Type.Equals(ItemTypers.Armor))
        {
            Value = player.Armor + rnd.Next(1, 3);
        }
    }

    /// <summary>
    /// Генерирование визуального оформления для предмета.
    /// </summary>
    /// <returns>void</returns>
    protected void GenerateVisual()
    {
        var rnd = new Random();
        if (Type.Equals(ItemTypers.Weapon))
        {
            Visual = WeaponVisual[rnd.Next(0, WeaponVisual.Length)];
        }
        else if (Type.Equals(ItemTypers.Armor))
        {
            Visual = ArmorVisual[rnd.Next(0, ArmorVisual.Length)];
        }
    }
}

/// <summary>
/// Сущность аптечки
/// </summary>
class Medicine : Item
{
    public Medicine(Player player)
    {
        Type = ItemTypers.Medicine;
        GenerateName();
    }
}

/// <summary>
/// Сущность меча.
/// </summary>
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

/// <summary>
/// Сущность щита.
/// </summary>
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

/// <summary>
/// Сущность сундука.
/// </summary>
class Chest
{
    public Item Item { private set; get; } // Предмет, лежащий в сундуке
    public Point Position { private set; get; } // Позиция сундука в комнате
    public Floor CurrentFloor { private set; get; } // Текущий этаж для сундука
    public Room CurrentRoom { private set; get; } // Комната, в котором сундук находится

    public Chest(Point position, int roomIndex, Floor floor)
    {
        Position = position;
        CurrentFloor = floor;
        CurrentRoom = CurrentFloor.Rooms[roomIndex];
        CurrentFloor.Rooms[roomIndex].Field[Position.Y, Position.X] = 98;
    }

    /// <summary>
    /// Создание предмета в момент открытия сундука.
    /// </summary>
    /// <returns>void</returns>
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


