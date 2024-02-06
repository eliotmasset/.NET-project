namespace BattleShip.API.Service;

public class BoardService
{

    public static readonly int size = 10;
    private static readonly char[,] boardPlayer1 = new char[size, size];
    private static readonly char[,] boardPlayer2 = new char[size, size];
    public static readonly int HORIZONTAL = 0;
    public static readonly int VERTICAL = 1;

    private static void CreateBoard(char[,] board, int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                board[i, j] = '\0';
            }
        }
    }

    private static bool CanPlaceShip(char[,] board, int x, int y, int orientation, int shipSize)
    {
        if (orientation == HORIZONTAL)
        {
            if (y + shipSize > size)
            {
                return false;
            }
            for (int i = 0; i < shipSize; i++)
            {
                if (board[x, y + i] != '\0')
                {
                    return false;
                }
            }
        }
        else
        {
            if (x + shipSize > size)
            {
                return false;
            }
            for (int i = 0; i < shipSize; i++)
            {
                if (board[x + i, y] != '\0')
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static void PlaceShip(char[,] board, int shipSize, char identifier)
    {
        int x = Random.Shared.Next(0, size);
        int y = Random.Shared.Next(0, size);
        int orientation = Random.Shared.Next(0, 2);
        while (!CanPlaceShip(board, x, y, orientation, shipSize))
        {
            x = Random.Shared.Next(0, size);
            y = Random.Shared.Next(0, size);
            orientation = Random.Shared.Next(0, 2);
        }
        if (orientation == HORIZONTAL)
        {
            for (int i = 0; i < shipSize; i++)
            {
                board[x, y + i] = identifier;
            }
        }
        else
        {
            for (int i = 0; i < shipSize; i++)
            {
                board[x + i, y] = identifier;
            }
        }
    }

    private static void PlaceShips(char[,] board)
    {
        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    PlaceShip(board, 4, 'A');
                    break;
                case 1:
                    PlaceShip(board, 3, 'B');
                    break;
                case 2:
                    PlaceShip(board, 3, 'C');
                    break;
                case 3:
                    PlaceShip(board, 2, 'D');
                    break;
                case 4:
                    PlaceShip(board, 2, 'E');
                    break;
                case 5:
                    PlaceShip(board, 1, 'F');
                    break;

            }
        }
    }

    public static void InitBoards() {
        CreateBoard(boardPlayer1, size);
        CreateBoard(boardPlayer2, size);
        PlaceShips(boardPlayer1);
        PlaceShips(boardPlayer2);
    }

    public static char[,] GetBoardPlayer1()
    {
        return boardPlayer1;
    }

    public static char[,] GetBoardPlayer2()
    {
        return boardPlayer2;
    }
}