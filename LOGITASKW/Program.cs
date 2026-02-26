using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PD.Herramientas;
using PD.Objetos.OPE;
using PD.Tablas.D365;
using PD.Tablas.FUEL;
using D365;
using D365.Helpers;
using D365.Helpers.D365FOBBIContaServices;
using D365.Helpers.D365FOBBICxCServices;
using D365.INFOD;
using INFO.Enums;
using INFO.Objetos.SAT;
using INFO.Tablas.D365;
using INFO.Tablas.EQUIV;
using INFO.Tablas.FUEL;
using Newtonsoft.Json;
using System.Configuration;

namespace LOGITASKW
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LOGI_Tools_PD oTools = new LOGI_Tools_PD();
            string CONEXION_LOGI = ConfigurationManager.AppSettings["CONEXION_LOGI"].ToString();// @"Data Source=10.20.128.149;Initial Catalog=admlog04;Persist Security Info=True;User ID=usr_pbasbil;Password=desarrollo;Connection Timeout=120";
           string CONEXION_LOG_BITACORA = ConfigurationManager.AppSettings["CONEXION_LOG_BITACORA"].ToString(); //@"Data Source=10.20.128.149;Initial Catalog=OTMLog;Persist Security Info=True;User ID=usr_pbasbil;Password=desarrollo;Connection Timeout=120";
            string response = string.Empty;
            LOGI_EventosBitacora_PD oEventosBitacora = new LOGI_EventosBitacora_PD(CONEXION_LOG_BITACORA, CONEXION_LOGI);
            response = oEventosBitacora.CreaprocesoNotificacionERRORES("");

        }
    }
}
