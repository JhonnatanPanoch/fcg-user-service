using FluentValidation;

namespace Fcg.User.Service.Application.Dtos.Usuario.Validations;
public class EfetuarLoginDtoValidator : AbstractValidator<EfetuarLoginDto>
{
    public EfetuarLoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória");
    }
}
