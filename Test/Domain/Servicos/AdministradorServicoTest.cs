namespace Minimal.Api.Test.Domain.Servicos
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Minimal.Api.Application.Profiles;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Extensions;
    using Minimal.Api.Dominio.Filter;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.ModelViews;
    using Minimal.Api.Dominio.Servicos;
    using Minimal.Api.Dominio.Settings;
    using Minimal.Api.Infraestrutura.Db;
    using MySqlConnector;

    [TestClass]
    public class AdministradorServicoTest
    {
        private static TestContext _testContext;
        private static IMapper _mapper;
        private static ApplicationRoutines _routines;
        private static AdministradorServico _servico;
        private static DbContexto _contexto;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            ArgumentNullException.ThrowIfNull(testContext);

            var config = new MapperConfiguration(profiles => profiles.AddProfiles([new AdministradorProfile()]));

            _testContext = testContext;
            _contexto = CriarContextoDeTeste();
            _mapper = config.CreateMapper();
            _routines = new ApplicationRoutines();
            _servico = new AdministradorServico(_contexto, _mapper, _routines);
        }

        [ClassCleanup]
        public static void Finish()
        {
            _contexto.Dispose();
        }


        private static DbContexto CriarContextoDeTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var settings = new ApplicationSettings(configuration);

            //configuração da string de conexão com o servidor mysql
            var sqlConnectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = settings.DatabaseSettings.HostName,
                Database = settings.DatabaseSettings.DatabaseName,
                UserID = settings.DatabaseSettings.UserName,
                Password = settings.DatabaseSettings.Password,
                Port = Convert.ToUInt32(settings.DatabaseSettings.Port),
                Pooling = true,
                MinimumPoolSize = 2,
                MaximumPoolSize = 10
            };

            var optionsBuilder = new DbContextOptionsBuilder<DbContexto>();
            optionsBuilder.UseMySql(sqlConnectionStringBuilder.ConnectionString, ServerVersion.AutoDetect(sqlConnectionStringBuilder.ConnectionString));

            return new DbContexto(optionsBuilder.Options);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            // Arrange
            var nome = Faker.Name.FullName();
            var email = Faker.Internet.Email();
            var senha = _routines.ToSHA256Hash("123456");
            var perfil = PerfilUsuario.Invalido;
            while (perfil == PerfilUsuario.Invalido)
            {
                perfil = Faker.Enum.Random<PerfilUsuario>();
            }

            var adm = new AdministradorDTO
            {
                Nome = nome,
                Email = email,
                Senha = senha,
                Perfil = perfil.GetDescription()
            };

            var filter = new CriteriaFilter
            {
                Pagina = 1,
                Nome = nome
            };


            // Act
            _servico.Incluir(adm);

            // Assert
            Assert.AreEqual(1, _servico.Todos(filter).Items.Count());
        }

        [TestMethod]
        public void QuandoBuscaPorIdRetornaUsuario()
        {
            var nome = Faker.Name.FullName();
            var email = Faker.Internet.Email();
            var senha = _routines.ToSHA256Hash("123456");
            var perfil = PerfilUsuario.Invalido;
            while (perfil == PerfilUsuario.Invalido)
            {
                perfil = Faker.Enum.Random<PerfilUsuario>();
            }

            var adm = new AdministradorDTO
            {
                Nome = nome,
                Email = email,
                Senha = senha,
                Perfil = perfil.GetDescription()
            };

            //var administradorServico = CriarServico(context, mapper, rotinas);

            // Act
            var resultId = _servico.Incluir(adm);
            var admDoBanco = _servico.BuscaPorId(resultId);

            // Assert
            Assert.IsNotNull(admDoBanco);
            Assert.AreEqual(resultId, admDoBanco.Id);
        }

        [TestMethod]
        public void QuandoBuscaPorIdRetornaNulo()
        {
            //Arrange
            
            // Act
            var admDoBanco = _servico.BuscaPorId(999999);

            // Assert
            Assert.IsNull(admDoBanco);
        }

        [TestMethod]
        public void QuandoTodosRetornaUsuarios()
        {
            //Arrange
            var filter = new CriteriaFilter
            {
                Pagina = 1,
            };

            // Act
            var users = _servico.Todos(filter);

            // Assert
            Assert.IsInstanceOfType<PagedResultDTO<AdministradorModelView>>(users);
            Assert.IsTrue(users.Items.Any());
        }

    }
}