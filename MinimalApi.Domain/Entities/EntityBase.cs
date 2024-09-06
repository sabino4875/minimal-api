namespace MinimalApi.Domain.Entities
{
    using System;
    /// <summary>
    /// Entidade base utilizada no mapeamento das demais entidades do projeto
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Código de identificação da entidade
        /// </summary>
        public Int32 Id { get; set; } = default!;
    }
}
