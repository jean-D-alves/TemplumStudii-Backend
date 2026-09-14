namespace templumStudii.models
{
    public class Time
    {
        public int Id { get; set; }
        public TimeSpan AccumulatedTime { get; set; } 
        public DateTime? StartedAt { get; set; }        
        public TimeSpan DefinedTime { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}