using Fcg.User.Service.Domain.Entities;

namespace Fcg.User.Service.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<UsuarioEntity>
{
    Task<UsuarioEntity?> ObterPorEmailAsync(string email);
}