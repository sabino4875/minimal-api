namespace Minimal.Api.Test.Helpers
{
    using FluentValidation;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Minimal.Api.Application.Validation;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Interfaces;
    using Minimal.Api.Dominio.ModelViews;
    using Minimal.Api.Test.Mocks;
    using System.Net.Http;

    public sealed class Setup
    {
        public const string PORT = "5001";
        private static TestContext _testContext = default!;
        private static WebApplicationFactory<Program> _http = default!;
        private static HttpClient _client = default!;

        public static HttpClient MockHttpClient => _client;

        public static void ClassInit(TestContext testContext)
        {
            Setup._testContext = testContext;
            Setup._http = new WebApplicationFactory<Program>();

            Setup._http = Setup._http.WithWebHostBuilder(builder =>
            {
                builder.
                //UseSetting("https_port", Setup.PORT).
                UseEnvironment("Testing");

                builder.UseUrls("https://localhost:7005");

                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
                    services.AddScoped<IVeiculoServico, VeiculoServicoMock>();

                    services.AddScoped<IValidator<VeiculoDTO>, VeiculoDTOValidation>();
                    services.AddScoped<IValidator<VeiculoModelView>, VeiculoModelViewValidation>();

                    services.AddScoped<IValidator<AdministradorDTO>, AdministradorDTOValidation>();
                    services.AddScoped<IValidator<AdministradorModelView>, AdministradorModelViewValidation>();

                    services.AddScoped<IValidator<LoginDTO>, LoginValidation>();
                    services.AddScoped<IValidator<AlteraSenhaDTO>, AlteraSenhaValidation>();
                });
            });

            Setup._client = Setup._http.CreateClient();
        }

        public static void ClassCleanup()
        {
            Setup._http.Dispose();
        }
    }
}