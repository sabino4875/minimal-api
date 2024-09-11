
namespace Minimal.Api.Dominio.DTOs
{
    using System;
    public class LoginDTO
    {
        public String Email { get; set; } = default!;
        public String Senha { get; set; } = default!;
    }
}