namespace Minimal.Api.Dominio.DTOs
{
    using System;
    public class VeiculoDTO
    {
        public String Modelo { get; set; } = default!;
        public String Marca { get; set; } = default!;
        public Nullable<Int32> Ano { get; set; } = default!;
    }
}