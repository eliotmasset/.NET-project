using BattleShip.API.Entities;
using BattleShip.Models.DTO;

namespace BattleShip.API.Services;

public class GameService
{
    private static List<Game> games = [];
    public static Game Start() {
        Game game = new();
        games.Add(game);
        return game;
    }

    public static Game? GetGame(int identifier) {
        return games.First(g => g.Identifier == identifier);
    }

    public static StateGameDTO Play(Game game, int x, int y) {
        if(BoardService.IsGameOver(game.BoardPlayer1) || BoardService.IsGameOver(game.BoardPlayer2)) {
            return new StateGameDTO {
                GameWinner = BoardService.IsGameOver(game.BoardPlayer1) ? game.Player2Name : game.Player1Name,
                StateAttack = "",
                Response = "",
                BoardPlayer1 = new BoardDto {
                    Grid = Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(0))
                        .Select(i => Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(1))
                            .Select(j => game.BoardPlayer1.Grid[i, j])
                            .ToArray())
                        .ToArray()
                },
                BoardPlayer2 = new BoardDto {
                    Grid = Enumerable.Range(0, game.BoardPlayer2.Grid.GetLength(0))
                        .Select(i => Enumerable.Range(0, game.BoardPlayer2.Grid.GetLength(1))
                            .Select(j => game.BoardPlayer2.Grid[i, j])
                            .ToArray())
                        .ToArray()
                }
            };
        }
        bool shooted = BoardService.Shoot(game.BoardPlayer2, x, y);
        bool gameOver = false;
        bool iaWin = false;
        int xIA = 0;
        int yIA = 0;
        if(shooted) {
            gameOver = BoardService.IsGameOver(game.BoardPlayer2);
        }
        if(!gameOver) {
            int[] move = game.PlayableMoves[Random.Shared.Next(0, game.PlayableMoves.Count)];
            xIA = move[0];
            yIA = move[1];
            game.PlayableMoves.Remove(move);
            BoardService.Shoot(game.BoardPlayer1, xIA, yIA);
            gameOver = BoardService.IsGameOver(game.BoardPlayer1);
            iaWin = gameOver;
        };
        return new StateGameDTO {
            GameWinner = gameOver ? iaWin ? game.Player2Name : game.Player1Name : "",
            StateAttack = shooted ? "Hit" : "Miss",
            Response = xIA + " " + yIA,
            BoardPlayer1 = new BoardDto {
                Grid = Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(0))
                    .Select(i => Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(1))
                        .Select(j => game.BoardPlayer1.Grid[i, j])
                        .ToArray())
                    .ToArray()
            },
            BoardPlayer2 = gameOver ? new BoardDto {
                Grid = Enumerable.Range(0, game.BoardPlayer2.Grid.GetLength(0))
                    .Select(i => Enumerable.Range(0, game.BoardPlayer2.Grid.GetLength(1))
                        .Select(j => game.BoardPlayer2.Grid[i, j])
                        .ToArray())
                    .ToArray()
            } : null
        };
    }
}