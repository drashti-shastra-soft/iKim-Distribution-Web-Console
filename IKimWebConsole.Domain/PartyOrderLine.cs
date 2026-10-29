namespace IKimWebConsole.Domain
{
    public class PartyOrderLine : IAuditable
    {
        public int Id { get; set; }
        public int PartyOrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
