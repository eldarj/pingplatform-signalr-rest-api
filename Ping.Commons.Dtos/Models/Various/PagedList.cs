using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Various
{
    public class PagedList<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public int Total { get; set; }
        public ICollection<T> Items { get; set; }
    }
}
