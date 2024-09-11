namespace Minimal.Api.Test.Domain.Entidades
{
    using Minimal.Api.Dominio.Entidades;
    using System;

    [TestClass]
    public class VeiculoTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arrange
            var id = Faker.RandomNumber.Next(1, 100);
            var marca = Faker.Lorem.GetFirstWord();
            var modelo = Faker.Lorem.Paragraph();
            var ano = Faker.RandomNumber.Next(1951, DateTime.Now.Year);

            // Act
            var veiculo = new Veiculo
            {
                Id = id,
                Marca = marca,
                Modelo = modelo,
                Ano = ano
            };

            // Assert
            Assert.AreEqual(id, veiculo.Id);
            Assert.AreEqual(marca, veiculo.Marca);
            Assert.AreEqual(modelo, veiculo.Modelo);
            Assert.AreEqual(ano, veiculo.Ano);
        }
    }
}
