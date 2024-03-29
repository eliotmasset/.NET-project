using BattleShip.API.Services;

namespace BattleShip.API.Entities;

public class Board
{
    public readonly int HORIZONTAL = 0;
    public readonly int VERTICAL = 1;

    public int size = 10;
    public char[,] Grid { get; set; }

    public Board(int _size = 10)
    {
        size = _size;
        Grid = new char[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Grid[i, j] = '\0';
            }
        }
    }

    public void PlaceShips() {
        BoardService.PlaceShips(this);
    }
}
