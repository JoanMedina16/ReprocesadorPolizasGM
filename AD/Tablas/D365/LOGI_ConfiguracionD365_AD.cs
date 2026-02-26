using INFO.Tablas.D365;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Tablas.D365
{
   public class LOGI_ConfiguracionD365_AD
    {
        internal Hashtable oHashParam = null;
        void GetObjeto(DataRow orow, ref LOGI_ConfiguracionD365_INFO otemp)
        {
            otemp.cia = orow["cia"] == DBNull.Value ? -1 : Convert.ToInt32(orow["cia"]);
            otemp.cianombre = orow["cianombre"] == DBNull.Value ? "" : Convert.ToString(orow["cianombre"]);
            otemp.ciad365 = orow["ciad365"] == DBNull.Value ? "" : Convert.ToString(orow["ciad365"]);
            otemp.URLApi = orow["URLApi"] == DBNull.Value ? "" : Convert.ToString(orow["URLApi"]);
            otemp.URLApilogin = orow["URLApilogin"] == DBNull.Value ? "" : Convert.ToString(orow["URLApilogin"]);
            otemp.intentos = orow["intentos"] == DBNull.Value ? -1 : Convert.ToInt32(orow["intentos"]);
            otemp.enviohorad365 = orow["enviohorad365"] == DBNull.Value ? -1 : Convert.ToInt32(orow["enviohorad365"]);
            otemp.enviomind365 = orow["enviomind365"] == DBNull.Value ? -1 : Convert.ToInt32(orow["enviomind365"]);
            otemp.enviosegd365 = orow["enviosegd365"] == DBNull.Value ? -1 : Convert.ToInt32(orow["enviosegd365"]);
            otemp.usuariod365 = orow["usuariod365"] == DBNull.Value ? "" : Convert.ToString(orow["usuariod365"]);
            otemp.passusrd365 = orow["passusrd365"] == DBNull.Value ? "" : Convert.ToString(orow["passusrd365"]);
            otemp.aprobador = orow["aprobador"] == DBNull.Value ? "" : Convert.ToString(orow["aprobador"]);
            otemp.clientID = orow["clientID"] == DBNull.Value ? "" : Convert.ToString(orow["clientID"]);
            otemp.validasaldo = orow["saldoclie"] == DBNull.Value ? 0 : Convert.ToInt32(orow["saldoclie"]);
            otemp.Conexion_eqv = orow["Conexion_eqv"] == DBNull.Value ? "" : Convert.ToString(orow["Conexion_eqv"]);
            otemp.Servidor_eqv = orow["Servidor_eqv"] == DBNull.Value ? "" : Convert.ToString(orow["Servidor_eqv"]);
            otemp.Usuario_eqv = orow["Usuario_eqv"] == DBNull.Value ? "" : Convert.ToString(orow["Usuario_eqv"]);
            otemp.Password_eqv = orow["Password_eqv"] == DBNull.Value ? "" : Convert.ToString(orow["Password_eqv"]);
            otemp.catalogo_eqv = orow["catalogo_eqv"] == DBNull.Value ? "" : Convert.ToString(orow["catalogo_eqv"]);
            otemp.plantilla = orow["plantilla"] == DBNull.Value ? "" : Convert.ToString(orow["plantilla"]);
            otemp.cuenta_uno = orow["cuenta_liq1"] == DBNull.Value ? "" : Convert.ToString(orow["cuenta_liq1"]);
            otemp.cuenta_dos = orow["cuenta_liq2"] == DBNull.Value ? "" : Convert.ToString(orow["cuenta_liq2"]);
            otemp.aprobadordisper = orow["aprobadordisp"] == DBNull.Value ? "" : Convert.ToString(orow["aprobadordisp"]);
            otemp.diariodisp = orow["diariodisp"] == DBNull.Value ? "" : Convert.ToString(orow["diariodisp"]);
            otemp.diariofondo = orow["diariofondo"] == DBNull.Value ? "" : Convert.ToString(orow["diariofondo"]);
            otemp.sincd365 = orow["sincd365"] == DBNull.Value ? 0 : Convert.ToInt32(orow["sincd365"]);
            otemp.cuentaviatico = orow["cuantaviatico"] == DBNull.Value ? "" : Convert.ToString(orow["cuantaviatico"]);
            otemp.conexionzap = orow["conexionzap"] == DBNull.Value ? "" : Convert.ToString(orow["conexionzap"]);

            otemp.rfc_tms = orow["rfc_tms"] == DBNull.Value ? "" : Convert.ToString(orow["rfc_tms"]);
            otemp.api_tms = orow["api_tms"] == DBNull.Value ? "" : Convert.ToString(orow["api_tms"]);
            otemp.url_tms = orow["url_tms"] == DBNull.Value ? "" : Convert.ToString(orow["url_tms"]);
            otemp.host_tms = orow["host_tms"] == DBNull.Value ? "" : Convert.ToString(orow["host_tms"]);
            otemp.puerto_mail = orow["puerto_mail"] == DBNull.Value ? "" : Convert.ToString(orow["puerto_mail"]);
            otemp.user_mail = orow["user_mail"] == DBNull.Value ? "" : Convert.ToString(orow["user_mail"]);
            otemp.password_mail = orow["password_mail"] == DBNull.Value ? "" : Convert.ToString(orow["password_mail"]);
            otemp.cuentas_cxc_mail = orow["cuentas_cxc_mail"] == DBNull.Value ? "" : Convert.ToString(orow["cuentas_cxc_mail"]);
            otemp.ssl_mail = orow["ssl_mail"] == DBNull.Value ? 0 : Convert.ToInt32(orow["ssl_mail"]);
            otemp.tiempo_mail = orow["tiempo_mail"] == DBNull.Value ? 0 : Convert.ToInt32(orow["tiempo_mail"]);
            otemp.intento_mail = orow["intento_mail"] == DBNull.Value ? 0 : Convert.ToInt32(orow["intento_mail"]);
            otemp.cuentas_cxp_mail = orow["cuentas_cxp_mail"] == DBNull.Value ? "" : Convert.ToString(orow["cuentas_cxp_mail"]);
            otemp.cuentas_mtto_mail = orow["cuentas_mtto_mail"] == DBNull.Value ? "" : Convert.ToString(orow["cuentas_mtto_mail"]);
            otemp.cuentas_gsto_mail = orow["cuentas_gsto_mail"] == DBNull.Value ? "" : Convert.ToString(orow["cuentas_gsto_mail"]);
            otemp.cuentas_soport_mail = orow["cuentas_soport_mail"] == DBNull.Value ? "" : Convert.ToString(orow["cuentas_soport_mail"]);

            otemp.urlAPIGM = orow["urlAPIGM"] == DBNull.Value ? "" : Convert.ToString(orow["urlAPIGM"]);
            otemp.EndPointConsulta = orow["EndPointConsulta"] == DBNull.Value ? "" : Convert.ToString(orow["EndPointConsulta"]);
            otemp.EndPointRespuesta = orow["EndPointRespuesta"] == DBNull.Value ? "" : Convert.ToString(orow["EndPointRespuesta"]);
            otemp.rfcAPI = orow["rfcAPI"] == DBNull.Value ? "" : Convert.ToString(orow["rfcAPI"]);
            otemp.UsuarioAPIGM = orow["UsuarioAPIGM"] == DBNull.Value ? "" : Convert.ToString(orow["UsuarioAPIGM"]);
            otemp.PasswordAPIGM = orow["PasswordAPIGM"] == DBNull.Value ? "" : Convert.ToString(orow["PasswordAPIGM"]);
        }

        /// <summary>
        /// Descripción: Query utilizado para recuperar los datos de la configuración de el ambiente D365
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="oConfiguracion">Lista de configuracion recuperada</param>
        /// <param name="oParam">Objeto de tipo poliaza para filtro de datos</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ListaConfiguracion(ref LOGI_ConexionSql_AD oConnection, ref LOGI_ConfiguracionD365_INFO oConfiguracion, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"SELECT * FROM lm_config_d365"); 
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.GetObjeto(odataset.Tables[0].Rows[0], ref oConfiguracion);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
        /// <summary>
        /// Descripción: Query utilizado para la actualización de los datos de la tabla lm_config_d365, se actualiza según los datos 
        /// enviados en objeto oParam
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string ActualizaConfiguracion(ref LOGI_ConexionSql_AD oConnection,  LOGI_ConfiguracionD365_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_config_d365");

            if (!string.IsNullOrEmpty(oParam.cianombre))
            {
                sConsultaSql += string.Format(" {0} cianombre = '{1}'", bSET ? "," : "SET", oParam.cianombre);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.ciad365))
            {
                sConsultaSql += string.Format(" {0} ciad365 = '{1}'", bSET ? "," : "SET", oParam.ciad365);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.aprobador))
            {
                sConsultaSql += string.Format(" {0} aprobador = '{1}'", bSET ? "," : "SET", oParam.aprobador);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.URLApi))
            {
                sConsultaSql += string.Format(" {0} URLApi = '{1}'", bSET ? "," : "SET", oParam.URLApi);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.URLApilogin))
            {
                sConsultaSql += string.Format(" {0} URLApilogin = '{1}'", bSET ? "," : "SET", oParam.URLApilogin);
                bSET = true;
            }

            if (oParam.intentos > 0)
            {
                sConsultaSql += string.Format(" {0} intentos = {1}", bSET ? "," : "SET", oParam.intentos);
                bSET = true;
            }

            if (oParam.enviohorad365 >= 0)
            {
                sConsultaSql += string.Format(" {0} enviohorad365 = {1}", bSET ? "," : "SET", oParam.enviohorad365);
                bSET = true;
            }

            if (oParam.enviomind365 > 0)
            {
                sConsultaSql += string.Format(" {0} enviomind365 = {1}", bSET ? "," : "SET", oParam.enviomind365);
                bSET = true;
            }

            if (oParam.enviosegd365 >= 0)
            {
                sConsultaSql += string.Format(" {0} enviosegd365 = {1}", bSET ? "," : "SET", oParam.enviosegd365);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.usuariod365))
            {
                sConsultaSql += string.Format(" {0} usuariod365 = '{1}'", bSET ? "," : "SET", oParam.usuariod365);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.passusrd365))
            {
                sConsultaSql += string.Format(" {0} passusrd365 = '{1}'", bSET ? "," : "SET", oParam.passusrd365);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.clientID))
            {
                sConsultaSql += string.Format(" {0} clientID = '{1}'", bSET ? "," : "SET", oParam.clientID);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.Conexion_eqv))
            {
                sConsultaSql += string.Format(" {0} Conexion_eqv = '{1}'", bSET ? "," : "SET", oParam.Conexion_eqv);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.Servidor_eqv))
            {
                sConsultaSql += string.Format(" {0} Servidor_eqv = '{1}'", bSET ? "," : "SET", oParam.Servidor_eqv);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.Usuario_eqv))
            {
                sConsultaSql += string.Format(" {0} Usuario_eqv = '{1}'", bSET ? "," : "SET", oParam.Usuario_eqv);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.Password_eqv))
            {
                sConsultaSql += string.Format(" {0} Password_eqv = '{1}'", bSET ? "," : "SET", oParam.Password_eqv);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.catalogo_eqv))
            {
                sConsultaSql += string.Format(" {0} catalogo_eqv = '{1}'", bSET ? "," : "SET", oParam.catalogo_eqv);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.plantilla))
            {
                sConsultaSql += string.Format(" {0} plantilla = '{1}'", bSET ? "," : "SET", oParam.plantilla);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.cuenta_uno))
            {
                sConsultaSql += string.Format(" {0} cuenta_liq1 = '{1}'", bSET ? "," : "SET", oParam.cuenta_uno);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.cuenta_dos))
            {
                sConsultaSql += string.Format(" {0} cuenta_liq2 = '{1}'", bSET ? "," : "SET", oParam.cuenta_dos);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.aprobadordisper))
            {
                sConsultaSql += string.Format(" {0} aprobadordisp = '{1}'", bSET ? "," : "SET", oParam.aprobadordisper);
                bSET = true;
            }


            if (!string.IsNullOrEmpty(oParam.diariodisp))
            {
                sConsultaSql += string.Format(" {0} diariodisp = '{1}'", bSET ? "," : "SET", oParam.diariodisp);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.diariofondo))
            {
                sConsultaSql += string.Format(" {0} diariofondo = '{1}'", bSET ? "," : "SET", oParam.diariofondo);
                bSET = true;
            }

            if (oParam.sincd365 >= 0)
            {
                sConsultaSql += string.Format(" {0} sincd365 = {1}", bSET ? "," : "SET", oParam.sincd365);
                bSET = true;                
            }
            if (!string.IsNullOrEmpty(oParam.cuentaviatico))
            {
                sConsultaSql += string.Format(" {0} cuantaviatico = '{1}'", bSET ? "," : "SET", oParam.cuentaviatico);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.conexionzap))
            {
                sConsultaSql += string.Format(" {0} conexionzap = '{1}'", bSET ? "," : "SET", oParam.conexionzap);
                bSET = true;
            }


            if (!string.IsNullOrEmpty(oParam.rfc_tms))
            {
                sConsultaSql += string.Format(" {0} rfc_tms = '{1}'", bSET ? "," : "SET", oParam.rfc_tms);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.api_tms))
            {
                sConsultaSql += string.Format(" {0} api_tms = '{1}'", bSET ? "," : "SET", oParam.api_tms);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.url_tms))
            {
                sConsultaSql += string.Format(" {0} url_tms = '{1}'", bSET ? "," : "SET", oParam.url_tms);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.host_tms))
            {
                sConsultaSql += string.Format(" {0} host_tms = '{1}'", bSET ? "," : "SET", oParam.host_tms);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.puerto_mail))
            {
                sConsultaSql += string.Format(" {0} puerto_mail = '{1}'", bSET ? "," : "SET", oParam.puerto_mail);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.user_mail))
            {
                sConsultaSql += string.Format(" {0} user_mail = '{1}'", bSET ? "," : "SET", oParam.user_mail);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.password_mail))
            {
                sConsultaSql += string.Format(" {0} password_mail = '{1}'", bSET ? "," : "SET", oParam.password_mail);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.cuentas_cxc_mail))
            {
                sConsultaSql += string.Format(" {0} cuentas_cxc_mail = '{1}'", bSET ? "," : "SET", oParam.cuentas_cxc_mail);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.cuentas_cxp_mail))
            {
                sConsultaSql += string.Format(" {0} cuentas_cxp_mail = '{1}'", bSET ? "," : "SET", oParam.cuentas_cxp_mail);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.cuentas_mtto_mail))
            {
                sConsultaSql += string.Format(" {0} cuentas_mtto_mail = '{1}'", bSET ? "," : "SET", oParam.cuentas_mtto_mail);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.cuentas_gsto_mail))
            {
                sConsultaSql += string.Format(" {0} cuentas_gsto_mail = '{1}'", bSET ? "," : "SET", oParam.cuentas_gsto_mail);
                bSET = true;
            }
            if (!string.IsNullOrEmpty(oParam.cuentas_soport_mail))
            {
                sConsultaSql += string.Format(" {0} cuentas_soport_mail = '{1}'", bSET ? "," : "SET", oParam.cuentas_soport_mail);
                bSET = true;
            }

            if (oParam.ssl_mail >= 0)
            {
                sConsultaSql += string.Format(" {0} ssl_mail = {1}", bSET ? "," : "SET", oParam.ssl_mail);
                bSET = true;
            }

            if (oParam.tiempo_mail >= 0)
            {
                sConsultaSql += string.Format(" {0} tiempo_mail = {1}", bSET ? "," : "SET", oParam.tiempo_mail);
                bSET = true;
            }

            if (oParam.intento_mail >= 0)
            {
                sConsultaSql += string.Format(" {0} intento_mail = {1}", bSET ? "," : "SET", oParam.intento_mail);
                bSET = true;
            }


            if (!string.IsNullOrEmpty(oParam.urlAPIGM))
            {
                sConsultaSql += string.Format(" {0} urlAPIGM = '{1}'", bSET ? "," : "SET", oParam.urlAPIGM);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.EndPointConsulta))
            {
                sConsultaSql += string.Format(" {0} EndPointConsulta = '{1}'", bSET ? "," : "SET", oParam.EndPointConsulta);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.EndPointRespuesta))
            {
                sConsultaSql += string.Format(" {0} EndPointRespuesta = '{1}'", bSET ? "," : "SET", oParam.EndPointRespuesta);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.rfcAPI))
            {
                sConsultaSql += string.Format(" {0} rfcAPI = '{1}'", bSET ? "," : "SET", oParam.rfcAPI);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.UsuarioAPIGM))
            {
                sConsultaSql += string.Format(" {0} UsuarioAPIGM = '{1}'", bSET ? "," : "SET", oParam.UsuarioAPIGM);
                bSET = true;
            }

            if (!string.IsNullOrEmpty(oParam.PasswordAPIGM))
            {
                sConsultaSql += string.Format(" {0} PasswordAPIGM = '{1}'", bSET ? "," : "SET", oParam.PasswordAPIGM);
                bSET = true;
            }


            sConsultaSql += string.Format(" WHERE cia = {0}", oParam.cia);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }

        /// <summary>
        /// Descripción: Query que activa o inactiva la comprobación de credito del cliente en D365
        /// Fecha: 23/03/2022
        /// </summary>
        /// <param name="oConnection">Referencia a conexión de base de datos</param>
        /// <param name="sUsuarioID">Usuario que ejectuta el proceso de actualizacion 920 = usuario de tipo sevicio</param>
        /// <param name="oParam">Obejeto que contiene las propiedades a actualizar</param>
        /// <param name="sConsultaSql">Salida de consulta ejectuada</param>
        /// <returns></returns>
        public string Actualizadisponibilidad(ref LOGI_ConexionSql_AD oConnection, LOGI_ConfiguracionD365_INFO oParam, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            oHashParam = new Hashtable();
            sConsultaSql = string.Empty;
            bool bSET = false;
            sConsultaSql = string.Format(@"UPDATE lm_config_d365"); 

            if (oParam.validasaldo >= 0)
            {
                sConsultaSql += string.Format(" {0} saldoclie = {1}", bSET ? "," : "SET", oParam.validasaldo);
                bSET = true;
            } 

            sConsultaSql += string.Format(" WHERE cia = {0}", oParam.cia);
            int icommand = oConnection.ExecuteCommand(sConsultaSql);
            return icommand > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}

