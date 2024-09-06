namespace MinimalApi.Domain.Entities
{
    using System;
    using MinimalApi.Domain.Enuns;

    /// <summary>
    /// Representa a abstra��o de um usu�rio do sistema
    /// </summary>
    public class Administrador: EntityBase
    {
        /// <summary>
        /// Nome do usu�rio
        /// </summary>
        public String Nome { get; set; } = default!;

        /// <summary>
        /// Email do usu�rio
        /// </summary>
        public String Email { get; set; } = default!;

        /// <summary>
        /// Senha do usu�rio
        /// </summary>
        public String Senha { get; set; } = default!;

        /// <summary>
        /// N�vel de acesso do usu�rio
        /// </summary>
        public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Visualizar;
    }
}