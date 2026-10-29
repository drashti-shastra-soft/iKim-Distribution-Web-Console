using System.Collections.Generic;

namespace IKimWebConsole.Domain
{
    public class OrderLine : IAuditable
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }
        public int QuantityRequested { get; set; }
        public int QuantityFulfilled { get; set; }
        public int FulfilledQuantity { get; set; }
        public string QuantityWithUnit { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public int PriceTierId { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

        public List<Domain.OrderLine> OrderLineItems { get; set; } = new List<Domain.OrderLine>();
    }
}
