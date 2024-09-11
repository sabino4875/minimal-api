namespace Minimal.Api.Dominio.Servicos
{
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Infraestrutura.Db;
    using Minimal.Api.Dominio.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Globalization;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.ModelViews;
    using Minimal.Api.Dominio.Filter;
    using AutoMapper;
    using Minimal.Api.Dominio.Helpers;
    using Serilog;

    public class VeiculoServico : IVeiculoServico
    {
        private readonly DbContexto _contexto;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;
        private readonly ILogger _logger;

        public VeiculoServico(DbContexto contexto, IMapper mapper, ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(contexto);
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(routines);

            _contexto = contexto;
            _mapper = mapper;
            _routines = routines;
            _logger = Log.ForContext<VeiculoServico>();
        }

        public Boolean Apagar(Int32 id)
        {
            ArgumentNullException.ThrowIfNull(id);
            try
            {
                var data = _contexto.Veiculos.Where(e => e.Id == id).FirstOrDefault();
                if (data != null)
                {
                    _contexto.Veiculos.Remove(data);
                    _contexto.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar excluir o registro.");
                throw;
            }
        }

        public Boolean Atualizar(VeiculoModelView veiculo)
        {
            ArgumentNullException.ThrowIfNull(veiculo);
            try
            {
                var exists = _contexto.Veiculos.Any(e => e.Id == veiculo.Id);
                if (exists)
                {
                    var data = _mapper.Map<Veiculo>(veiculo);
                    _contexto.Veiculos.Update(data);
                    _contexto.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar atualizar o registro.");
                throw;
            }
        }

        public VeiculoModelView BuscaPorId(Int32 id)
        {
            ArgumentNullException.ThrowIfNull(id);

            try
            {
                var data = _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
                if (data != null)
                {
                    var result = _mapper.Map<VeiculoModelView>(data);
                    return result;
                }
                return null;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar localizar o registro.");
                throw;
            }
        }

        public Int32 Incluir(VeiculoDTO veiculo)
        {
            ArgumentNullException.ThrowIfNull(veiculo);

            try
            {
                var data = _mapper.Map<Veiculo>(veiculo);
                _contexto.Veiculos.Add(data);
                _contexto.SaveChanges();
                return data.Id;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar incluir o registro.");
                throw;
            }
        }

        public PagedResultDTO<VeiculoModelView> Todos(CriteriaFilter criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);
            try
            {
                var query = _contexto.Veiculos.AsQueryable();
                if (_routines.ContainsValue(criteria.Nome))
                {
                    query = query.Where(v => EF.Functions.Like(v.Modelo.ToLower(CultureInfo.CurrentCulture), $"%{criteria.Nome}%"));
                }

                if (_routines.ContainsValue(criteria.Marca))
                {
                    query = query.Where(v => EF.Functions.Like(v.Marca.ToLower(CultureInfo.CurrentCulture), $"%{criteria.Marca}%"));
                }

                var itensPorPagina = 10;
                var offset = criteria.Pagina.HasValue ? criteria.Pagina.Value - 1 : 0;
                if (offset < 0) offset = 0;
                offset *= itensPorPagina;

                var count = query.Count();
                query = query.Skip(offset).Take(itensPorPagina);

                var items = _mapper.Map<VeiculoModelView[]>(query.ToArray());
                var result = new PagedResultDTO<VeiculoModelView>(count, itensPorPagina, criteria.Pagina ?? 1, items);

                return result;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar listar os registros.");
                throw;
            }
        }
    }
}