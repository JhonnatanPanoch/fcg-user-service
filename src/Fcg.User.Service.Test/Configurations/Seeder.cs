using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Enums;
using Fcg.User.Service.Infra.Contexts;

namespace Fcg.User.Service.Test.Configurations;

public static class Seeder
{
    private static readonly object _lock = new();

    public static AppDbContext Seed(this AppDbContext db)
    {
        lock (_lock)
        {
            if (!db.Usuarios.Any())
            {
                db.Usuarios.Add(
                    new UsuarioEntity()
                    {
                        Id = new Guid("8df82259-adf6-434e-8c79-fad698b35c1a"),
                        Nome = "User1",
                        Email = "User1@email.com",
                        Role = RoleEnum.Usuario,
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("Senha@123")
                    });

                db.Usuarios.Add(
                    new UsuarioEntity()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "UserLogin",
                        Email = "UserLogin@email.com",
                        Role = RoleEnum.Usuario,
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("Senha@123")
                    });

                db.Usuarios.Add(
                    new UsuarioEntity()
                    {
                        Id = new Guid("b7178613-909f-4353-917e-a5e8b18a34a3"),
                        Nome = "Admin1",
                        Email = "Admin@email.com",
                        Role = RoleEnum.Admin,
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("Senha@123")
                    });

                db.Usuarios.Add(
                    new UsuarioEntity()
                    {
                        Id = new Guid("fcaff526-37ed-4409-b874-d9a4deb7eedd"),
                        Nome = "UserDelecao",
                        Email = "UserDelecao@email.com",
                        Role = RoleEnum.Admin,
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("Senha@123")
                    });

                db.Jogos.Add(
                    new JogoEntity()
                    {
                        Id = new Guid("32e60c8c-d5bb-4378-afdd-28c07cc71b91"),
                        Nome = "Jogo 1",
                        Descricao = "Descrição do Jogo 1",
                        Preco = 15M,
                        Ativo = true,
                        Promocoes = null
                    });

                db.Jogos.Add(
                    new JogoEntity()
                    {
                        Id = new Guid("ef29ddbc-350d-4fa3-8a96-96cd828e9b4e"),
                        Nome = "Jogo 2",
                        Descricao = "Descrição do Jogo 2",
                        Preco = 25.5M,
                        Ativo = true,
                        Promocoes = null
                    });

                db.Jogos.Add(
                    new JogoEntity()
                    {
                        Id = new Guid("8fcc98f3-0729-4d9b-9771-0be9a6255410"),
                        Nome = "Jogo 3",
                        Descricao = "Descrição do Jogo 2 - Específico para remoção nos testes automatizados",
                        Preco = 25.5M,
                        Ativo = true,
                        Promocoes = null
                    });

                db.SaveChanges();
            }
        }

        return db;
    }
}
