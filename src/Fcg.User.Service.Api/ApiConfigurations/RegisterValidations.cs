using Fcg.User.Service.Application.Dtos.Conta.Validations;
using Fcg.User.Service.Application.Dtos.Usuario.Validations;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Fcg.User.Service.Api.ApiConfigurations;
public static class RegisterValidations
{
    public static IServiceCollection AddAbstractValidations(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CadastrarUsuarioDtoValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(AtualizarUsuarioDtoValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(EfetuarLoginDtoValidator).Assembly);
        
        services.AddValidatorsFromAssembly(typeof(AtualizarContaDtoValidator).Assembly);
        
        services.AddFluentValidationAutoValidation(options =>
        {
            options.OverrideDefaultResultFactoryWith<CustomValidatorResult>();
        });
        
        return services;
    }
}