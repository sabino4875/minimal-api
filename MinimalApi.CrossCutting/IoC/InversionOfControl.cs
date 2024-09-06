namespace MinimalApi.CrossCutting.IoC
{
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Application.Services.Impl;
    using MinimalApi.Application.Services;
    using MinimalApi.Application.Validations;
    using MinimalApi.Domain.Repositories;
    using MinimalApi.InfraStructure.Repositories;
    using System;
    using MinimalApi.Application.Profiles;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using MinimalApi.Domain.Enuns;
    using MinimalApi.Domain.Extensions;
    using MinimalApi.CrossCutting.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using MinimalApi.InfraStructure.Db;
    using MySqlConnector;
    using MinimalApi.Domain.Settings;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Gerenciamento de inicialização e escopo das demais classes utilizadas pelo sistema
    /// </summary>
    public static class InversionOfControl
    {
        /// <summary>
        /// Configuração de tempo de execução dos serviços utilizados no sistema
        /// </summary>
        /// <param name="services">Contém os métodos para inicialização e configuração das classes e bibliotecas utilizadas no sistema</param>
        /// <param name="tokenUtils">Contém os métodos para manipulação de tokens jwt</param>
        /// <param name="settings">Contém as configurações do sistema</param>
        public static void AddInfraStructure(this IServiceCollection services, ITokenUtils tokenUtils, ApplicationSettings settings)
        {
            ArgumentNullException.ThrowIfNull(services);

            //AutoMapper
            services.AddAutoMapper(settings =>
            {
                settings.AddProfiles([new VeiculoProfile(), new AdministradorProfile()]);
            });

            //Repositories
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IAdministradorRepository, AdministradorRepository>();

            //Services
            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<IAdministradorService, AdministradorService>();

            //Validation
            services.AddScoped<IValidator<CreateVeiculoDTO>, CreateVeiculoValidation>();
            services.AddScoped<IValidator<VeiculoDTO>, VeiculoValidation>();

            services.AddScoped<IValidator<CreateAdministradorDTO>, CreateAdministradorValidation>();
            services.AddScoped<IValidator<AdministradorDTO>, UpdateAdministradorValidation>();

            services.AddScoped<IValidator<LoginDTO>, LoginValidation>();

            services.AddAuthorizationBuilder().AddPolicy(
                    "Bearer",
                    new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser().Build()
                ).AddPolicy(
                    "AuthenticatedUsersPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole([PerfilUsuario.Admin.GetDescription(),
                                                 PerfilUsuario.Editor.GetDescription(),
                                                 PerfilUsuario.Visualizar.GetDescription()]
                                               );
                    }
                ).AddPolicy(
                    "AdminAndEditorPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole([PerfilUsuario.Admin.GetDescription(),
                                                 PerfilUsuario.Editor.GetDescription()]
                                               );
                    }
                ).AddPolicy(
                    "AdminOnlyPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole([PerfilUsuario.Admin.GetDescription()]);
                    }
                );

            services.AddDbContext<DbContexto>(
                options =>
                {
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

                    options.UseMySql(sqlConnectionStringBuilder.ConnectionString, ServerVersion.AutoDetect(sqlConnectionStringBuilder.ConnectionString));
                }
            );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenUtils.GetTokenValidationParameters();
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromHours(1);
            });

            services.AddControllers().AddNewtonsoftJson(settings => 
            {
                settings.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                settings.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                settings.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
        }
    }
}
