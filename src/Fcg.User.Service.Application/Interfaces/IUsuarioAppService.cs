using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Domain.Enums;

namespace Fcg.User.Service.Application.Interfaces;

public interface IUsuarioAppService
{
    Task<IEnumerable<UsuarioDto>> ObterTodosAsync();
    Task<UsuarioDto?> ObterPorIdAsync(Guid id);
    Task<UsuarioDto?> ObterPorEmailAsync(string email);
    Task<UsuarioDto> CadastrarAsync(CadastrarUsuarioDto dto);
    Task AtualizarAsync(Guid id, AtualizarUsuarioDto dto);
    Task AtualizarRoleAsync(Guid id, RoleEnum role);
    Task RemoverAsync(Guid id);
    Task<string> EfetuarLoginAsync(string email, string senha);
}