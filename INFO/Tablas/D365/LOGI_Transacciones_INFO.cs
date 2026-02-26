using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_Transacciones_INFO
    {
        public String FolioAsiento { get; set; }                
        public int intento { get; set; }
        public String peticion { get; set; }
        public String mensaje { get; set; }
        public String urlweb { get; set; }
        public String respuesta { get; set; }
        public int intento_mayor { get; set; }
        public int intento_menor { get; set; }
        public string FechaModificacion { get; set; }


    }
}
