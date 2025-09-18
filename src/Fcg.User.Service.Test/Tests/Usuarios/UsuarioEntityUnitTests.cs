using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Enums;
using FluentAssertions;

namespace Fcg.User.Service.Test.Tests.Usuarios;

public class UsuarioEntityUnitTests
{
    [Fact]
    public void ConstrutorPadrao_DeveInicializarPropriedadesComValoresPadrao()
    {
        // Act
        var usuario = new UsuarioEntity();

        // Assert
        usuario.Nome.Should().BeEmpty();
        usuario.Email.Should().BeEmpty();
        usuario.SenhaHash.Should().BeEmpty();
        usuario.Role.Should().Be(RoleEnum.Usuario);
        usuario.JogosAdquiridos.Should().NotBeNull();
        usuario.JogosAdquiridos.Should().BeEmpty();
    }

    [Fact]
    public void ConstrutorParametrizado_DeveAtribuirPropriedadesCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Admin User";
        var email = "admin@gmail.com";
        var senhaHash = "Admin@123";
        var role = RoleEnum.Admin;
        var jogos = new List<JogoAdquiridoEntity>();

        // Act
        var usuario = new UsuarioEntity(id, nome, email, senhaHash, role, jogos);

        // Assert
        usuario.Id.Should().Be(id);
        usuario.Nome.Should().Be(nome);
        usuario.Email.Should().Be(email);
        usuario.SenhaHash.Should().Be(senhaHash);
        usuario.Role.Should().Be(role);
        usuario.JogosAdquiridos.Should().BeSameAs(jogos);
    }
}
