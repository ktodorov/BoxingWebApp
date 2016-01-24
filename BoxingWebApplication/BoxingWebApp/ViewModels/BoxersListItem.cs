using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingWebApp.ViewModels
{
    public class BoxersListItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public BoxersListItem()
        { }

        public BoxersListItem(string name)
        {
            Name = name;
        }
    }
}
