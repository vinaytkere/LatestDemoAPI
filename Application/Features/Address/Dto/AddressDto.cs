/// <summary>
/// Data transfer object representing an address.
/// </summary>
namespace Application.Features.Address.Dto
{
    public class AddressDto
    {
        /// <summary>
        /// Identifier of the address.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Country portion.
        /// </summary>
        public string Country { get; set; } = default!;
        /// <summary>
        /// State or province.
        /// </summary>
        public string State { get; set; } = default!;
        /// <summary>
        /// City name.
        /// </summary>
        public string City { get; set; } = default!;
        /// <summary>
        /// Postal code.
        /// </summary>
        public string PinCode { get; set; } = default!;
        /// <summary>
        /// Optional landmark.
        /// </summary>
        public string? LandMark { get; set; }
    }
}
