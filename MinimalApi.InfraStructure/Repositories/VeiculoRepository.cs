namespace MinimalApi.InfraStructure.Repositories
{
    using MinimalApi.Domain.Entities;
    using MinimalApi.Domain.Repositories;
    using MinimalApi.InfraStructure.Db;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class VeiculoRepository(DbContexto context) : Repository<Veiculo>(context), IVeiculoRepository
    {
    }
}
