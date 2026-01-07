using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Test.Configurations;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Fcg.User.Service.Test.Tests.Usuarios;

public class ObterTodosTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ObterTodosTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenCache.GetAdminToken(factory.Services));
    }

    [Fact(DisplayName = "Deve retornar todos os usuários")]
    [Trait("Api", "UsuarioController")]
    public async Task Quando_ExistemUsuarios_Entao_RetornaListaDeUsuarios()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/usuario");
        var content = JsonConvert.DeserializeObject<List<UsuarioDto>>(await response.Content.ReadAsStringAsync());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content.Should().HaveCountGreaterThan(1);
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
