using BattleShip.API.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

BoardService.InitBoards();

app.MapGet("/board/player1", () =>
{
    char[,] boardPlayer1Char = BoardService.GetBoardPlayer1();
    return Enumerable.Range(0, boardPlayer1Char.GetLength(0))
        .Select(i => Enumerable.Range(0, boardPlayer1Char.GetLength(1))
            .Select(j => boardPlayer1Char[i, j])
            .ToArray())
        .ToArray();
})
.WithName("GetBoardPlayer1")
.WithOpenApi();

app.MapGet("/board/player2", () =>
{
    char[,] boardPlayer2Char = BoardService.GetBoardPlayer1();
    return Enumerable.Range(0, boardPlayer2Char.GetLength(0))
        .Select(i => Enumerable.Range(0, boardPlayer2Char.GetLength(1))
            .Select(j => boardPlayer2Char[i, j])
            .ToArray())
        .ToArray();
})
.WithName("GetBoardPlayer2")
.WithOpenApi();

app.Run();