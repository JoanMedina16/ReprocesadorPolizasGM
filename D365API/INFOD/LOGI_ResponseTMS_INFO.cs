using D365API.Helpers;
using D365API.Helpers.D365FOBBICxCServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365API.INFOD
{
    public class LOGI_ResponseTMS_INFO
    {
        public bool Success { get; set; }
        public string VersionAPI { get; set; }
       // public string IdRequest { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public List< LOGI_RequestTMS_INFO> Result { get; set; }
       // {    "Success": true,"VersionAPI": "25.14.01.04","PaginaActual": "1","TotalPaginas": 1,"Result": {

        //{ "Success":true, "VersionAPI":"1.31.09.02", "IdRequest":"", "Result":[ { "Mensaje":"No se encontro informacion con el filtro seleccionado" } ] }
    }
    public class LOGI_RequestTMS_INFO
    {
        public string Mensaje { get; set; }
        public List<BBICargarFacturaCxCContractEnc> Encabezado{ get; set; }
        
    }

}
