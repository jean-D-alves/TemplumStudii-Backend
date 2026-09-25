using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using templumStudii.models;
using TemplumStudii.data;
using TemplumStudii.DTOs.Time;

namespace TemplumStudii.repositories
{
    public class TimeRepository
    {
        private readonly TemplumStudiiContext _context;

        public TimeRepository(TemplumStudiiContext context)
        {
            _context = context;
        }
        public async Task<Time> AddAsync(Time time)
        {
            EntityEntry<Time> response = await _context.Times.AddAsync(time);
            await _context.SaveChangesAsync();

            return response.Entity;
        }
        public async Task<Time?> FindByIdAsync(int id)
        {
            return await _context.Times.FindAsync(id);
        }
        public async Task<Time?> FindByUserIdAsync(int userId)
        {
            return await _context.Times.FirstOrDefaultAsync(t => t.UserId == userId);
        }
        public async Task<Time> DeleteTimeAsync(Time time)
        {
            _context.Times.Remove(time);
            await _context.SaveChangesAsync();
            return time;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
