using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingWebApp.ViewModels
{
    public class PredictionsListItem
    {
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        public MatchesListItem Match { get; set; }

        [Required]
        public int PredictedBoxerId { get; set; }

        public BoxersListItem PredictedBoxer { get; set; }

        public int UserId { get; set; }

        public UsersListItem User { get; set; }
    }
}
