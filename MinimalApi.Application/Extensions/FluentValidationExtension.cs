namespace MinimalApi.Application.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation.Results;

    /// <summary>
    /// Métodos de extensão da classe ValidationResult
    /// </summary>
    public static class FluentValidationExtension
    {
        /// <summary>
        /// Converte os erros para um dicionário de dados
        /// </summary>
        /// <param name="validationResult">O retorno de um validador</param>
        /// <returns>O dicionário gerado</returns>
        public static IDictionary<String, String[]> ToDictionary(this ValidationResult validationResult)
        {
            ArgumentNullException.ThrowIfNull(validationResult);
            return validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => $"{x.ErrorCode} - ${x.ErrorMessage}").ToArray()
                );
        }
    }
}
