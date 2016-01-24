using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingWebApp.Models
{
    public class ApiRequest
    {
        public string EndPoint { get; set; }

        public object Request { get; set; }
    }
}
