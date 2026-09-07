namespace templumStudii.models
{
    public class User
    {
        public int Id { get; set; }
        public required string name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public string studies { get; set; } = "";

        public ICollection<Time> Times { get; set; } = new List<Time>();
        public ICollection<Sequence> Sequences { get; set; } = new List<Sequence>();
    }
}