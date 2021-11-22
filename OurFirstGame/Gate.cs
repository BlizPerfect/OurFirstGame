using System;
using System.Drawing;

class Gate
{
    // 0
    //3 1
    // 2
    public Point GatePosition;
    public int GateId;
    public int NextRoomIndex;
    public int NextGateId;

    public Gate(Point gatePosition, int nextRoomId, int gateId, int nextGateId)
    {
        GatePosition = gatePosition;
        NextRoomIndex = nextRoomId;
        GateId = gateId;
        NextGateId = nextGateId;
    }
}
