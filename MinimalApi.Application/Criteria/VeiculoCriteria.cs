namespace MinimalApi.Application.Criteria
{
    using System;
    /// <summary>
    /// Filtro utilizado nas requisições de uma entidade veículo
    /// </summary>
    public class VeiculoCriteria
    {
        /// <summary>
        /// Página atual
        /// </summary>
        public Nullable<Int32> Pagina { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        public String Modelo { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        public String Marca { get; set; }
    }
}
