using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos
{
   public class LOGI_Tablainformacion_INFO
    {
        public List<LOGI_Columnas_INFO> lstHeader { get; set; }
        public List<List<LOGI_Filas_INFO>> lstValues { get; set; }
    }

    public class LOGI_Columnas_INFO
    {
        public string sColum { get; set; }
        public string sType { get; set; }
    }

    public class LOGI_Filas_INFO
    {
        public string sValor { get; set; }
    }
}