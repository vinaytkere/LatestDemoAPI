/// <summary>
/// AutoMapper profile configuration for address mappings.
/// </summary>
namespace Application.Features.Address
{
    public class AddressProfile : Profile
    {
        /// <summary>
        /// Configure the mapping rules for address objects.
        /// </summary>
        public AddressProfile()
        {
            CreateMap<Domain.Address, AddressDto>();

            CreateMap<CreateAddressCommand, Domain.Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateAddressCommand, Domain.Address>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
