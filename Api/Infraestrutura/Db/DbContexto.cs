namespace Minimal.Api.Infraestrutura.Db
{
    using Microsoft.EntityFrameworkCore;
    using Minimal.Api.Infraestrutura.Configurations;
    using Minimal.Api.Dominio.Entidades;
    using System;
    public class DbContexto(DbContextOptions<DbContexto> options) : DbContext(options)
    {
        public DbSet<Administrador> Administradores { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Administrador>(new AdministradorConfiguration());
            modelBuilder.ApplyConfiguration<Veiculo>(new VeiculoConfiguration());
        }
    }
}