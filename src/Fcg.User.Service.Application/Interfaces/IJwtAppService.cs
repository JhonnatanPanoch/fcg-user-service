namespace Fcg.User.Service.Application.Interfaces;

public interface IJwtAppService
{
    string GerarToken(Guid idUsuario, string email, string role);
}