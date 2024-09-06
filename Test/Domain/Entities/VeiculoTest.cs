namespace Api.Application.Domain.Entities
{
    using MinimalApi.Domain.Entities;
    using System;
    using System.Security.Cryptography;
    using Xunit;

    public class VeiculoTest
    {
        [Fact]
        [Trait("Novo veículo", "Testes")]
        public void TestarPropriedadesGetSetVeiculo()
        {
            //Arrange
            var id = RandomNumberGenerator.GetInt32(1, 100);
            var marca = Faker.Address.City();
            var modelo = Faker.Address.StreetAddress();
            var ano = RandomNumberGenerator.GetInt32(1950, 2024);
            //Act
            var veiculo = new Veiculo
            { 
                Id = id,
                Marca = marca,
                Modelo = modelo,
                Ano = ano
            };
            //Assert
            Assert.Equal(id, veiculo.Id);
            Assert.Equal(marca, veiculo.Marca);
            Assert.Equal(modelo, veiculo.Modelo);
            Assert.Equal(ano, veiculo.Ano);
        }
    }
}
