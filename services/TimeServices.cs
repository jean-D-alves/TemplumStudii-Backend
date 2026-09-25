using Microsoft.EntityFrameworkCore;
using templumStudii.models;
using TemplumStudii.DTOs.Time;
using TemplumStudii.repositories;
namespace templumStudii.services {

    public class TimeService
    {
        private readonly TimeRepository _repository;

        public TimeService(TimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<TimeResponse> FindByUserIdAsync(int userId)
        {
            var time = await _repository.FindByUserIdAsync(userId);
            if (time is null)
                throw new KeyNotFoundException("Registro de tempo não encontrado.");

            return new TimeResponse
            {
                Id = time.Id,
                DefinedTime = time.DefinedTime,
                DefinedDate = time.DefinedDate,
                AccumulatedTime = time.AccumulatedTime,
                StartedAt = time.StartedAt,
                ServerNow = DateTime.UtcNow
            };
        }

        public async Task<TimeResponse> DefineAsync( TimeSpan definedTime, int userId)
        {
            var existing = await _repository.FindByUserIdAsync(userId);

            if (existing is not null)
            {
                existing.DefinedTime = definedTime;
                await _repository.SaveChangesAsync();
                return new TimeResponse { Id = existing.Id, DefinedTime = existing.DefinedTime, AccumulatedTime = existing.AccumulatedTime };
            }

            var time = new Time {
                DefinedTime = definedTime,
                AccumulatedTime = TimeSpan.Zero,
                DefinedDate = DateTime.UtcNow,
                StartedAt = null,
                UserId = userId
            };
            var saved = await _repository.AddAsync(time);
            return new TimeResponse {
                Id = saved.Id,
                DefinedTime = saved.DefinedTime,
                DefinedDate = saved.DefinedDate,
                AccumulatedTime = saved.AccumulatedTime
            };
        }
        public async Task<TimeResponse?> StartAsync(int userId)
        {
            var time = await _repository.FindByUserIdAsync(userId);
            if (time is null)
                throw new KeyNotFoundException("Registro de tempo não encontrado.");

            if (time.StartedAt is not null)
                throw new InvalidOperationException("O cronômetro já está rodando.");

            var now = DateTime.UtcNow;
            time.StartedAt = now;
            await _repository.SaveChangesAsync();

            return new TimeResponse
            {
                Id = time.Id,
                DefinedTime = time.DefinedTime,
                AccumulatedTime = time.AccumulatedTime,
                StartedAt = time.StartedAt,
                ServerNow = now
            };
        }
        public async Task<StopResponse> StopAsync(int userId)
        {
            var time = await _repository.FindByUserIdAsync(userId);
            if (time is null)
                throw new KeyNotFoundException("Registro de tempo não encontrado.");

            if (time.StartedAt is not null)
            {
                time.AccumulatedTime += DateTime.UtcNow - time.StartedAt.Value;
                time.StartedAt = null;
            }

            await _repository.SaveChangesAsync();

            return new StopResponse
            {
                Id = time.Id,
                AccumulatedTime = time.AccumulatedTime,
                DefinedTime = time.DefinedTime,
                GoalReached = time.AccumulatedTime >= time.DefinedTime
            };
        }
        public async Task<String> ValidateTimeAsync(int userId)
        {
            var time = await _repository.FindByUserIdAsync(userId);
            if (time is null)
                throw new KeyNotFoundException("Registro de tempo não encontrado.");
            DateTime definedDate = time.DefinedDate;
            TimeSpan diference = DateTime.UtcNow - definedDate;
            if(diference >= TimeSpan.FromDays(7))
            {
                await DeleteTimeAsync(userId);
                return "Time not is valid";
            }
            else
            {
                return "Time is valid";
            }

        }
        public async Task<String> DeleteTimeAsync(int userId)
        {
            Time? time = await _repository.FindByUserIdAsync(userId);
            if (time == null)
            {
                throw new Exception("Time not found");
            }

            Time Delete = await _repository.DeleteTimeAsync(time);
            if(Delete == null)
            {
                throw new Exception("Error deleting time");
            }
            return "Time deleted successfully";
            
        }
    }
}

