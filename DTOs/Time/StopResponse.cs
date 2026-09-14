namespace TemplumStudii.DTOs.Time
{
    public class StopResponse
    {
        public int Id { get; set; }
        public TimeSpan AccumulatedTime { get; set; }
        public TimeSpan DefinedTime { get; set; }
        public bool GoalReached { get; set; }
    }
}