using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IKimWebConsole.Helper
{
    public static class PaginationHelper
    {
        public static string GetPaginationSummary(Domain.Pagination pagination)
        {
            if (pagination.TotalRecord == 0)
                return "No entries to show.";

            int start = pagination.Skip + 1;
            int end = Math.Min(pagination.Skip + pagination.Take, pagination.TotalRecord);

            return $"Showing {start} to {end} of {pagination.TotalRecord} entries";
        }
    }
}