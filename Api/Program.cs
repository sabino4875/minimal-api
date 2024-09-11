namespace Minimal.Api
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Minimal.Api.Authentication;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.Settings;
    using Minimal.Api.Dominio.Interfaces;
    using Minimal.Api.Dominio. Servicos;
    using Minimal.Api.Infraestrutura.Db;
    using MySqlConnector;
    using System;
    using System.Text;
    using Microsoft.AspNetCore.Builder;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using System.Globalization;
    using Microsoft.Extensions.Hosting;
    using Serilog.Formatting.Compact;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Extensions;
    using Minimal.Api.Dominio.ModelViews;
    using Microsoft.AspNetCore.Http;
    using Minimal.Api.Infraestrutura.InitialData;
    using Minimal.Api.Application.Profiles;
    using FluentValidation;
    using Minimal.Api.Application.Validation;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.EndPoints;
    using Minimal.Api.Middlewares;
    using Microsoft.AspNetCore.Http.Json;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Minimal.Api.Converters;

    sealed class Program
    {
        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            //AutoMapper
            services.AddAutoMapper(settings =>
            {
                settings.AddProfiles([new VeiculoProfile(), new AdministradorProfile()]);
            });

            //singleton classes
            var settings = new ApplicationSettings(configuration);
            var routines = new ApplicationRoutines();
            var tokenUtils = new TokenUtils(settings, routines);

            services.AddSingleton<ApplicationSettings>(settings);
            services.AddSingleton<ApplicationRoutines>(routines);
            services.AddSingleton<TokenUtils>(tokenUtils);

            services.AddSerilog((services, lc) => lc
               .ReadFrom.Services(services)
               .Enrich.FromLogContext()
               .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
               .WriteTo.File(formatter: new CompactJsonFormatter(),
                             path: "./logs/log-.txt",
                             rollingInterval: RollingInterval.Day,
                             rollOnFileSizeLimit: true,
                             retainedFileCountLimit: 10,
                             encoding: Encoding.UTF8
               )
           );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = tokenUtils.GetTokenValidationParameters();
            });

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
                                             PerfilUsuario.Visualizar.GetDescription()]);
                }
            ).AddPolicy(
                "AdminAndEditorPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole([PerfilUsuario.Admin.GetDescription(),
                                             PerfilUsuario.Editor.GetDescription()]);
                }
            ).AddPolicy(
                "AdminOnlyPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole([PerfilUsuario.Admin.GetDescription()]);
                }
            );

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

            //services
            services.AddScoped<IAdministradorServico, AdministradorServico>();
            services.AddScoped<IVeiculoServico, VeiculoServico>();

            //Validation
            services.AddScoped<IValidator<VeiculoDTO>, VeiculoDTOValidation>();
            services.AddScoped<IValidator<VeiculoModelView>, VeiculoModelViewValidation>();

            services.AddScoped<IValidator<AdministradorDTO>, AdministradorDTOValidation>();
            services.AddScoped<IValidator<AdministradorModelView>, AdministradorModelViewValidation>();

            services.AddScoped<IValidator<LoginDTO>, LoginValidation>();
            services.AddScoped<IValidator<AlteraSenhaDTO>, AlteraSenhaValidation>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Minimal Api",
                    Version = "1.0",
                    Description = "Solução do Desafio de projeto referente a um sistema de cadastro de veículos utilizando minimal apis com Entity Framework e o uso de tokens para a validação do usuário. Feito para o bootcamp XP Inc. - Full Stack Developer",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Danilo Aparecido",
                        Url = new Uri("https://www.linkedin.com/in/danilo-aparecido-dos-santos-03101034/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Termos de uso",
                        Url = new Uri("https://app.dio.me/terms/")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor, informe o token abaixo",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        []
                    }
                });
            });

            services.AddDbContext<DbContexto>(options =>
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
            });

            services.Configure<JsonOptions>(options => 
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.WriteIndented = true;
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.SerializerOptions.Converters.Add(new JsonGuidConverter());
            });
        }

        static void ConfigureApplication(WebApplication app)
        {
            //headers de resposta das solicitações
            var policyCollection = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365) // maxage = one year in seconds
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .RemoveServerHeader()
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddObjectSrc().None();
                    builder.AddFormAction().Self();
                    builder.AddFrameAncestors().None();
                })
                .AddCrossOriginOpenerPolicy(builder =>
                {
                    builder.SameOrigin();
                })
                .AddCrossOriginEmbedderPolicy(builder =>
                {
                    builder.RequireCorp();
                })
                .AddCrossOriginResourcePolicy(builder =>
                {
                    builder.SameOrigin();
                }); //.AddCustomHeader("X-My-Test-Header", "Header value")

            //dados iniciais do sistema
            using (IServiceScope scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedData.ImportData(services);
            }

            app.UseSerilogRequestLogging();

            app.UseSecurityHeaders(policyCollection);
            app.UseCors();
            app.UseHsts();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalApi V1"));
            }

            //middlewares
            app.UseApplicationMiddlewares();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/", (HttpContext context) =>
            {
                var builder = new UriBuilder
                {
                    Scheme = context.Request.Scheme,
                    Host = context.Request.Host.Host,
                    Port = context.Request.Host.Port ?? 80,
                    Path = context.Request.PathBase
                };
                builder.Path += "swagger";

                return new Home("Bem vindo a API de veículos - Minimal API", builder.Uri);
            })
            .AllowAnonymous()
            .WithName("GetIndex")
            .WithTags("Home");

            app.MappingAdministradorEndPoints();
            app.MappingVeiculoEndPoints();
        }

        static void Main(String[] args)
        {
            // The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
            // logger configured in `AddSerilog()` below, once configuration and dependency-injection have both been
            // set up successfully.
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
                            .CreateBootstrapLogger();

            Log.Information("Starting up!");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
                builder.Configuration.AddJsonFile("settings.json", false, true);

                Program.ConfigureServices(builder.Services, builder.Configuration);

                //configuração do servidor kestrel
                builder.WebHost.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.AddServerHeader = false;
                    serverOptions.Limits.MaxRequestBodySize = 80_000_000;
                });

                var app = builder.Build();

                Program.ConfigureApplication(app);

                app.Run();
            }
            catch (ApplicationException ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly.");
            }
            finally
            {
                Log.Information("Finishing app!");
                Log.CloseAndFlush();
            }
        }
    }
}