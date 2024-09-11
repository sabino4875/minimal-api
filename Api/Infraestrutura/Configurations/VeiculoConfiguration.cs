namespace Minimal.Api.Infraestrutura.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Minimal.Api.Dominio.Entidades;
    using System;
    /// <summary>
    /// Representa o mapeamento da entidade veículo para uma tabela no banco de dados
    /// </summary>
    public sealed class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        /// <summary>
        /// Método que faz o mapeamento da entidade para o banco de dados
        /// </summary>
        /// <param name="builder">Api contendo métodos e rotinas para o mapeamento da entidade</param>
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.ToTable("Veiculos");
            builder.HasKey(e => e.Id).HasName("PK_VEICULOS");
            builder.Property<Int32>(e => e.Id).HasColumnName("Id").IsRequired(true).ValueGeneratedOnAdd();
            builder.Property<String>(e => e.Modelo).HasColumnName("Modelo").IsRequired(true).HasMaxLength(150);
            builder.Property<String>(e => e.Marca).HasColumnName("Marca").IsRequired(true).HasMaxLength(100);
            builder.Property<Int32>(e => e.Ano).HasColumnName("Ano").IsRequired(true);
        }
    }
}
