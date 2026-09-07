namespace templumStudii.models
{
    public class Sequence
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int LargerSequence { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}