namespace Fcg.User.Service.Application.Dtos.Jogo;
public class JogoAdquiridoDto
{
    public Guid CodigoComprovanteCompra { get; set; }
    public DateTime DataAquisicao { get; set; }
    public JogoDto Jogo { get; set; }
}
