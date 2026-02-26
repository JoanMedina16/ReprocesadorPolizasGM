using INFO.Objetos.LMAE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Objetos.LMAE
{
    public class LOGI_ListaMaestra_AD
    {
        internal Hashtable oHashParam = null;

        void GetObjeto(DataRow oorow, ref LOGI_ListaMaestra_INFO oLista)
        {
            oLista.corporativo = oorow["vehic"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["vehic"]);
            oLista.economico = oorow["numeco"] == DBNull.Value ? "NA" : Convert.ToString(oorow["numeco"]);
            oLista.estatus = oorow["estatus"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["estatus"]);
            oLista.ccosto = oorow["ccosto"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["ccosto"]);
            oLista.depto = oorow["depto"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["depto"]);
            oLista.suc = oorow["suc"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["suc"]);
            oLista.marca = oorow["nommarca"] == DBNull.Value ? "NA" : Convert.ToString(oorow["nommarca"]);
            oLista.descripcion = oorow["descrip"] == DBNull.Value ? "NA" : Convert.ToString(oorow["descrip"]);
            oLista.anio = oorow["ano"] == DBNull.Value ? -1 : Convert.ToInt32(oorow["ano"]);
            oLista.serie = oorow["serie"] == DBNull.Value ? "NA" : Convert.ToString(oorow["serie"]);
            oLista.nmotor = oorow["nmotor"] == DBNull.Value ? "NA" : Convert.ToString(oorow["nmotor"]);
            oLista.tipovehiculo = oorow["nomtipo"] == DBNull.Value ? "NA" : Convert.ToString(oorow["nomtipo"]);
            oLista.placa = oorow["placa"] == DBNull.Value ? "NA" : Convert.ToString(oorow["placa"]);
            oLista.tipotracto = oorow["nomttracto"] == DBNull.Value ? "NA" : Convert.ToString(oorow["nomttracto"]);
            oLista.responsable = oorow["nomrespon"] == DBNull.Value ? "NA" : Convert.ToString(oorow["nomrespon"]);
            oLista.nombreestatus = oorow["nomestoper"] == DBNull.Value ? "NA" : Convert.ToString(oorow["nomestoper"]);
        }

        void LoopDataSet(DataSet objDataSet, ref List<LOGI_ListaMaestra_INFO> lstListamaestra)
        {
            LOGI_ListaMaestra_INFO objTemp = new LOGI_ListaMaestra_INFO();
            foreach (DataRow oorow in objDataSet.Tables[0].Rows)
            {
                objTemp = new LOGI_ListaMaestra_INFO();
                this.GetObjeto(oorow, ref objTemp);
                lstListamaestra.Add(objTemp);
            }
        }
        public string ListaUnidadesmaestra(ref LOGI_ConexionSql_AD oConnection, ref List<LOGI_ListaMaestra_INFO> listaMestra, LOGI_ListaMaestra_INFO oParams, out string sConsultaSql)
        {
            DataSet odataset = new DataSet();
            sConsultaSql = string.Empty;
            bool bAnd = true;

            //Destruye objetos temporales
            sConsultaSql = string.Format(@"if object_id('tempdb.dbo.#ttracto') is not null drop table dbo.#ttracto");

            //Crea tabla temporal desde Instancia OPE

            sConsultaSql += string.Format(@"
                          
                           select cuenta,scuenta,nombre,nomtiny 
                           into dbo.#ttracto 
                           from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.128.193\ADMBLOGISTIC;UID=admcorp;PWD=lv_ftww@xhwus',admblogistics.dbo.oa_scuentas) oa_scuentas 
                           where cuenta = (select top 1 ctatipotractor from config)");


            sConsultaSql += string.Format(@"
                        
                        select distinct vehic = cast(sav.clave as integer), numeco = sav.descrip, marca = sav.sctamarca, 
        nommarca = (select marcas.nombre 
                      from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.oa_scuentas) marcas 
                     where marcas.cuenta = 41 and marcas.scuenta = sav.sctamarca), 
        descrip = isnull(fic.descrip,''), sav.ano, serie = isnull(sav.serie,''), nmotor = isnull(sav.nmotor,''), 
        tipovehic = sav.sctatipo, nomtipo = (select tipos.nombre 
                                               from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.oa_scuentas) tipos 
                                              where tipos.cuenta = 29 and tipos.scuenta = sav.sctatipo), 
        placa = isnull(fic.placa, sav.placa), propiet = sav.ciaprop,
        nompropiet = (select propiets.nomciaab 
                        from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.cias) propiets 
                       where propiets.cia = sav.ciaprop and propiets.suc = 1), 
        cia = (select top 1 cia from oa_config), suc = isnull(fic.suc, sav.sucubica),
        nomsuc = isnull((select sucs.nomciaab from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.cias) sucs 
                  where sucs.cia = (select top 1 cia from oa_config) and sucs.suc = isnull(fic.suc,sav.sucubica)),''),
        ccosto = isnull(fic.ccosto,sav.sctacentro),
        depto = sav.sctadepto,
        nomccosto = (select ccostos.nombre from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.oa_scuentas) ccostos 
                     where ccostos.cuenta = 243 and ccostos.scuenta = isnull(fic.ccosto,sav.sctacentro)), 
        toneladas = isnull(fic.toneladas, sav.toneladas), ejes = isnull(fic.ejes, sav.ejes), 
        oper = isnull(fic.oper,''), nomoper   = isnull((select nomoper from operaciones where oper = fic.oper),''), 
        estatusoper= isnull(fic.estatusoper,''), nomestoper = isnull((select nomestatus from estatusopers where estatusoper = fic.estatusoper),''), 
        fecbaja = isnull(fic.fecbaja, ''), fechbaja = space(10), docbaja = isnull(fic.docbaja,''), motivo = isnull(fic.motivo,''), 
        nommotivo = isnull((select nommotivo from motivos where motivo = fic.motivo),''), 
     	  reemplazo = isnull(fic.reemvehic, ''), observaciones = isnull(fic.observaciones,''), 
      	          inciso    = isnull(fic.incisopol, ''), iave = isnull(fic.iave, ''), orden = isnull(fic.orden, ''),  
      	          adaptacion= isnull(fic.adaptacion,''), nomadaptacion = space(100), sav.estatus, 
        nomestatus = (select estatus.nomestatus 
                        from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.oa_estatus) estatus 
                       where estatus = sav.estatus), 
        responsable = isnull(fic.resguardo,''),nomrespon = isnull((select nomrespons from responsables where responsable = fic.resguardo),''), 
        ubicasav = isnull(fic.ubicasav,''), pos = 0, 
        isnull(tt.cuenta,(select top 1 ctatipotractor from config)) as cttracto, isnull(tt.scuenta,0) as ttracto, isnull(tt.nombre,'') as nomttracto 
from openrowset('SQLOLEDB','DRIVER={{SQL Server}};SERVER=10.20.129.30;UID=admcorp;PWD=lv_ftww@xhwus',admcorp04.dbo.oa_catalogo) sav 
                left join ficvehic fic    on sav.cuenta = 423 and sav.clave = fic.vehic 
                left join dbo.#ttracto tt on fic.ctatipotractor=tt.cuenta and fic.sctatipotractor=tt.scuenta 
where sav.cuenta     = 423 ", oParams.cia);


            if (!string.IsNullOrEmpty(oParams.filtro_corporativo))
            {
                sConsultaSql += string.Format(" {0} sav.clave LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParams.filtro_corporativo);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParams.filtro_economico))
            {
                sConsultaSql += string.Format(" {0} sav.descrip LIKE '%{1}%'", bAnd ? "AND" : "WHERE", oParams.filtro_economico);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParams.filtro_centro))
            {
                sConsultaSql += string.Format(" {0} sav.sctacentro = {1}", bAnd ? "AND" : "WHERE", oParams.filtro_centro);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParams.filtro_sucursal))
            {
                sConsultaSql += string.Format(" {0} sav.sucubica = {1}", bAnd ? "AND" : "WHERE", oParams.filtro_sucursal);
                bAnd = true;
            }
            else
            {
                sConsultaSql += string.Format(@"
            and((sav.ciaubica = {0}
            and sav.sucubica between 0 and 7) or(sav.ciaprop = {0}
            and sav.sucprop = 1)
            or(sav.ctaclien = 10 and sav.sctaclien in ({0},387)))", oParams.cia);
            }

            if (!string.IsNullOrEmpty(oParams.filtro_depto))
            {
                sConsultaSql += string.Format(" {0} sav.sctadepto = {1}", bAnd ? "AND" : "WHERE", oParams.filtro_depto);
                bAnd = true;
            }

            if (!string.IsNullOrEmpty(oParams.filtro_estatus))
            {
                sConsultaSql += string.Format(" {0} sav.estatus = {1}", bAnd ? "AND" : "WHERE", oParams.filtro_estatus);
                bAnd = true;
            }

            sConsultaSql += " order by vehic, sav.descrip";
            odataset = oConnection.FillDataSet(sConsultaSql);
            this.LoopDataSet(odataset, ref listaMestra);
            return odataset.Tables[0].Rows.Count > 0 ? "OK" : "SIN RESULTADOS";
        }
    }
}
