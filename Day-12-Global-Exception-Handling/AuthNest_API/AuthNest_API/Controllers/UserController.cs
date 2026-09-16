using AuthNest_API.DTOS;
using AuthNest_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthNest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    /// <summary>
    /// Provides endpoints for user registration, authentication,
    /// email confirmation, and retrieving users.
    /// </summary>
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Creates a new user account and sends an email confirmation link.
        /// </summary>
        /// <response code="200">User created successfully.</response>
        /// <response code="400">Invalid registration data.</response>
        /// <response code="409">The email is already registered.</response>
        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(userDto);

            return Ok(result);
        }


        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid login data.</response>
        /// <response code="401">Invalid credentials or email not confirmed.</response>
        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.Login(loginDto);

            return Ok(result);
        }


        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        /// <response code="200">Users retrieved successfully.</response>
        /// <response code="401">Authentication is required.</response>
        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();

            return Ok(users);
        }


        /// <summary>
        /// Confirms a user's email address using the confirmation token.
        /// </summary>
        /// <response code="200">Email confirmed successfully.</response>
        /// <response code="400">Invalid confirmation token.</response>
        [HttpGet("confirm-email")]
        public IActionResult ConfirmEmail(string token)
        {
            var result = _userService.ConfirmEmail(token);

            return Ok(result);
        }
    }
}