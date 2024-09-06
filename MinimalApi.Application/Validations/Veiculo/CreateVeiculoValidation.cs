namespace MinimalApi.Application.Validations
{
    using FluentValidation;
    using MinimalApi.Application.DTOs;
    using System;

    /// <summary>
    /// Classe responsável pela validação de uma inclusão na entidade veículo
    /// </summary>
    public class CreateVeiculoValidation : AbstractValidator<CreateVeiculoDTO>
    {
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public CreateVeiculoValidation()
        {
            RuleFor(e => e.Modelo).NotNull().NotEmpty().WithMessage("O campo modelo deve ser informado.");
            RuleFor(e => e.Modelo).Length(3, 150).WithMessage("O campo modelo deve ter entre 3 e 150 caracteres.");

            RuleFor(e => e.Marca).NotNull().NotEmpty().WithMessage("O campo marca deve ser informado.");
            RuleFor(e => e.Marca).Length(3, 100).WithMessage("O campo marca deve ter entre 3 e 150 caracteres.");

            RuleFor(e => e.Ano).NotNull().NotEmpty().WithMessage("O campo ano deve ser informado.");
            RuleFor(e => e.Ano).Must(IsValidAno).WithMessage("Veículo muito antigo. Somente é permitido o cadastro de veículos a partir de 1950.");
        }

        /// <summary>
        /// Rotina para validação do campo data
        /// </summary>
        /// <param name="ano">Valor a ser validado</param>
        /// <returns>Resultado da operação</returns>
        private Boolean IsValidAno(Nullable<Int32> ano)
        {
            if (ano != null)
            {
                if (ano > 1949)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
