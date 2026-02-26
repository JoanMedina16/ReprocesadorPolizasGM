using INFO.Objetos.MOTF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.MOTF
{
   public class LOGI_PortesViaje_AD
    {
        internal Hashtable oHashParam = null;
        internal string nom_cliente = string.Empty, nom_origen = string.Empty, nom_destino = string.Empty, nom_operador = string.Empty;
        void GetObjeto(DataRow oorow, ref LOGI_PorteViaje_INFO oViaje)
        {
            oViaje.PorteId = oorow["porte_id"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["porte_id"]);
            oViaje.Fechacreacion = oorow["fecha_creacion"] == DBNull.Value ? "NA" : Convert.ToDateTime(oorow["fecha_creacion"]).ToString("dd/MM/yyyy hh:mm:ss tt");
            oViaje.Subtotal = oorow["subtotal"] == DBNull.Value ? 0 : Convert.ToDecimal(oorow["subtotal"]);
            oViaje.Total = oorow["total"] == DBNull.Value ? 0 : Convert.ToDecimal(oorow["total"]);
            oViaje.Folioviaje = oorow["folio_viaje_otm"] == DBNull.Value ? "" : Convert.ToString(oorow["folio_viaje_otm"]);
            oViaje.cuentaclie = oorow["cliente_cuenta"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["cliente_cuenta"]);
            oViaje.subclie = oorow["cliente_scuenta"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["cliente_scuenta"]);
            nom_cliente = oorow["nom_cliente"] == DBNull.Value ? "NO EXISTE" : Convert.ToString(oorow["nom_cliente"]);
            oViaje.Cliente = string.Format("{0}-{1}", oViaje.cuentaclie, nom_cliente);
            oViaje.cuentaori = oorow["origen_otm"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["origen_otm"]);
            nom_origen = oorow["nom_origen"] == DBNull.Value ? "NO EXISTE" : Convert.ToString(oorow["nom_origen"]);
            oViaje.Origen = string.Format("{0}-{1}", oViaje.cuentaori, nom_origen);
            oViaje.cuentadest = oorow["destino_otm"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["destino_otm"]);
            nom_destino = oorow["nom_destino"] == DBNull.Value ? "NO EXISTE" : Convert.ToString(oorow["nom_destino"]);
            oViaje.Destino = string.Format("{0}-{1}", oViaje.cuentadest, nom_destino);
            oViaje.cuentaoper = oorow["operador_otm"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["operador_otm"]);
            nom_operador = oorow["nom_operador"] == DBNull.Value ? "" : Convert.ToString(oorow["nom_operador"]);
            oViaje.Operador = oViaje.Destino = string.Format("{0}-{1}", oViaje.cuentaoper, nom_operador);
            oViaje.facturable = 1;
            if (nom_cliente == "NO EXISTE" || nom_origen == "NO EXISTE" || nom_destino == "NO EXISTE" || nom_operador == "NO EXISTE")
                oViaje.facturable = 0;
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_PorteViaje_INFO> lstViajes)
        {
            LOGI_PorteViaje_INFO objTemp = new LOGI_PorteViaje_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_PorteViaje_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstViajes.Add(objTemp);
            }
        }
        /// <summary>
        /// Descripción: Query utulizado para listar los registros de bandeja de facturación,
        /// se listan todos los viajes pendientes de facturar = 0, Facturados = 3
        /// Autor: Ing. Grupo consultor GB
        /// Fecha: 08/03/2022
        /// </summary>
        /// <param name="oConnection">Objeto de tipo conexión de base de datos</param>
        /// <param name="lstViajes">Referencia de colleción donde se retornan los viajes</param>
        /// <param name="oViaje">Objeto de tipo viaje utilizado para filtros de búsqueda</param>
        /// <param name="sConsultaSql">Parametro de salida con el query ejecutado</param>
        /// <returns></returns>
        public string ListaPortesViajes(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_PorteViaje_INFO> lstViajes, LOGI_PorteViaje_INFO oViaje, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            sConsultaSql = string.Format(@"select T1.porte_id, T1.fecha_creacion, T1.subtotal, T1.total, T1.folio_viaje_otm, T1.cliente_cuenta, T1.cliente_scuenta, T5.nomclie as nom_cliente,
                                           T1.origen_otm, T3.nomlugar as nom_origen, T1.destino_otm ,T4.nomlugar as nom_destino, T1.operador_otm,
                                           T2.nombre as nom_operador from lm_otm_portes T1 LEFT JOIN oa_scuentas T2 ON (T2.cuenta = 25 and T2.scuenta = T1.operador_otm)
                                           LEFT JOIN lm_lugares  T3 ON (T3.cuenta = 802 and T3.scuenta = T1.origen_otm)
                                           LEFT JOIN lm_lugares  T4 ON (T4.cuenta = 802 and T4.scuenta = T1.destino_otm)
                                           LEFT JOIN lm_clientes T5 ON (T5.cuenta = T1.cliente_cuenta and T5.scuenta = T1.cliente_scuenta) where T1.estatus = 0");

            sConsultaSql += " order by fecha_creacion desc";            
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref lstViajes);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }

    }
}
