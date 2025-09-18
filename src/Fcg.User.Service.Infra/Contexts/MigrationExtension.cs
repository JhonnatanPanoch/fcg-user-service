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

        var adminExists = await dbContext.Usuarios.AnyAsync(u => u.Email == "admin@gmail.com");
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

            // Jogo
            await dbContext.Jogos.AddAsync(new JogoEntity
            {
                Id = jogoId,
                Nome = "Elden Ring",
                Descricao = "Jogo de ação e aventura em mundo aberto",
                Preco = 299.90m,
                Ativo = true
            });

            await dbContext.Jogos.AddAsync(new JogoEntity
            {
                Id = Guid.NewGuid(),
                Nome = "Minecraft",
                Descricao = "Jogo de ação e aventura de construção sandbox",
                Preco = 155.50m,
                Ativo = true
            });

            // Promoção
            await dbContext.Promocoes.AddAsync(new PromocaoEntity
            {
                Id = promocaoId,
                JogoId = jogoId,
                PrecoPromocional = 199.90m,
                DataInicio = DateTime.UtcNow.AddDays(-5),
                DataFim = DateTime.UtcNow.AddDays(5)
            });

            // Jogo Adquirido
            await dbContext.JogosAdquiridos.AddAsync(new JogoAdquiridoEntity
            {
                Id = jogoAdquiridoId,
                UsuarioId = adminId,
                JogoId = jogoId,
                DataAquisicao = DateTime.UtcNow.AddDays(-1),
                PrecoPago = 199.90m
            });

            await dbContext.SaveChangesAsync();
        }

        return app;
    }
}
