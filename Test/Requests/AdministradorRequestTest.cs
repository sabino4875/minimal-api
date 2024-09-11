namespace Minimal.Api.Test.Requests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minimal.Api.Authentication;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Serialization;
    using Minimal.Api.Test.Helpers;
    using System.Net;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    [TestClass]
    public class AdministradorRequestTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
            _testContext = testContext;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TestarMetodoLogin()
        {
            // Arrange
            var loginDTO = new LoginDTO
            {
                Email = "administrador@teste.com",
                Senha = "123456"
            };

            //var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

            // Act
            
            _testContext.WriteLine($"{loginDTO.Email} - {loginDTO.Senha}");
            _testContext.WriteLine(Setup.MockHttpClient.BaseAddress.ToString());
            var response = await Setup.MockHttpClient.PostAsJsonAsync<LoginDTO>("/administrador/login", loginDTO).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _testContext.WriteLine($"result: {result}");


            //var admLogado = JsonSerializer.Deserialize<TokenInfo>(result, TokenInfoContext.Default.TokenInfo); --não funciona

            var obj = JsonDocument.Parse(result);
            var admLogado = new TokenInfo(
                tokenId: obj.RootElement.GetProperty("tokenId").GetGuid(),
                accessToken: obj.RootElement.GetProperty("accessToken").GetString(),
                clientId: obj.RootElement.GetProperty("clientId").GetString(),
                createdAt: obj.RootElement.GetProperty("createdAt").GetInt64(),
                expiresIn: obj.RootElement.GetProperty("expiresIn").GetInt64()
            );

            Assert.IsNotNull(admLogado.ClientId);
            Assert.IsNotNull(admLogado.TokenId);
            Assert.IsNotNull(admLogado.ExpiresIn);
            Assert.IsNotNull(admLogado.AccessToken);
            Assert.IsNotNull(admLogado.CreatedAt);

            _testContext.WriteLine(admLogado.AccessToken);
        }
    }
}