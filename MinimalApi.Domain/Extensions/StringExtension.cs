namespace MinimalApi.Domain.Extensions
{
    using System;

    /// <summary>
    /// Métodos de extensão da classe String
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Recupera a descrição do valor selecionado na enum
        /// </summary>
        /// <typeparam name="T">Enum que será validado</typeparam>
        /// <param name="value">Descrição do enum</param>
        /// <param name="defaultValue">Valor padrão para o caso de algum error</param>
        /// <returns>Caso correto, retorna o enum referente ao valor informado, caso contrário, o valor padrão.</returns>
        public static T ParseEnum<T>(this String value, T defaultValue) where T : Enum
        {
            if (Enum.TryParse(typeof(T), value, out Object result))
            {
                return (T)result;
            }
            return defaultValue;
        }
    }
}
