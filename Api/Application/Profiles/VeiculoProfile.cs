namespace Minimal.Api.Application.Profiles
{
    using AutoMapper;
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Entidades;
    using Minimal.Api.Dominio.ModelViews;

    public class VeiculoProfile : Profile
    {
        public VeiculoProfile()
        {
            CreateMap<VeiculoDTO, Veiculo>();
            CreateMap<VeiculoModelView, Veiculo>();
            CreateMap<Veiculo, VeiculoModelView>();
        }
    }
}