using Microsoft.EntityFrameworkCore;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        // Register user
        public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(userTBL user)
        {
            // Lowercase the email
            user.Email = user.Email.ToLower();

            // Check if email exists
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return (false, "Email is already taken.");
            }

            // Hash password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, string.Empty);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        // Authenticate user
        public async Task<userTBL?> AuthenticateUserAsync(string email, string password)
        {
            email = email.ToLower();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }
    }
}

