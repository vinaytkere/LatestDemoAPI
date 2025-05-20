namespace Domain
{
    public class Address : BaseEntity
    {
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PinCode { get; set; } = default!;
        public string? LandMark { get; set; }
    }
}