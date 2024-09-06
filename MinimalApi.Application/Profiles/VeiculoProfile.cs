namespace MinimalApi.Application.Profiles
{
    using AutoMapper;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Entities;

    /// <summary>
    /// Classe responsável pelo mapeamento da entidade Veiculo
    /// </summary>
    public class VeiculoProfile : Profile
    {
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public VeiculoProfile()
        {
            CreateMap<CreateVeiculoDTO, Veiculo>();
            CreateMap<VeiculoDTO, Veiculo>();
            CreateMap<Veiculo, VeiculoDTO>();
        }
    }
}
