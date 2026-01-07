using Fcg.User.Service.Application.ClientContracts.GamePurchase;
using Fcg.User.Service.Application.ClientContracts.Jogo;
using Fcg.User.Service.Application.Dtos.Biblioteca;
using Fcg.User.Service.Application.Dtos.Jogo;
using Fcg.User.Service.Application.Interfaces;

namespace Fcg.User.Service.Application.AppServices;
public class BibliotecaAppService : IBibliotecaAppService
{
    private readonly IGamePurchaseServiceClient _purchaseGameService;
    private readonly IGamesServiceClient _jogosServiceClient;

    public BibliotecaAppService(
        IGamePurchaseServiceClient purchaseGameService,
        IGamesServiceClient jogosServiceClient)
    {
        _purchaseGameService = purchaseGameService;
        _jogosServiceClient = jogosServiceClient;
    }

    public async Task<BibliotecaJogosDto> ConsultarJogosBibliotecaAsync(Guid userId)
    {
        var transacoes = await _purchaseGameService.ConsultarTransacaoAsync(userId);

        var jogosTasks = transacoes
            .SelectMany(item => item.Jogos.Select(jogo => new
            {
                item.CompraId,
                jogo.JogoId,
                jogo.DataAquisicao
            }))
            .Select(async x =>
            {
                var detalhes = await _jogosServiceClient.ObterPorId(x.JogoId);
                return new JogoAdquiridoDto
                {
                    CodigoComprovanteCompra = x.CompraId,
                    DataAquisicao = x.DataAquisicao,
                    Jogo = new JogoDto
                    {
                        Id = detalhes.Id,
                        Nome = detalhes.Nome,
                        Descricao = detalhes.Descricao,
                        Preco = detalhes.Preco,
                        Ativo = detalhes.Ativo
                    }
                };
            });

        var jogosAdquiridos = await Task.WhenAll(jogosTasks);

        return new BibliotecaJogosDto(jogosAdquiridos.ToList());
    }
}
