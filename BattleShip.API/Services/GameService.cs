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

    public static StateGameDTO Play(LeaderBoardService leaderBoardService, Game game, int x, int y, string username) {
        if(BoardService.IsGameOver(game.BoardPlayer1) || BoardService.IsGameOver(game.BoardPlayer2)) {
            if(BoardService.IsGameOver(game.BoardPlayer2)) {
                leaderBoardService.AddScore(username, 10*game.BoardPlayer1View.size);
            }
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
        game.movesPlayer.Add([x, y]);
        bool gameOver = false;
        bool iaWin = false;
        int xIA = 0;
        int yIA = 0;
        if(shooted) {
            gameOver = BoardService.IsGameOver(game.BoardPlayer2);
        }
        if(!gameOver) {
            int[] move = getBestMoveIA(game.BoardPlayer1View, game.difficulty);
            xIA = move[0];
            yIA = move[1];
            BoardService.Shoot(game.BoardPlayer1, xIA, yIA, game.BoardPlayer1View);
            game.movesIA.Add([xIA, yIA]);
            gameOver = BoardService.IsGameOver(game.BoardPlayer1);
            iaWin = gameOver;
        };
        if(gameOver && !iaWin) {
            leaderBoardService.AddScore(username, 10*game.BoardPlayer1View.size);
        }
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

    public static int[] getBestMoveIA(Board view, int difficulty) {
        int[] move = new int[2];
        int x = 0;
        int y = 0;
        bool found = false;
        List<int[]> possibleMoves = [];
        if(difficulty == Game.HARD || (difficulty == Game.MEDIUM && Random.Shared.Next(0, 2) == 0)) {
            for (int i = 0; i < view.size; i++)
            {
                for (int j = 0; j < view.size; j++)
                {
                    if(view.Grid[i, j] == 'X') {
                        if(i > 0 && view.Grid[i - 1, j] == '\0') {
                            x = i - 1;
                            y = j;
                            found = true;
                            break;
                        } else if(i < view.size - 1 && view.Grid[i + 1, j] == '\0') {
                            x = i + 1;
                            y = j;
                            found = true;
                            break;
                        } else if(j > 0 && view.Grid[i, j - 1] == '\0') {
                            x = i;
                            y = j - 1;
                            found = true;
                            break;
                        } else if(j < view.size - 1 && view.Grid[i, j + 1] == '\0') {
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
        } else {
          for (int i = 0; i < view.size; i++)
          {
              for (int j = 0; j < view.size; j++)
              {
                  if(view.Grid[i, j] == '\0') {
                      possibleMoves.Add([i, j]);
                  }
              }
          }
        }
        move[0] = x;
        move[1] = y;
        if(!found) {
            int[] randomMove = possibleMoves[Random.Shared.Next(0, possibleMoves.Count)];
            move = randomMove;
        }
        return move;
    }

    public static bool CancelMove(Game game) {
      if(game.movesPlayer.Count == 0) return false;
      int xIA = game.movesPlayer[game.movesPlayer.Count - 1][0];
      int yIA = game.movesPlayer[game.movesPlayer.Count - 1][1];
      int xPlayer = game.movesIA[game.movesIA.Count - 1][0];
      int yPlayer = game.movesIA[game.movesIA.Count - 1][1];
      switch(game.BoardPlayer1.Grid[xPlayer, yPlayer]) {
        case 'O':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = '\0';
          break;
        case 'a':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = 'A';
          break;
        case 'b':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = 'B';
          break;
        case 'c':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = 'C';
          break;
        case 'd':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = 'D';
          break;
        case 'e':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = 'E';
          break;
        case 'f':
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = 'F';
          break;
        default:
          game.BoardPlayer1.Grid[xPlayer, yPlayer] = '\0';
          break;
      }
      switch(game.BoardPlayer2.Grid[xIA, yIA]) {
        case 'O':
          game.BoardPlayer2.Grid[xIA, yIA] = '\0';
          break;
        case 'a':
          game.BoardPlayer2.Grid[xIA, yIA] = 'A';
          break;
        case 'b':
          game.BoardPlayer2.Grid[xIA, yIA] = 'B';
          break;
        case 'c':
          game.BoardPlayer2.Grid[xIA, yIA] = 'C';
          break;
        case 'd':
          game.BoardPlayer2.Grid[xIA, yIA] = 'D';
          break;
        case 'e':
          game.BoardPlayer2.Grid[xIA, yIA] = 'E';
          break;
        case 'f':
          game.BoardPlayer2.Grid[xIA, yIA] = 'F';
          break;
        default:
          game.BoardPlayer2.Grid[xIA, yIA] = '\0';
          break;
      }
      game.BoardPlayer1View.Grid[xPlayer, yPlayer] = '\0';
      game.BoardPlayer2View.Grid[xIA, yIA] = '\0';
      game.movesPlayer.RemoveAt(game.movesPlayer.Count - 1);
      game.movesIA.RemoveAt(game.movesIA.Count - 1);
      return true;
    }
}
