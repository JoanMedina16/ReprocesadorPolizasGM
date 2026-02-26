using AD;
using AD.Tablas.CAT;
using INFO.Tablas;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.CAT
{
   public class LOGI_Usuarios_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Usuarios_AD oUsuarioAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Usuarios_PD.cs";
        const string CONST_MODULO = "Usuarios";

        public LOGI_Usuarios_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oUsuarioAD = new LOGI_Usuarios_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string GuardaUsuario(LOGI_Usuarios_INFO oUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                oConnection.StarTransacction();
                sReponse = oUsuarioAD.NuevoUsuario(ref oConnection, oUsuario, out sConsultaSql);
                if (sReponse != "OK")
                    throw new Exception("Información no se ha podido crear");
                if (oUsuario.iPerfil != 1)
                {
                    //Extraemos los accesos a modulos que se le han asignado al usuario
                    var lstPermisos = oUsuario.sPermisosmod.TrimEnd(',').Split(',');
                    foreach (string key in lstPermisos)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            sReponse = oUsuarioAD.Creapermiso(ref oConnection, key, oUsuario.iUsuario, out sConsultaSql);
                            if (sReponse != "OK")
                                throw new Exception("Los permisos no se han podido asignar");
                        }
                    }

                    ///Extraemos los diarios que se les ha habilitado
                    var lstDiarios = oUsuario.sPermisodiario.TrimEnd(',').Split(',');
                    foreach (string key in lstDiarios)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            sReponse = oUsuarioAD.CreapermisoDiario(ref oConnection, key, oUsuario.iUsuario, out sConsultaSql);
                            if (sReponse != "OK")
                                throw new Exception("Los permisos a diarios no se han podido asignar");
                        }
                    }

                }
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                if (oConnection != null)
                    oConnection.RollBackTransacction();
                oTool.LogError(ex, "GuardaUsuario", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido crear el registro de usuario {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string EditaUsuario(LOGI_Usuarios_INFO oUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                oConnection.StarTransacction();
                oUsuario.iActivo = 1;
                sReponse = oUsuarioAD.EditaUsuario(ref oConnection, oUsuario, out sConsultaSql);
                if (sReponse != "OK")
                    throw new Exception("Datos sin poder editar");
                //Eliminamos todos los permisos que tenga el usuario 
                oUsuarioAD.EliminaPermisos(ref oConnection, oUsuario.iUsuario, out sConsultaSql);
                oUsuarioAD.EliminaPermisosDiario(ref oConnection, oUsuario.iUsuario, out sConsultaSql);
                if (oUsuario.iPerfil != 1)
                {
                    //Extraemos los accesos a modulos que se le han asignado al usuario
                    var lstPermisos = oUsuario.sPermisosmod.TrimEnd(',').Split(',');
                    foreach (string key in lstPermisos)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {                            
                            sReponse = oUsuarioAD.Creapermiso(ref oConnection, key, oUsuario.iUsuario, out sConsultaSql);
                            if (sReponse != "OK")
                                throw new Exception("Los permisos no se han podido asignar");
                        }
                    }

                    ///Extraemos los diarios que se les ha habilitado
                    var lstDiarios = oUsuario.sPermisodiario.TrimEnd(',').Split(',');
                    foreach (string key in lstDiarios)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            sReponse = oUsuarioAD.CreapermisoDiario(ref oConnection, key, oUsuario.iUsuario, out sConsultaSql);
                            if (sReponse != "OK")
                                throw new Exception("Los permisos a diarios no se han podido asignar");
                        }
                    }
                }
                oConnection.CommitTransacction();
            }
            catch (Exception ex)
            {
                if (oConnection != null)
                    oConnection.RollBackTransacction();
                oTool.LogError(ex, "EditaUsuario", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido editar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string Eliminausuario(LOGI_Usuarios_INFO oUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = oUsuarioAD.EditaUsuario(ref oConnection, oUsuario, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "Eliminausuario", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido eliminar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaUsuarios(ref List<LOGI_Usuarios_INFO> lstUsuarios, LOGI_Usuarios_INFO oUsuario)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = oUsuarioAD.RecuperaUsuario(ref oConnection, ref lstUsuarios, oUsuario, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaUsuarios", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }


    }
}

