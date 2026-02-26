using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_Requisicion_INFO
    {
        public int PURCHREQTYPE { get; set; }
        public string ORIGINATOR { get; set; }
        public string RECID { get; set; }
        public string PURCHREQID { get; set; }
        public string PURCHREQNAME { get; set; }
        public string REQUIREDDATE { get; set; }
        public int REQUISITIONSTATUS { get; set; }
        public string ESTATUS { get; set; }
        public Double TOTAL { get; set; }

        public string FECHA_INICIO { get; set; }
        public string FECHA_FINAL { get; set; }

        public Double MINIMO_TOTAL { get; set; }
        public Double MAXIMO_TOTAL { get; set; }
        public string MAIACCOUNT { get; set; }
        public string SUCURSAL { get; set; }
        public string CENTRO_DE_COSTO { get; set; }
        public string DEPARTAMENTO { get; set; }
    }

    public class LOGI_Requisicion_Line_INFO
    {
        public string RECID { get; set; }
        public string DELIVERYNAME { get; set; }
        public string ITEMID { get; set; }
        public Double LINEAMOUNT { get; set; }
        public Double PURCHQTY { get; set; }
        public string NAME { get; set; }
        public string TAXGROUP { get; set; }
        public Double PURCHPRICE { get; set; } 

    }
}
