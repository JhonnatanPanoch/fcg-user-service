using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Domain.Enums;
using Fcg.User.Service.Domain.Exceptions;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Fcg.User.Service.Test.Configurations;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Fcg.User.Service.Test.Tests.Usuarios;

public class ObterPorIdTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ObterPorIdTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenCache.GetAdminToken(factory.Services));
    }

    [Fact(DisplayName = "Deve retornar um usuário pelo ID")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdValido_Entao_RetornaUsuario()
    {
        /// Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1a";

        /// Act
        var response = await _client.GetAsync($"/api/v1/usuario/{id}");
        var content = JsonConvert.DeserializeObject<UsuarioDto>(await response.Content.ReadAsStringAsync());

        /// Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content.Id.Should().Be(Guid.Parse(id));
        content.Nome.Should().Be("User1");
        content.Email.Should().Be("User1@email.com");
        content.Role.Should().Be(RoleEnum.Usuario);
    }

    [Fact(DisplayName = "Deve retornar Recurso não encontrado")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdInexistente_Entao_RetornaNotFound()
    {
        /// Arrange
        var id = "8df82259-adf6-434e-8c79-fad698b35c1b";

        /// Act
        var response = await _client.GetAsync($"/api/v1/usuario/{id}");
        var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        /// Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        content.Should().NotBeNull();
        content.Message.Should().Be(new NotFoundException().Message);
    }

    [Fact(DisplayName = "Deve retornar erro de validação do ID")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_IdInvalido_Entao_RetornaBadRequest()
    {
        /// Arrange
        var id = Guid.Empty;

        /// Act
        var response = await _client.GetAsync($"/api/v1/usuario/{id}");
        var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

        /// Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().NotBeNull();
        content.Message.Should().Be("Erros de validação");
        content.Details?.Values.SelectMany(v => v).Should().Contain("O parâmetro informado não pode ser vazio.");
    }

    [Fact(DisplayName = "Deve retornar erro por token faltando")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_TokenFaltando_Entao_RetornaUnauthorized()
    {
        /// Arrange
        var id = Guid.Empty;
        _client.DefaultRequestHeaders.Authorization = null;

        /// Act
        var response = await _client.GetAsync($"/api/v1/usuario/{id}");

        /// Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
