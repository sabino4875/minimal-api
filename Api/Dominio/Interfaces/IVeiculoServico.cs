namespace Minimal.Api.Dominio.Interfaces
{
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Filter;
    using Minimal.Api.Dominio.ModelViews;
    using System;
    public interface IVeiculoServico
    {
        PagedResultDTO<VeiculoModelView> Todos(CriteriaFilter criteria);
        VeiculoModelView BuscaPorId(Int32 id);
        Int32 Incluir(VeiculoDTO veiculo);
        Boolean Atualizar(VeiculoModelView veiculo);
        Boolean Apagar(Int32 id);
    }
}