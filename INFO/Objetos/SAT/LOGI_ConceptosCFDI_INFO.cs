using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Objetos.SAT
{
   public class LOGI_ConceptosCFDI_INFO
    {
        public string ClaveProdServ { get; set; }
        public string NoIdentificacion { get; set; }
        public decimal Cantidad { get; set; }
        public string ClaveUnidad { get; set; }
        public string Unidad { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal Descuento { get; set; }
        public List<ImpuestoConceptoCFDI> Impuestos { get; set; }
        public InformacionAduaneraConceptoCFDI DatosAduana { get; set; }
    }
}
