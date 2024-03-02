using Microsoft.EntityFrameworkCore;
using BattleShip.Models.DTO;

public class LeaderBoardContext : DbContext
{
    public DbSet<LeaderBoardDTO> LeaderBoards { get; set; }

    public LeaderBoardContext(DbContextOptions<LeaderBoardContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=LeaderBoardDb.sqlite;");
    }
}
