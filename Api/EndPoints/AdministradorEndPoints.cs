namespace MinimalApi.EndPoints
{
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using MinimalApi.CrossCutting.Authentication;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MinimalApi.Application.Services;
    using MinimalApi.Domain.Helpers;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Application.Criteria;
    using MinimalApi.Domain.Enuns;
    using MinimalApi.Domain.Extensions;
    using MinimalApi.CrossCuttind.Filters;

    /// <summary>
    /// Endpoints da entidade Usuário
    /// </summary>
    public static class AdministradorEndPoints
    {
        /// <summary>
        /// Mapeamento das rotas utilizadas pela entidade administrador
        /// </summary>
        /// <param name="app">Classe utilizadas para mapear o pipeline Http e rotas</param>
        /// <returns>As rotas</returns>
        public static RouteGroupBuilder MappingAdministradorEndPoints(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var resourceName = "Administradores";

            var endpoints = app.MapGroup("/administrador");

            endpoints.MapPost("/login", LoginAction)
                .AllowAnonymous()
                .WithName($"{resourceName} - Login")
                .WithSummary("Login")
                .WithDescription("Efetua o login no sistema, gerando um token de acesso.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(TokenInfo))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapGet("/", ListAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Listagem")
                .WithSummary("Listagem de usuários")
                .WithDescription("Efetua a listagem de usuários, de acordo com os critérios informados.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(PagedResultDTO<AdministradorDTO>))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapGet("/{id}", FindAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Localizar")
                .WithSummary("Localizar usuário")
                .WithDescription("Retorna um usuário cadastrado no sistema, de acordo com os critérios informados.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(AdministradorDTO))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPost("/", SaveAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Incluir")
                .WithSummary("Incluir usuário")
                .WithDescription("Cadastra um novo usuário no sistema")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status201Created, contentType: "application/json", responseType: typeof(Created<AdministradorDTO>))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPut("/", UpdateAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Alterar")
                .WithSummary("Alterar usuário")
                .WithDescription("Altera os dados cadastrais do usuário")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status204NoContent, contentType: "application/json", responseType: typeof(NoContent))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            return endpoints;
        }

        static async Task<Results<Ok<TokenInfo>, UnauthorizedHttpResult, BadRequest<MensagemDTO>, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> LoginAction(
            [FromServices] IAdministradorService service,
            [FromServices] IValidator<LoginDTO> validator,
            [FromServices] ApplicationRoutines routines,
            [FromServices] TokenUtils utils,
            [FromBody] LoginDTO entity
        )
        {
            try
            {
                if (entity != null)
                {
                    var validationResult = await validator.ValidateAsync(entity).ConfigureAwait(false);
                    if (validationResult.IsValid)
                    {
                        var senha = routines.ToSHA256Hash(entity.Senha);
                        entity.Senha = senha;

                        var success = service.Login(entity);
                        if (success)
                        {
                            var info = utils.GenerateToken(Guid.NewGuid(), entity.Email);
                            return TypedResults.Ok(info);
                        }
                        return TypedResults.Unauthorized();
                    }
                    //erros de validação
                    return TypedResults.UnprocessableEntity(validationResult.ToDictionary());
                }

                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe os dados para login" });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar fazer o login",
                                            detail: ex.Message);
            }
        }

        static Results<Ok<PagedResultDTO<AdministradorDTO>>, ProblemHttpResult> ListAction(
            [FromServices] IAdministradorService service,
            [FromServices] ApplicationRoutines routines,
            [AsParameters] AdministradorFilter filter
        )
        {
            var criteria = new AdministradorCriteria
            {
                Pagina = 1,
                Nome = String.Empty,
                Email = String.Empty,
                Perfil = PerfilUsuario.Invalido.GetDescription()
            };

            if (filter != null)
            {
                criteria.Pagina = filter.Pagina ?? 1;

                if (routines.ContainsValue(filter.Nome))
                {
                    criteria.Nome = filter.Nome;
                }

                if (routines.ContainsValue(filter.Email))
                {
                    criteria.Email = filter.Email;
                }

                if (routines.ContainsValue(filter.Perfil))
                {
                    criteria.Perfil = filter.Perfil;
                }
            }

            try
            {
                var result = service.ListAll(criteria);
                return TypedResults.Ok(result);
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar listar os usuários",
                                            detail: ex.Message);
            }
        }

        static Results<Ok<AdministradorDTO>, NotFound, BadRequest<MensagemDTO>, ProblemHttpResult> FindAction(
            [FromServices] IAdministradorService service,
            [FromServices] ApplicationRoutines routines,
            [FromRoute] String id
        )
        {
            try
            {
                if (routines.ContainsValue(id))
                {
                    if (Int32.TryParse(id, out Int32 number))
                    {
                        var result = service.Find(number);
                        if (result != null)
                        {
                            return TypedResults.Ok(result);
                        }
                        return TypedResults.NotFound();
                    }
                    return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Id informado inválido." });
                }
                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe o id do usuário." });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar localizar o usuário",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<Created<AdministradorDTO>, BadRequest<MensagemDTO>, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> SaveAction(
            [FromServices] IAdministradorService service,
            [FromServices] IValidator<CreateAdministradorDTO> validator,
            [FromServices] ApplicationRoutines routines,
            [FromBody] CreateAdministradorDTO entity
        )
        {
            try
            {
                if (entity != null)
                {
                    var entityValidation = await validator.ValidateAsync(entity).ConfigureAwait(false);
                    if (entityValidation.IsValid)
                    {
                        //criptografa a senha
                        var senha = routines.ToSHA256Hash(entity.Senha);
                        entity.Senha = senha;

                        var createdId = service.Insert(entity);
                        return TypedResults.Created($"/administrador/{createdId}", new AdministradorDTO
                        {
                            Id = createdId,
                            Email = entity.Email,
                            Nome = entity.Nome
                        });
                    }
                    return TypedResults.UnprocessableEntity(entityValidation.ToDictionary());
                }
                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe os dados do usuário." });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar cadastrar o usuário",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<NoContent, BadRequest<MensagemDTO>, NotFound, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> UpdateAction(
            [FromServices] IAdministradorService service,
            [FromServices] IValidator<AdministradorDTO> validator,
            [FromServices] ApplicationRoutines routines,
            [FromBody] AdministradorDTO entity
        )
        {
            try
            {
                if (entity != null)
                {
                    var entityValidation = await validator.ValidateAsync(entity).ConfigureAwait(false);
                    if (entityValidation.IsValid)
                    {
                        if (service.Update(entity))
                        {
                            return TypedResults.NoContent();
                        }
                        return TypedResults.NotFound();
                    }
                    return TypedResults.UnprocessableEntity(entityValidation.ToDictionary());
                }

                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe os dados do usuário" });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar alterar os dados cadastrais do usuário",
                                            detail: ex.Message);
            }
        }

    }
}
