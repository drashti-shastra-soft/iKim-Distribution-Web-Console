namespace IKimWebConsole.Domain
{
    public class PurchaseItem : IAuditable
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int ItemId { get; set; }
        public int PriceTierId { get; set; }
        public decimal Rate { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal TotalAmount { get; set; }
        public int AuditId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Item { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
