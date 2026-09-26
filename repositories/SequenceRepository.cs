using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using templumStudii.models;
using TemplumStudii.data;
using TemplumStudii.DTOs.Sequences;

namespace TemplumStudii.repositories
{
    public class SequenceRepository
    {
        private readonly TemplumStudiiContext _context;

        public SequenceRepository(TemplumStudiiContext context)
        {
            _context = context;
        }

        public async Task<Sequence> AddAsync(Sequence sequence)
        {
            EntityEntry<Sequence> response = await _context.Sequences.AddAsync(sequence);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<Sequence?> FindByIdAsync(int userId)
        {
            return await _context.Sequences.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
