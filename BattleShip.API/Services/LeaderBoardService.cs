using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Models.DTO;

public class LeaderBoardService
{
    private readonly LeaderBoardContext _context;

    public LeaderBoardService(LeaderBoardContext context)
    {
        _context = context;
    }

    public List<LeaderBoardDTO> GetAll()
    {
        return _context.LeaderBoards.OrderBy(lb => lb.Score).ToList();
    }

    public bool AddScore(string name, int score)
    {
        var entry = _context.LeaderBoards.FirstOrDefault(lb => lb.Name == name);
        if (entry != null)
        {
            entry.Score = entry.Score + score;
            _context.LeaderBoards.Update(entry);
            _context.SaveChangesAsync();
            return true;
        } else {
          _context.LeaderBoards.Add(new LeaderBoardDTO { Name = name, Score = score });
          _context.SaveChangesAsync();
          return true;
        }
    }
}
