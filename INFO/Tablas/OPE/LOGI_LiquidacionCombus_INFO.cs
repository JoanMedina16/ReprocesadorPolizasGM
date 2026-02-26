using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.OPE
{
    public class LOGI_LiquidacionCombus_INFO
    {
        public int bitacoraID { get; set; }
        public string cuenta { get; set; }
        public Decimal total { get; set; }
        public string area { get; set; }
        public string centro { get; set; }
        public string depto { get; set; }
        public string vehiculo { get; set; }
        public string economico { get; set; }
        public string filial { get; set; }
        public string sucursal { get; set; }
        public string viaje { get; set; }
        public int liquidacionID { get; set; }
        public string estacion { get; set; }
        public string comentarios { get; set; }
        public string texto { get; set; }
        public DateTime fechaLiquidacion { get; set; }
        public string sFechaLiquidacion { get; set; }
        public DateTime fecharegistro { get; set; }
        public string sFecharegistro { get; set; }

        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string codigooper { get; set; }
        public string operador { get; set; }

        public string folio { get; set; }
        public string folioelectronico { get; set; }
        public int estatus { get; set; }
        public Decimal litros { get; set; }
        public Decimal preciolitro { get; set; }
        public string poliza { get; set; }
        public string foliod365 { get; set; }
        public string estacionclave { get; set; }
        public string FechaDocto { get; set; }
 public string sFolios { get; set; }
    }

    public class LOGI_LiquidacionBitacora_INFO
    { 
    public string folioTMS { get; set; }
        public string folioViaje { get; set; }
        public int operadorID { get; set; }
        public int tractoID { get; set; }
        public string recID { get; set; }
        public string SessionID { get; set; }
    }

    public class LOGI_Combustibles_INFO
    {
        public string cuentatms { get; set; }
        public string Concepto { get; set; }
        public string cuentad365 { get; set; }
        public string almacen365 { get; set; }
        public string sucursal365 { get; set; }
    }
}
