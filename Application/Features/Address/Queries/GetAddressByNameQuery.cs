using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Address.Queries
{
    public record SearchAddressByNameQuery(string Name) : IRequest<List<AddressDto>>;
}
