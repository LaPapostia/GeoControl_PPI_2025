using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Models.Notations
{
    public class Zone
    {
        public int zonmar_consecutivoP { get; set; }
        public string zonmar_nombre { get; set; }
        public string zonmar_coordenadas { get; set; }
        public DateTime zonmar_hora_inicio { get; set; }
        public DateTime zonmar_hora_fin { get; set; }
        public string zonmar_observacion { get; set; }
        public bool zonmar_estado { get; set; }

        public Zone()
        {
            zonmar_consecutivoP = 0;
            zonmar_nombre = string.Empty;
            zonmar_coordenadas = string.Empty;
            zonmar_hora_inicio = DateTime.MinValue;
            zonmar_hora_fin = DateTime.MinValue;
            zonmar_observacion = string.Empty;
            zonmar_estado = false;
        }
    }
}
