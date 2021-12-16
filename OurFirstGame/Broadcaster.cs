using System;

/// <summary>
/// Сущность, отвечающая за комментирование происходящего на экране.
/// </summary>
class Broadcaster
{
    private int Row = 40; //Значение строки, в которой будет написана фраза.

    /// <summary>
    /// Написание переданной фразы на экране.
    /// </summary>
    /// <returns>void</returns>
    private void WritePhrase(string phrase)
    {
        Console.SetCursorPosition(0, Row);
        Console.Write(phrase);
        Console.ReadKey();
        Console.SetCursorPosition(0, Row);
        for (var i = 0; i < phrase.Length; i++)
        {
            Console.Write(" ");
        }
        Console.SetCursorPosition(0, 0);
    }

    /// <summary>
    /// Написание фразы о смерти врага на экране.
    /// </summary>
    /// <returns>void</returns>
    public void EnemyIsDead(Mob enemy)
    {
        var phrase = enemy.Name + " умирает!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о получении урона от врага на экране.
    /// </summary>
    /// <returns>void</returns>
    public void EnemyHit(Mob enemy, Player player)
    {
        var phrase = "";
        if (enemy.Male)
        {
            phrase = "Вы заблокировали своим щитом " + player.Armor + " ед. урона, но " + enemy.Name + " всё же нанес вам " + (enemy.Attack - player.Armor) + " ед. урона!";
        }
        else
        {
            phrase = "Вы заблокировали своим щитом " + player.Armor + " ед. урона, но " + enemy.Name + " всё же нанесла вам " + (enemy.Attack - player.Armor) + " ед. урона!";
        }
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы полном поглащении урона врагом на экране.
    /// </summary>
    /// <returns>void</returns>
    public void EnemyHitBlock(Mob enemy)
    {
        var phrase = "Вы полностью заблокировали урон от " + enemy.NameForBlock + " своим щитом!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы промахе врага по вам на экране.
    /// </summary>
    /// <returns>void</returns>
    public void EnemyMiss(Mob enemy)
    {
        var phrase = enemy.Name + " атакует вас и промахивается!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о нанесении урона врагу на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerHit(Mob enemy, Player player)
    {
        var phrase = "";
        if (enemy.Male)
        {
            phrase = "Вы пробили броню " + enemy.NameForBlock + " и нанесли ему " + (player.Attack - enemy.Armor) + " ед. урона!";
        }
        else
        {
            phrase = "Вы пробили броню " + enemy.NameForBlock + " и нанесли ей " + (player.Attack - enemy.Armor) + " ед. урона!";
        }
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о безуспешной попытке нанести урон врагу на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerHitBlock(Mob enemy, Player player)
    {
        var phrase = "Вам не удалось пробить броню " + enemy.NameForBlock + "!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о промахе по врагу на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerMiss(Mob enemy)
    {
        var phrase = "Вы промазали по " + enemy.NameForHitAndMiss + "!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о смене щита на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerSwapArmor(Chest chest)
    {
        var phrase = "Вы нашли " + chest.Item.Name + "! Ваш показатель брони стал: " + chest.Item.Value;
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о смене меча на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerSwapWeapon(Chest chest)
    {
        var phrase = "Вы взяли в руки " + chest.Item.Name + "! Ваш показатель атаки стал: " + chest.Item.Value;
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о подборе аптечки на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerPickMedicine()
    {
        var phrase = "Вы нашли аптечку!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о лечении игрока на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerHeal()
    {
        var phrase = "Вы использовали аптечку, ваше здоровье на максимуме!";
        WritePhrase(phrase);
    }

    /// <summary>
    /// Написание фразы о смерти игрока на экране.
    /// </summary>
    /// <returns>void</returns>
    public void PlayerLastBreath()
    {
        var phrase = "Вы тянетесь в рюкзак за аптечкой, но не находите её. Ваши Глаза застилает тьма...";
        WritePhrase(phrase);
    }
}
