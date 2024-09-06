namespace MinimalApi.Application.Services
{
    using MinimalApi.Application.Criteria;
    using MinimalApi.Application.DTOs;
    using System;

    /// <summary>
    /// Contem os métodos para o acesso as rotinas de manipulação de dados da entidade veículo
    /// </summary>
    public interface IVeiculoService : IDisposable
    {
        /// <summary>
        /// Recupera todos os veículos
        /// </summary>
        /// <param name="criteria">Critérios utilizados na consulta</param>
        /// <returns>Listagem contendo todos os dados de paginação da entidade veículo</returns>
        PagedResultDTO<VeiculoDTO> ListAll(VeiculoCriteria criteria);
        
        /// <summary>
        /// Recupera um veículo
        /// </summary>
        /// <param name="id">Id do veículo</param>
        /// <returns>O veículo caso exista</returns>
        VeiculoDTO Find(Int32 id);
        
        /// <summary>
        /// Cadastra um novo veículo
        /// </summary>
        /// <param name="entity">Dados do veículo</param>
        /// <returns>O id do veículo cadastrado</returns>
        Int32 Insert(CreateVeiculoDTO entity);
        
        /// <summary>
        /// Altera um veículo
        /// </summary>
        /// <param name="entity">Dados do veículo</param>
        /// <returns>O resultado da operação</returns>
        Boolean Update(VeiculoDTO entity);

        /// <summary>
        /// Exclui um veículo
        /// </summary>
        /// <param name="id">Id do veículo</param>
        /// <returns>O resultado da operação</returns>
        Boolean Delete(Int32 id);
    }
}