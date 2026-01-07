using Fcg.User.Service.Domain.Exceptions;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Fcg.User.Service.Test.Configurations;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Fcg.User.Service.Test.Tests.Usuarios;

public class RemoverTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RemoverTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenCache.GetAdminToken(factory.Services));
    }

    [Fact(DisplayName = "Deve remover um usuário existente")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdValido_Entao_RemoveUsuarioERetornaNoContent()
    {
        // Arrange
        var id = "fcaff526-37ed-4409-b874-d9a4deb7eedd";

        // Act
        var response = await _client.DeleteAsync($"/api/v1/usuario/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Deve retornar NotFound ao remover usuário inexistente")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdInexistente_Remover_Entao_RetornaNotFound()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1b";

        // Act
        var response = await _client.DeleteAsync($"/api/v1/usuario/{id}");
        var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        error.Should().NotBeNull();
        error.Message.Should().Be(new NotFoundException().Message);
    }

    [Fact(DisplayName = "Deve retornar erro de validação para ID vazio ao remover")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdVazio_Remover_Entao_RetornaBadRequest()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var response = await _client.DeleteAsync($"/api/v1/usuario/{id}");
        var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error.Message.Should().Be("Erros de validação");
        error.Details?.Values.SelectMany(v => v).Should().Contain("O parâmetro informado não pode ser vazio.");
    }

    [Fact(DisplayName = "Deve retornar Unauthorized ao remover sem token")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_TokenFaltando_Remover_Entao_RetornaUnauthorized()
    {
        // Arrange
        var id = "fcaff526-37ed-4409-b874-d9a4deb7eedd";
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.DeleteAsync($"/api/v1/usuario/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
