using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Models.Notations
{
    public class Notation
    {
        public int mar_consecutivoP { get; set; }
        public string marusu_cedula { get; set; }
        public int marzonmar_consecutivo { get; set; }
        public DateTime mar_fecha { get; set; }
        public string mar_tipo { get; set; }
        public string mar_archivo { get; set; }
        public bool mar_estado { get; set; }
        public string mar_coordenadas { get; set; }
        public string mar_observacion { get; set; }
        public bool? Creation{ get; set; }

        public Notation()
        {
            this.mar_consecutivoP = 0;
            this.marusu_cedula = string.Empty;
            this.marzonmar_consecutivo = 0;
            this.mar_fecha = DateTime.Now;
            this.mar_tipo = string.Empty;
            this.mar_archivo = string.Empty;
            this.mar_estado = false;
            this.mar_coordenadas = string.Empty;
            this.mar_observacion = string.Empty;
        }

    }
}
