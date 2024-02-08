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
    }
}