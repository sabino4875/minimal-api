namespace MinimalApi.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System;
    using MinimalApi.Domain.Entities;

    /// <summary>
    /// Api contendo os métodos para manipulação de dados de uma entidade no sistema
    /// </summary>
    /// <typeparam name="TEntity">Entidade</typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : EntityBase
    {
        /// <summary>
        /// Grava os dados de uma nova entidade
        /// </summary>
        /// <param name="entity">Dados da entidade</param>
        /// <returns>O status da operação</returns>
        Int32 Insert(TEntity entity);

        /// <summary>
        /// Atualiza os dados de uma entidade
        /// </summary>
        /// <param name="entity">Dados da entidade</param>
        /// <returns>O status da operação</returns>
        Boolean Update(TEntity entity);

        /// <summary>
        /// Exclui uma ou mais entidades
        /// </summary>
        /// <param name="criteria">Critério utilizado na exclusão</param>
        /// <returns>O status da operação</returns>
        Boolean Delete(Expression<Func<TEntity, Boolean>> criteria);

        /// <summary>
        /// Procura por uma entidade em específico 
        /// </summary>
        /// <param name="criteria">Critério utilizado na consulta</param>
        /// <returns>A entidade caso exista</returns>
        TEntity Find(Expression<Func<TEntity, Boolean>> criteria);

        /// <summary>
        /// Lista todas as entidade
        /// </summary>
        /// <param name="criteria">Critério utilizado na consulta</param>
        /// <param name="limit">Quantidade de registros a serem retornados</param>
        /// <param name="offset">Registros a serem pulados</param>
        /// <returns>A listagem de entidades</returns>
        IEnumerable<TEntity> ListAll(Expression<Func<TEntity, Boolean>> criteria, Int32 limit, Int32 offset);

        /// <summary>
        /// Contagem de registros
        /// </summary>
        /// <param name="criteria">Critério utilizado na contagem</param>
        /// <returns>A contagem</returns>
        Int64 Count(Expression<Func<TEntity, Boolean>> criteria);
    }
}
