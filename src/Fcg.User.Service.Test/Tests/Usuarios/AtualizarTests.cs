using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Domain.Exceptions;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Fcg.User.Service.Test.Configurations;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Fcg.User.Service.Test.Tests.Usuarios;

public class AtualizarTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AtualizarTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenCache.GetAdminToken(factory.Services));
    }

    [Fact(DisplayName = "Deve atualizar um usuário existente")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_DadosValidos_Entao_AtualizaUsuarioERetornaNoContent()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1a";
        var dto = new AtualizarUsuarioDto
        {
            Nome = "Novo Nome",
            Email = "novonome@email.com"
        };
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Deve retornar NotFound para usuário inexistente")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdInexistente_Entao_RetornaNotFound()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1b";
        var dto = new AtualizarUsuarioDto
        {
            Nome = "Nome",
            Email = "email@email.com"
        };
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}?id={id}", content);
        var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        error.Should().NotBeNull();
        error.Message.Should().Be(new NotFoundException().Message);
    }

    [Fact(DisplayName = "Deve retornar erro de validação para ID vazio")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdVazio_Entao_RetornaBadRequest()
    {
        // Arrange
        var id = Guid.Empty;
        var dto = new AtualizarUsuarioDto
        {
            Nome = "Nome",
            Email = "email@email.com"
        };
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}", content);
        var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error.Message.Should().Be("Erros de validação");
        error.Details?.Values.SelectMany(v => v).Should().Contain("O parâmetro informado não pode ser vazio.");
    }

    [Fact(DisplayName = "Deve retornar Unauthorized quando token está faltando")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_TokenFaltando_Entao_RetornaUnauthorized()
    {
        // Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1a";
        var dto = new AtualizarUsuarioDto
        {
            Nome = "Nome",
            Email = "email@email.com"
        };
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.PutAsync($"/api/v1/usuario/{id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}