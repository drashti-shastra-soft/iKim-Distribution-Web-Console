using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public class ItemFulfillment : IAuditable
    {
        public int Id { get; set; }
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int QuantityRequested { get; set; }
        public int QuantityFulfilled { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
