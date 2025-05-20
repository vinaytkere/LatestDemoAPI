namespace Application.Features.Address
{
    public class AddressProfile : Profile
    {
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