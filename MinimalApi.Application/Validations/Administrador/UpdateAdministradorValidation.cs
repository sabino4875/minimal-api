namespace MinimalApi.Application.Validations
{
    using FluentValidation;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Enuns;
    using MinimalApi.Domain.Extensions;
    using MinimalApi.Domain.Helpers;
    using System;

    /// <summary>
    /// Classe responsável pela validação de uma alteração na entidade usuário
    /// </summary>
    public class UpdateAdministradorValidation : AbstractValidator<AdministradorDTO>
    {
        private readonly ApplicationRoutines _routines;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public UpdateAdministradorValidation(ApplicationRoutines routines)
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

        /// <summary>
        /// Rotina para validação do campo id
        /// </summary>
        /// <param name="id">Valor a ser validado</param>
        /// <returns>Resultado da operação</returns>
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

        /// <summary>
        /// Rotina para validação do campo status
        /// </summary>
        /// <param name="perfil">Valor a ser validado</param>
        /// <returns></returns>
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
