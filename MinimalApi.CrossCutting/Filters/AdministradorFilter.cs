namespace MinimalApi.CrossCuttind.Filters
{
    using Microsoft.AspNetCore.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Filtro utilizado nas requisições de uma entidade usuário
    /// </summary>
    public class AdministradorFilter
    {
        /// <summary>
        /// Página atual
        /// </summary>
        public Nullable<Int32> Pagina { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public String Nome { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Perfil
        /// </summary>
        public String Perfil { get; set; }

        /// <summary>
        /// Valida os dados da requisição
        /// </summary>
        /// <param name="context">Contexto http</param>
        /// <param name="_">Parâmetros adicionais - não utilizado</param>
        /// <returns></returns>
        public static ValueTask<AdministradorFilter> BindAsync(HttpContext context, ParameterInfo _)
        {
            ArgumentNullException.ThrowIfNull(context);

            var page = 1;
            var nome = String.Empty;
            var email = String.Empty;
            var perfil = String.Empty;

            var data = context.Request.Query["pagina"];
            if (Int32.TryParse(data, out Int32 number))
            {
                page = number;
            }

            data = context.Request.Query["nome"];
            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                nome = data;
            }

            data = context.Request.Query["email"];
            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                email = data;
            }

            data = context.Request.Query["perfil"];
            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                perfil = data;
            }

            var result = new AdministradorFilter { Pagina = page, Nome = nome.Trim(), Email = email.Trim(), Perfil = perfil.Trim() };
            return ValueTask.FromResult<AdministradorFilter>(result);
        }
    }
}
