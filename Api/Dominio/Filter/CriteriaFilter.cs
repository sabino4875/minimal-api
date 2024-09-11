using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Minimal.Api.Dominio.Filter
{
    public class CriteriaFilter
    {
        public Nullable<Int32> Pagina { get; set; }

        public String Nome { get; set; }
        public String Marca { get; set; }

        public static ValueTask<CriteriaFilter> BindAsync(HttpContext context, ParameterInfo _)
        {
            ArgumentNullException.ThrowIfNull(context);

            String data = context.Request.Query["pagina"];
            var page = 1;
            var nome = String.Empty;
            var marca = String.Empty;

            if (Int32.TryParse(data, out Int32 number))
            {
                page = number;
            }

            data = context.Request.Query["nome"];

            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                nome = data;
            }

            data = context.Request.Query["marca"];

            if (!String.IsNullOrEmpty(data) && !String.IsNullOrWhiteSpace(data))
            {
                marca = data;
            }

            var result = new CriteriaFilter { Pagina = page, Nome = nome, Marca = marca };
            return ValueTask.FromResult<CriteriaFilter>(result);
        }
    }
}
