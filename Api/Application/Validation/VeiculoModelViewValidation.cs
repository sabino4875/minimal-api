namespace Minimal.Api.Application.Validation
{
    using FluentValidation;
    using Minimal.Api.Dominio.ModelViews;
    using System;

    public class VeiculoModelViewValidation : AbstractValidator<VeiculoModelView>
    {
        public VeiculoModelViewValidation()
        {
            RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("O campo id deve ser informado.");
            RuleFor(e => e.Id).Must(IsValidId).WithMessage("Valor informado inválido para o campo id.");

            RuleFor(e => e.Modelo).NotNull().NotEmpty().WithMessage("O campo modelo deve ser informado.");
            RuleFor(e => e.Modelo).Length(3, 150).WithMessage("O campo modelo deve ter entre 3 e 150 caracteres.");

            RuleFor(e => e.Marca).NotNull().NotEmpty().WithMessage("O campo marca deve ser informado.");
            RuleFor(e => e.Marca).Length(3, 100).WithMessage("O campo marca deve ter entre 3 e 100 caracteres.");

            RuleFor(e => e.Ano).NotNull().NotEmpty().WithMessage("O campo ano deve ser informado.");
            RuleFor(e => e.Ano).Must(IsValidAno).WithMessage("Veículo muito antigo. Somente é permitido o cadastro de veículos a partir de 1950.");
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
