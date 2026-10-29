using System;

namespace IKimWebConsole.Domain
{
    public class PartyOrder : IAuditable
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Store { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryLocation { get; set; }
        public int? NumOfCups { get; set; }
        public int? NumOfSpoons { get; set; }
        public int? NumOfGlass { get; set; }
        public decimal TotalAmount { get; set; }
        public string WithWaiter { get; set; }
        public string Narration { get; set; }
        public int? WaiterCharge { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
