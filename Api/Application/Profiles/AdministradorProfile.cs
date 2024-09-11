namespace Minimal.Api.Application.Profiles
{
    using AutoMapper;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Extensions;
    using Minimal.Api.Dominio.ModelViews;

    public class AdministradorProfile : Profile
    {
        public AdministradorProfile()
        {
            CreateMap<AdministradorDTO, Administrador>()
                .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Perfil.ParseEnum(PerfilUsuario.Invalido)));
            CreateMap<AdministradorModelView, Administrador>()
                .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Perfil.ParseEnum(PerfilUsuario.Invalido)));
            CreateMap<Administrador, AdministradorModelView>()
                .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Perfil.GetDescription()));

        }
    }
}