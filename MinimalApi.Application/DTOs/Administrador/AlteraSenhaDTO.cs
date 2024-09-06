namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Representa os dados para acesso ao sistema
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AlteraSenhaDTO
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

        /// <summary>
        /// Código utilizado na validação
        /// </summary>
        [JsonProperty("codigo")]
        public String Codigo { get; set; } = default!;
    }
}
