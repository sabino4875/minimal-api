namespace MinimalApi.Application.DTOs
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System;
 
    /// <summary>
    /// Representa o resultado de uma listagem contendo a paginação de uma entidade
    /// </summary>
    /// <typeparam name="TEntity">Entidade</typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PagedResultDTO<TEntity> where TEntity : class
    {
        private readonly Int32 _page;
        private readonly Int32 _totalPages;
        private readonly Int64 _total;
        private readonly IEnumerable<TEntity> _items;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="count">Total de registros na tabela</param>
        /// <param name="page">Página atual</param>
        /// <param name="itemsPerPage">Quantidade de registros por página</param>
        /// <param name="items">Listagem com dados da entidade</param>
        public PagedResultDTO(Int64 count, Int32 page, Int32 itemsPerPage, IEnumerable<TEntity> items)
        {
            ArgumentNullException.ThrowIfNull(count);
            ArgumentNullException.ThrowIfNull(page);
            ArgumentNullException.ThrowIfNull(itemsPerPage);
            ArgumentNullException.ThrowIfNull(items);

            _page = page;
            if (_page < 1) _page = 1;

            if (itemsPerPage < 10) itemsPerPage = 10;
            if(itemsPerPage > 50) itemsPerPage = 50;
            _total = count;
            if(_total < 0) _total = 0;
            
            _totalPages = Convert.ToInt32(Math.Floor(Convert.ToDecimal(count) / Convert.ToDecimal(itemsPerPage)));

            var calc = count - (_totalPages * itemsPerPage);
            if (_totalPages < 1) _totalPages = 1;

            if (calc > 0)
            {
                _totalPages += 1;
            }

            _items = items;
        }

        /// <summary>
        /// Total de registros
        /// </summary>
        [JsonProperty("total")]
        public Int64 Total => _total;


        /// <summary>
        /// Página atual
        /// </summary>
        [JsonProperty("page")]
        public Int32 Page => _page;

        /// <summary>
        /// Total de páginas disponíveis
        /// </summary>
        [JsonProperty("totalPages")]
        public Int32 TotalPages => _totalPages;

        /// <summary>
        /// Resultado da consulta
        /// </summary>
        [JsonProperty("items")]
        public IEnumerable<TEntity> Items => _items;
    }
}
