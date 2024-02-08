namespace BattleShip.API.Services;

using BattleShip.Models.DTO;
using FluentValidation;

public class BoardValidator : AbstractValidator<BoardDto>
{
    public BoardValidator() 
    {
        RuleFor(x => x.Grid)
            .NotNull().WithMessage("La grille ne peut pas être nulle")
            .Must(grid => grid != null && grid.Length > 0).WithMessage("La grille ne peut pas être vide")
            .Must(grid => grid is char[][]).WithMessage("La grille doit être de type char[][]")
            .Must(grid => grid.Length == 10 && grid.All(row => row.Length == 10)).WithMessage("La grille doit être de taille 10x10");      
    }

    private bool BeValidGrid(char[][] grid) 
    {
        if (grid == null) {
            return false;
        }

        if(grid.Length == 0) {
            return false;
        }

        if (!(grid is char[][])) 
        {
            return false;
        }

        foreach(var row in grid) {
            if (row == null || row.Length == 0)
                return false;
            
            if (row.Length != grid[0].Length) 
            {
                return false;
            }
        }
        return true;
    }
}
