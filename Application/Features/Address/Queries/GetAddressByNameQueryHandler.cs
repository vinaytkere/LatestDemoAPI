using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Address.Queries
{
    public class GetAddressByNameQueryHandler : IRequestHandler<SearchAddressByNameQuery, List<AddressDto>>
    {
        private readonly IRepository<Domain.Address> _repository;
        private readonly IMapper _mapper;

        public GetAddressByNameQueryHandler(IRepository<Domain.Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AddressDto>> Handle(SearchAddressByNameQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.SearchByNameAsync(request.Name);
            return _mapper.Map<List<AddressDto>>(entities);
        }
    }
}
