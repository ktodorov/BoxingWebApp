using System;

namespace Boxing.Core.Sql.Configurations
{
    public class LoginEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string AuthToken { get; set; }
    }
}
