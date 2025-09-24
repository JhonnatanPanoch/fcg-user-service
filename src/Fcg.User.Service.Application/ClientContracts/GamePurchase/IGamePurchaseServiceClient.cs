namespace Fcg.User.Service.Application.ClientContracts.GamePurchase;
public interface IGamePurchaseServiceClient
{
    /// <summary>
    /// Consulta a API de Compra de Jogos para obter os jogos disponíveis na biblioteca de um usuário.
    /// </summary>
    /// <param name="userId">Identificação do usuário.</param>
    /// <returns>Um DTO com a lista de jogos adquiridos, ou nulo se a chamada falhar.</returns>
    Task<List<TransacaoJogosCompraDto>> ConsultarTransacaoAsync(Guid userId);
}
