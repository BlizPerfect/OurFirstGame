using System;

class Broadcaster
{
    private int Row = 40;

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

    public void EnemyIsDead(Mob enemy)
    {
        var phrase = enemy.Name + " умирает!";
        WritePhrase(phrase);
    }
    public void EnemyHit(Mob enemy, Player player)
    {
        var phrase = "";
        if (enemy.Male)
        {
            phrase = "Ваша броня взяла на себя " + player.Armor + " ед. урона, но " + enemy.Name + " всё же нанес вам " + (enemy.Attack - player.Armor) + " ед. урона!";
        }
        else
        {
            phrase = "Ваша броня взяла на себя " + player.Armor + " ед. урона, но " + enemy.Name + " всё же нанесла вам " + (enemy.Attack - player.Armor) + " ед. урона!";
        }
        WritePhrase(phrase);
    }
    public void EnemyHitBlock(Mob enemy)
    {
        var phrase = "Вы полностью заблокировали урон от " + enemy.NameForBlock + " своей бронёй!";
        WritePhrase(phrase);
    }

    public void EnemyMiss(Mob enemy)
    {
        var phrase = enemy.Name + " атакует вас и промахивается!";
        WritePhrase(phrase);
    }
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
    public void PlayerHitBlock(Mob enemy, Player player)
    {
        var phrase = "Вам не удалось пробить броню " + enemy.NameForBlock + "!";
        WritePhrase(phrase);
    }
    public void PlayerMiss(Mob enemy)
    {
        var phrase = "Вы промазали по " + enemy.NameForHitAndMiss + "!";
        WritePhrase(phrase);
    }
}
