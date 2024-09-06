namespace MinimalApi.Application.Profiles
{
    using AutoMapper;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Entities;

    /// <summary>
    /// Classe responsável pelo mapeamento da entidade Usuario
    /// </summary>
    public class AdministradorProfile : Profile
    {
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public AdministradorProfile()
        {
            CreateMap<CreateAdministradorDTO, Administrador>();
            CreateMap<AdministradorDTO, Administrador>();
            CreateMap<Administrador, AdministradorDTO>();
        }
    }
}
