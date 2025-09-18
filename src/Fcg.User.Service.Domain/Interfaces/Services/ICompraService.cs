using Fcg.User.Service.Domain.Entities;

namespace Fcg.User.Service.Domain.Interfaces.Services;

public interface ICompraService
{
    decimal ObterValorPromocionalJogo(JogoEntity jogo);
}