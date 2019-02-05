namespace WebApplication1.Database.Entities
{
    public class Password
    {
        public int Id { get; set; }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
        
        public User User { get; set; }
    }
}