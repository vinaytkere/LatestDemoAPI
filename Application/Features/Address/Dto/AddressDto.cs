namespace Application.Features.Address.Dto
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PinCode { get; set; } = default!;
        public string? LandMark { get; set; }
    }
}