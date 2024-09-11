namespace Minimal.Api.Dominio.ModelViews
{
    using System;

    public class VeiculoModelView
    {
        public Nullable<Int32> Id { get; set; } = default!;
        public String Modelo { get; set; } = default!;
        public String Marca { get; set; } = default!;
        public Nullable<Int32> Ano { get; set; } = default!;
    }
}
