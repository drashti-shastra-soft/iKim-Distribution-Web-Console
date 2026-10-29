using System;

namespace IKimWebConsole.Domain
{
    public class Purchase : IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string BillNumber { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; } = true;
        public int AuditId { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

    }
}
