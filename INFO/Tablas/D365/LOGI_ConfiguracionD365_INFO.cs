using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas.D365
{
    public class LOGI_ConfiguracionD365_INFO
    {
        public int cia { get; set; }
        public String cianombre { get; set; }
        public string ciad365 { get; set; }
        public string aprobador { get; set; }
        public string aprobadordisper { get; set; }
        public String URLApi { get; set; }
        public String URLApilogin { get; set; }
        public int intentos { get; set; }
        public int enviohorad365 { get; set; }
        public int enviomind365 { get; set; }
        public int enviosegd365 { get; set; }
        public String usuariod365 { get; set; }
        public String passusrd365 { get; set; }
        public String clientID { get; set; }
        public int validasaldo { get; set; }

        public string Conexion_eqv { get; set; }
        public string Servidor_eqv { get; set; }
        public string Usuario_eqv { get; set; }
        public string Password_eqv { get; set; }
        public string catalogo_eqv { get; set; }
        public string plantilla { get; set; }
        public string cuenta_uno { get; set; }
        public string cuenta_dos { get; set; }
        public string diariodisp { get; set; }
        public string diariofondo { get; set; }
        public int sincd365 { get; set; }
        public string cuentaviatico { get; set; }
        public string conexionzap { get; set; }
        public string rfc_tms { get; set; }
        public string api_tms { get; set; }
        public string url_tms { get; set; }
        public string host_tms { get; set; }
        public string puerto_mail { get; set; }
        public string user_mail { get; set; }
        public string password_mail { get; set; }
        public int ssl_mail { get; set; }
        public string cuentas_cxc_mail { get; set; }
        public string cuentas_cxp_mail { get; set; }
        public string cuentas_mtto_mail { get; set; }
        public string cuentas_gsto_mail { get; set; }
        public string cuentas_soport_mail { get; set; } 
        public int tiempo_mail { get; set; }
        public int intento_mail { get; set; }



        public string urlAPIGM { get; set; }
        public string EndPointConsulta { get; set; }
        public string EndPointRespuesta { get; set; }
        public string rfcAPI { get; set; }
        public string UsuarioAPIGM { get; set; }
        public string PasswordAPIGM { get; set; }
    }
}
