namespace BattleShip.API.Services;

using BattleShip.API.Entities;

public class BoardService
{
    public static readonly int HORIZONTAL = 0;
    public static readonly int VERTICAL = 1;

    public static bool CanPlaceShip(Board board, int x, int y, int orientation, int shipSize)
    {
        if (orientation == HORIZONTAL)
        {
            if (y + shipSize > Board.size)
            {
                return false;
            }
            for (int i = 0; i < shipSize; i++)
            {
                if (board.Grid[x, y + i] != '\0')
                {
                    return false;
                }
            }
        }
        else
        {
            if (x + shipSize > Board.size)
            {
                return false;
            }
            for (int i = 0; i < shipSize; i++)
            {
                if (board.Grid[x + i, y] != '\0')
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static void PlaceShip(Board board, int shipSize, char identifier)
    {
        int x = Random.Shared.Next(0, Board.size);
        int y = Random.Shared.Next(0, Board.size);
        int orientation = Random.Shared.Next(0, 2);
        while (!CanPlaceShip(board, x, y, orientation, shipSize))
        {
            x = Random.Shared.Next(0, Board.size);
            y = Random.Shared.Next(0, Board.size);
            orientation = Random.Shared.Next(0, 2);
        }
        if (orientation == HORIZONTAL)
        {
            for (int i = 0; i < shipSize; i++)
            {
                board.Grid[x, y + i] = identifier;
            }
        }
        else
        {
            for (int i = 0; i < shipSize; i++)
            {
                board.Grid[x + i, y] = identifier;
            }
        }
    }

    public static void PlaceShips(Board board)
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

    private static char GetLowerCase(char c) {
        switch(c) {
            case 'A':
                return 'a';
            case 'B':
                return 'b';
            case 'C':
                return 'c';
            case 'D':
                return 'd';
            case 'E':
                return 'e';
            case 'F':
                return 'f';
            default:
                return '\0';
        }
    }

    public static bool Shoot(Board board, int x, int y, Board view) {
        if(x < 0 || x >= Board.size || y < 0 || y >= Board.size) {
            return false;
        }
        char[] arr =  ['a', 'b', 'c', 'd', 'e', 'f'];
        if(arr.Contains(board.Grid[x, y])) {
            return true;
        }
        if(board.Grid[x, y] != '\0' && board.Grid[x, y] != 'O') {
            board.Grid[x, y] = GetLowerCase(board.Grid[x, y]);
            view.Grid[x, y] = 'X';
            return true;
        }
        board.Grid[x, y] = 'O';
        view.Grid[x, y] = 'O';
        return false;
    }

    public static bool IsGameOver(Board board) {
        char[] arr =  ['a', 'b', 'c', 'd', 'e', 'f', 'O', '\0'];
        for (int i = 0; i < Board.size; i++)
        {
            for (int j = 0; j < Board.size; j++)
            {
                if(!arr.Contains(board.Grid[i, j])) {
                    return false;
                }
            }
        }
        return true;
    }
}