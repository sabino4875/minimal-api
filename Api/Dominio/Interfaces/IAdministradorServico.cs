namespace Minimal.Api.Dominio.Interfaces
{
    using Minimal.Api.Dominio.DTOs;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Filter;
    using Minimal.Api.Dominio.ModelViews;
    using System;
    public interface IAdministradorServico
    {
        Boolean Login(String email, String password);
        Int32 Incluir(AdministradorDTO administrador);
        Boolean AlterarDadosCadastrais(AdministradorModelView administrador);
        AdministradorModelView BuscaPorId(Int32 id);
        PagedResultDTO<AdministradorModelView> Todos(CriteriaFilter criteria);
        PerfilUsuario RecuperaPerfilUsuario(String email);
        Boolean AlteraSenha(String email, String senha, String codigo);
    }
}