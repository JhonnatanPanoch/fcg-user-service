using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.User.Service.Infra.Contexts;
public static class MigrationExtension
{
    public static async Task<WebApplication> ApplyMigrationsWithSeedsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!dbContext.Database.IsRelational())
            return app;

        dbContext.Database.Migrate();

        var adminExists = await dbContext.Usuarios.AnyAsync(u => u.Role == RoleEnum.Admin);
        if (!adminExists)
        {
            var adminId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();
            var promocaoId = Guid.NewGuid();
            var jogoAdquiridoId = Guid.NewGuid();

            // Usuário
            await dbContext.Usuarios.AddAsync(new UsuarioEntity
            {
                Id = adminId,
                Nome = "Admin User",
                Email = "admin@gmail.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = RoleEnum.Admin
            });

            await dbContext.Usuarios.AddAsync(new UsuarioEntity
            {
                Id = userId,
                Nome = "Default User",
                Email = "defuser@gmail.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                Role = RoleEnum.Usuario
            });

            await dbContext.SaveChangesAsync();
        }

        return app;
    }
}
