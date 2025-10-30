using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Models.Auth
{
    public class AuthCode
    {
        public int cod_id { get; set; }
        public string cod_code { get; set; }
        public string codusu_cedula { get; set; }
        public DateTime? cod_fecha_expiracion { get; set; }


        public AuthCode()
        {
            cod_id = 0;
            cod_code = string.Empty;
            codusu_cedula = string.Empty;
            cod_fecha_expiracion = null;

        }
    }
}
