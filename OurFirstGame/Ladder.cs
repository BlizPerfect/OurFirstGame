using System;
using System.Drawing;

/// <summary>
/// Сущность лестницы.
/// </summary>
class Ladder
{
    public Point Position { protected set; get; } // Позиция лестницы в комнате
    public int RoomIndex { protected set; get; } // Индекс комнаты, в которой находится лестница
    public Ladder(Point position, int roomIndex)
    {
        Position = position;
        RoomIndex = roomIndex;
    }
}
