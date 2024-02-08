namespace BattleShip.API.Services;

using BattleShip.Models.DTO;
using FluentValidation;

public class GameValidator: AbstractValidator<GameDto>
{
    public GameValidator()
    {
        RuleFor(data => data.PlayerName).ToString();
        RuleFor(data => data.Identifier).NotNull()
        .Must(identifier => int.TryParse(identifier.ToString(), out _))
        .WithMessage("L'identifiant doit être un entier valide");
    }
}