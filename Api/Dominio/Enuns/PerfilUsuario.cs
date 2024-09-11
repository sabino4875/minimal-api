namespace Minimal.Api.Dominio.Enuns
{

    using System.ComponentModel;

    /// <summary>
    /// Perfil do usu�rio
    /// </summary>
    public enum PerfilUsuario
    {
        /// <summary>
        /// Administrador do sistema
        /// </summary>
        [Description("Administrator")]
        Admin,
        /// <summary>
        /// Permite a maioria das a��es no sistema
        /// </summary>
        [Description("Editor")]
        Editor,
        /// <summary>
        /// Permite apenas a visualiza��o dos dados
        /// </summary>
        [Description("Viewer")]
        Visualizar,

        /// <summary>
        /// Perfil informado inv�lido - Utilizado em valida��es
        /// </summary>
        Invalido
    }
}
