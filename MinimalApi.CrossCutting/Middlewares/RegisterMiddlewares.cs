namespace MinimalApi.CrossCutting.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Classe estática contendo o método para registro dos middlewares utilizados no sistema
    /// </summary>
    public static class RegisterMiddlewares
    {

        /// <summary>
        /// Registra os middlewares utilizados no sistema
        /// </summary>
        /// <param name="builder">Classe que contem o pipeline de uma requisição</param>
        public static void UseApplicationMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
