using System;
using System.Threading.Tasks;
using LoginAuthApp.Data;
using LoginAuthApp.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace LoginAuthApp.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = BC.HashPassword(password),
                Email = email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null && BC.Verify(password, user.PasswordHash);
        }
    }
}