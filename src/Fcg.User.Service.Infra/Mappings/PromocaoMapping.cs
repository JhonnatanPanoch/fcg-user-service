﻿using Fcg.User.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fcg.User.Service.Infra.Mappings;
public class PromocaoMapping : IEntityTypeConfiguration<PromocaoEntity>
{
    public void Configure(EntityTypeBuilder<PromocaoEntity> builder)
    {
        builder.ToTable("Promocoes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PrecoPromocional)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.DataInicio)
            .IsRequired();

        builder.Property(p => p.DataFim)
            .IsRequired();

        builder.Ignore(p => p.Ativa);
    }
}
