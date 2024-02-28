using BattleShip.API.Entities;
using BattleShip.API.Services;
using BattleShip.Models.DTO;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder.WithOrigins("http://localhost:5182").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseAuthorization();

app.MapGet("/game/start", () =>
{
    Game game = GameService.Start();
    return new GameDto
    {
        PlayerName = game.Player1Name,
        Identifier = game.Identifier,
        VisibleBoard = new BoardDto
        {
            Grid = Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(0))
                    .Select(i => Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(1))
                        .Select(j => game.BoardPlayer1.Grid[i, j])
                        .ToArray())
                    .ToArray()
        }
    };
})
.WithName("GameStart")
.WithOpenApi();

app.MapGet("/game/board/{identifier}", (int identifier) => {
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    return Results.Ok(new BoardDto
        {
            Grid = Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(0))
                    .Select(i => Enumerable.Range(0, game.BoardPlayer1.Grid.GetLength(1))
                        .Select(j => game.BoardPlayer1.Grid[i, j])
                        .ToArray())
                    .ToArray()
    });
})
.WithName("GetBoard")
.WithOpenApi();

app.MapGet("/game/boardView/{identifier}", (int identifier) => {
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    return Results.Ok(new BoardDto
        {
            Grid = Enumerable.Range(0, game.BoardPlayer2View.Grid.GetLength(0))
                    .Select(i => Enumerable.Range(0, game.BoardPlayer2View.Grid.GetLength(1))
                        .Select(j => game.BoardPlayer2View.Grid[i, j])
                        .ToArray())
                    .ToArray()
    });
})
.WithName("GetBoardView")
.WithOpenApi();

app.MapGet("/game/attack/{identifier}/{x}/{y}", (int identifier, int x, int y) =>
{
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    return Results.Ok(GameService.Play(game, x, y));
})
.WithName("GameAttack")
.WithOpenApi();

app.MapPost("/game/board", (int identifier, BoardDto board) =>
{
    ValidationResult validationResult = new BoardValidator().Validate(board);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    char[,] boardParse = new char[board.Grid.GetLength(0), board.Grid.GetLength(0)];
    for (int i = 0; i < board.Grid.GetLength(0); i++)
    {
        for (int j = 0; j < board.Grid.GetLength(0); j++)
        {
            boardParse[i, j] = board.Grid[i][j];
        }
    }
    game.BoardPlayer1 = new Board(game.BoardPlayer1.size)
    {
        Grid = boardParse
    };
    return Results.Ok();
})
.WithName("GameSetBoard")
.WithOpenApi();

app.MapPost("/game/board/size", (int identifier, int size) =>
{
  if(size <= 5) {
    return Results.BadRequest("Size should be greater than 5");
  }
  Game? game = GameService.GetGame(identifier);
  if(game == null) return Results.NotFound(new { Message = "Game not found" });
  game.setBoardsSize(size);
  return Results.Ok();
})
.WithName("GameSetBoardSize")
.WithOpenApi();

app.MapPost("/game/difficulty", (int identifier, int difficulty) =>
{
  if(difficulty < Game.EASY || difficulty > Game.HARD) {
    return Results.BadRequest("Difficulty should be between 1 and 3");
  }
  Game? game = GameService.GetGame(identifier);
  if(game == null) return Results.NotFound(new { Message = "Game not found" });
  game.difficulty = difficulty;
  return Results.Ok();
})
.WithName("GameSetDifficulty")
.WithOpenApi();

app.MapPost("/game/cancel", (int identifier) =>
{
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    GameService.CancelMove(game);
    return Results.Ok();
})
.WithName("GameCancel")
.WithOpenApi();

app.MapPost("/game/stop", (int identifier) =>
{
    var gameDto = new GameDto();
    ValidationResult validationResult = new GameValidator().Validate(gameDto);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    GameService.Stop(game);
    return Results.Ok();
})
.WithName("GameStop")
.WithOpenApi();

app.Run();
