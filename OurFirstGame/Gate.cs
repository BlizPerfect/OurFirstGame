using System;
using System.Drawing;

/// <summary>
/// Сущность врат - переходы между комнатами.
/// </summary>
class Gate
{
    public Point GatePosition { private set; get; } // Позиция врат в комнате
    public int GateId { private set; get; } // Идентификатор врат
    public int NextRoomIndex { private set; get; } // Идентификатор комнаты, в которую ведут врата
    public int NextGateId { private set; get; } // Идентификатор врат, в которые ведут текущие врата

    public Gate(Point gatePosition, int nextRoomId, int gateId, int nextGateId)
    {
        GatePosition = gatePosition;
        NextRoomIndex = nextRoomId;
        GateId = gateId;
        NextGateId = nextGateId;
    }
}
