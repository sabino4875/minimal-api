namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Representa os dados de cadastro de uma nova entidade usuário
    /// </summary>
    public class CreateAdministradorDTO
    {
        /// <summary>
        /// Nome
        /// </summary>
        [JsonProperty("nome")]
        public String Nome { get; set; } = default!;

        /// <summary>
        /// E-mail
        /// </summary>
        [JsonProperty("email")]
        public String Email { get; set; } = default!;

        /// <summary>
        /// Senha
        /// </summary>
        [JsonProperty("senha")]
        public String Senha { get; set; } = default!;

        /// <summary>
        /// Perfil
        /// </summary>
        [JsonProperty("perfil")]
        public String Perfil { get; set; } = default!;
    }
}
