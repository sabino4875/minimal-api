namespace Minimal.Api.Dominio.Settings
{
    using System;

    /// <summary>
    /// Classe que representa as configurações de acesso a um banco de dados
    /// </summary>
    public sealed class DatabaseItemSettings
    {
        private readonly String _hostName;
        private readonly String _databaseName;
        private readonly String _userName;
        private readonly String _password;
        private readonly Int32 _port;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="hostName">Servidor</param>
        /// <param name="databaseName">banco de dados</param>
        /// <param name="userName">usuário</param>
        /// <param name="password">senha</param>
        /// <param name="port">porta</param>
        public DatabaseItemSettings(String hostName, String databaseName,
                                String userName, String password, Int32 port)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(hostName);
            ArgumentNullException.ThrowIfNullOrEmpty(databaseName);
            ArgumentNullException.ThrowIfNullOrEmpty(userName);
            ArgumentNullException.ThrowIfNullOrEmpty(password);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(hostName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(databaseName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(userName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(password);

            ArgumentNullException.ThrowIfNull(port);

            _hostName = hostName;
            _databaseName = databaseName;
            _userName = userName;
            _password = password;
            _port = port;
        }

        /// <summary>
        /// servidor
        /// </summary>
        public String HostName => _hostName;

        /// <summary>
        /// Banco de dados
        /// </summary>
        public String DatabaseName => _databaseName;

        /// <summary>
        /// Usuário
        /// </summary>
        public String UserName => _userName;

        /// <summary>
        /// Senha
        /// </summary>
        public String Password => _password;

        /// <summary>
        /// Porta
        /// </summary>
        public Int32 Port => _port;
    }
}
