namespace Minimal.Api.Test.Mocks
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Minimal.Api.Application.Profiles;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Dominio.Filter;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.Interfaces;
    using Minimal.Api.Dominio.ModelViews;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class VeiculoServicoMock : IVeiculoServico
    {
        private readonly List<Veiculo> _veiculos;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;

        public VeiculoServicoMock()
        {
            var veiculos = new List<Veiculo>();
            _routines = new ApplicationRoutines();
            var config = new MapperConfiguration(profiles => profiles.AddProfiles([new VeiculoProfile()]));
            var _mapper = config.CreateMapper();

            while (veiculos.Count < 10)
            {
                var id = Faker.RandomNumber.Next(1, 100);
                var marca = Faker.Lorem.GetFirstWord();
                var modelo = Faker.Lorem.GetFirstWord();
                var ano = Faker.RandomNumber.Next(1951, DateTime.Now.Year); 

                var veiculo = new Veiculo
                {
                    Id = id,
                    Marca = marca,
                    Modelo = modelo,
                    Ano = ano
                };

                if (!veiculos.Any(e => e.Id == id || e.Marca == marca || e.Modelo == modelo))
                {
                    veiculos.Add(veiculo);
                }
            }
            _veiculos = veiculos;
        }

        public Boolean Apagar(Int32 id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var data = _veiculos.Where(e => e.Id == id).FirstOrDefault();
            if (data != null) 
            { 
                var index = _veiculos.IndexOf(data);
                _veiculos.RemoveAt(index);
                return true;
            }
            return false;
        }

        public Boolean Atualizar(VeiculoModelView veiculo)
        {
            ArgumentNullException.ThrowIfNull(veiculo);

            var info = _veiculos.Where(e => e.Id == veiculo.Id).FirstOrDefault();
            if (info != null) 
            {
                var index = _veiculos.IndexOf(info);
                var data = _mapper.Map<Veiculo>(veiculo);
                _veiculos[index] = data;
                return true;
            }
            return false;
        }

        public VeiculoModelView BuscaPorId(Int32 id)
        {
            ArgumentNullException.ThrowIfNull(id);

            var data = _veiculos.Where(e => e.Id == id).FirstOrDefault();
            if (data != null)
            {
                var result = _mapper.Map<VeiculoModelView>(data);
                return result;
            }
            return null;
        }

        public Int32 Incluir(VeiculoDTO veiculo)
        {
            var data = _mapper.Map<Veiculo>(veiculo);
            var id = Faker.RandomNumber.Next(1, 100);
            while (_veiculos.Any(e => e.Id == id))
            {
                id = Faker.RandomNumber.Next(1, 100);
            }
            data.Id = id;

            _veiculos.Add(data);
            return id;
        }

        public PagedResultDTO<VeiculoModelView> Todos(CriteriaFilter criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            var query = _veiculos.AsQueryable();
            var itensPorPagina = 10;
            var offset = criteria.Pagina.HasValue ? criteria.Pagina.Value - 1 : 0;
            if (offset < 0) offset = 0;
            offset *= itensPorPagina;

            if (_routines.ContainsValue(criteria.Nome))
            {
                query = query.Where(e => EF.Functions.Like(e.Modelo.ToLower(CultureInfo.CurrentCulture), $"%{criteria.Nome}%"));
            }

            if (_routines.ContainsValue(criteria.Marca))
            {
                query = query.Where(e => EF.Functions.Like(e.Marca.ToLower(CultureInfo.CurrentCulture), $"%{criteria.Marca}%"));
            }

            var count = query.Count();
            query = query.Skip(offset).Take(itensPorPagina);

            var resultQuery = _mapper.Map<VeiculoModelView[]>(query.ToArray());
            var result = new PagedResultDTO<VeiculoModelView>(count, itensPorPagina, criteria.Pagina ?? 1, resultQuery);

            return result;
        }
    }
}
