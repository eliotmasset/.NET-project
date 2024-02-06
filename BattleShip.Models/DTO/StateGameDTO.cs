namespace BattleShip.Models.DTO;

public class StateGameDTO
{
    public string GameWinner { get; set; }
    public string StateAttack { get; set; }
    public string Response { get; set; }
    public BoardDto BoardPlayer1 { get; set; }
    public BoardDto BoardPlayer2 { get; set; }
}