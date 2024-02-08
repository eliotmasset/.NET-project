using BattleShip.API.Entities;
using BattleShip.Models.DTO;

namespace BattleShip.API.Services;

public class GameService
{
    private static List<Game> games = [];

    public static Game Start() {
        Game game = new(games);
        games.Add(game);
        return game;
    }

    public static bool Stop(Game game) {
        games.Remove(game);
        return true;
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
        bool shooted = BoardService.Shoot(game.BoardPlayer2, x, y, game.BoardPlayer2View);
        bool gameOver = false;
        bool iaWin = false;
        int xIA = 0;
        int yIA = 0;
        if(shooted) {
            gameOver = BoardService.IsGameOver(game.BoardPlayer2);
        }
        if(!gameOver) {
            int[] move = getBestMoveIA(game.BoardPlayer1View);
            xIA = move[0];
            yIA = move[1];
            BoardService.Shoot(game.BoardPlayer1, xIA, yIA, game.BoardPlayer1View);
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
            } : new BoardDto {
                Grid = Enumerable.Range(0, game.BoardPlayer2View.Grid.GetLength(0))
                    .Select(i => Enumerable.Range(0, game.BoardPlayer2View.Grid.GetLength(1))
                        .Select(j => game.BoardPlayer2View.Grid[i, j])
                        .ToArray())
                    .ToArray()
            }
        };
    }

    public static int[] getBestMoveIA(Board view) {
        int[] move = new int[2];
        int x = 0;
        int y = 0;
        bool found = false;
        List<int[]> possibleMoves = [];
        for (int i = 0; i < Board.size; i++)
        {
            for (int j = 0; j < Board.size; j++)
            {
                if(view.Grid[i, j] == 'X') {
                    if(i > 0 && view.Grid[i - 1, j] == '\0') {
                        x = i - 1;
                        y = j;
                        found = true;
                        break;
                    } else if(i < Board.size - 1 && view.Grid[i + 1, j] == '\0') {
                        x = i + 1;
                        y = j;
                        found = true;
                        break;
                    } else if(j > 0 && view.Grid[i, j - 1] == '\0') {
                        x = i;
                        y = j - 1;
                        found = true;
                        break;
                    } else if(j < Board.size - 1 && view.Grid[i, j + 1] == '\0') {
                        x = i;
                        y = j + 1;
                        found = true;
                        break;
                    }
                }
                if(view.Grid[i, j] == '\0') {
                    possibleMoves.Add([i, j]);
                }
            }
            if(found) break;
        }
        move[0] = x;
        move[1] = y;
        if(!found) {
            int[] randomMove = possibleMoves[Random.Shared.Next(0, possibleMoves.Count)];
            move = randomMove;
        }
        return move;
    }
}