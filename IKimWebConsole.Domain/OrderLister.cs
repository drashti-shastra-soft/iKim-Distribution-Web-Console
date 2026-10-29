using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class OrderLister
    {
        public string Type { get; set; }
        public List<Order> List { get; set; } = new List<Order>();

        public Order SearchCriteria { get; set; } = new Order();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
