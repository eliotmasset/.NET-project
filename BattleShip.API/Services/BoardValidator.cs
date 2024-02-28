namespace BattleShip.API.Services;

using BattleShip.Models.DTO;
using FluentValidation;

public class BoardValidator : AbstractValidator<BoardDto>
{
    public BoardValidator() 
    {
        RuleFor(x => x.Grid)
            .NotNull().WithMessage("La grille ne peut pas être nulle")
            .Must(grid => grid is char[][]).WithMessage("La grille doit être de type char[][]")
            .Must(grid => grid.Length >= 6 && grid.Length <= 20 && grid.All(row => row.Length == grid.Length)).WithMessage("La grille doit être de taille supérieur à 5 ou inférieur à 21 et être carré");      
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
