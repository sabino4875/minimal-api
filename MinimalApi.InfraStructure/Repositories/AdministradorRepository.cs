namespace MinimalApi.InfraStructure.Repositories
{
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Repositories;
    using MinimalApi.InfraStructure.Db;
    using System.Linq;
    using System;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class AdministradorRepository(DbContexto context) : Repository<Administrador>(context), IAdministradorRepository
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Boolean Login(String email, String senha)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(email);
            ArgumentNullException.ThrowIfNullOrEmpty(senha);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(email);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(senha);

            try
            {
                var item = this.DbSet.Where(e => e.Email == email).FirstOrDefault();
                if (item != null)
                {
                    if (item.Senha.Equals(senha, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "Houve um erro ao tentar efetuar o login.");
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Boolean AlteraSenha(String email, String senha, String codigo)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(email);
            ArgumentNullException.ThrowIfNullOrEmpty(senha);
            ArgumentNullException.ThrowIfNullOrEmpty(codigo);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(email);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(senha);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(codigo);

            try
            {
                var item = this.DbSet.Where(e => e.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (item != null)
                {
                    item.Senha = senha;
                    this.DbSet.Update(item);
                    this.Context.SaveChanges(true);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "Houve um erro ao tentar alterar a senha.");
                throw;
            }
        }
    }
}
