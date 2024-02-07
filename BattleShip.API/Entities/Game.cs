namespace BattleShip.API.Entities;

public class Game
{

    public Board BoardPlayer1 { get; set; }
    public Board BoardPlayer2  { get; set; }
    public Board BoardPlayer1View { get; set; }
    public Board BoardPlayer2View { get; set; }
    public int Identifier { get; set; }
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public List<int[]> PlayableMoves { get; set; }

    public Game(List<Game> games)
    {
        BoardPlayer1 = new();
        BoardPlayer2 = new();
        BoardPlayer1.PlaceShips();
        BoardPlayer2.PlaceShips();
        BoardPlayer1View = new();
        BoardPlayer2View = new();
        do {
            Identifier = Random.Shared.Next(0, 999999999);
        } while(games.Any(g => g.Identifier == Identifier));
        Player1Name = "Player 1";
        Player2Name = "Player 2";
        PlayableMoves = [
            [0, 0], [0, 1], [0, 2], [0, 3], [0, 4], [0, 5], [0, 6], [0, 7], [0, 8], [0, 9],
            [1, 0], [1, 1], [1, 2], [1, 3], [1, 4], [1, 5], [1, 6], [1, 7], [1, 8], [1, 9],
            [2, 0], [2, 1], [2, 2], [2, 3], [2, 4], [2, 5], [2, 6], [2, 7], [2, 8], [2, 9],
            [3, 0], [3, 1], [3, 2], [3, 3], [3, 4], [3, 5], [3, 6], [3, 7], [3, 8], [3, 9],
            [4, 0], [4, 1], [4, 2], [4, 3], [4, 4], [4, 5], [4, 6], [4, 7], [4, 8], [4, 9],
            [5, 0], [5, 1], [5, 2], [5, 3], [5, 4], [5, 5], [5, 6], [5, 7], [5, 8], [5, 9],
            [6, 0], [6, 1], [6, 2], [6, 3], [6, 4], [6, 5], [6, 6], [6, 7], [6, 8], [6, 9],
            [7, 0], [7, 1], [7, 2], [7, 3], [7, 4], [7, 5], [7, 6], [7, 7], [7, 8], [7, 9],
            [8, 0], [8, 1], [8, 2], [8, 3], [8, 4], [8, 5], [8, 6], [8, 7], [8, 8], [8, 9],
            [9, 0], [9, 1], [9, 2], [9, 3], [9, 4], [9, 5], [9, 6], [9, 7], [9, 8], [9, 9]
        ];
    }
}