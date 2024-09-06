namespace MinimalApi.Domain.Repositories
{
    using MinimalApi.Domain.Entities;
    using System;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public interface IAdministradorRepository : IRepository<Administrador>
    {
        /// <summary>
        /// Rotina para acesso ao sistema
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns>Resultado da operação</returns>
        Boolean Login(String email, String senha);

        /// <summary>
        /// Rotina para validação e alteração de senha do usuário
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <param name="codigo">Código de validação</param>
        /// <returns>Resultado da operação</returns>
        Boolean AlteraSenha(String email, String senha, String codigo);
    }
}
