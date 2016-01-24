namespace Boxing.Core.Sql.Configurations
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string FullName { get; set; }
        
        public bool IsAdmin { get; set; }

        public double Rating { get; set; }
    }
}
