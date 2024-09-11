namespace Minimal.Api.Dominio.Settings
{
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Classe que representa as configurações do sistema
    /// </summary>
    public sealed class ApplicationSettings
    {
        private readonly DatabaseItemSettings _databaseSettings;
        private readonly JwtItemSettings _jwtSettings;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="configuration">Representa um conjunto de valores chave/valor para configurações do sistema</param>
        public ApplicationSettings(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            Int32 dbPort = 3306;
            Int32 jwtSeconds = 86400; //1 dia

            var strValue = configuration["MYSQL_PORT"] ?? configuration.GetConnectionString("Port");
            if (Int32.TryParse(strValue, out Int32 port))
            {
                dbPort = port;
            }

            strValue = configuration["JWT_SECONDS"] ?? configuration["Jwt:Seconds"];
            if (Int32.TryParse(strValue, out Int32 seconds))
            {
                jwtSeconds = seconds;
            }

            _databaseSettings = new DatabaseItemSettings(
                hostName: configuration["MYSQL_HOST"] ?? configuration.GetConnectionString("Host"),
                databaseName: configuration["MYSQL_DATABASE"] ?? configuration.GetConnectionString("Database"),
                userName: configuration["MYSQL_USER"] ?? configuration.GetConnectionString("UserName"),
                password: configuration["MYSQL_PASSWORD"] ?? configuration.GetConnectionString("Password"),
                port: dbPort
            );
            _jwtSettings = new JwtItemSettings(
                issuer: configuration["JWT_ISSUER"] ?? configuration["Jwt:Issuer"],
                audience: configuration["JWT_AUDIENCE"] ?? configuration["Jwt:Audience"],
                secret: configuration["JWT_SECRET"] ?? configuration["Jwt:Secret"],
                seconds: jwtSeconds
            );
        }

        /// <summary>
        /// Configurações para acesso ao banco de dados
        /// </summary>
        public DatabaseItemSettings DatabaseSettings => _databaseSettings;
        /// <summary>
        /// Configurações para configuração, criação, validação e uso de tokens
        /// </summary>
        public JwtItemSettings JwtSettings => _jwtSettings;
    }
}
