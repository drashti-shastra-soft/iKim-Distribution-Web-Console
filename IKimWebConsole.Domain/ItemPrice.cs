using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class ItemPrice : IAuditable
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int PricingTierId { get; set; }
        public string PricingTierName { get; set; }
        public decimal Price { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
