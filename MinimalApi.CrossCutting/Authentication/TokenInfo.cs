namespace MinimalApi.CrossCutting.Authentication
{
    using System;
    /// <summary>
    /// Classe que representa os dados de um token de autenticação do sistema
    /// </summary>
    /// <param name="accessToken">Token gerado pelo sistema</param>
    /// <param name="tokenId">Identificação do sistema</param>
    /// <param name="clientId">Identificação do usuário</param>
    /// <param name="createdAt">Data de criação</param>
    /// <param name="expiresIn">Data de validade</param>
    public class TokenInfo(String accessToken, Guid tokenId, String clientId, Int64 createdAt, Int64 expiresIn)
    {
        /// <summary>
        /// Token gerado pelo sistema
        /// </summary>
        public String AccessToken { get; private set; } = accessToken;

        /// <summary>
        /// Identificação do sistema
        /// </summary>
        public Guid TokenId { get; private set; } = tokenId;

        /// <summary>
        /// Identificação do usuário
        /// </summary>
        public String ClientId { get; private set; } = clientId;

        /// <summary>
        /// Data de criação
        /// </summary>
        public Int64 CreatedAt { get; private set; } = createdAt;

        /// <summary>
        /// Data de validade
        /// </summary>
        public Int64 ExpiresIn { get; private set; } = expiresIn;
    }
}
