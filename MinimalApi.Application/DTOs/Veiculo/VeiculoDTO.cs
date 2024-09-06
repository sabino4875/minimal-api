namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Classe que representa os dados de navega��o / edi��o de um ve�culo
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public record VeiculoDTO
    {
        /// <summary>
        /// C�digo identificador da entidade
        /// </summary>
        [JsonProperty("id")]
        public Nullable<Int32> Id { get; set; }

        /// <summary>
        /// Marca do ve�culo
        /// </summary>
        [JsonProperty("marca")]
        public String Marca { get; set; } = default!;

        /// <summary>
        /// Nodelo do ve�culo
        /// </summary>
        [JsonProperty("modelo")]
        public String Modelo { get; set; } = default!;

        /// <summary>
        /// Ano de fabrica��o do ve�culo
        /// </summary>
        [JsonProperty("ano")]
        public Nullable<Int32> Ano { get; set; } = default!;
    }
}