namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Monta uma mensagem de retorno de requisição
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MensagemDTO
    {
        /// <summary>
        /// Mensagem
        /// </summary>
        [JsonProperty("mensagem")]
        public String Mensagem { get; set; }
    }
}
