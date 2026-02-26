using D365API.INFOD;
using INFO.Tablas.D365;
using Newtonsoft.Json;
using PD.Herramientas;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


namespace D365API
{
    public class GMTApi
    {
        internal RestClient oCliente = null;
        internal RestRequest oRequest = null;
        internal IRestResponse oResponse = null;
        internal string Conexion = null;
        internal LOGI_ConfiguracionD365_INFO oConfig = null;
        public GMTApi(LOGI_ConfiguracionD365_INFO oconf, string cnx)
        {
            Conexion = cnx;
            oConfig = oconf;
        }
        public class PrepolizaRequest
        {
            public string FechaInicial { get; set; }
            public string FechaFinal { get; set; }
            public string Pagina { get; set; }
            public string Proceso { get; set; }
            public string Estatus { get; set; }
            public int NumeroPolizaDesde { get; set; }
            public int NumeroPolizaHasta { get; set; }
        }
        //public class Parametros
        //{
        //    public string URLAPI { get; set; }
        //    public string EndPointConsulta { get; set; }
        //    public string EndPointRespuesta { get; set; }
        //    public string Rfc { get; set; }
        //    public string Usuario { get; set; }
        //    public string Password { get; set; }
        //}

        public class RespuestaPoliza
        {
            public string IdPoliza { get; set; }
            public string Estatus { get; set; }
        }
        public class ListaPolizas
        {
            public List<RespuestaPoliza> Polizas { get; set; }
        }
        public void BitacoraAPI(string Evento, string Descripcion)
        {

            string sPathBase = string.Format(@"{0}\Polizas\", AppDomain.CurrentDomain.BaseDirectory);
            string Archivo= string.Format(@"{0}{1}.txt", sPathBase, "Registros");
            try
            {
                if (!File.Exists(Archivo))
                {
                    File.WriteAllText(Archivo, Evento);
                }
                else
                {
                    File.AppendAllText(Archivo, Evento);
                }
                Console.WriteLine("Archivo creado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public IRestResponse getPolizas(DateTime date, string Proceso, string Pagina)
        {
            Conexion oCnx = new Conexion(Conexion);
            var usuario = oConfig.UsuarioAPIGM;
            var pass = oConfig.PasswordAPIGM;
            var credenciales = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{usuario}:{pass}")
            );

            oCliente = new RestClient(oConfig.urlAPIGM + oConfig.EndPointConsulta); 
            oCliente.Timeout = 900000000;
            oRequest = new RestRequest(Method.POST);
            oRequest.AddHeader("Content-Type", "application/json");
            oRequest.AddHeader("Aplicacion", "1");
            oRequest.AddHeader("rfc", oConfig.rfcAPI);
            oRequest.AddHeader("Authorization", $"Basic {credenciales}");

            var content = new PrepolizaRequest
            {
                FechaInicial = date.ToString("yyyyMMdd"),
                FechaFinal = date.ToString("yyyyMMdd"),//"20250902"
                Pagina = Pagina,
                Proceso = Proceso, //"Pasivos",
                Estatus = "0"
            };

            var body = JsonConvert.SerializeObject(content);
            oRequest.AddParameter("application/json", body, ParameterType.RequestBody);
            oRequest.AddBody(body);
            ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
            oResponse = oCliente.Execute(oRequest);
            return oResponse;
                        
        }
        public IRestResponse getPolizasPasivos(DateTime date, string Proceso, string Pagina)
        {
            Conexion oCnx = new Conexion(Conexion);
            var usuario = oConfig.UsuarioAPIGM;
            var pass = oConfig.PasswordAPIGM;
            var credenciales = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{usuario}:{pass}")
            );

            oCliente = new RestClient(oConfig.urlAPIGM + oConfig.EndPointConsulta);
            oCliente.Timeout = 900000000;
            oRequest = new RestRequest(Method.POST);
            oRequest.AddHeader("Content-Type", "application/json");
            oRequest.AddHeader("Aplicacion", "1");
            oRequest.AddHeader("rfc", oConfig.rfcAPI);
            oRequest.AddHeader("Authorization", $"Basic {credenciales}");

            var content = new PrepolizaRequest
            {
                FechaInicial = date.ToString("yyyyMMdd"),
                FechaFinal = date.ToString("yyyyMMdd"),//"20250902"
                Pagina = Pagina,
                Proceso = Proceso, //"Pasivos",
                Estatus = "0"      };

            var body = JsonConvert.SerializeObject(content);
            oRequest.AddParameter("application/json", body, ParameterType.RequestBody);
            oRequest.AddBody(body);
            ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
            oResponse = oCliente.Execute(oRequest);
            return oResponse;

        }
        public bool responsePolizas(DateTime date)
        {

            var usuario = oConfig.UsuarioAPIGM;
            var pass = oConfig.PasswordAPIGM;

            var credenciales = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{usuario}:{pass}")
            );
            oCliente = new RestClient(oConfig.urlAPIGM + oConfig.EndPointRespuesta);
            oCliente.Timeout = 900000000;
            oRequest = new RestRequest(Method.POST);
            oRequest.AddHeader("Content-Type", "application/json");
            oRequest.AddHeader("Aplicacion", "1");
            oRequest.AddHeader("rfc", oConfig.rfcAPI);
            oRequest.AddHeader("Authorization", $"Basic {credenciales}");

            GestorPolizas oobj = new GestorPolizas();
            List<GMTApi.RespuestaPoliza> Polizas = oobj.MostrarRegistrosCorrectos(date);
            if (Polizas.Count > 0)
            {
                var content = JsonConvert.SerializeObject(
                new { Polizas = Polizas }, Formatting.None);
                string limpio = content.ToString();

                limpio = limpio.Replace("\\\"", "\"");
                limpio = limpio.Trim('\\');
                limpio = limpio.Trim();

                var body = limpio; 
                oRequest.AddParameter("application/json", body, ParameterType.RequestBody);
                oRequest.AddBody(body);
                ServicePointManager.ServerCertificateValidationCallback = LOGI_Tools_PD.CertificateValidationCallBack;
                oResponse = oCliente.Execute(oRequest);
                if (oResponse.Content.Contains("success"))
                {
                    //oobj.EliminarRegistros(Polizas);
                    oobj.MoverAPprocesados(Polizas, date);
                }

                return true;
            }
            else
                return true;
        }
    }
}