namespace Fcg.User.Service.Application.Dtos.Conta;

public class JogosContaDto 
{
    public Guid IdComprovante { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime DataAquisicao { get; set; }
    public decimal ValorPago { get; set; }
}