namespace Minimal.Api.EndPoints
{
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Minimal.Api.Authentication;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.Interfaces;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.ModelViews;
    using Minimal.Api.Dominio.Filter;

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
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(BadRequestResult))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemHttpResult))
                .WithOpenApi();

            endpoints.MapGet("/", ListAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Listagem")
                .WithSummary("Listagem de usuários")
                .WithDescription("Efetua a listagem de usuários, de acordo com os critérios informados.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(PagedResultDTO<AdministradorModelView>))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedHttpResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidHttpResult))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemHttpResult))
                .WithOpenApi();

            endpoints.MapGet("/{id}", FindAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Localizar")
                .WithSummary("Localizar usuário")
                .WithDescription("Retorna um usuário cadastrado no sistema, de acordo com os critérios informados.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(AdministradorModelView))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(BadRequestResult))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedHttpResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidHttpResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPost("/", SaveAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Incluir")
                .WithSummary("Incluir usuário")
                .WithDescription("Cadastra um novo usuário no sistema")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status201Created, contentType: "application/json", responseType: typeof(Created<AdministradorModelView>))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(BadRequestResult))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedHttpResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidHttpResult))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPut("/{id}", UpdateAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Alterar")
                .WithSummary("Alterar usuário")
                .WithDescription("Altera os dados cadastrais do usuário")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status204NoContent, contentType: "application/json", responseType: typeof(NoContent))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(BadRequestResult))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedHttpResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidHttpResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPut("/", ChangePasswordAction)
                .RequireAuthorization(["AuthenticatedUsersPolicy"])
                .WithName($"{resourceName} - Alterar senha")
                .WithSummary("Alterar senha do usuário")
                .WithDescription("Altera a senha do usuário")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status204NoContent, contentType: "application/json", responseType: typeof(NoContent))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(BadRequestResult))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedHttpResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidHttpResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();


            return endpoints;
        }

        static async Task<Results<Ok<TokenInfo>, UnauthorizedHttpResult, BadRequest, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> LoginAction(
            [FromServices] IAdministradorServico service,
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

                        var success = service.Login(entity.Email, entity.Senha);
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

                return TypedResults.BadRequest();
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar fazer o login",
                                            detail: ex.Message);
            }
        }

        static Results<Ok<PagedResultDTO<AdministradorModelView>>, ProblemHttpResult> ListAction(
            [FromServices] IAdministradorServico service,
            [FromServices] ApplicationRoutines routines,
            [AsParameters] CriteriaFilter filter
        )
        {
            var criteria = new CriteriaFilter
            {
                Nome = String.Empty,
                Pagina = 1
            };

            if (filter != null)
            {
                criteria.Pagina = filter.Pagina ?? 1;

                if (routines.ContainsValue(filter.Nome))
                {
                    criteria.Nome = filter.Nome;
                }
            }

            try
            {
                var result = service.Todos(criteria);
                return TypedResults.Ok(result);
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar listar os usuários",
                                            detail: ex.Message);
            }
        }

        static Results<Ok<AdministradorModelView>, NotFound, BadRequest, ProblemHttpResult> FindAction(
            [FromServices] IAdministradorServico service,
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
                        var result = service.BuscaPorId(number);
                        if (result != null)
                        {
                            return TypedResults.Ok(result);
                        }
                        return TypedResults.NotFound();
                    }
                }
                return TypedResults.BadRequest();
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar localizar o usuário",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<Created<AdministradorModelView>, BadRequest, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> SaveAction(
            [FromServices] IAdministradorServico service,
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
                        //criptografa a senha
                        var senha = routines.ToSHA256Hash(entity.Senha);
                        entity.Senha = senha;

                        var createdId = service.Incluir(entity);
                        return TypedResults.Created($"/administrador/{createdId}", new AdministradorModelView
                        {
                            Id = createdId,
                            Email = entity.Email,
                            Nome = entity.Nome
                        });
                    }
                    return TypedResults.UnprocessableEntity(entityValidation.ToDictionary());
                }
                return TypedResults.BadRequest();
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar cadastrar o usuário",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<NoContent, BadRequest, NotFound, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> UpdateAction(
            [FromServices] IAdministradorServico service,
            [FromServices] IValidator<AdministradorModelView> validator,
            [FromServices] ApplicationRoutines routines,
            [FromBody] AdministradorModelView entity
        )
        {
            try
            {
                if(entity!=null)
                {
                    var entityValidation = await validator.ValidateAsync(entity).ConfigureAwait(false);
                    if (entityValidation.IsValid)
                    {
                        if (service.AlterarDadosCadastrais(entity))
                        {
                            return TypedResults.NoContent();
                        }
                        return TypedResults.NotFound();
                    }
                    return TypedResults.UnprocessableEntity(entityValidation.ToDictionary());

                }
                return TypedResults.BadRequest();
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar alterar os dados cadastrais do usuário",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<NoContent, NotFound, UnprocessableEntity<IDictionary<String, String[]>>, BadRequest, ProblemHttpResult>> ChangePasswordAction(
            [FromServices] IAdministradorServico service,
            [FromServices] IValidator<AlteraSenhaDTO> validator,
            [FromServices] ApplicationRoutines routines,
            [FromBody] AlteraSenhaDTO entity
        )
        {
            try
            {
                if (entity != null)
                {
                    var entityValidation = await validator.ValidateAsync(entity).ConfigureAwait(false);
                    if (entityValidation.IsValid)
                    {
                        if (service.AlteraSenha(entity.Email, entity.Senha, entity.Codigo))
                        {
                            return TypedResults.NoContent();
                        }
                        return TypedResults.NotFound();
                    }
                    return TypedResults.UnprocessableEntity(entityValidation.ToDictionary());
                }

                return TypedResults.BadRequest();
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar alterar a senha do usuário",
                                            detail: ex.Message);
            }
        }
    }
}
