namespace Minimal.Api.Dominio.ModelViews
{
    using System;
    public class AdministradorModelView
    {
        public Nullable<Int32> Id { get; set; } = default!;
        public String Nome { get; set; } = default!;
        public String Email { get; set; } = default!;
        public String Perfil { get; set; } = default!;
    }
}