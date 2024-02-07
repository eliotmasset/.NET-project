using BattleShip.API.Entities;
using BattleShip.API.Services;
using BattleShip.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder.WithOrigins("http://localhost:5182").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.MapGet("/game/attack/{identifier}/{x}/{y}", (int identifier, int x, int y) =>
{
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    return Results.Ok(GameService.Play(game, x, y));
})
.WithName("GameAttack")
.WithOpenApi();

app.Run();