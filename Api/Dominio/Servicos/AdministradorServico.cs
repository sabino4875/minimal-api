namespace Minimal.Api.Dominio.Servicos
{
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Infraestrutura.Db;
    using Minimal.Api.Dominio.Interfaces;
    using System.Linq;
    using System;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.DTOs;
    using AutoMapper;
    using Minimal.Api.Dominio.ModelViews;
    using Minimal.Api.Dominio.Filter;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using Minimal.Api.Dominio.Helpers;
    using Serilog;

    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;
        private readonly ILogger _logger;

        public AdministradorServico(DbContexto contexto, IMapper mapper, ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(contexto);
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(routines);

            _contexto = contexto;
            _mapper = mapper;
            _routines = routines;
            _logger = Log.ForContext<AdministradorServico>();
        }

        public Boolean AlterarDadosCadastrais(AdministradorModelView administrador)
        {
            ArgumentNullException.ThrowIfNull(administrador);
            try
            {
                var exists = _contexto.Administradores.Any(e => e.Id == administrador.Id);
                if (exists)
                {
                    var data = _mapper.Map<Administrador>(administrador);
                    _contexto.Administradores.Update(data);
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

        public Boolean AlteraSenha(String email, String senha, String codigo)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(senha);
            ArgumentNullException.ThrowIfNull(codigo);

            try
            {
                var data = _contexto.Administradores.Where(e => e.Email == email).FirstOrDefault();
                if (data != null)
                {
                    data.Senha = senha;
                    _contexto.Administradores.Update(data);
                    _contexto.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar alterar a senha do usuário.");
                throw;
            }
        }

        public AdministradorModelView BuscaPorId(Int32 id)
        {
            ArgumentNullException.ThrowIfNull(id);

            try
            {
                var data = _contexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
                if (data != null)
                {
                    var result = _mapper.Map<AdministradorModelView>(data);
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

        public Int32 Incluir(AdministradorDTO administrador)
        {
            ArgumentNullException.ThrowIfNull(administrador);

            try
            {
                var data = _mapper.Map<Administrador>(administrador);
                _contexto.Administradores.Add(data);
                _contexto.SaveChanges();
                return data.Id;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar incluir o registro.");
                throw;
            }
        }

        public Boolean Login(String email, String password)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            try
            {
                var adm = _contexto.Administradores.Where(a => a.Email == email && a.Senha == password).FirstOrDefault();
                return adm != null;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar efetuar o login.");
                throw;
            }
        }

        public PerfilUsuario RecuperaPerfilUsuario(String email)
        {
            ArgumentNullException.ThrowIfNull(email);

            try
            {
                var result = PerfilUsuario.Invalido;
                var data = _contexto.Administradores.Where(e => e.Email == email).FirstOrDefault();
                if (data != null)
                {
                    result = data.Perfil;
                }
                return result;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar recuperar o perfil do usuário.");
                throw;
            }
        }

        public PagedResultDTO<AdministradorModelView> Todos(CriteriaFilter criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            try
            {
                var query = _contexto.Administradores.AsQueryable();
                var itensPorPagina = 10;
                var offset = criteria.Pagina.HasValue ? criteria.Pagina.Value - 1 : 0;
                if (offset < 0) offset = 0;
                offset *= itensPorPagina;

                if (_routines.ContainsValue(criteria.Nome))
                {
                    query = query.Where(e => EF.Functions.Like(e.Nome, $"%{criteria.Nome}%"));
                }

                var count = query.Count();
                query = query.Skip(offset).Take(itensPorPagina);

                var resultQuery = _mapper.Map<AdministradorModelView[]>(query.ToArray());
                var result = new PagedResultDTO<AdministradorModelView>(count, itensPorPagina, criteria.Pagina ?? 1, resultQuery);

                return result;
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar listar os usuários.");
                throw;
            }
        }
    }
}