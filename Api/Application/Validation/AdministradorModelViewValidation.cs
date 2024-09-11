namespace Minimal.Api.Application.Validation
{
    using FluentValidation;
    using Minimal.Api.Dominio.Enuns;
    using Minimal.Api.Dominio.Extensions;
    using Minimal.Api.Dominio.Helpers;
    using Minimal.Api.Dominio.ModelViews;
    using System;

    public class AdministradorModelViewValidation : AbstractValidator<AdministradorModelView>
    {
        private readonly ApplicationRoutines _routines;

        public AdministradorModelViewValidation(ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(routines);
            _routines = routines;

            RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("O campo is deve ser informado.");
            RuleFor(e => e.Id).Must(IsValidId).WithMessage("Id informado inválido.");

            RuleFor(e => e.Nome).NotNull().NotEmpty().WithMessage("O campo nome deve ser informado.");
            RuleFor(e => e.Nome).Length(3, 50).WithMessage("O campo nome deve ter entre 3 e 50 caracteres.");

            RuleFor(e => e.Email).NotNull().NotEmpty().WithMessage("O campo e-mail deve ser informado.");
            RuleFor(e => e.Email).Length(2, 100).WithMessage("O campo e-mail deve ter até 100 caracteres.");
            RuleFor(e => e.Email).EmailAddress().WithMessage("E-mail informado inválido.");

            RuleFor(e => e.Perfil).NotNull().NotEmpty().WithMessage("O campo perfil deve ser informado.");
            RuleFor(e => e.Perfil).Length(3, 20).WithMessage("O campo perfil deve ter entre 3 e 20 caracteres.");
            RuleFor(e => e.Perfil).Must(IsValidPerfil).WithMessage("Perfil informado inválido.");
        }

        private Boolean IsValidId(Nullable<Int32> id)
        {
            if (id != null)
            {
                if (id > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean IsValidPerfil(String perfil)
        {
            if (_routines.ContainsValue(perfil))
            {
                var valid = perfil.ParseEnum<PerfilUsuario>(PerfilUsuario.Invalido);
                if (valid != PerfilUsuario.Invalido) return true;
            }
            return false;
        }
    }
}
