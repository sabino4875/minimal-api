namespace Minimal.Api.Test.Mocks
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Minimal.Api.Application.Profiles;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Filter;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.Interfaces;
    using Minimal.Api.Dominio.ModelViews;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class AdministradorServicoMock : IAdministradorServico
    {
        private readonly List<Administrador> _administradores;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;

        public AdministradorServicoMock()
        {
            var admins = new List<Administrador>();
            _routines = new ApplicationRoutines();
            var config = new MapperConfiguration(profiles => profiles.AddProfiles([new AdministradorProfile()]));
            var _mapper = config.CreateMapper();

            admins.Add(new Administrador
                {
                    Id = 1,
                    Nome = "Administrador",
                    Email = "administrador@teste.com",
                    Senha = _routines.ToSHA256Hash("123456"),
                    Perfil = PerfilUsuario.Admin
                }
            );


            while (admins.Count < 10)
            {
                var id = Faker.RandomNumber.Next(1, 100);
                var nome = Faker.Name.FullName();
                var email = Faker.Internet.Email();
                var senha = _routines.ToSHA256Hash(Faker.Identification.UkNationalInsuranceNumber());
                var perfil = PerfilUsuario.Invalido;
                while (perfil == PerfilUsuario.Invalido)
                {
                    perfil = Faker.Enum.Random<PerfilUsuario>();
                }

                var adm = new Administrador
                {
                    Id = id,
                    Nome = nome,
                    Email = email,
                    Senha = senha,
                    Perfil = perfil
                };
                if(!admins.Any(e => e.Id == id || e.Email == email))
                {
                    admins.Add(adm);
                }
            }

            _administradores = admins;
        }

        public Boolean AlterarDadosCadastrais(AdministradorModelView administrador)
        {
            ArgumentNullException.ThrowIfNull(administrador);

            var data = _administradores.Where(e => e.Id == administrador.Id).FirstOrDefault();

            if (data!=null)
            {
                var result = _mapper.Map<Administrador>(administrador);
                var index = _administradores.IndexOf(data);
                _administradores[index] = result;
            }

            return false;
        }

        public Boolean AlteraSenha(String email, String senha, String codigo)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(senha);
            ArgumentNullException.ThrowIfNull(codigo);

            var data = _administradores.Where(e => e.Email == email).FirstOrDefault();

            if (data != null)
            {
                var index = _administradores.IndexOf(data);
                data.Senha = senha;
                _administradores[index] = data;
            }

            return false;
        }

        public AdministradorModelView BuscaPorId(Int32 id)
        {
            ArgumentNullException.ThrowIfNull(id);

            var data = _administradores.Where(e => e.Id == id).FirstOrDefault();

            if (data != null)
            {
                var result = _mapper.Map<AdministradorModelView>(data);
                return result;
            }

            return null;
        }

        public Int32 Incluir(AdministradorDTO administrador)
        {
            ArgumentNullException.ThrowIfNull(administrador);

            var data = _mapper.Map<Administrador>(administrador);
            var id = Faker.RandomNumber.Next(1, 100);
            while (_administradores.Any(e => e.Id == id))
            {
                id = Faker.RandomNumber.Next(1, 100);
            }
            data.Id = id;
            
            _administradores.Add(data);
            return id;
        }

        public Boolean Login(String email, String password)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            return _administradores.Any(e => e.Email == email && e.Senha == password);
        }

        public PerfilUsuario RecuperaPerfilUsuario(String email)
        {
            ArgumentNullException.ThrowIfNull(email);

            var data = _administradores.Where(e => e.Email == email).FirstOrDefault();
            if (data != null)
            {
                return data.Perfil;
            }
            return PerfilUsuario.Invalido;
        }

        public PagedResultDTO<AdministradorModelView> Todos(CriteriaFilter criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            var query = _administradores.AsQueryable();
            var itensPorPagina = 10;
            var offset = criteria.Pagina.HasValue ? criteria.Pagina.Value - 1 : 0;
            if (offset < 0) offset = 0;
            offset *= itensPorPagina;

            if (_routines.ContainsValue(criteria.Nome))
            {
                query = query.Where(e => EF.Functions.Like(e.Nome.ToLower(CultureInfo.CurrentCulture), $"%{criteria.Nome}%"));
            }

            var count = query.Count();
            query = query.Skip(offset).Take(itensPorPagina);

            var resultQuery = _mapper.Map<AdministradorModelView[]>(query.ToArray());
            var result = new PagedResultDTO<AdministradorModelView>(count, itensPorPagina, criteria.Pagina ?? 1, resultQuery);

            return result;
        }
    }
}