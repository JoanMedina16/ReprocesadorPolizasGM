using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.CAT
{
    public class LOGI_Roles_INFO
    {
        public Int32 rol { get; set; }
        public string nombre { get; set; }
        public int activo { get; set; }
        public int eliminado { get; set; }
        public string fechacreacion { get; set; }
        public int usuarioID { get; set; }
        public bool bIgnorarol { get; set; }
        public string permisos { get; set; }
        public List<LOGI_Matrizrol_INFO> lstMatriz { get; set; }
    }

    public class LOGI_Permisos_INFO
    {
        public Int32 permiso { get; set; }
        public string descripcion { get; set; }

    }

    public class LOGI_Matrizrol_INFO
    {
        public string descripcion { get; set; }
        public string cuenta_inicial { get; set; }
        public string cuenta_final { get; set; }

        public Decimal rango_inicial { get; set; }
        public Decimal rango_final { get; set; } 

    }

}
