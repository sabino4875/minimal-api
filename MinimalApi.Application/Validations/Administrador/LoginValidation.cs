namespace MinimalApi.Application.Validations
{
    using FluentValidation;
    using MinimalApi.Application.DTOs;

    /// <summary>
    /// Classe responsável pela validação de login de um usuário
    /// </summary>
    public class LoginValidation : AbstractValidator<LoginDTO>
    {
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public LoginValidation()
        {
            RuleFor(e => e.Email).NotNull().NotEmpty().WithMessage("O campo e-mail deve ser informado.");
            RuleFor(e => e.Email).EmailAddress().WithMessage("E-mail informado inválido.");

            RuleFor(e => e.Senha).NotNull().NotEmpty().WithMessage("O campo senha deve ser informado.");
        }
    }
}
