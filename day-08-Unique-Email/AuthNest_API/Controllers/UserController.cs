using AuthNest_API.Data;
using AuthNest_API.DTOS;
using AuthNest_API.Model;
using AuthNest_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace AuthNest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _db;
        private readonly JwtService _jwtService;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public UsersController(
     UserDbContext db,
     JwtService jwtService, IMemoryCache cache, IEmailService emailService)
        {
            _db = db;
            _jwtService = jwtService;
            _cache = cache;
            _emailService = emailService;

        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _db.Users
    .FirstOrDefault(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                return Conflict("Email is already registered.");
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                IsEmailConfirmed = false,
                EmailConfirmationToken = Guid.NewGuid().ToString()
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            _cache.Remove("users");


            var confirmationLink =
    $"https://localhost:7210/api/Users/confirm-email?token={user.EmailConfirmationToken}";

            await _emailService.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"Hello {user.Username}, please confirm your email by clicking this link: {confirmationLink}"
            );


            await _emailService.SendEmailAsync(
            user.Email,
             "Welcome to AuthNest",
              $"Hello {user.Username}, welcome to AuthNest!"
             );

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email
            });
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _db.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            if (!user.IsEmailConfirmed)
            {
                return Unauthorized("Please confirm your email first.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash
            );

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                message = "Login successful",
                token = token
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _cache.Get<List<User>>("users");

            if (users == null)
            {
                Console.WriteLine("CACHE MISS Getting users from database");

                users = _db.Users.ToList();

                _cache.Set("users", users);
            }
            else
            {
                Console.WriteLine("CACHE HIT Getting users from cache");
            }

            return Ok(users);
        }


        


        [HttpGet("confirm-email")]
        public IActionResult ConfirmEmail(string token)
        {
            var user = _db.Users.FirstOrDefault(u => u.EmailConfirmationToken == token);

            if (user == null)
            {
                return BadRequest("Invalid confirmation token.");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            _db.SaveChanges();

            return Ok("Email confirmed successfully!");
        }

    }
}