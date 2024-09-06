namespace MinimalApi.InfraStructure.Db
{
    using Microsoft.EntityFrameworkCore;
    using MinimalApi.Domain.Entities;
    using MinimalApi.InfraStructure.Configurations;
    using System;

    /// <summary>
    /// Classe que representa uma conex�o com o banco de dados
    /// </summary>
    /// <param name="options">Op��es de inicializa��o do banco de dados</param>
    public class DbContexto(DbContextOptions<DbContexto> options) : DbContext(options)
    {
        /// <summary>
        /// Representa o acesso aos dados da tabela de usu�rios
        /// </summary>
        public DbSet<Administrador> Administradores { get; set; } = default!;

        /// <summary>
        /// Representa o acesso aos dados da tabela de ve�culos
        /// </summary>
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        /// <summary>
        /// M�todo utilizado nas configura��es de mapeamento das tabelas do banco de dados para as entidades
        /// </summary>
        /// <param name="modelBuilder">Api para configura��o de entidades e o seu mapeamento com o banco de dados</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Administrador>(new AdministradorConfiguration());
            modelBuilder.ApplyConfiguration<Veiculo>(new VeiculoConfiguration());
        }
    }
}