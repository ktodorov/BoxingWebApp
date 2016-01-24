using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingWebApp.ViewModels
{
    public class LoginsListItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UsersListItem User { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string AuthToken { get; set; }
    }
}
