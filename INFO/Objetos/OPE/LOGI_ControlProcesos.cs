using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.OPE
{
    public class LOGI_ControlProcesos_INFO
    {
        public DateTime FechaInicial { get; set; }
        public int ProMovimientosBancarios { get; set; }
        public int ProFacturas { get; set; }
        public int ProCancelacionFacturas { get; set; }
        public int ProNotasCredito { get; set; }
        public int ProPasivos { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime NuevaFechaSolicitud { get; set; }
        public int Estado { get; set; }
    }

    public class LOGI_EstadoControlProcesos_INFO
    {
        public bool ProFacturas { get; set; }
        public bool ProCancelacionFacturas { get; set; }
        public bool ProNotasCredito { get; set; }
        public bool ProMovimientosBancarios { get; set; }
        public bool ProPasivos { get; set; }
    }
}
