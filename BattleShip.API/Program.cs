using BattleShip.API.Entities;
using BattleShip.API.Services;
using BattleShip.Models.DTO;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };
        c.TokenValidationParameters.RoleClaimType = "roles";
        c.TokenValidationParameters.NameClaimType = "https://BattleShipAppIMT.comname";
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder.WithOrigins("https://localhost:5182").AllowAnyHeader().AllowAnyMethod());
});

//builder.Services.AddCors();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
        policy.RequireAuthenticatedUser()
    );
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LeaderBoardContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<LeaderBoardService>();

var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

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
.RequireAuthorization()
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
.RequireAuthorization()
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
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/game/attack/{identifier}/{x}/{y}", (HttpContext httpContext, IServiceProvider serviceProvider, int identifier, int x, int y) =>
{
    var leaderBoardService = serviceProvider.GetRequiredService<LeaderBoardService>();
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    var name = httpContext.User.Claims.FirstOrDefault(c => c.Type == "https://BattleShipAppIMT.comname")?.Value;
    return Results.Ok(GameService.Play(leaderBoardService, game, x, y, name));
})
.WithName("GameAttack")
.RequireAuthorization()
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
.RequireAuthorization()
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
.RequireAuthorization()
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
.RequireAuthorization()
.WithOpenApi();

app.MapPost("/game/cancel", (int identifier) =>
{
    Game? game = GameService.GetGame(identifier);
    if(game == null) return Results.NotFound(new { Message = "Game not found" });
    GameService.CancelMove(game);
    return Results.Ok();
})
.WithName("GameCancel")
.RequireAuthorization()
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
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/leaderboard", (HttpContext httpContext, IServiceProvider serviceProvider) => {
    var leaderBoardService = serviceProvider.GetRequiredService<LeaderBoardService>();
    return Results.Ok(leaderBoardService.GetAll());
})
.WithName("GetLeaderboard")
.RequireAuthorization()
.WithOpenApi();

app.Run();
