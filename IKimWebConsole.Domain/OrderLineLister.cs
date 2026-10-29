using System.Collections.Generic;

namespace IKimWebConsole.Domain
{
    public class OrderLineLister
    {
        public List<OrderLine> List { get; set; } = new List<OrderLine>();

        public OrderLine SearchCriteria { get; set; } = new OrderLine();

        public Pagination Pagination { get; set; } = new Pagination();

        public string Store { get; set; } = string.Empty;
    }
}
