using AuthNest_API.Data;
using AuthNest_API.DTOS;
using AuthNest_API.Model;
using AuthNest_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthNest_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _db;
        private readonly JwtService _jwtService;
        public UsersController(
     UserDbContext db,
     JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _db.Users.Add(user);
            _db.SaveChanges();

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
            var users = _db.Users.ToList();

            return Ok(users);
        }
    }
}