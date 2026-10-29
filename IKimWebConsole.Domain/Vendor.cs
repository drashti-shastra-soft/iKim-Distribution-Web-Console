namespace IKimWebConsole.Domain
{
    public class Vendor : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string GSTNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public int? AddressId { get; set; }
        public bool IsActive { get; set; } = true;
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
