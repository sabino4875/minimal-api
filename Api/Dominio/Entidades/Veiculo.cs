namespace Minimal.Api.Dominio.Entidades
{
    using System;
    public class Veiculo
    {
        public Int32 Id { get; set; } = default!;

        public String Modelo { get; set; } = default!;

        public String Marca { get; set; } = default!;

        public Int32 Ano { get; set; } = default!;
    }
}