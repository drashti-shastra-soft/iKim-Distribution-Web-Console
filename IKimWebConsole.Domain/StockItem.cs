using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class StockItem : IAuditable
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public int PriceTierId { get; set; }
        public string Stock { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public string QuantityWithUnit { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
