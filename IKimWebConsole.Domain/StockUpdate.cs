using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class StockUpdate : IAuditable
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }
        public decimal Quantity { get; set; }
        public string Operation { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Category { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
