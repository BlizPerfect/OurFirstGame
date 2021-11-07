using System;
using System.Drawing;

class Room
{
    public int[,] Field;
    public int Rows;
    public int Columns;
    public bool IsStart;
    public Point Position;
    public int RightColumn;
    public int BottomRow;
    public bool HorizontalConection;
    public Gate Gate;
    public int RoomId;

    public Room(int rows, int columns, Point position, bool isStart, int roomId)
    {
        Field = new int[rows, columns];
        Rows = Field.GetUpperBound(0) + 1;
        Columns = Field.GetUpperBound(1) + 1;
        Position = position;
        IsStart = isStart;
        RightColumn = Position.X + Columns;
        BottomRow = Position.Y + Rows;
        RoomId = roomId;
        FillField();
    }

    public void FillField()
    {
        CreateBorders();
        if (IsStart)
        {
            Field[1, 1] = 8;
        }
    }

    public void CreateBorders()
    {
        //Создаем основные стены
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                //Console.SetCursorPosition(j, i);
                if (i == 0)
                {
                    if (j == 0)
                    {
                        Field[i, j] = 2;
                    }
                    else if (j == Columns - 1)
                    {
                        Field[i, j] = 3;
                    }
                    else
                    {
                        Field[i, j] = 4;
                    }
                }
                else if (i == Rows - 1)
                {
                    if (j == 0)
                    {
                        Field[i, j] = 5;
                    }
                    else if (j == Columns - 1)
                    {
                        Field[i, j] = 6;
                    }
                    else
                    {
                        Field[i, j] = 4;
                    }
                }
                else if ((j == 0 || j == Columns - 1) && (i != 0 || i != Rows - 1))
                {
                    Field[i, j] = 7;
                }
            }
        }
    }
}
