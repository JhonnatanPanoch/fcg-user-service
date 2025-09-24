using Fcg.User.Service.Application.Dtos.Jogo;

namespace Fcg.User.Service.Application.Dtos.Biblioteca;
public class BibliotecaJogosDto
{
    public List<JogoAdquiridoDto> Jogos { get; set; }

    public BibliotecaJogosDto(List<JogoAdquiridoDto> jogos)
    {
        Jogos = jogos;
    }

    public BibliotecaJogosDto()
    {
        Jogos = [];
    }
}
