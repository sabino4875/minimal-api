using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Minimal.Api.Authentication;
using Minimal.Api.Dominio.Helpers;
using System.Globalization;
using System.Threading.Tasks;
using System;
using Minimal.Api.Dominio.Interfaces;
using System.Linq;

namespace Minimal.Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly ApplicationRoutines _routines;
        private readonly RequestDelegate _requestDelegate;
        private readonly TokenUtils _tokenUtils;

        public JwtMiddleware(RequestDelegate requestDelegate, ApplicationRoutines routines, TokenUtils tokenUtils)
        {
            ArgumentNullException.ThrowIfNull(requestDelegate);
            ArgumentNullException.ThrowIfNull(routines);
            ArgumentNullException.ThrowIfNull(tokenUtils);

            _routines = routines;
            _requestDelegate = requestDelegate;
            _tokenUtils = tokenUtils;
        }

        public async Task Invoke(HttpContext context, IAdministradorServico service)
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
