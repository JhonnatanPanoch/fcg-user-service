﻿namespace Fcg.User.Service.Application.ClientContracts.GamePurchase;
public class TransacaoJogosCompraDto
{
    public Guid CompraId { get; set; }
    public IEnumerable<TransacaoJogosDto> Jogos { get; set; }
}
