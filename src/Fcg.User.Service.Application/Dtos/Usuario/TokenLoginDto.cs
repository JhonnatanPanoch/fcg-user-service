namespace Fcg.User.Service.Application.Dtos.Usuario;
public class TokenLoginDto
{
    public string Token { get; set; }

    public TokenLoginDto(string token)
    {
        Token = token;
    }
}
