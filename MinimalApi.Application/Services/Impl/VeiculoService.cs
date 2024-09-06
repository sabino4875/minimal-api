namespace MinimalApi.Application.Services.Impl
{
    using AutoMapper;
    using MinimalApi.Application.Criteria;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Helpers;
    using MinimalApi.Domain.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
	
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class VeiculoService : IVeiculoService
    {
		private readonly IVeiculoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;
        private Boolean _disposable;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="repository">Contém as rotinas de manipulação de dados do veículo</param>
        /// <param name="mapper">Contém rotinas para o mapeamento da entidade veículo</param>
        /// <param name="routines">Rotinas de uso geral no sistema</param>
        public VeiculoService(IVeiculoRepository repository, IMapper mapper, ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(routines);

            _repository = repository;
            _mapper = mapper;
            _routines = routines;
            _disposable = true;
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
        /// Exclui uma ou mais entidades
        /// </summary>
        /// <param name="id">Código de identificação  da entidade</param>
        /// <returns>O status da operação</returns>
        public Boolean Delete(Int32 id)
        {
            return _repository.Delete(e => e.Id == id);
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
        ~VeiculoService()
        {
            Dispose(false);
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public VeiculoDTO Find(Int32 id)
        {
            var data = _repository.Find(e => e.Id == id);
            if (data != null)
            {
                var result = _mapper.Map<VeiculoDTO>(data);
                return result;
            }
            return null;
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public Int32 Insert(CreateVeiculoDTO entity)
        {
            var data = _mapper.Map<Veiculo>(entity);
            return _repository.Insert(data);
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
        public PagedResultDTO<VeiculoDTO> ListAll(VeiculoCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);
            var limit = 30;
            var page = criteria.Pagina ?? 1;
            if (page < 1) page = 1;
            var offset = (page-1) * limit;

            Expression<Func<Veiculo, Boolean>> filter = MountFilter(criteria);

            var data = _repository.ListAll(filter, limit, offset);
            var count = _repository.Count(filter);
            var model = _mapper.Map<IEnumerable<VeiculoDTO>>(data);
            var result = new PagedResultDTO<VeiculoDTO>(count, page, limit, model);
            return result;
        }


		/// <summary>
		/// <inheritdoc/>
		/// </summary>
        public Boolean Update(VeiculoDTO entity)
        {
            var data = _mapper.Map<Veiculo>(entity);
            return _repository.Update(data);
        }

        private Expression<Func<Veiculo,Boolean>> MountFilter(VeiculoCriteria criteria)
        {
            var modelo = criteria.Modelo.Trim();
            var marca = criteria.Marca.Trim();

            Expression<Func<Veiculo, Boolean>> filter = (e => true);

            var filterOption = 0;
            if (_routines.ContainsValue(modelo)) filterOption += 2;
            if (_routines.ContainsValue(marca)) filterOption += 3;

            if (filterOption > 0)
            {
                switch (filterOption)
                {
                    case 2: filter = filter.FilterByModelo(modelo); break;
                    case 3: filter = filter.FilterByMarca(marca); break;
                    case 5: filter = filter.FilterByNModeloAndMarca(modelo, marca); break;
                }
            }
            return filter;
        }
    }

    static class VeiculoFilterExtension
    {
        public static Expression<Func<Veiculo, Boolean>> FilterByNModeloAndMarca(this Expression<Func<Veiculo, Boolean>> filter, String modelo, String marca)
        {
            filter = (e => e.Modelo.Contains(modelo) && e.Marca == marca);
            return filter;
        }

        public static Expression<Func<Veiculo, Boolean>> FilterByModelo(this Expression<Func<Veiculo, Boolean>> filter, String modelo)
        {
            filter = (e => e.Modelo.Contains(modelo));
            return filter;
        }

        public static Expression<Func<Veiculo, Boolean>> FilterByMarca(this Expression<Func<Veiculo, Boolean>> filter, String marca)
        {
            filter = (e => e.Marca == marca);
            return filter;
        }
    }
}
