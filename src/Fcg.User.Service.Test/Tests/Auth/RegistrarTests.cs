using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Domain.Enums;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Fcg.User.Service.Test.Configurations;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Fcg.User.Service.Test.Tests.Auth;

public class RegistrarTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RegistrarTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Sucesso no registro de um novo usuário.")]
    [Trait("Api", "AuthController")]
    public async Task Quando_ChamadaValida_Entao_RetornaSucesso()
    {
        // Arrange
        var requestBody = new CadastrarUsuarioDto()
        {
            Nome = "User2",
            Email = "User2@email.com",
            Senha = "Senha@123",
            ConfirmaSenha = "Senha@123",
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/registrar", content);
        var responseContent = JsonConvert.DeserializeObject<UsuarioDto>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseContent.Should().NotBeNull();
        responseContent.Id.Should().NotBeEmpty();
        responseContent.Nome.Should().Be(requestBody.Nome);
        responseContent.Email.Should().Be(requestBody.Email);
        responseContent.Role.Should().Be(RoleEnum.Usuario);
    }

    [Fact(DisplayName = "Falha no registro de um novo usuário quando dados de entrada inválidos.")]
    [Trait("Api", "AuthController")]
    public async Task Quando_ParametrosInválidos_Entao_RetornaFalha()
    {
        // Arrange
        var requestBody = new CadastrarUsuarioDto();

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/registrar", content);
        var responseContent = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().NotBeNull();
        responseContent.Details.Should().NotBeNullOrEmpty();
        responseContent.Details.Count.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = "Falha no registro de um novo usuário quando dados de entrada inválidos.")]
    [Trait("Api", "AuthController")]
    public async Task Quando_UsuarioExistente_Entao_RetornaFalha()
    {
        // Arrange
        var requestBody = new CadastrarUsuarioDto()
        {
            Nome = "User1",
            Email = "User1@email.com",
            Senha = "Senha@123",
            ConfirmaSenha = "Senha@123",
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/registrar", content);
        var responseContent = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        responseContent.Should().NotBeNull();
        responseContent.Message.Should().NotBeNullOrEmpty();
    }
}
