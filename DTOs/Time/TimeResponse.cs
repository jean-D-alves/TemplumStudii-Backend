namespace TemplumStudii.DTOs.Time
{
    public class TimeResponse
    {
        public int Id { get; set; }
        public TimeSpan AccumulatedTime { get; set; }
        public DateTime? StartedAt { get; set; }
        public TimeSpan DefinedTime { get; set; }
        public DateTime DefinedDate { get; set; }
        public DateTime ServerNow { get; set; }
    }
}