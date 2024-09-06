namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Representa os dados para acesso ao sistema
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LoginDTO
    {
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
    }
}

