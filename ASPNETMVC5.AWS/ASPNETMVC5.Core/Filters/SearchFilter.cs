using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETMVC5.Core.Filters
{
    public class SearchFilter
    {
        public SearchFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int? UserRoleId { get; set; }
    }
}
