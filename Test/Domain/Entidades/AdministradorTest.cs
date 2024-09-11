namespace Minimal.Api.Test.Domain.Entidades
{
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Helpers;

    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arrange
            var routines = new ApplicationRoutines();

            var id = Faker.RandomNumber.Next(1,100);
            var nome = Faker.Name.FullName();
            var email = Faker.Internet.Email();
            var senha = routines.ToSHA256Hash(Faker.Identification.UkNationalInsuranceNumber());
            var perfil = PerfilUsuario.Invalido;
            while(perfil == PerfilUsuario.Invalido)
            {
                perfil = Faker.Enum.Random<PerfilUsuario>();
            }

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
            Assert.AreEqual(id, adm.Id);
            Assert.AreEqual(nome, adm.Nome);
            Assert.AreEqual(email, adm.Email);
            Assert.AreEqual(senha, adm.Senha);
            Assert.AreEqual(perfil, adm.Perfil);
        }
    }
}