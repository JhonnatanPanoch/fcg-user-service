using Fcg.User.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fcg.User.Service.Infra.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<UsuarioEntity>
{
    public void Configure(EntityTypeBuilder<UsuarioEntity> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Role)
            .IsRequired();
    }
}