using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Models.Auth
{
    public class Authentication
    {
        public string Identification { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResult
    {
        public bool Result { get; set; }
    }
}
