namespace Minimal.Api.Middlewares
{
    using Microsoft.AspNetCore.Builder;
    public static class RegisterMiddlewares
    {
        public static void UseApplicationMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
