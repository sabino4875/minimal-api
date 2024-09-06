namespace Test.Domain.Entidades
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Repositories;
    using MinimalApi.InfraStructure.Db;
    using Moq;
    using System;
    using System.IO;
    using System.Reflection;
    using Xunit;
    using Xunit.Abstractions;

    public class SampleFixture : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class AdministradorRepositoryTest : IClassFixture<SampleFixture>
    {
        private readonly Mock<IAdministradorRepository> mock;
        private readonly ITestOutputHelper _output;
        private readonly SampleFixture _fixture;    
        private Boolean _disposable;

        public AdministradorRepositoryTest(ITestOutputHelper output, SampleFixture fixture) 
        {
            _output = output;
            _fixture = fixture;

            var admin = new Administrador();

            _disposable = true;
            mock = new(MockBehavior.Strict);

            mock.Setup(m => m.Insert(admin)).Returns(() => admin.Id);
            mock.Setup(m => m.Insert(admin)).Throws(exception: new ApplicationException("Houve um erro ao incluir o registro."));
        }

        private DbContexto CriarContextoDeTeste()
        {
            var builder = new ConfigurationBuilder()                
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var options = new DbContextOptions<DbContexto>();
            return new DbContexto(configuration);
        }


        public void TestandoSalvarAdministrador()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administrador();
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);

            // Assert
            Assert.AreEqual(1, administradorServico.Todos(1).Count());
        }

        public void TestandoBuscaPorId()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administrador();
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);
            var admDoBanco = administradorServico.BuscaPorId(adm.Id);

            // Assert
            Assert.AreEqual(1, admDoBanco?.Id);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if(disposing && _disposable)
            {
                _disposable = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AdministradorRepositoryTest()
        {
            Dispose(false);
        }
    }

}