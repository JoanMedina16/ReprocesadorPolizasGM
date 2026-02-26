using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.LMAE
{
    public class LOGI_ListaMaestra_INFO
    {
        public int cia { get; set; }

        public int corporativo { get; set; }
        public string economico { get; set; }
        public string marca { get; set; }
        public string descripcion { get; set; }
        public int anio { get; set; }
        public string serie { get; set; }
        public string nmotor { get; set; }
        public string tipovehiculo { get; set; }
        public string placa { get; set; }
        public string tipotracto { get; set; }
        public int estatus { get; set; }
        public int suc { get; set; }
        public string sucursal { get; set; }
        public int ccosto { get; set; }
        public int depto { get; set; }
        public string departamento { get; set; }
        public string centrocosto { get; set; }
        public string responsable { get; set; }
        public string nombreestatus { get; set; }

        public string filtro_economico { get; set; }
        public string filtro_corporativo { get; set; }
        public string filtro_centro { get; set; }
        public string filtro_sucursal { get; set; }
        public string filtro_depto { get; set; }
        public string filtro_estatus { get; set; }
    }
}
