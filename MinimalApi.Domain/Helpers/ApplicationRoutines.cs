namespace MinimalApi.Domain.Helpers
{
    using System.Globalization;
    using System.Text;
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Classe contendo rotinas de uso geral utilizadas no sistema
    /// </summary>
    public sealed class ApplicationRoutines
    {
    }

    /// <summary>
    /// Métodos de extensão da entidade ApplicationRoutines
    /// </summary>
    public static class ApplicationRoutinesExtension
    {
        /// <summary>
        /// Verifica se um campo String contem algum valor informado.
        /// </summary>
        /// <param name="value">Valor a ser verificado</param>
        /// <param name="routines">Entidade base</param>
        /// <returns>O resultado da operação</returns>
        public static Boolean ContainsValue(this ApplicationRoutines routines, String value)
        {
            ArgumentNullException.ThrowIfNull(routines);
            if (String.IsNullOrEmpty(value)) return false;
            if (String.IsNullOrWhiteSpace(value)) return false;
            if (value.Trim().Length < 1) return false;
            return true;
        }

        /// <summary>
        /// Convert um hash no formato base 64 para String
        /// </summary>
        /// <param name="value">Hash a ser convertido para String</param>
        /// <param name="routines">Entidade base</param>
        /// <returns>O valor correspondente ao hash no formato baser64</returns>
        public static String FromBase64String(this ApplicationRoutines routines, String value)
        {
            ArgumentNullException.ThrowIfNull(routines);

            if (routines.ContainsValue(value))
            {
                try
                {
                    var decoded = Convert.FromBase64String(value);
                    return Encoding.UTF8.GetString(decoded);
                }
                catch
                {
                    throw;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Gera um hash no formato base 64
        /// </summary>
        /// <param name="value">Valor a ser utilizado na geração do hash</param>
        /// <param name="routines">Entidade base</param>
        /// <returns>O hash do valor no formato base64</returns>
        public static String ToBase64String(this ApplicationRoutines routines, String value)
        {
            ArgumentNullException.ThrowIfNull(routines);
            if (routines.ContainsValue(value))
            {
                try
                {
                    var encoded = Encoding.UTF8.GetBytes(value);
                    return Convert.ToBase64String(encoded);
                }
                catch
                {
                    throw;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Cria um hash de 256 bits
        /// </summary>
        /// <param name="value">Valor a ser utilizado na geração do hash</param>
        /// <param name="routines">Entidade base</param>
        /// <returns>O hash do valor no formato sha256</returns>
        public static String ToSHA256Hash(this ApplicationRoutines routines, String value)
        {
            ArgumentNullException.ThrowIfNull(routines);
            if (routines.ContainsValue(value))
            {
                try
                {
                    var data = SHA256.HashData(Encoding.UTF8.GetBytes(value));
                    var result = new StringBuilder();
                    for (var i = 0; i < data.Length; i++)
                    {
                        result.Append(data[i].ToString("X2", CultureInfo.CurrentCulture));
                    }
                    return result.ToString();
                }
                catch
                {
                    throw;
                }
            }
            return String.Empty;
        }

    }
}

