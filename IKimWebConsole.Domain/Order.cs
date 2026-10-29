using System;
using System.Collections.Generic;

namespace IKimWebConsole.Domain
{
    public class Order : IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int StoreId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public int AuditId { get; set; }
        public string Store { get; set; }
        public int ItemId { get; set; }
        public int QuantityRequested { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public string Token { get; set; }
    }
}
