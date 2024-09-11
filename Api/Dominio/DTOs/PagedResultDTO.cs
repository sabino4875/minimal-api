namespace Minimal.Api.Dominio.DTOs
{
    using System.Collections.Generic;
    using System;
    public class PagedResultDTO<TEntity> where TEntity : class
    {
        private readonly Int32 _page;
        private readonly Int32 _itemsPerPage;
        private readonly Int32 _totalPages;
        private readonly IEnumerable<TEntity> _items;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="count">Total de registros na tabela</param>
        /// <param name="page">Página atual</param>
        /// <param name="items">Listagem com dados da entidade</param>
        public PagedResultDTO(Int64 count, Int32 itemsPerPage, Int32 page,TEntity[] items)
        {
            ArgumentNullException.ThrowIfNull(count);
            ArgumentNullException.ThrowIfNull(page);
            ArgumentNullException.ThrowIfNull(itemsPerPage);
            ArgumentNullException.ThrowIfNull(items);
            _page = page;
            if(_page < 1) page = 1;
            _itemsPerPage = itemsPerPage;
            if(_itemsPerPage < 1) itemsPerPage = 10;
            if(count < 0) count = 0;

            _totalPages = Convert.ToInt32(Math.Floor(Convert.ToDecimal(count) / Convert.ToDecimal(_itemsPerPage)));

            var calc = count - (_totalPages * itemsPerPage);
            if (_totalPages < 1) _totalPages = 1;

            if (calc > 0)
            {
                _totalPages += 1;
            }

            _items = items;
        }

        /// <summary>
        /// Página atual
        /// </summary>
        public Int32 Page => _page;

        /// <summary>
        /// Total de páginas disponíveis
        /// </summary>
        public Int32 TotalPages => _totalPages;

        /// <summary>
        /// Total de itens por página
        /// </summary>
        public Int32 ItemsPerPage => _itemsPerPage;

        /// <summary>
        /// Resultado da consulta
        /// </summary>
        public IEnumerable<TEntity> Items => _items;
    }
}
