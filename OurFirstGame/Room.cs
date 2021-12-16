using System;
using System.Collections.Generic;
using System.Drawing;


/// <summary>
/// Сущность комнаты
/// </summary>
class Room
{
    public int[,] Field { private set; get; } //Поле комнаты
    public int Rows { private set; get; } // Кол-во строк в комнтае
    public int Columns { private set; get; } // Кол-во столбцов в комнате
    public Point Position { private set; get; } // Позиция команты на глобальной карте
    public int RightColumn { private set; get; } // Позиция правого столбца
    public int BottomRow { private set; get; } // Позиция нижней строки
    public bool HorizontalConection;
    public List<Gate> Gates { private set; get; } // Список врат комнаты
    public int RoomId { private set; get; } // Идентификатор комнаты

    public Room(int rows, int columns, Point position, int roomId)
    {
        Gates = new List<Gate>();
        Field = new int[rows, columns];
        Rows = Field.GetUpperBound(0) + 1;
        Columns = Field.GetUpperBound(1) + 1;
        Position = position;
        RightColumn = Position.X + Columns;
        BottomRow = Position.Y + Rows;
        RoomId = roomId;
        FillField();
    }

    /// <summary>
    /// Заполнение поля
    /// </summary>
    /// <returns>void</returns>
    private void FillField()
    {
        CreateBorders();
    }

    /// <summary>
    ///Создание стен в комнате
    /// </summary>
    /// <returns>void</returns>
    private void CreateBorders()
    {
        //Создаем основные стены
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
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

    /// <summary>
    /// Перерисовка комнаты
    /// </summary>
    /// <returns>void</returns>
    public void ReDrawRoom(Dictionary<int, string> dictionary)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Console.SetCursorPosition(Position.X + j, Position.Y + i);
                Console.Write(dictionary[Field[i, j]]);
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Перерисовка одной клетки в комнате
    /// </summary>
    /// <returns>void</returns>
    public void ReDrawOneCell(int x, int y, Dictionary<int, string> dictionary)
    {
        Console.SetCursorPosition(Position.X + x, Position.Y + y);
        Console.Write(dictionary[Field[y, x]]);
    }

    /// <summary>
    /// Смена позиции игрока в комнате
    /// </summary>
    /// <returns>void</returns>
    public void ChangePlayerPosition(Point oldPlayerPosition, Point playerPosition, Dictionary<int, string> dictionary)
    {
        Console.SetCursorPosition(Position.X + oldPlayerPosition.X, Position.Y + oldPlayerPosition.Y);
        Console.Write(dictionary[Field[oldPlayerPosition.Y, oldPlayerPosition.X]]);

        Console.SetCursorPosition(Position.X + playerPosition.X, Position.Y + playerPosition.Y);
        Console.Write(dictionary[Field[playerPosition.Y, playerPosition.X]]);
    }
}
