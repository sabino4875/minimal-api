namespace Minimal.Api.Dominio.Enuns
{

    using System.ComponentModel;

    /// <summary>
    /// Perfil do usuário
    /// </summary>
    public enum PerfilUsuario
    {
        /// <summary>
        /// Administrador do sistema
        /// </summary>
        [Description("Administrator")]
        Admin,
        /// <summary>
        /// Permite a maioria das ações no sistema
        /// </summary>
        [Description("Editor")]
        Editor,
        /// <summary>
        /// Permite apenas a visualização dos dados
        /// </summary>
        [Description("Viewer")]
        Visualizar,

        /// <summary>
        /// Perfil informado inválido - Utilizado em validações
        /// </summary>
        Invalido
    }
}
