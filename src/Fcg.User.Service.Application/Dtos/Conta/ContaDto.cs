namespace Fcg.User.Service.Application.Dtos.Conta;

public class ContaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public List<JogosContaDto> Jogos { get; set; }
}
