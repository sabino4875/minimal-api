namespace Minimal.Api.Authentication
{
    using System;
    public class TokenInfo(Guid tokenId, String accessToken, String clientId, Int64 createdAt, Int64 expiresIn)
    {
        private readonly Guid _tokenId = tokenId;
        private readonly String _accessToken = accessToken;
        private readonly String _clientId = clientId;
        private readonly Int64 _createdAt = createdAt;
        private readonly Int64 _expiresIn = expiresIn;

        /// <summary>
        /// Token gerado pelo sistema
        /// </summary>
        public String AccessToken => _accessToken;

        /// <summary>
        /// Identificação do sistema
        /// </summary>
        public Guid TokenId => _tokenId;

        /// <summary>
        /// Identificação do usuário
        /// </summary>
        public String ClientId => _clientId;

        /// <summary>
        /// Data de criação
        /// </summary>
        public Int64 CreatedAt => _createdAt;

        /// <summary>
        /// Data de validade
        /// </summary>
        public Int64 ExpiresIn => _expiresIn;
    }
}
