namespace Minimal.Api.Infraestrutura.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Minimal.Api.Dominio.Extensions;
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Dominio.Enuns;
    using System;

    /// <summary>
    /// Representa o mapeamento da entidade administrador para uma tabela no banco de dados
    /// </summary>
    public class AdministradorConfiguration : IEntityTypeConfiguration<Administrador>
    {
        /// <summary>
        /// Método que faz o mapeamento da entidade para o banco de dados
        /// </summary>
        /// <param name="builder">Api contendo métodos e rotinas para o mapeamento da entidade</param>
        public void Configure(EntityTypeBuilder<Administrador> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var converter = new ValueConverter<PerfilUsuario, String>(
                v => v.GetDescription(),
                v => v.ParseEnum<PerfilUsuario>(PerfilUsuario.Visualizar)
            );

            builder.ToTable("Usuarios");
            builder.HasKey(e => e.Id).HasName("PK_USUARIOS");
            builder.HasAlternateKey(e => e.Email).HasName("UK_USUARIO_EMAIL");
            builder.Property<Int32>(e => e.Id).HasColumnName("Id").IsRequired(true).ValueGeneratedOnAdd();
            builder.Property<String>(e => e.Nome).HasColumnName("Nome").IsRequired(true).HasMaxLength(50);
            builder.Property<String>(e => e.Email).HasColumnName("Email").IsRequired(true).HasMaxLength(100);
            builder.Property<String>(e => e.Senha).HasColumnName("Senha").IsRequired(true).HasMaxLength(128);
            builder.Property(e => e.Perfil).HasColumnName("Perfil").HasColumnType("VARCHAR(20)").IsRequired(true).HasConversion(converter);
        }
    }
}
