using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class Pagination
    {
        private int _pageSize = 20;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                Take = value;
            }
        }

        public int Take { get; set; } = 20;
        public int Skip { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;
        public int TotalPage { get; set; } = 1;
        public int TotalRecord { get; set; } = 0;
        public string PaginationSummary { get; set; } = string.Empty;
    }
}
