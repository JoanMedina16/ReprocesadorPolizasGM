function HistorialPlantilla() {
    var oThis = this, oParams = "",
        lstFiltros = { txtFolio: "#txtFolio", cmbtipo: "#cmbtipo", txtInicio: "#txtFechacrinicio", txtFin:"#txtFechacrfin" },
        lstCaja = {  },
        lstElemnts = {  },
        lstmodales = {  }
        lstTablas = { tblPlantilla:"#tbl-historiales" },
            lstBotones = { btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnEliminado: "#chkEliminado" },
            Controller = "../Metodos/D365/Historial-Dispersiones-Datos.aspx/";

    this.m_InicializaEvents = function () {

        oGlob.m_CargaDetePicker(lstFiltros.txtInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFin);
        oThis.m_EventoBusqueda();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {
            oGlob.m_AbremodalFiltro();

        });

     
    }
    this.m_EventoBusqueda = function () {
        var oPlantilla = {
            activo: $(lstBotones.btnEliminado).is(':checked') ? 0 : 1,
            FolioAsistente: $(lstFiltros.txtFolio).val(),
            fechainicio: $(lstFiltros.txtInicio).val(),
            fechafin: $(lstFiltros.txtFin).val(),
            tipo: $(lstFiltros.cmbtipo).val() == "" ? -1 : $(lstFiltros.cmbtipo).val()
        };
        pm = "oParam: " + JSON.stringify(oPlantilla);
        oThis.m_ListaPlantillas(pm);
        oGlob.m_CloseFilters();
    }
    this.m_LimpiaFiltros = function () {
        $.each(lstFiltros, function (key, value) {
            $(value).val("");
        });
    }
    this.m_LimpiaCajasForm = function () {
        $.each(lstCaja, function (key, value) {
            $(value).val("");
        });
    }

    this.EliminaAsistente = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "Eliminaplantilla",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                oGlob.m_showLoader();
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_EventoBusqueda();
                        break;
                    case "-1":
                        oGlob.m_showModalSession();
                        break;
                    case "-2":
                        oGlob.m_showModalPermiso();
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(response.mensaje);
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    }


    this.m_ListaPlantillas = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListaHistorial",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_ViewloaderGrid(lstTablas.tblPlantilla);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        break;
                    case "-1":
                        oGlob.m_showModalSession();
                        break;
                    case "-2":
                        oGlob.m_showModalPermiso();
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(response.mensaje);
                        break;
                    case "SIN RESULTADOS":
                        oGlob.m_AlertWarning('No se han encontrado documentos con los filtros proporcionados');
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
                oGlob.m_HideloaderGrid(lstTablas.tblPlantilla);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblPlantilla);
            }
        });
    }  

    this.DescargaAdjunto = function (evt, jID) {
        var row = $(lstTablas.tblPlantilla).jqGrid('getRowData', jID);
        window.open('Paginas/Generic-Table-Export.aspx?modo=plantilla&identificador=' + row.FolioAsistente, '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
    }

    this.Eliminaplant = function (evt, jID) {
        var row = $(lstTablas.tblPlantilla).jqGrid('getRowData', jID);
        if (row.activo == 1) {
            oGlob.m_Confirma_Accion('Una vez elimina la plantilla, los movimientos relacionados quedarán habilitados para ser usuados nuevamente.¿Estás seguro que deseas continuar?', function (response) {
                if (response) {

                    var oPlantilla = {
                        FolioAsistente: row.FolioAsistente
                    };
                    pm = "oParam: " + JSON.stringify(oPlantilla);
                    oThis.EliminaAsistente(pm);

                }
            });
        } else oGlob.m_AlertWarning("La acción no se puede llevar a cabo. La plantilla ya ha sido eliminada con anteriorirdad.");
    }

    
     
    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblPlantilla;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center" },
                { label: ".", name: "rDel", type: "text", width: 50, align: "center" },
                { label: "Folio asistente", name: "FolioAsistente", type: "text", width: 140, align: "center" },
                { label: "Tipo", name: "rTipo", type: "text", width: 150, align: "center" },
                { label: "Nombre asistente", name: "plantillanom", type: "text", width: 320, align: "left" },
                { label: "Encabezado xls", name: "rHeader", type: "text", width: 150, align: "left" },
                { label: "Detalle xls", name: "rLine", type: "text", width: 150, align: "left" },
                { label: "Fecha creado", name: "fechacreado", type: "text", width: 140, align: "center" },
                {
                    label: "Activo", name: "activo", width: 90, editable: true, edittype: 'checkbox', editoptions: { value: "1:0" }, align: "center",
                    formatter: "checkbox", formatoptions: { disabled: true }
                },
                { label: "Fecha eliminado", name: "fechaeliminado", type: "text", width: 140, align: "center" },
                { label: "tipo", name: "tipo", type: "text", width: 140, align: "center", hidden: true },
                { label: "pathcabecera", name: "pathcabecera", type: "text", width: 140, align: "center", hidden: true },
                { label: "pathdetalle", name: "pathdetalle", type: "text", width: 140, align: "center", hidden: true },

            ],
            caption: 'Listado de plantillas creadas',
            multiselect: false,
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                var ActiveComment = '', CssButton = 'secondary';

                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var Tipo = 'Dispersión electrónica';
                    if (row.tipo == "2")
                        Tipo = 'Dispersión fondo fijo';
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" title=\"Descarga plantilla\" onClick=\"oHisplantilla.DescargaAdjunto(this, " + key + ")\"><i class=\"fas fa-download \" ></i></button>";                    
                    var rowDel = "<button  type=\"button\" class=\"btn btn-sm alert-danger\"  title=\"Elimina plantilla\" onClick=\"oHisplantilla.Eliminaplant(this, " + key + ")\"> <i class=\"fas fa-trash-alt\" data-label=\"Elimina plantilla\" ></i></button>";
                    var rowHeader = "<a target=\"_blank\" rel=\"noopener noreferrer\" href=\"Paginas/Generic-Table-Export.aspx?modo=archivo&identificador="+row.pathcabecera+"\">Descargar cabecera excel</a>";
                    var rowLine = "<a target=\"_blank\" rel=\"noopener noreferrer\" href=\"Paginas/Generic-Table-Export.aspx?modo=archivo&identificador="+row.pathdetalle+"\">Descargar detalle excel</a>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit, rDel: rowDel, rTipo: Tipo, rHeader: rowHeader, rLine: rowLine});
                    
                }
            },
            pager: "#jqGridPager"
        });
    }
}