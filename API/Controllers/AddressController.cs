/// <summary>
/// Endpoints for managing addresses.
/// </summary>
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Injects the mediator instance.
        /// </summary>
        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Creates a new address (Admin only).
        /// </summary>
        public async Task<IActionResult> Create(CreateAddressCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Updates an existing address (Admin only).
        /// </summary>
        public async Task<IActionResult> Update(Guid id, UpdateAddressCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Deletes an address (Admin only).
        /// </summary>
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteAddressCommand(id));
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpGet("{id:guid}")]
        /// <summary>
        /// Retrieves an address by id.
        /// </summary>
        public async Task<IActionResult> GetById(Guid id)
        {
            var address = await _mediator.Send(new GetAddressByIdQuery(id));
            return address is not null ? Ok(address) : NotFound();
        }

        [HttpGet]
        /// <summary>
        /// Retrieves a paged list of addresses.
        /// </summary>
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? city = null,
            [FromQuery] string? state = null)
        {
            var query = new GetAllAddressesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                City = city,
                State = state
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _mediator.Send(new SearchAddressByNameQuery(name));
            return Ok(result);
        }
    }
}