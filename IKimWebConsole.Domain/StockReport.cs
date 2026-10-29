using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class StockReport
    {
        public string Category { get; set; }
        public string Item { get; set; }
        public string FactoryStock { get; set; }
        public string GodownStock { get; set; }
    }
}
