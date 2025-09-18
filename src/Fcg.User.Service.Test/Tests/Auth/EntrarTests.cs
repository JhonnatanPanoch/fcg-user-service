using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Fcg.User.Service.Test.Configurations;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Fcg.User.Service.Test.Tests.Auth;

public class EntrarTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EntrarTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Sucesso ao efetuar solicitação de login.")]
    [Trait("Api", "AuthController")]
    public async Task Quando_ChamadaValida_Entao_RetornaToken()
    {
        // Arrange
        var requestBody = new LoginDto()
        {
            Email = "UserLogin@email.com",
            Senha = "Senha@123",
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/entrar", content);
        var responseContent = JsonConvert.DeserializeObject<TokenLoginDto>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.Token.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Falha ao efetuar login com a senha incorreta.")]
    [Trait("Api", "AuthController")]
    public async Task Quando_SenhaIncorreta_Entao_RetornaFalha()
    {
        // Arrange
        var requestBody = new LoginDto()
        {
            Email = "User1@email.com",
            Senha = "senhaincorreta",
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/entrar", content);
        var responseContent = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        responseContent.Should().NotBeNull();
        responseContent.Message.Should().Be("Houve um erro ao efetuar login, verifique os dados e tente novamente.");
    }
}
