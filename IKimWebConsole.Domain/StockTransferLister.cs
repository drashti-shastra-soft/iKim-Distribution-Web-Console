using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class StockTransferLister
    {
        public List<StockTransfer> List { get; set; } = new List<StockTransfer>();

        public StockTransfer SearchCriteria { get; set; } = new StockTransfer();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
