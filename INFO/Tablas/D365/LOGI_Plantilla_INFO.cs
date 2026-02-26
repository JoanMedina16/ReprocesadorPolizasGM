using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_Plantilla_INFO
    {
        public String FolioAsistente { get; set; }
        public String FolioAsistentemath { get; set; }
        public string plantillanom { get; set; }
        public int ano { get; set; }
        public int tipo { get; set; }

        public int mes { get; set; }

        public string pathcabecera { get; set; }
        public string pathdetalle { get; set; }
        public string fechacreado { get; set; }
        public int usuariocreo { get; set; }
        public int activo { get; set; }
        public string fechaeliminado { get; set; }
        public int usuarioelimino { get; set; }
        public string fechainicio { get; set; }
        public string fechafin { get; set; }
    }
}
