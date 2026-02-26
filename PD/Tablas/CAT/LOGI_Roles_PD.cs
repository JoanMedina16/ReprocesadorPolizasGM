using AD;
using AD.Tablas.CAT;
using INFO.Tablas.CAT;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.CAT
{
    public class LOGI_Roles_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Roles_AD oRolAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Roles_PD.cs";
        const string CONST_MODULO = "Roles";

        public LOGI_Roles_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oRolAD = new LOGI_Roles_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaRoles(ref List<LOGI_Roles_INFO> lstRoles, LOGI_Roles_INFO perfil)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oRolAD.ListaRoles(ref oConnection, ref lstRoles, perfil, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaRoles", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }


        public string NuevoRol(LOGI_Roles_INFO Perfil)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                int iNuevoPerfil = -1;
                sReponse = oRolAD.NuevoRol(ref oConnection, Perfil, ref iNuevoPerfil, out sConsultaSql);
                var lstAccesos = Perfil.permisos.Split(',');
                foreach (var oacceso in lstAccesos)
                    sReponse = oRolAD.NuevopermisoRol(ref oConnection, iNuevoPerfil, Convert.ToInt32(oacceso), out sConsultaSql);
                if (Perfil.lstMatriz != null)
                {
                    int linea = 0;
                    foreach (LOGI_Matrizrol_INFO matriz in Perfil.lstMatriz)
                    {
                        sReponse = oRolAD.Nuevamatrizrol(ref oConnection, Perfil.rol, linea, matriz, out sConsultaSql);
                        if (sReponse != "OK")
                            throw new Exception("No se ha podido registrar la matriz para el perfil");
                        linea++;
                    }

                }
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "NuevoRol", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string EditaRol(LOGI_Roles_INFO Perfil)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oRolAD.EditaRol(ref oConnection, Perfil, out sConsultaSql);
                oRolAD.Eliminapermisosrol(ref oConnection, Perfil.rol, out sConsultaSql);
                oRolAD.Eliminamatrizporol(ref oConnection, Perfil.rol, out sConsultaSql);
                var lstAccesos = Perfil.permisos.Split(',');
                foreach (var oacceso in lstAccesos)
                    sReponse = oRolAD.NuevopermisoRol(ref oConnection, Perfil.rol, Convert.ToInt32(oacceso), out sConsultaSql);

                if (Perfil.lstMatriz != null)
                {
                    int linea = 0;
                    foreach (LOGI_Matrizrol_INFO matriz in Perfil.lstMatriz)
                    {
                        sReponse = oRolAD.Nuevamatrizrol(ref oConnection, Perfil.rol, linea, matriz, out sConsultaSql);
                        if (sReponse != "OK")
                            throw new Exception("No se ha podido registrar la matriz para el perfil");
                        linea++;
                    }
                }
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "EditaRol", CONST_CLASE, CONST_MODULO, sConsultaSql);
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

    public class LOGI_Permisos_PD
    {

        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Permisos_AD oPermisoAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Permisos_PD.cs";
        const string CONST_MODULO = "Permisos";

        public LOGI_Permisos_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oPermisoAD = new LOGI_Permisos_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaPermisosByPerfil(ref List<LOGI_Permisos_INFO> lstPermisos, int iPerfilID)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = oPermisoAD.RecuperaPermisosByRol(ref oConnection, ref lstPermisos, iPerfilID, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaPermisosByPerfil", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaMatrizByperfil(ref List<LOGI_Matrizrol_INFO> lstMatriz, int iPerfilID)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = oPermisoAD.RecuperaMatrizByPerfil(ref oConnection, ref lstMatriz, iPerfilID, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaMatrizByperfil", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {

                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }

        public string ListaPermisos(ref List<LOGI_Permisos_INFO> lstPermisos)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                sReponse = oPermisoAD.RecuperaPermisos(ref oConnection, ref lstPermisos, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaPermisos", CONST_CLASE, CONST_MODULO, sConsultaSql);
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


