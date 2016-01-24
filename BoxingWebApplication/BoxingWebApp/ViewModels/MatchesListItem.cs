using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingWebApp.ViewModels
{
    public class MatchesListItem
    {
        public int Id { get; set; }

        [Required]
        public int Boxer1Id { get; set; }
        public BoxersListItem Boxer1 { get; set; }

        [Required]
        public int Boxer2Id { get; set; }
        public BoxersListItem Boxer2 { get; set; }

        public string Address { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public string Description { get; set; }

        public int? WinnerId { get; set; }
        public BoxersListItem Winner { get; set; }
    }
}
