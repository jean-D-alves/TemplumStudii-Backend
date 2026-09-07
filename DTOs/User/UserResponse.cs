namespace templumStudii.Dtos.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Studies { get; set; } = "";
    }
}