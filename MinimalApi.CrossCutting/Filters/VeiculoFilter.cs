namespace MinimalApi.CrossCutting.Filters
{
    using Microsoft.AspNetCore.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Filtro utilizado nas requisições de uma entidade veículo
    /// </summary>
    public class VeiculoFilter
    {
        /// <summary>
        /// Página atual
        /// </summary>
        public Nullable<Int32> Pagina { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        public String Modelo { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        public String Marca { get; set; }

        /// <summary>
        /// Valida os dados da requisição
        /// </summary>
        /// <param name="context">Contexto http</param>
        /// <param name="_">Parâmetros adicionais - não utilizado</param>
        /// <returns></returns>
        public static ValueTask<VeiculoFilter> BindAsync(HttpContext context, ParameterInfo _)
        {
            ArgumentNullException.ThrowIfNull(context);

            var page = 1;
            var modelo = String.Empty;
            var marca = String.Empty;

            var data = context.Request.Query["pagina"];
            if (Int32.TryParse(data, out Int32 number))
            {
                page = number;
            }

            data = context.Request.Query["modelo"];
            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                modelo = data;
            }

            data = context.Request.Query["marca"];
            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                marca = data;
            }


            var result = new VeiculoFilter { Pagina = page, Modelo = modelo.Trim(), Marca = marca.Trim() };
            return ValueTask.FromResult<VeiculoFilter>(result);
        }
    }
}
