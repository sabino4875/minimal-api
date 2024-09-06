namespace MinimalApi.InfraStructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Repositories;
    using MinimalApi.InfraStructure.Db;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Contrato representando as rotinas para manipulação de dados no sistema
    /// </summary>
    /// <typeparam name="TEntity">Entidade</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly DbContexto _context;
        private readonly ILogger _logger;
        private readonly DbSet<TEntity> _dbSet;
        private bool _disposable;

        /// <summary>
        /// Fornece o acesso e manipulação de dados em um banco de dados pré-configurado
        /// </summary>
        protected DbContexto Context => _context;
        /// <summary>
        /// Fornece o acesso as rotinas para gravação de mensagens de log
        /// </summary>
        protected ILogger Logger => _logger;

        /// <summary>
        /// Fornece o acesso e rotinas para manipulação de dados da entidade
        /// </summary>
        protected DbSet<TEntity> DbSet => _dbSet;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="context">Classe para acesso e manipulação de dados em um banco de dados pré-configurado</param>
        public Repository(DbContexto context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
            _logger = Log.ForContext<TEntity>();
            _disposable = true;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Método destrutor da classe
        /// </summary>
        /// <param name="disposing">Executa rotinas adicionais</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposable && disposing)
            {
                _disposable = false;
            }
        }

        /// <summary>
        /// Contagem de registros
        /// </summary>
        /// <param name="criteria">Critério utilizado na contagem</param>
        /// <returns>A contagem</returns>
        public long Count(Expression<Func<TEntity, bool>> criteria)
        {
            try
            {
                var result = _dbSet.Count(criteria);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar fazer a contagem dos registros.");
                throw;
            }
        }

        /// <summary>
        /// Exclui uma ou mais entidades
        /// </summary>
        /// <param name="criteria">Critério utilizado na exclusão</param>
        /// <returns>O status da operação</returns>
        public bool Delete(Expression<Func<TEntity, bool>> criteria)
        {
            try
            {
                var _items = _dbSet.Where(criteria).ToArray();
                if (_items.Length > 0)
                {
                    _dbSet.RemoveRange(_items);
                    _context.SaveChanges(true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar excluir o registro.");
                throw;
            }
        }

        /// <summary>
        /// Método destrutor da classe
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método destrutor da classe
        /// </summary>
        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        /// Procura por uma entidade em específico 
        /// </summary>
        /// <param name="criteria">Critério utilizado na consulta</param>
        /// <returns>A entidade caso exista</returns>
        public TEntity Find(Expression<Func<TEntity, bool>> criteria)
        {
            try
            {
                var _item = _dbSet.Where(criteria).FirstOrDefault();
                return _item;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar localizar o registro.");
                throw;
            }
        }

        /// <summary>
        /// Grava os dados de uma nova entidade
        /// </summary>
        /// <param name="entity">Dados da entidade</param>
        /// <returns>O status da operação</returns>
        public int Insert(TEntity entity)
        {
            try
            {
                var result = _dbSet.Add(entity);
                _context.SaveChanges(true);
                return result.Entity.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar incluir o registro.");
                throw;
            }
        }

        /// <summary>
        /// Lista todas as entidade
        /// </summary>
        /// <param name="criteria">Critério utilizado na consulta</param>
        /// <param name="limit">Quantidade de registros a serem retornados</param>
        /// <param name="offset">Registros a serem pulados</param>
        /// <returns>A listagem de entidades</returns>
        public IEnumerable<TEntity> ListAll(Expression<Func<TEntity, bool>> criteria, int limit, int offset)
        {
            try
            {
                var result = _dbSet.Where(criteria).Skip(offset).Take(limit).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar listar os registros.");
                throw;
            }
        }

        /// <summary>
        /// Atualiza os dados de uma entidade
        /// </summary>
        /// <param name="entity">Dados da entidade</param>
        /// <returns>O status da operação</returns>
        public bool Update(TEntity entity)
        {
            try
            {
                var result = _dbSet.Where(e => e.Id == entity.Id).FirstOrDefault();
                if (result != null)
                {
                    _dbSet.Update(entity);
                    _context.SaveChanges(true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar alterar o registro.");
                throw;
            }
        }
    }
}
