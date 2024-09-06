namespace MinimalApi.CrossCutting.Authentication
{
    using Microsoft.AspNetCore.Http;
    using MinimalApi.Application.Services;
    using System;

    /// <summary>
    /// Contrato contendo os métodos de validação, configuração e geração do token do sistema
    /// </summary>
    public interface ITokenUtils
    {
        /// <summary>
        /// Configuração de utilização e propriedades do token
        /// </summary>
        /// <returns>A classe contendo as confugirações do token</returns>
        Microsoft.IdentityModel.Tokens.TokenValidationParameters GetTokenValidationParameters();

        /// <summary>
        /// Gera o token do usuário para acesso ao sistema
        /// </summary>
        /// <param name="id">Token id</param>
        /// <param name="clientId">Id do cliente</param>
        /// <returns>O token gerado</returns>
        TokenInfo GenerateToken(Guid id, String clientId);

        /// <summary>
        /// Rotina utilizada para validar o token do usuário
        /// </summary>
        /// <param name="context">Classe contendo o contexto da requisição</param>
        /// <param name="token">Token informado pelo usuário</param>
        /// <param name="service">Rotinas para manipulação de dados da entidade usuário</param>
        void ValidateToken(HttpContext context, String token, IAdministradorService service);
    }
}
