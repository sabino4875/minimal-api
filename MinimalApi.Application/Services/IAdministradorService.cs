namespace MinimalApi.Application.Services
{
    using MinimalApi.Application.Criteria;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Enuns;
    using System;

    /// <summary>
    /// Contem os métodos para o acesso as rotinas de manipulação de dados da entidade usuário
    /// </summary>
    public interface IAdministradorService : IDisposable
    {
        /// <summary>
        /// Geração do token para o a cesso ao sistema 
        /// </summary>
        /// <param name="loginDTO">Dados de login</param>
        /// <returns>O resultado da operação</returns>
        Boolean Login(LoginDTO loginDTO);
        
        /// <summary>
        /// Inclui um novo usuário
        /// </summary>
        /// <param name="entity">Dados do usuário</param>
        /// <returns>O id do administrador</returns>
        Int32 Insert(CreateAdministradorDTO entity);

        /// <summary>
        /// Atualiza os dados do usuário
        /// </summary>
        /// <param name="entity">Dados do usuário</param>
        /// <returns>O resultado da operação</returns>
        Boolean Update(AdministradorDTO entity);


        /// <summary>
        /// Recupera um usuário
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>O usuário caso exista</returns>
        AdministradorDTO Find(Int32 id);
        
        /// <summary>
        /// Recupera todos os usuários
        /// </summary>
        /// <param name="criteria">Critérios utilizados na consulta</param>
        /// <returns>A listagem contendo a páginação de dados da entidade usuário</returns>
        PagedResultDTO<AdministradorDTO> ListAll(AdministradorCriteria criteria);

        /// <summary>
        /// Recupera o perfil do usuário informado
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <returns>O perfil do usuário</returns>
        PerfilUsuario RecuperaPerfilUsuario(String email);

        /// <summary>
        /// Rotina para validação e alteração de senha da entidade
        /// </summary>
        /// <param name="entity">Dados para consulta e alteração da senha</param>
        /// <returns>Resultado da operação</returns>
        Boolean AlteraSenha(AlteraSenhaDTO entity);
    }
}