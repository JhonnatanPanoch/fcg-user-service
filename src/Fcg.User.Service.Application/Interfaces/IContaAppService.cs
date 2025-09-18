using Fcg.User.Service.Application.Dtos.Conta;

namespace Fcg.User.Service.Application.Interfaces;

public interface IContaAppService
{
    Task AtualizarAsync(AtualizarContaDto dto);
    Task DescadastrarAsync();
    Task<ContaDto> ObterAsync();
}