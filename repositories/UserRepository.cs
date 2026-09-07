using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using templumStudii.Dtos.User;
using templumStudii.models;
using TemplumStudii.data;

namespace templumStudii.repositories
{
    public class UserRepository
    {
        private readonly TemplumStudiiContext _context;
        public UserRepository(TemplumStudiiContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Reader()
        {
            List<User> response = await _context.Users.ToListAsync();
            return response;

        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            return user;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            return user;
        }

        public async Task<User> CreateAsync(User user)
        {
            EntityEntry<User> response = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return response.Entity;
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
