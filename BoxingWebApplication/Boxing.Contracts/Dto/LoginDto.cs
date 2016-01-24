using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Dto
{
    public class LoginDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserDto User { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string AuthToken { get; set; }
    }
}
