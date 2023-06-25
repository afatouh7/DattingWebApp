using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace APi.Helpers
{
    public class PagedLIst<T>:List<T>
    {
        public PagedLIst(IEnumerable<T> items, int count, int pafeNumber, int pageSize)
        {
            CurrentPage = pafeNumber;
            TotalPages = (int)Math.Ceiling(count/(double) pageSize);
            PageSize = pageSize;
            TOtalCount = count;
          AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TOtalCount { get; set; } 
        public static async Task<PagedLIst<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedLIst<T>(items, count, pageNumber, pageSize);
        }

    }
}
