using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_Documentos_INFO
    {
        public int numerodoc { get; set; }
        public String nombre { get; set; }
        public String diario { get; set; }
        public String metodo { get; set; }
        public int activo { get; set; }
    }

    public class LOGI_DocumentosProcesa_INFO
    {
        public int Id { get; set; }
        public String Proceso { get; set; }
    }
}
