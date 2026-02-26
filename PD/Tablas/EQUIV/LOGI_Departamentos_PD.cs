using AD;
using AD.Tablas.EQUIV;
using INFO.Tablas.EQUIV;
using PD.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Tablas.EQUIV
{
   public class LOGI_Departamentos_PD
    {
        internal LOGI_Tools_PD oTool = null;
        internal LOGI_Departamentos_AD oDeptoAD = null;
        internal LOGI_ConexionSql_AD oConnection = null;
        const string CONST_CLASE = "LOGI_Departamentos_PD.cs";
        const string CONST_MODULO = "Departamentos";

        public LOGI_Departamentos_PD(string sConnection)
        {
            oConnection = new LOGI_ConexionSql_AD(sConnection);
            oDeptoAD = new LOGI_Departamentos_AD();
            oTool = new LOGI_Tools_PD();
        }

        public string ListaDepartamentos(ref List<LOGI_Catalogos_INFO> lstDepartamento, LOGI_Catalogos_INFO oDepto)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oDeptoAD.RecuperaDepartamentos(ref oConnection, ref lstDepartamento, oDepto, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "ListaDepartamentos", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido recuperar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        }
        public string AgregaDepartamento(LOGI_Catalogos_INFO oDepto)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oDeptoAD.NuevaDepartamento(ref oConnection, oDepto, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "AgregaDepartamento", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
            }
            finally
            {
                if (oConnection != null)
                    oConnection.CloseConnection();
            }
            return sReponse;
        } 
        public string EditaDepartamento(LOGI_Catalogos_INFO oDepto)
        {
            string sReponse = string.Empty, sConsultaSql = string.Empty;
            sReponse = "ERROR";
            try
            {
                oConnection.OpenConnection();
                sReponse = oDeptoAD.EditaDepartamento(ref oConnection, oDepto, out sConsultaSql);
            }
            catch (Exception ex)
            {
                oTool.LogError(ex, "EditaDepartamento", CONST_CLASE, CONST_MODULO, sConsultaSql);
                sReponse = string.Format("No se ha podido actualizar la información {0}", ex.Message);
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
