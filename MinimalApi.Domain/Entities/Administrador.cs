namespace MinimalApi.Domain.Entities
{
    using System;
    using MinimalApi.Domain.Enuns;

    /// <summary>
    /// Representa a abstração de um usuário do sistema
    /// </summary>
    public class Administrador: EntityBase
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public String Nome { get; set; } = default!;

        /// <summary>
        /// Email do usuário
        /// </summary>
        public String Email { get; set; } = default!;

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public String Senha { get; set; } = default!;

        /// <summary>
        /// Nível de acesso do usuário
        /// </summary>
        public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Visualizar;
    }
}