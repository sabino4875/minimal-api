
namespace Minimal.Api.Dominio.DTOs
{
    using System;
    public class AdministradorDTO
    {
        public String Nome { get; set; } = default!;
        public String Email { get; set; } = default!;
        public String Senha { get; set; } = default!;
        public String Perfil { get; set; } = default!;
    }
}
