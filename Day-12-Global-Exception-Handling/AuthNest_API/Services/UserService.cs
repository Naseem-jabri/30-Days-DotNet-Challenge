using AuthNest_API.Data;
using AuthNest_API.DTOS;
using AuthNest_API.Middleware;
using AuthNest_API.Model;
using Microsoft.Extensions.Caching.Memory;

namespace AuthNest_API.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _db;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;

        public UserService(
            UserDbContext db,
            IMemoryCache cache,
            IEmailService emailService,
            JwtService jwtService)
        {
            _db = db;
            _cache = cache;
            _emailService = emailService;
            _jwtService = jwtService;
        }


        public async Task<object> CreateUserAsync(CreateUserDto userDto)
        {
            var existingUser = _db.Users
                .FirstOrDefault(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                throw new ConflictException(
                    "Email is already registered.");
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

            return new
            {
                user.Id,
                user.Username,
                user.Email
            };
        }


        public object Login(LoginDto loginDto)
        {
            var user = _db.Users.FirstOrDefault(
                u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            if (!user.IsEmailConfirmed)
            {
                throw new UnauthorizedException(
                    "Please confirm your email first.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash
            );

            if (!isPasswordValid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);

            return new
            {
                message = "Login successful",
                token = token
            };
        }


        public string ConfirmEmail(string token)
        {
            var user = _db.Users.FirstOrDefault(
                u => u.EmailConfirmationToken == token);

            if (user == null)
            {
                throw new BadRequestException(
                    "Invalid confirmation token.");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            _db.SaveChanges();

            return "Email confirmed successfully!";
        }


        public List<User> GetUsers()
        {
            var users = _cache.Get<List<User>>("users");

            if (users == null)
            {
                users = _db.Users.ToList();

                _cache.Set("users", users);
            }

            return users;
        }
    }
}