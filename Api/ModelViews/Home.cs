namespace MinimalApi.ModelViews
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Homepage da api
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Home
    {
        /// <summary>
        /// Mensagem de boas vindas
        /// </summary>
        [JsonProperty("mensagem")]
        public string Mensagem { get => "Bem vindo a API de veículos - Minimal API"; }

        /// <summary>
        /// Acesso a documentação da api
        /// </summary>
        [JsonProperty("doc")]
        public string Doc { get => "/swagger"; }
    }
}