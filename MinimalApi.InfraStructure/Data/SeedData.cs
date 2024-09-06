namespace MinimalApi.InfraStructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Enuns;
    using MinimalApi.Domain.Helpers;
    using System.Collections.Generic;
    using System;
    using MinimalApi.InfraStructure.Db;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    /// <summary>
    /// Dados iniciais de uso para o sistema
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Importa os dados iniciais utilizados pelas tabelas
        /// </summary>
        /// <param name="serviceProvider">Responsável pela recuperação de serviços registrados no sistema</param>
        /// <returns>Execução do processo</returns>
        public static void ImportData(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            using var context = new DbContexto(
                serviceProvider.GetRequiredService<DbContextOptions<DbContexto>>()
            );

            var routines = serviceProvider.GetRequiredService<ApplicationRoutines>();

            SeedDB(context, routines);
        }

        /// <summary>
        /// Rotina contendo os dados que serão importados
        /// </summary>
        /// <param name="context">Contexto para acesso ao banco de dados</param>
        /// <param name="routines">Rotinas de uso geral no sistema</param>
        private static void SeedDB(DbContexto context, ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(routines);

            if (!context.Administradores.Any())
            {
                var administrador = new Administrador
                {
                    Nome = "Administrador",
                    Email = "administrador@teste.com",
                    Senha = routines.ToSHA256Hash("123456"),
                    Perfil = PerfilUsuario.Admin
                };
                context.Administradores.Add(administrador);
                context.SaveChanges();
            }

            if (!context.Veiculos.Any())
            {
                var veiculos = new List<Veiculo>();
                for (var i = 1; i < 93; i++)
                {
                    veiculos.Add(new Veiculo { Modelo = $"AUMARK 3.5 - 11DT 2.8 4x2 TB Diesel {i}", Marca = $"FOTON {i}", Ano = 1951 + i });
                }

                context.Veiculos.AddRange(veiculos);
                context.SaveChanges();
            }
        }
    }
}
