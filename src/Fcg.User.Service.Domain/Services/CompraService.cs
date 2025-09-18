using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Interfaces.Services;

namespace Fcg.User.Service.Domain.Services;
public class CompraService : ICompraService
{
    public decimal ObterValorPromocionalJogo(JogoEntity jogo)
    {
        var promocaoAtiva = jogo.Promocoes.FirstOrDefault(p => p.Ativa);
        if (promocaoAtiva is not null)
            return promocaoAtiva.PrecoPromocional;

        return jogo.Preco;
    }
}
