using Fcg.User.Service.Application.Dtos.Usuario;
using FluentValidation;

namespace Fcg.User.Service.Application.Dtos.Usuario.Validations;
public class CadastrarUsuarioDtoValidator : AbstractValidator<CadastrarUsuarioDto>
{
    public CadastrarUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("Formato de e-mail inválido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres")
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula")
            .Matches(@"\d").WithMessage("A senha deve conter pelo menos um número")
            .Matches(@"[\W_]").WithMessage("A senha deve conter pelo menos um caractere especial");

        RuleFor(x => x.ConfirmaSenha)
            .NotEmpty().WithMessage("A confirmação de senha é obrigatória")
            .Equal(x => x.Senha).WithMessage("A confirmação de senha deve ser igual à senha");
    }
}
