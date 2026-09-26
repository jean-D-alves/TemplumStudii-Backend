using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using templumStudii.models;
using TemplumStudii.data;
using TemplumStudii.DTOs.Sequences;
using TemplumStudii.repositories;

namespace TemplumStudii.services
{
    public class SequenceServices
    {

        private readonly SequenceRepository _repository;

        public SequenceServices(SequenceRepository repository)
        {
            _repository = repository;
        }
        public async Task<Sequence> AddAsync(int userId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }
            Sequence sequence = new Sequence
            {
                Value = 0,
                LargerSequence = 0,
                UserId = userId
            };
            Sequence response = await _repository.AddAsync(sequence);
            return response;
        }

        public async Task<Sequence?> FindByIdAsync(int userId)
        {
            return await _repository.FindByIdAsync(userId);
        }
    }
}
