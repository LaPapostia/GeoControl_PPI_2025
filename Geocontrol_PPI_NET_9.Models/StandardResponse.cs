using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Models
{
    public class StandardResponse
    {
        public bool result { get; set; }
        public string message { get; set; }

        public StandardResponse()
        {
            result = false;
            message = string.Empty;
        }
    }
}
