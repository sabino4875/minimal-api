namespace MinimalApi.Application.Services.Impl
{
    using AutoMapper;
    using MinimalApi.Application.Criteria;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Enuns;
    using MinimalApi.Domain.Extensions;
    using MinimalApi.Domain.Helpers;
    using MinimalApi.Domain.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
	
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class AdministradorService : IAdministradorService
    {
		private readonly IAdministradorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;
        private Boolean _disposable;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="repository">Contém as rotinas de manipulação de dados do usuário</param>
        /// <param name="mapper">Contém rotinas para o mapeamento da entidade usuário</param>
        /// <param name="routines">Rotinas de uso geral no sistema</param>
        public AdministradorService(IAdministradorRepository repository, IMapper mapper, ApplicationRoutines routines)
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
		/// <inheritdoc/>
		/// </summary>
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
        ~AdministradorService()
        {
            Dispose(false);
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public AdministradorDTO Find(Int32 id)
        {
            var data = _repository.Find(e => e.Id == id);
            if (data != null)
            {
                var result = _mapper.Map<AdministradorDTO>(data);
                return result;
            }
            return null;
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public Int32 Insert(CreateAdministradorDTO entity)
        {
            var data = _mapper.Map<Administrador>(entity);
            return _repository.Insert(data);
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
        public PagedResultDTO<AdministradorDTO> ListAll(AdministradorCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);
            var limit = 30;
            var page = criteria.Pagina ?? 1;
            if(page < 1) page = 1;
            var offset = (page - 1) * limit;
            Expression<Func<Administrador, Boolean>> filter = MountFilter(criteria);
            
            var data = _repository.ListAll(filter, limit, offset);
            var count = _repository.Count(filter);
            var model = _mapper.Map<IEnumerable<AdministradorDTO>>(data);
            var result = new PagedResultDTO<AdministradorDTO>(count, limit, offset, model);
            return result;
        }


		/// <summary>
		/// <inheritdoc/>
		/// </summary>
        public Boolean Update(AdministradorDTO entity)
        {
            var data = _mapper.Map<Administrador>(entity);
            return _repository.Update(data);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Boolean Login(LoginDTO loginDTO)
        {
            ArgumentNullException.ThrowIfNull(loginDTO);
            return _repository.Login(loginDTO.Email, loginDTO.Senha);
        }

        private Expression<Func<Administrador, Boolean>> MountFilter(AdministradorCriteria criteria)
        {
            var email = criteria.Email.Trim();
            var nome = criteria.Nome.Trim();
            var perfilStr = criteria.Perfil.Trim();
            var perfil = PerfilUsuario.Invalido;

            if(_routines.ContainsValue(perfilStr))
            {
                perfil = perfilStr.ParseEnum<PerfilUsuario>(PerfilUsuario.Invalido);
            }

            Expression<Func<Administrador, Boolean>> filter = (e => true);

            var filterOption = 0;
            if (_routines.ContainsValue(email)) filterOption += 2;
            if (_routines.ContainsValue(nome)) filterOption += 3;
            if (perfil != PerfilUsuario.Invalido) filterOption += 4;

            if(filterOption > 0)
            {
                switch(filterOption)
                {
                    case 2: filter = filter.FilterByEmail(email); break;//e-mail
                    case 3: filter = filter.FilterByName(nome); break;//nome
                    case 4: filter = filter.FilterByPerfil(perfil); break;//perfil
                    case 5: filter = filter.FilterByNameAndEmail(nome, email); break;//nome && e-mail
                    case 6: filter = filter.FilterByEmailAndPerfil(email, perfil); break;//e-mail && perfil
                    case 7: filter = filter.FilterByNameAndPerfil(nome, perfil); break;//nome && perfil
                    case 9: filter = filter.FilterByNameEmailAndPerfil(nome, email, perfil); break;//nome && e-mail && perfil
                }
            }

            return filter;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PerfilUsuario RecuperaPerfilUsuario(String email)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(email);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(email);

            var result = _repository.Find(e => e.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (result != null)
            {
                return result.Perfil;
            }
            return PerfilUsuario.Invalido;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Boolean AlteraSenha(AlteraSenhaDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return _repository.AlteraSenha(entity.Email, entity.Senha, entity.Codigo);
        }
    }

    static class AdministradorFilterExtension
    {
        public static Expression<Func<Administrador, Boolean>> FilterByNameEmailAndPerfil(this Expression<Func<Administrador, Boolean>> filter, String nome, String email, PerfilUsuario perfil)
        {
            filter = (e => e.Nome.Contains(nome) && e.Email == email && e.Perfil == perfil);
            return filter;
        }

        public static Expression<Func<Administrador, Boolean>> FilterByNameAndEmail(this Expression<Func<Administrador, Boolean>> filter, String nome, String email)
        {
           filter = (e => e.Nome.Contains(nome) && e.Email == email);
            return filter;
        }

        public static Expression<Func<Administrador, Boolean>> FilterByNameAndPerfil(this Expression<Func<Administrador, Boolean>> filter, String nome, PerfilUsuario perfil)
        {
            filter = (e => e.Nome.Contains(nome) && e.Perfil == perfil);
            return filter;
        }

        public static Expression<Func<Administrador, Boolean>> FilterByEmailAndPerfil(this Expression<Func<Administrador, Boolean>> filter, String email, PerfilUsuario perfil)
        {
            filter = (e => e.Email == email && e.Perfil == perfil);
            return filter;
        }

        public static Expression<Func<Administrador, Boolean>> FilterByName(this Expression<Func<Administrador, Boolean>> filter, String nome)
        {
            filter = (e => e.Nome.Contains(nome));
            return filter;
        }

        public static Expression<Func<Administrador, Boolean>> FilterByEmail(this Expression<Func<Administrador, Boolean>> filter, String email)
        {
            filter = (e => e.Email == email);
            return filter;
        }

        public static Expression<Func<Administrador, Boolean>> FilterByPerfil(this Expression<Func<Administrador, Boolean>> filter, PerfilUsuario perfil)
        {
            filter = (e => e.Perfil == perfil);
            return filter;
        }
    }
}
