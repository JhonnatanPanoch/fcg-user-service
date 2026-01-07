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

                db.SaveChanges();
            }
        }

        return db;
    }
}
