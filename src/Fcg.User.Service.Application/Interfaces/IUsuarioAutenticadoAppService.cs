using Fcg.User.Service.Domain.Entities;

namespace Fcg.User.Service.Application.Interfaces;

public interface IUsuarioAutenticadoAppService
{
    string? ObterEmail();
    Task<UsuarioEntity> ObterUsuarioAutenticadoAsync();
}