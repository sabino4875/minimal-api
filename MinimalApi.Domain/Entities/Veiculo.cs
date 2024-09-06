namespace MinimalApi.Domain.Entities
{
    using System;

    /// <summary>
    /// Representa a abstra��o de um ve�culo
    /// </summary>
    public class Veiculo: EntityBase
    {
        /// <summary>
        /// Modelo do ve�culo
        /// </summary>
        public String Modelo { get; set; } = default!;

        /// <summary>
        /// Marca do ve�culo
        /// </summary>
        public String Marca { get; set; } = default!;

        /// <summary>
        /// Ano de fabrica��o do ve�culo
        /// </summary>
        public Int32 Ano { get; set; } = default!;
    }
}