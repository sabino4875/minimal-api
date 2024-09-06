namespace MinimalApi.Application.Services
{
    using MinimalApi.Application.Criteria;
    using MinimalApi.Application.DTOs;
    using System;

    /// <summary>
    /// Contem os m�todos para o acesso as rotinas de manipula��o de dados da entidade ve�culo
    /// </summary>
    public interface IVeiculoService : IDisposable
    {
        /// <summary>
        /// Recupera todos os ve�culos
        /// </summary>
        /// <param name="criteria">Crit�rios utilizados na consulta</param>
        /// <returns>Listagem contendo todos os dados de pagina��o da entidade ve�culo</returns>
        PagedResultDTO<VeiculoDTO> ListAll(VeiculoCriteria criteria);
        
        /// <summary>
        /// Recupera um ve�culo
        /// </summary>
        /// <param name="id">Id do ve�culo</param>
        /// <returns>O ve�culo caso exista</returns>
        VeiculoDTO Find(Int32 id);
        
        /// <summary>
        /// Cadastra um novo ve�culo
        /// </summary>
        /// <param name="entity">Dados do ve�culo</param>
        /// <returns>O id do ve�culo cadastrado</returns>
        Int32 Insert(CreateVeiculoDTO entity);
        
        /// <summary>
        /// Altera um ve�culo
        /// </summary>
        /// <param name="entity">Dados do ve�culo</param>
        /// <returns>O resultado da opera��o</returns>
        Boolean Update(VeiculoDTO entity);

        /// <summary>
        /// Exclui um ve�culo
        /// </summary>
        /// <param name="id">Id do ve�culo</param>
        /// <returns>O resultado da opera��o</returns>
        Boolean Delete(Int32 id);
    }
}