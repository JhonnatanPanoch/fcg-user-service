using Fcg.User.Service.Application.Dtos.Usuario;
using FluentValidation;

namespace Fcg.User.Service.Application.Dtos.Usuario.Validations;
public class AtualizarUsuarioDtoValidator : AbstractValidator<AtualizarUsuarioDto>
{
    public AtualizarUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("Formato de e-mail inválido");
    }
}
