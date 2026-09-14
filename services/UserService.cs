using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using templumStudii.dtos;
using templumStudii.Dtos.User;
using templumStudii.models;
using templumStudii.repositories;

namespace templumStudii.services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > 254)
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && addr.Host.Contains('.');
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task<List<User>> ReaderAsync()
        {
            List<User> response = await _userRepository.Reader();
            return response;
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found");
            }
            return user;
        }
        public async Task<User> GetUserByEmailAsync(LoginRequest request)
        {
            string email = request.Email.Trim().ToLowerInvariant();
            User user = await VerifyUserByEmailAsync(email);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Password or email is invalid");
            }
            return user;
        }

        public async Task<User> VerifyUserByEmailAsync(string email)
        {
            User? user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found");
            }

            return user;
        }
        public async Task<User> DeleteUserAsync(int id)
        {
            User? user = await _userRepository.DeleteUserAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found");
            }
            return user;
        }
        public async Task<User> CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            string email = user.email.Trim().ToLowerInvariant();
            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Email or password invalid");
            }

            User? existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already in use");
            }

            user.email = email;
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            User response = await _userRepository.CreateAsync(user);

            return response;
        }
    }
}