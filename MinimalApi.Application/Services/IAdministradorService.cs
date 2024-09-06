namespace MinimalApi.Application.Services
{
    using MinimalApi.Application.Criteria;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Enuns;
    using System;

    /// <summary>
    /// Contem os m�todos para o acesso as rotinas de manipula��o de dados da entidade usu�rio
    /// </summary>
    public interface IAdministradorService : IDisposable
    {
        /// <summary>
        /// Gera��o do token para o a cesso ao sistema 
        /// </summary>
        /// <param name="loginDTO">Dados de login</param>
        /// <returns>O resultado da opera��o</returns>
        Boolean Login(LoginDTO loginDTO);
        
        /// <summary>
        /// Inclui um novo usu�rio
        /// </summary>
        /// <param name="entity">Dados do usu�rio</param>
        /// <returns>O id do administrador</returns>
        Int32 Insert(CreateAdministradorDTO entity);

        /// <summary>
        /// Atualiza os dados do usu�rio
        /// </summary>
        /// <param name="entity">Dados do usu�rio</param>
        /// <returns>O resultado da opera��o</returns>
        Boolean Update(AdministradorDTO entity);


        /// <summary>
        /// Recupera um usu�rio
        /// </summary>
        /// <param name="id">Id do usu�rio</param>
        /// <returns>O usu�rio caso exista</returns>
        AdministradorDTO Find(Int32 id);
        
        /// <summary>
        /// Recupera todos os usu�rios
        /// </summary>
        /// <param name="criteria">Crit�rios utilizados na consulta</param>
        /// <returns>A listagem contendo a p�gina��o de dados da entidade usu�rio</returns>
        PagedResultDTO<AdministradorDTO> ListAll(AdministradorCriteria criteria);

        /// <summary>
        /// Recupera o perfil do usu�rio informado
        /// </summary>
        /// <param name="email">E-mail do usu�rio</param>
        /// <returns>O perfil do usu�rio</returns>
        PerfilUsuario RecuperaPerfilUsuario(String email);

        /// <summary>
        /// Rotina para valida��o e altera��o de senha da entidade
        /// </summary>
        /// <param name="entity">Dados para consulta e altera��o da senha</param>
        /// <returns>Resultado da opera��o</returns>
        Boolean AlteraSenha(AlteraSenhaDTO entity);
    }
}