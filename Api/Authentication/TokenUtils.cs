namespace Minimal.Api.Authentication
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Claims;
    using System.Text;
    using System;
    using Serilog;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.Settings;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Interfaces;
    using Minimal.Api.Dominio.Extensions;

    /// <summary>
    /// Classe contendo a implementação dos métodos de validação, configuração e geração do token do sistema
    /// </summary>
    public class TokenUtils : ITokenUtils
    {
        private readonly ApplicationSettings _settings;
        private readonly ApplicationRoutines _routines;
        private readonly ILogger _logger;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="settings">Configurações do sistema</param>
        /// <param name="routines">Rotinas de uso geral</param>
        public TokenUtils(ApplicationSettings settings, ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(routines);

            _settings = settings;
            _routines = routines;
            _logger = Log.ForContext<TokenUtils>();
        }

        /// <summary>
        /// Gera o token do usuário para acesso ao sistema
        /// </summary>
        /// <param name="id">Código de identificação do token</param>
        /// <param name="clientId">Identificação do usuário</param>
        /// <returns>O token gerado</returns>
        public TokenInfo GenerateToken(Guid id, String clientId)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(clientId);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(clientId);

            try
            {
                var issuer = _settings.JwtSettings.Issuer;
                var audience = _settings.JwtSettings.Audience;
                var key = Encoding.UTF8.GetBytes(_settings.JwtSettings.Secret);

                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);

                var createdIn = DateTime.Now;
                var expiresIn = createdIn.AddSeconds(_settings.JwtSettings.Seconds);
                var centuryBegin = new DateTime(1970, 1, 1);
                var exp = new TimeSpan(expiresIn.Ticks - centuryBegin.Ticks).TotalSeconds;
                var now = new TimeSpan(createdIn.Ticks - centuryBegin.Ticks).TotalSeconds;

                var claims = new Dictionary<String, Object>
                {
                    ["id"] = clientId,
                    ["jti"] = id.ToString("N")
                    //["perfil"] = administrador.Perfil,
                    //[ClaimTypes.Role] = administrador.Perfil
                };

                var descriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    Issuer = issuer,
                    Audience = audience,
                    Claims = claims,
                    NotBefore = createdIn,
                    Expires = expiresIn,
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                        securityKey,
                        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature,
                        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.Sha256Digest
                    ),
                    TokenType = "JWT"
                };

                var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
                handler.SetDefaultTimesOnTokenCreation = false;
                var jwtToken = handler.CreateToken(descriptor);

                _logger.Information("Token gerado com sucesso.");

                return new TokenInfo(
                    tokenId: id,
                    accessToken:  jwtToken,
                    clientId: clientId,
                    createdAt: Convert.ToInt64(Math.Floor(now)),
                    expiresIn: Convert.ToInt64(Math.Floor(exp))
                );
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar gerar o token.");
                throw;
            }
        }

        /// <summary>
        /// Configuração de utilização e propriedades do token
        /// </summary>
        /// <returns>A classe contendo as confugirações do token</returns>
        public Microsoft.IdentityModel.Tokens.TokenValidationParameters GetTokenValidationParameters()
        {
            try
            {
                var issuer = _settings.JwtSettings.Issuer;
                var audience = _settings.JwtSettings.Audience;
                var key = Encoding.UTF8.GetBytes(_settings.JwtSettings.Secret);

                var result = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //configurações do token
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    //Verifica se o que foi informado no token está em conformidade
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    //verifica se o token informado ainda é valido
                    ValidateLifetime = true,
                    //Verifica a ssinatura do token
                    ValidateIssuerSigningKey = true,
                    //Tempo de tolerância para a expiração de um token
                    //utilizado caso tenha problemas no sincronismo
                    //com outros computadores no processo de comunicação
                    ClockSkew = TimeSpan.Zero
                };
                _logger.Information("Parâmetros gerados com sucesso.");
                return result;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar gerar os parâmetros de validação do token.");
                throw;
            }
        }

        /// <summary>
        /// Rotina utilizada para validar o token do usuário
        /// </summary>
        /// <param name="context">Classe contendo o contexto da requisição</param>
        /// <param name="token">Token informado pelo usuário</param>
        /// <param name="service">Rotinas para manipulação de dados da entidade usuário</param> 
        public void ValidateToken(HttpContext context, String token, IAdministradorServico service)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNullOrEmpty(token);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(token);
            ArgumentNullException.ThrowIfNull(service);

            try
            {
                var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
                var issuer = _settings.JwtSettings.Issuer;
                var audience = _settings.JwtSettings.Audience;
                var key = Encoding.UTF8.GetBytes(_settings.JwtSettings.Secret);

                var validation = handler.ValidateTokenAsync(token, GetTokenValidationParameters()).Result;
                if (validation != null)
                {
                    if (validation.IsValid)
                    {
                        Object value;
                        String id = String.Empty;
                        String clientId = String.Empty;

                        if (validation.Claims.TryGetValue("id", out value))
                        {
                            clientId = Convert.ToString(value, CultureInfo.CurrentCulture);
                        }
                        if (validation.Claims.TryGetValue("jti", out value))
                        {
                            id = Convert.ToString(value, CultureInfo.CurrentCulture);
                        }

                        if (_routines.ContainsValue(clientId) && _routines.ContainsValue(id))
                        {
                            var perfil = service.RecuperaPerfilUsuario(clientId);
                            if (perfil != PerfilUsuario.Invalido)
                            {
                                _logger.Information("Token validado com sucesso.");
                                var identity = new ClaimsIdentity(
                                    [
                                        new Claim("UserId", clientId, ClaimValueTypes.String),
                                        new Claim(ClaimTypes.Role, perfil.GetDescription(), ClaimValueTypes.String),
                                        new Claim("TokenId", id, ClaimValueTypes.String)
                                    ], "Custom");
                                context.User = new ClaimsPrincipal(identity);
                                //context.Items[ClaimTypes.Role] = perfil.GetDescription();
                                //context.Items["user"] = clientId;
                            }
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar validar o token.");
                //não bloquear a execução no caso de falha da validação do token
                //o usuário não foi vinculado ao contexto e não pode acessar as rotas seguras
            }
        }
    }
}
