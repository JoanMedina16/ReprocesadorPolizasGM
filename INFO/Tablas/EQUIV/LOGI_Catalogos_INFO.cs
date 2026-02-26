using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.EQUIV
{
   public class LOGI_Catalogos_INFO : LOGI_Catalogos_Filtro_INFO
    {
        public int identificador { get; set; }
        public string sCodigo { get; set; }
        public string sClave { get; set; }//se utiliza para clave otm de articulos
        public string sAX365 { get; set; }
        public string sAX365_II { get; set; } //Valor opcional  para catalogo de clientes
        public string sDescripcion { get; set; }
        public string sAX2009 { get; set; }
        public string sAX2012 { get; set; }
        public int iCuentamayor { get; set; }
        public int iCuenta { get; set; }
        public int iSubcuenta { get; set; }
        public int iTipo { get; set; }
        public string sPlanning { get; set; }
        public int iActivo { get; set; }
        public int iEliminado { get; set; }
        public int iDUsuario { get; set; }
        public int iEmpresa { get; set; }
        public int iNumconcepto { get; set; }
        public int iArea { get; set; }
        public string sNombrearea { get; set; }
        public string sCodArea { get; set; }
        public string sNombreempresa { get; set; }
        public string sFechaalta { get; set; }
        public DateTime dtFechAlta { get; set; }
        public DateTime dtFechaUltModificacion { get; set; }
        public DateTime dtFechaBaja { get; set; }

        public string sAxgrupoventa { get; set; }
        public string sAxgrupoarticulo { get; set; }

        public string sFechareginicio { get; set; }
        public string sFecharegfin { get; set; }
        //public string sUsuarioCrea { get; set; }
        //public string sUsuarioEdita { get; set; }
        public string sNombreAdjunto { get; set; }
        public string sEmpresas { get; set; }
        public string sAreas { get; set; }
        public string sNombreCodEmpresa { get; set; }
        public string sFiltroIN { get; set; }
    }

    public class LOGI_Catalogos_Filtro_INFO
    {
        public string sAX365MATCH { get; set; }
        public string sAX2009MATCH { get; set; }
        public string sAX2012MATCH { get; set; }
        public string sPlanningMATCH { get; set; }
        public int identificadorMATCH { get; set; }
    }
}
