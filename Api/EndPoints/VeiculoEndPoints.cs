namespace MinimalApi.EndPoints
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using FluentValidation;
    using System.Threading.Tasks;
    using MinimalApi.Application.Services;
    using MinimalApi.Domain.Helpers;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Application.Criteria;
    using MinimalApi.CrossCutting.Filters;

    /// <summary>
    /// Endpoints da entidade veículo
    /// </summary>
    public static class VeiculoEndPoints
    {
        /// <summary>
        /// Mapeamento das rotas utilizadas pela entidade veículo
        /// </summary>
        /// <param name="app">Classe utilizadas para mapear o pipeline Http e rotas</param>
        /// <returns>As rotas</returns>
        public static RouteGroupBuilder MappingVeiculoEndPoints(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var resourceName = "Veículos";

            var endpoints = app.MapGroup("/veiculo");

            endpoints.MapGet("/", ListAction)
                .RequireAuthorization(["AuthenticatedUsersPolicy"])
                .WithName($"{resourceName} - Listagem")
                .WithSummary("Listagem de veículos")
                .WithDescription("Efetua a listagem de veículos, de acordo com os critérios informados.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(PagedResultDTO<VeiculoDTO>))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapGet("/{id}", FindAction)
                .RequireAuthorization(["AdminAndEditorPolicy"])
                .WithName($"{resourceName} - Localizar")
                .WithSummary("Localizar veículo")
                .WithDescription("Retorna um veículo cadastrado no sistema, de acordo com os critérios informados.")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status200OK, contentType: "application/json", responseType: typeof(VeiculoDTO))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPost("/", SaveAction)
                .RequireAuthorization(["AdminAndEditorPolicy"])
                .WithName($"{resourceName} - Incluir")
                .WithSummary("Incluir veículo")
                .WithDescription("Cadastra um novo veículo no sistema")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status201Created, contentType: "application/json", responseType: typeof(Created<VeiculoDTO>))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapPut("/", UpdateAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Alterar")
                .WithSummary("Alterar veículo")
                .WithDescription("Altera os dados cadastrais do veículo")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status204NoContent, contentType: "application/json", responseType: typeof(NoContent))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status422UnprocessableEntity, contentType: "application/json", responseType: typeof(UnprocessableEntity<IDictionary<String, String[]>>))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            endpoints.MapDelete("/{id}", DeleteAction)
                .RequireAuthorization(["AdminOnlyPolicy"])
                .WithName($"{resourceName} - Excluir")
                .WithSummary("Exclui veículo")
                .WithDescription("Exclui um veículo de acordo com os critérios informados")
                .WithTags([resourceName])
                .Produces(statusCode: StatusCodes.Status204NoContent, contentType: "application/json", responseType: typeof(NoContent))
                .Produces(statusCode: StatusCodes.Status400BadRequest, contentType: "application/json", responseType: typeof(MensagemDTO))
                .Produces(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json", responseType: typeof(UnauthorizedResult))
                .Produces(statusCode: StatusCodes.Status403Forbidden, contentType: "application/json", responseType: typeof(ForbidResult))
                .Produces(statusCode: StatusCodes.Status404NotFound, contentType: "application/json", responseType: typeof(NotFound))
                .Produces(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/json", responseType: typeof(ProblemDetails))
                .WithOpenApi();

            return endpoints;
        }

        static Results<Ok<PagedResultDTO<VeiculoDTO>>, ProblemHttpResult> ListAction(
            [FromServices] IVeiculoService service,
            [FromServices] ApplicationRoutines routines,
            [AsParameters] VeiculoFilter filter
        )
        {
            var criteria = new VeiculoCriteria
            {
                Pagina = 1,
                Modelo = String.Empty,
                Marca = String.Empty
            };

            if (filter != null)
            {
                criteria.Pagina = filter.Pagina ?? 1;

                if (routines.ContainsValue(filter.Modelo))
                {
                    criteria.Modelo = filter.Modelo;
                }

                if (routines.ContainsValue(filter.Marca))
                {
                    criteria.Marca = filter.Marca;
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
                                            title: "Houve um erro ao tentar listar os veículos",
                                            detail: ex.Message);
            }
        }

        static Results<Ok<VeiculoDTO>, NotFound, BadRequest<MensagemDTO>, ProblemHttpResult> FindAction(
            [FromServices] IVeiculoService service,
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
                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe o id do veículo." });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar localizar o veículo",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<Created<VeiculoDTO>, BadRequest<MensagemDTO>, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> SaveAction(
            [FromServices] IVeiculoService service,
            [FromServices] IValidator<CreateVeiculoDTO> validator,
            [FromServices] ApplicationRoutines routines,
            [FromBody] CreateVeiculoDTO entity
        )
        {
            try
            {
                if (entity != null)
                {
                    var entityValidation = await validator.ValidateAsync(entity).ConfigureAwait(false);
                    if (entityValidation.IsValid)
                    {
                        var createdId = service.Insert(entity);
                        return TypedResults.Created($"/veiculo/{createdId}", new VeiculoDTO
                        {
                            Id = createdId,
                            Marca = entity.Marca,
                            Ano = entity.Ano,
                            Modelo = entity.Modelo
                        });
                    }
                    return TypedResults.UnprocessableEntity(entityValidation.ToDictionary());
                }
                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe os dados do veículo." });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar cadastrar o veículo",
                                            detail: ex.Message);
            }
        }

        static async Task<Results<NoContent, BadRequest<MensagemDTO>, NotFound, UnprocessableEntity<IDictionary<String, String[]>>, ProblemHttpResult>> UpdateAction(
            [FromServices] IVeiculoService service,
            [FromServices] IValidator<VeiculoDTO> validator,
            [FromServices] ApplicationRoutines routines,
            [FromBody] VeiculoDTO entity
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

                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe os dados do veículo" });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar alterar os dados cadastrais do veículo",
                                            detail: ex.Message);
            }
        }

        static Results<NoContent, BadRequest<MensagemDTO>, NotFound, ProblemHttpResult> DeleteAction(
            [FromServices] IVeiculoService service,
            [FromServices] ApplicationRoutines routines,
            [FromQuery] String id
        )
        {
            try
            {
                if (routines.ContainsValue(id))
                {
                    if (Int32.TryParse(id, out Int32 number))
                    {
                        if (service.Delete(number))
                        {
                            return TypedResults.NoContent();
                        }
                        return TypedResults.NotFound();
                    }
                    return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Id informado inválido." });
                }

                return TypedResults.BadRequest(new MensagemDTO { Mensagem = "Por favor, informe o id do veículo." });
            }
            catch (ApplicationException ex)
            {
                return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError,
                                            title: "Houve um erro ao tentar excluir o veículo",
                                            detail: ex.Message);
            }
        }
    }
}
