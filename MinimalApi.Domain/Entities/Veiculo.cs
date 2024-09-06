namespace MinimalApi.Domain.Entities
{
    using System;

    /// <summary>
    /// Representa a abstração de um veículo
    /// </summary>
    public class Veiculo: EntityBase
    {
        /// <summary>
        /// Modelo do veículo
        /// </summary>
        public String Modelo { get; set; } = default!;

        /// <summary>
        /// Marca do veículo
        /// </summary>
        public String Marca { get; set; } = default!;

        /// <summary>
        /// Ano de fabricação do veículo
        /// </summary>
        public Int32 Ano { get; set; } = default!;
    }
}