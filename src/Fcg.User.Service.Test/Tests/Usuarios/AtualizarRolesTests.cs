using Fcg.User.Service.Domain.Exceptions.Responses;
using Fcg.User.Service.Domain.Exceptions;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Fcg.User.Service.Domain.Enums;
using Fcg.User.Service.Test.Configurations;

namespace Fcg.User.Service.Test.Tests.Usuarios;

public class AtualizarRolesTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AtualizarRolesTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenCache.GetAdminToken(factory.Services));
    }

    [Fact(DisplayName = "Deve atualizar a role de um usuário existente")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_DadosValidos_Entao_AtualizaRoleERetornaNoContent()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1a";
        var content = new StringContent(JsonConvert.SerializeObject(RoleEnum.Admin), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}/role", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Deve retornar NotFound ao atualizar role de usuário inexistente")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdInexistente_AtualizarRole_Entao_RetornaNotFound()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1b";
        var content = new StringContent(JsonConvert.SerializeObject(RoleEnum.Admin), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}/role", content);
        var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        error.Should().NotBeNull();
        error.Message.Should().Be(new NotFoundException().Message);
    }

    [Fact(DisplayName = "Deve retornar erro de validação para ID vazio ao atualizar role")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdVazio_AtualizarRole_Entao_RetornaBadRequest()
    {
        // Arrange
        var id = Guid.Empty;
        var content = new StringContent(JsonConvert.SerializeObject(RoleEnum.Admin), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}/role", content);
        var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error.Message.Should().Be("Erros de validação");
        error.Details?.Values.SelectMany(v => v).Should().Contain("O parâmetro informado não pode ser vazio.");
    }

    [Fact(DisplayName = "Deve retornar Unauthorized ao atualizar role sem token")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_TokenFaltando_AtualizarRole_Entao_RetornaUnauthorized()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1a";
        var content = new StringContent(JsonConvert.SerializeObject(RoleEnum.Admin), Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}/role", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
