namespace templumStudii.models
{
    public class Time
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int DefinedTime { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}