/// <summary>
/// Exposes authentication related endpoints.
/// </summary>
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor injecting the mediator.
        /// </summary>
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost("token")]
        //[AllowAnonymous]
        ///// <summary>
        ///// Generates a JWT token for valid login credentials.
        ///// </summary>
        //public async Task<IActionResult> GenerateToken(LoginCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
        //}

        [HttpPost("register")]
        [AllowAnonymous]
        /// <summary>
        /// Registers a new user account.
        /// </summary>
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        /// <summary>
        /// Generates a JWT token for valid login credentials.
        /// </summary>
        public async Task<IActionResult> GenerateToken(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : Unauthorized(result.Error);
        }
    }
}

