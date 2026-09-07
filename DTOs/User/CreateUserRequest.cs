namespace templumStudii.Dtos.User
{
    public class CreateUserRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Studies { get; set; } = "";
    }
}