﻿namespace Application.Common.Models
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
