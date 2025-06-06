/// <summary>
/// Represents an address record stored in the system.
/// </summary>
namespace Domain
{
    public class Address : BaseEntity
    {
        /// <summary>
        /// Country portion of the address.
        /// </summary>
        public string Country { get; set; } = default!;
        /// <summary>
        /// State or province.
        /// </summary>
        public string State { get; set; } = default!;
        /// <summary>
        /// Name of the city.
        /// </summary>
        public string City { get; set; } = default!;
        /// <summary>
        /// Postal code for the area.
        /// </summary>
        public string PinCode { get; set; } = default!;
        /// <summary>
        /// Optional landmark or description.
        /// </summary>
        public string? LandMark { get; set; }
    }
}
