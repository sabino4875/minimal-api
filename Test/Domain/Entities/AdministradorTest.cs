namespace Test.Domain.Entities
{
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Enuns;
    using System.Security.Cryptography;
    using Xunit;

    public class AdministradorTest
    {
        [Fact]
        [Trait("Novo administrador", "Testes")]
        public void TestarPropriedadesGetSetAdministrador()
        {
            // Arrange
            var id = RandomNumberGenerator.GetInt32(1, 100);
            var nome = Faker.Name.FullName();
            var email = Faker.Internet.Email(nome);
            var senha = Faker.Identification.SocialSecurityNumber();
            var perfil = PerfilUsuario.Admin;

            // Act
            var adm = new Administrador 
            {
                Id = id,
                Nome = nome,
                Email = email,
                Senha = senha,
                Perfil = perfil
            };

            // Assert
            Assert.Equal(id, adm.Id);
            Assert.Equal(nome, adm.Nome);
            Assert.Equal(email, adm.Email);
            Assert.Equal(senha, adm.Senha);
            Assert.Equal(perfil, adm.Perfil);
        }
    }
}