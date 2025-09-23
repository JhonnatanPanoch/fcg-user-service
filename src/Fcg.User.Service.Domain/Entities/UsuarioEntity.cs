using Fcg.User.Service.Domain.Enums;

namespace Fcg.User.Service.Domain.Entities;

public class UsuarioEntity : EntityBase
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string SenhaHash { get; set; } = string.Empty;

    public RoleEnum Role { get; set; } = RoleEnum.Usuario;


    public UsuarioEntity()
    {
    }

    public UsuarioEntity(
        Guid id,
        string nome,
        string email,
        string senhaHash,
        RoleEnum role)
    {
        Id = id;
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
    }
}