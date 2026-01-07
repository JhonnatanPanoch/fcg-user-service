using Fcg.User.Service.Application.ClientContracts.GamePurchase;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fcg.User.Service.Infra.Clients.GamePurchase;

public class GamePurchaseServiceClient(
    IHttpContextAccessor httpContextAccessor,
    IHttpClientFactory httpClientFactory,
    ILogger<BaseHttpClient> logger) : BaseHttpClient(
        httpContextAccessor,
        httpClientFactory, 
        logger,
        "GamePurchaseService"), IGamePurchaseServiceClient
{
    public async Task<List<TransacaoJogosCompraDto>> ConsultarTransacaoAsync(Guid idUsuario)
    {
        return await GetAsync<List<TransacaoJogosCompraDto>>($"api/v1/transacoes/{idUsuario}");
    }
}