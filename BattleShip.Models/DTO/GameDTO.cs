namespace BattleShip.Models.DTO;

public class GameDto
{
    public string PlayerName { get; set; }
    public int Identifier { get; set; }
    public BoardDto VisibleBoard { get; set; }
}