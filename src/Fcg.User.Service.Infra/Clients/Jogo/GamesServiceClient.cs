using Fcg.User.Service.Application.ClientContracts.Jogo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fcg.User.Service.Infra.Clients.Jogo;

public class GamesServiceClient(
    IHttpContextAccessor httpContextAccessor,
    IHttpClientFactory httpClientFactory,
    ILogger<BaseHttpClient> logger) : BaseHttpClient(
        httpContextAccessor,
        httpClientFactory, 
        logger, 
        "GamesService"), IGamesServiceClient
{
    public async Task<JogoResponseDto> ObterPorId(Guid jogoId)
    {
        return await GetAsync<JogoResponseDto>($"api/v1/jogos/{jogoId}");
    }
}