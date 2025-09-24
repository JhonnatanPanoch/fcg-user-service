namespace Fcg.User.Service.Application.ClientContracts.Jogo;
public interface IGamesServiceClient
{
    /// <summary>
    /// Consulta a API de Jogos para obter os detalhes de um jogo por id.
    /// </summary>
    /// <param name="jogoId">ID do jogo a serem consultados.</param>
    /// <returns>Um DTO com jogo e seus detalhes, ou nulo se a chamada falhar.</returns>
    Task<JogoResponseDto> ObterPorId(Guid jogoId);
}
