namespace MinimalApi
{
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Serilog.Formatting.Compact;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using MinimalApi.EndPoints;
    using MinimalApi.ModelViews;
    using Microsoft.AspNetCore.Http;
    using MinimalApi.CrossCutting.IoC;
    using MinimalApi.CrossCutting.Middlewares;
    using MinimalApi.Domain.Settings;
    using MinimalApi.Domain.Helpers;
    using MinimalApi.CrossCutting.Authentication;
    using MinimalApi.InfraStructure.Data;

    internal sealed class Program
    {
        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //singleton classes
            var settings = new ApplicationSettings(configuration);
            var routines = new ApplicationRoutines();
            var tokenUtils = new TokenUtils(settings, routines);

            services.AddSingleton<ApplicationSettings>(settings);
            services.AddSingleton<ApplicationRoutines>(routines);
            services.AddSingleton<TokenUtils>(tokenUtils);

            //configuração do log
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

            //serviços e rotinas do aplicativo
            services.AddInfraStructure(tokenUtils, settings);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
            using (var scope = app.Services.CreateScope())
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

            app.MapGet("/", () =>
            {
                return new Home();
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