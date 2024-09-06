namespace MinimalApi.Application.Validations
{
    using FluentValidation;
    using MinimalApi.Application.DTOs;
    using MinimalApi.Domain.Enuns;
    using MinimalApi.Domain.Extensions;
    using MinimalApi.Domain.Helpers;
    using System;
    using System.Linq;

    /// <summary>
    /// Classe responsável pela validação de uma inclusão na entidade usuário
    /// </summary>
    public class CreateAdministradorValidation : AbstractValidator<CreateAdministradorDTO>
    {
        private readonly ApplicationRoutines _routines;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="routines">Rotinas de uso geral no sistema</param>
        public CreateAdministradorValidation(ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(routines);
            _routines = routines;

            RuleFor(e => e.Nome).NotNull().NotEmpty().WithMessage("O campo nome deve ser informado.");
            RuleFor(e => e.Nome).Length(3, 50).WithMessage("O campo nome deve ter entre 3 e 50 caracteres.");

            RuleFor(e => e.Email).NotNull().NotEmpty().WithMessage("O campo e-mail deve ser informado.");
            RuleFor(e => e.Email).Length(2,100).WithMessage("O campo e-mail deve ter até 100 caracteres.");
            RuleFor(e => e.Email).EmailAddress().WithMessage("E-mail informado inválido.");

            RuleFor(e => e.Senha).NotNull().NotEmpty().WithMessage("O campo senha deve ser informado.");
            RuleFor(e => e.Senha).Length(8, 128).WithMessage("O campo senha deve ter entre 8 e 128 caracteres.");
            RuleFor(e => e.Senha).Must(IsValidSenha).WithMessage("Senha informada inválida.");

            RuleFor(e => e.Perfil).NotNull().NotEmpty().WithMessage("O campo perfil deve ser informado.");
            RuleFor(e => e.Perfil).Length(3, 20).WithMessage("O campo perfil deve ter entre 3 e 20 caracteres.");
            RuleFor(e => e.Perfil).Must(IsValidPerfil).WithMessage("Perfil informado inválido.");
        }

        private Boolean IsValidSenha(String senha)
        {
            if(_routines.ContainsValue(senha))
            {
                if (senha.Trim().Length < 8) return false;

                var upper = senha.Count(Char.IsUpper);//mínimo de 2 caracteres
                var lower = senha.Count(Char.IsLower);//mínimo de 2 caracteres
                var number = senha.Count(Char.IsNumber);//mínimo de 2 caracteres
                var special = senha.Count(Char.IsSymbol) + senha.Count(Char.IsPunctuation);//mínimo de 2 caracteres

                if (upper < 2 || lower < 2 || number < 2 || special < 2) return false;

                return true;
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
