namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Representa os dados de visualização / edição de uma entidade usuário
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdministradorDTO
    {
        /// <summary>
        /// Cóodigo de identificação da entidade
        /// </summary>
        [JsonProperty("id")]
        public Nullable<Int32> Id { get; set; } = default!;
        
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
        /// Perfil
        /// </summary>
        [JsonProperty("perfil")]
        public String Perfil { get; set; } = default!;
    }
}