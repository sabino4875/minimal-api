namespace Minimal.Api.Dominio.DTOs
{
    using System;

    public class AlteraSenhaDTO
    {
        public String Email { get; set; }
        public String Senha { get; set; }
        public String Codigo { get; set; }
    }
}
