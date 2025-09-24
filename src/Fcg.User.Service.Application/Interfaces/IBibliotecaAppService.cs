using Fcg.User.Service.Application.Dtos.Biblioteca;

namespace Fcg.User.Service.Application.Interfaces;

public interface IBibliotecaAppService
{
    Task<BibliotecaJogosDto> ConsultarJogosBibliotecaAsync(Guid userId);
}