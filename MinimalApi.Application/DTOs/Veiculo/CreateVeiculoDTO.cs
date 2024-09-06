namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Classe que representa os dados de inclusão de um novo veículo
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CreateVeiculoDTO
    {
        /// <summary>
        /// Marca do veículo
        /// </summary>
        [JsonProperty("marca")]
        public String Marca { get; set; } = default!;

        /// <summary>
        /// Nodelo do veículo
        /// </summary>
        [JsonProperty("modelo")]
        public String Modelo { get; set; } = default!;

        /// <summary>
        /// Ano de fabricação do veículo
        /// </summary>
        [JsonProperty("ano")]
        public Nullable<Int32> Ano { get; set; } = default!;

    }
}
