namespace Minimal.Api.Dominio.Entidades
{
    using Minimal.Api.Dominio.Enuns;
    using System;

    public class Administrador
    {
        public Int32 Id { get; set; } = default!;
        public String Nome { get; set; } = default!;

        public String Email { get; set; } = default!;

        public String Senha { get; set; } = default!;

        public PerfilUsuario Perfil { get; set; } = default!;
    }
}

