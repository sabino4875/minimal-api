namespace MinimalApi.Application.Criteria
{
    using System;
    /// <summary>
    /// Filtro utilizado nas requisições de uma entidade usuário
    /// </summary>
    public class AdministradorCriteria
    {
        /// <summary>
        /// Página atual
        /// </summary>
        public Nullable<Int32> Pagina { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public String Nome { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Perfil
        /// </summary>
        public String Perfil { get; set; }

    }
}
