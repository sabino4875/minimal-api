namespace MinimalApi.CrossCutting.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System.Globalization;
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using MinimalApi.CrossCutting.Authentication;
    using MinimalApi.Application.Services;
    using MinimalApi.Domain.Helpers;

    /// <summary>
    /// Classe contendo o método para validação do token
    /// </summary>
    public class JwtMiddleware
    {
        private readonly ApplicationRoutines _routines;
        private readonly RequestDelegate _requestDelegate;
        private readonly TokenUtils _tokenUtils;
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="requestDelegate">Função para processamento de uma requisição http</param>
        /// <param name="routines">Classe contendo rotinas de uso geral no sistema</param>
        /// <param name="tokenUtils">Classe contendo rotinas para validação do token do usuário</param>
        public JwtMiddleware(RequestDelegate requestDelegate, ApplicationRoutines routines, TokenUtils tokenUtils)
        {
            ArgumentNullException.ThrowIfNull(requestDelegate);
            ArgumentNullException.ThrowIfNull(routines);
            ArgumentNullException.ThrowIfNull(tokenUtils);

            _routines = routines;
            _requestDelegate = requestDelegate;
            _tokenUtils = tokenUtils;
        }

        /// <summary>
        /// Rotina para execução de forma automática da ação de validação do token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="service">Contém rotinas para manipulação de dados da entidade usuário</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IAdministradorService service)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(service);

            StringValues value = StringValues.Empty;
            if (context.Request.Headers.TryGetValue("Authorization", out value))
            {
                var token = Convert.ToString(value, CultureInfo.CurrentCulture).Split(" ").Last();
                if (_routines.ContainsValue(token))
                {
                    _tokenUtils.ValidateToken(context, token, service);
                }
            }
            await _requestDelegate(context).ConfigureAwait(false);
        }
    }
}
