namespace Minimal.Api.Dominio.Settings
{
    using System;

    /// <summary>
    /// Classe que representa as configurações de um token
    /// </summary>
    public sealed class JwtItemSettings
    {
        private readonly String _issuer;
        private readonly String _audience;
        private readonly String _secret;
        private readonly Int32 _seconds;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="issuer">Uma sequência de caracteres que identifica quem emitiu o token</param>
        /// <param name="audience">Uma sequência ou matriz de sequências que identifica os destinatários aos quais o token se destina</param>
        /// <param name="secret">Chave utilizada na proteção e validação do token</param>
        /// <param name="seconds">Duração do token</param>
        public JwtItemSettings(String issuer, String audience, String secret, Int32 seconds)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(issuer);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(audience);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(secret);

            ArgumentNullException.ThrowIfNullOrEmpty(issuer);
            ArgumentNullException.ThrowIfNullOrEmpty(audience);
            ArgumentNullException.ThrowIfNullOrEmpty(secret);

            ArgumentNullException.ThrowIfNull(seconds);

            _issuer = issuer;
            _audience = audience;
            _secret = secret;
            _seconds = seconds;
        }


        /// <summary>
        /// Uma sequência de caracteres que identifica quem emitiu o token
        /// </summary>
        public String Issuer => _issuer;

        /// <summary>
        /// Uma sequência ou matriz de sequências que identifica os destinatários aos quais o token se destina
        /// </summary>
        public String Audience => _audience;

        /// <summary>
        /// Chave utilizada na proteção e validação do token
        /// </summary>
        public String Secret => _secret;

        /// <summary>
        /// Duração do token
        /// </summary>
        public Int32 Seconds => _seconds;
    }
}
