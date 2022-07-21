using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProEventos.Persistence.Helpers
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage {get;set;}
        public int TotalPage { get; set; }
        
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public PageList(){    }

        

        public PageList(List<T> items,int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            TotalPage = (int)Math.Ceiling( count/(double)pageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(
            IQueryable<T> sources, int pageNumber, int pageSize)
        {
            var count  = await sources.CountAsync();
            var items = await sources.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
            return new PageList<T>(items,count,pageNumber,pageSize);

        }
    }
}