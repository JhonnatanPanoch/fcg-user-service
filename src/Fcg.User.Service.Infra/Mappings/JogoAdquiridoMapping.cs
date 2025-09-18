using Fcg.User.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fcg.User.Service.Infra.Mappings;
public class JogoAdquiridoMapping : IEntityTypeConfiguration<JogoAdquiridoEntity>
{
    public void Configure(EntityTypeBuilder<JogoAdquiridoEntity> builder)
    {
        builder.ToTable("JogosAdquiridos");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.DataAquisicao)
            .IsRequired();

        builder.Property(j => j.PrecoPago)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.HasOne(j => j.Usuario)
            .WithMany(u => u.JogosAdquiridos)
            .HasForeignKey(j => j.UsuarioId);
    }
}
