function Dispersiones() {
    var oThis = this, oParams = "",
        lstFiltros = { txtFoliovij: "#txtFoliovij", txtOperador: "#txtOperador", txtrfc: "#txtrfc", txtFechacinicio: "#txtFechacinicio", txtFechacfin: "#txtFechacfin", cmbsuc: "#cmbsuc" },
        lstCaja = { txtAsistente: "#txtAsistente", cmbanco: "#cmbbancos", txtRef: "#txtRef", cmbtipo: "#cmbtipo", cmbFondofijo:"#cmbbfondofi" },
        lstElemnts = { div_banco: "#div_banco", div_fondofij:"#div_fondofij"},
        lstmodales = {}
    lstTablas = { tbldispersiones: "#tbl-dispersiones" },
        lstBotones = { btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnref: "#btnref", btnNuevo: "#btnNuevo", btnChange:"#btncambia" },
        Controller = "../Metodos/D365/Dispersiones-Datos.aspx/";

    this.m_InicializaEvents = function () {        
        
        oGlob.m_CargaDetePicker(lstFiltros.txtFechacinicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFechacfin);
        oThis.m_Cargabancos();
        oThis.m_CargaFondofijo();
        oThis.m_CargaSucursales();
        oThis.m_DeshabilitaElementos(true);

        $(lstCaja.cmbtipo).unbind();
        $(lstCaja.cmbtipo).change(function () {
            if ($(this).val() != "") {

                oThis.m_DeshabilitaElementos(false);
                oThis.m_EventoBusqueda();
                oThis.m_AsignaAsistente();
                oGlob.m_DeshabilitaElemento(lstBotones.btnChange, false); 
                if ($(this).val() == "1") {
                    oGlob.m_OcultaElemnto(lstElemnts.div_banco, false);
                    oGlob.m_OcultaElemnto(lstElemnts.div_fondofij, true);
                    $(lstCaja.cmbanco).chosen();                    
                } else {
                    oGlob.m_OcultaElemnto(lstElemnts.div_banco, true);
                    oGlob.m_OcultaElemnto(lstElemnts.div_fondofij, false);
                    $(lstCaja.cmbFondofijo).chosen();
                }

            } else oGlob.m_AlertWarning('Selecciona un formato de plantilla valido');
        });

        $(lstBotones.btnChange).unbind();
        $(lstBotones.btnChange).click(function () {
            oGlob.m_Confirma_Accion('Una vez cambiado la opción de la plantilla los cambios realizados se perderán.¿Deseas continuar?', function (response) {
                if (response) { 
                    oThis.m_DeshabilitaElementos(true);
                    $(lstTablas.tbldispersiones).jqGrid('clearGridData', true).trigger('reloadGrid');
                    $.jgrid.gridUnload(lstTablas.tbldispersiones);
                    oGlob.m_DeshabilitaElemento(lstBotones.btnChange, true); 
                    oGlob.m_OcultaElemnto(lstElemnts.div_banco, true);
                    oGlob.m_OcultaElemnto(lstElemnts.div_fondofij, true);
                    $(lstCaja.cmbtipo).val("");
                }
            });
        });

        $(lstCaja.cmbanco).unbind();
        $(lstCaja.cmbanco).change(function () {
            if ($(this).val() != "") {
                var data = $(lstTablas.tbldispersiones).jqGrid("getGridParam", "data");
                for (var i = 0; i < data.length; i++) {
                    $(lstTablas.tbldispersiones).jqGrid("setCell", (i + 1), "nambanco", $(this).val());
                }
            } else oGlob.m_AlertWarning('Selecciona un banco válido');
        });

        $(lstCaja.cmbFondofijo).unbind();
        $(lstCaja.cmbFondofijo).change(function () {
            if ($(this).val() != "") {
                var data = $(lstTablas.tbldispersiones).jqGrid("getGridParam", "data");
                for (var i = 0; i < data.length; i++) {
                    $(lstTablas.tbldispersiones).jqGrid("setCell", (i + 1), "FondoFijo", $(this).val());
                }
            } else oGlob.m_AlertWarning('Selecciona un fondo fijo válido');
        });

        $(lstBotones.btnref).unbind();
        $(lstBotones.btnref).click(function () {
            if ($(txtRef).val() != "") {
                var lstChecks = $(lstTablas.tbldispersiones).jqGrid('getGridParam', 'selarrrow');
                if (lstChecks != null && lstChecks.length > 0) {
                    for (var i = 0; i < lstChecks.length; i++) {
                        $(lstTablas.tbldispersiones).jqGrid("setCell", lstChecks[i], "refbanco", $(txtRef).val());
                    }
                    $(txtRef).val("");
                    $(lstTablas.tbldispersiones).jqGrid('resetSelection');
                } else oGlob.m_AlertWarning('No se ha detectado la selección de póliza(s) para asignar la referencia bancaria');
            } else oGlob.m_AlertWarning('Favor de proporcionar la referencia bancaria');
        });


        $(lstBotones.btnNuevo).unbind();
        $(lstBotones.btnNuevo).click(function () {
            var lstChkPlantilla = $(lstTablas.tbldispersiones).jqGrid('getGridParam', 'selarrrow');
            var lstPolizasChks = new Array(0);
            if (lstChkPlantilla != null && lstChkPlantilla.length > 0) {
                var bContinua = true;
                for (var i = 0; i < lstChkPlantilla.length; i++) {
                    var row = $(lstTablas.tbldispersiones).jqGrid('getRowData', lstChkPlantilla[i]);

                    if ($(lstCaja.cmbtipo).val() == "1") {
                        if (row.nambanco == "") {
                            oGlob.m_AlertWarning('No se ha asignado el banco beneficiario');
                            bContinua = false;
                            break;
                        }
                    } else {

                        if (row.FondoFijo == "") {
                            oGlob.m_AlertWarning('No se ha asignado el fondo fijo');
                            bContinua = false;
                            break;
                        }
                    }
                    if (row.refbanco == "") {
                        oGlob.m_AlertWarning('Todas las líneas a exportar deben tener asignada una referencia bancaria');
                        bContinua = false;
                        break;
                    }
                    if (row.rfc == "") {
                        oGlob.m_AlertWarning('Todas las líneas a exportar deben tener asignada un RFC para el operador');
                        bContinua = false;
                        break;
                    }
                    lstPolizasChks.push({
                        fecha: row.fecha,
                        descrip: row.descrip,
                        cargo: row.cargo,
                        nambanco: row.nambanco,
                        FondoFijo: row.FondoFijo,
                        refbanco: row.refbanco,
                        operador: row.operador,
                        rfc: row.rfc,
                        cuenta: row.cuenta,
                        ano: row.ano,
                        mes: row.mes,
                        poliza: row.poliza,
                        linea: row.linea,
                        Defaultdim: row.Defaultdim,
                        AccDisplay: row.AccDisplay
                    });
                }
                if (bContinua) {
                    oGlob.m_Confirma_Accion('Se ha detectado la selección de <b>' + lstChkPlantilla.length + ' póliza(s)</b>.Una vez utilizadas estás no estarán disponibles.¿Deseas continuar?', function (response) {
                        if (response) {

                            pm = "Asistente:'" + $(lstCaja.txtAsistente).val() + "',tipo:" + $(lstCaja.cmbtipo).val() + ",lstPolizas: " + JSON.stringify(lstPolizasChks) + "";
                            oThis.m_CreaPlantilla(pm);
                        }
                    });
                }

            } else oGlob.m_AlertWarning('No se ha detectado la selección de póliza(s) para asignar la referencia bancaria');
        });

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {
            oGlob.m_AbreModal('#modalfiltro', function (status) {
                if (status) {
                    $('#modalfiltro').find(':input').not(':button, :submit, :reset, :hidden, :checkbox, :radio').val('');
                    $(lstFiltros.cmbsuc).chosen();
                }
            });
        });

        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();

        });
        oGlob.m_hideLoader();
    }
    this.m_DeshabilitaElementos = function (jEstado) {

        oGlob.m_DeshabilitaElemento(lstBotones.btnref, jEstado);
        oGlob.m_DeshabilitaElemento(lstBotones.btnNuevo, jEstado);
        oGlob.m_DeshabilitaElemento(lstBotones.btnFiltra, jEstado);
        oGlob.m_DeshabilitaElemento(lstCaja.txtAsistente, jEstado);        
        oGlob.m_DeshabilitaElemento(lstCaja.txtRef, jEstado); 
        oGlob.m_DeshabilitaElemento(lstCaja.cmbtipo, !jEstado);
        oGlob.m_DeshabilitaElemento(lstBotones.btnChange, !jEstado); 
    }
    this.m_EventoBusqueda = function () {
        var oDispersion = {
            usado: 0,
            operador: $(lstFiltros.txtOperador).val(),
            rfc: $(lstFiltros.txtrfc).val(),
            viaje: $(lstFiltros.txtFoliovij).val(),
            fechainicio: $(lstFiltros.txtFechacinicio).val(),
            fechafin: $(lstFiltros.txtFechacfin).val(),
            sucursal: $(lstFiltros.cmbsuc).val(),
            tipo: $(lstCaja.cmbtipo).val()
            
        };
        pm = "oDispersion: " + JSON.stringify(oDispersion);
        oThis.m_ListaDispersiones(pm);
        oGlob.m_CloseFilters();
    }

    this.m_AsignaAsistente = function () {
        var oDate = new Date();
        var Tipo = $(lstCaja.cmbtipo).val() == "1" ? "Dispersión banco" : "Dispersión fondo fijo";
        var lablname =  Tipo+ " " + oDate.getDate() + "/"
            + (oDate.getMonth() + 1) + "/"
            + oDate.getFullYear() + " "
            + oDate.getHours() + ":"
            + oDate.getMinutes() + ":"
            + oDate.getSeconds();
        $(lstCaja.txtAsistente).val(lablname);
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

    this.m_ListaDispersiones = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "Listadispersiones",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnref, true);
                oGlob.m_ViewloaderGrid(lstTablas.tbldispersiones);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnref, false);
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
                oGlob.m_HideloaderGrid(lstTablas.tbldispersiones);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tbldispersiones);
            }
        });
    }

    this.m_CreaPlantilla = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "CreaPlantilla",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_EventoBusqueda();
                        oGlob.m_AlertSuccess("El asistente se ha creado con éxito folio asignado <b>" + response.data + "</b>. El registro se encuentra disponible en historicos");
                        window.open('Paginas/Generic-Table-Export.aspx?modo=plantilla&identificador=' + response.data, '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
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
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');                
            }
        });
    }

    this.m_Cargabancos = function () {
        $.ajax({
            type: "POST",
            url: Controller + "ListaBancos",
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var opcion = "";
                        var select = $(lstCaja.cmbanco);
                        select[0].options.length = 0;
                        opcion = $("<option />", {
                            value: "",
                            text: "Selecciona el banco beneficiario"
                        });
                        select.append(opcion);

                        for (var i = 0; i < response.data.length; i++) {

                            opcion = $("<option />", {
                                value: response.data[i].sAX365,
                                text: response.data[i].sAX365 + " - " + response.data[i].sDescripcion
                            });
                            select.append(opcion);
                        }                        
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(response.mensaje);
                        break;
                    case "SIN RESULTADOS":
                        oGlob.m_AlertWarning('No se ha cargado la información de bancos');
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_CargaFondofijo = function () {
        $.ajax({
            type: "POST",
            url: Controller + "ListaFondofijo",
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var opcion = "";
                        var select = $(lstCaja.cmbFondofijo);
                        select[0].options.length = 0;
                        opcion = $("<option />", {
                            value: "",
                            text: "Selecciona el fondo fijo"
                        });
                        select.append(opcion);

                        for (var i = 0; i < response.data.length; i++) {

                            opcion = $("<option />", {
                                value: response.data[i].sAX365,
                                text: response.data[i].sAX365 + " - " + response.data[i].sDescripcion
                            });
                            select.append(opcion);
                        }                        
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(response.mensaje);
                        break;
                    case "SIN RESULTADOS":
                        oGlob.m_AlertWarning('No se ha cargado la información de bancos');
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_CargaSucursales = function () {
        $.ajax({
            type: "POST",
            url: Controller + "Listsucursales",
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var opcion = "";
                        var select = $(lstFiltros.cmbsuc);
                        select[0].options.length = 0;
                        opcion = $("<option />", {
                            value: "",
                            text: "Selecciona la sucursal"
                        });
                        select.append(opcion);

                        for (var i = 0; i < response.data.length; i++) {

                            opcion = $("<option />", {
                                value: response.data[i].suc,
                                text: response.data[i].suc + " - " + response.data[i].nomcia
                            });
                            select.append(opcion);
                        }
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(response.mensaje);
                        break;
                    case "SIN RESULTADOS":
                        oGlob.m_AlertWarning('No se ha cargado la información de bancos');
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    }


    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tbldispersiones;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [

                { label: "Fecha transacción", name: "fecha", type: "text", width: 130, align: "center" },
                { label: "Descripción", name: "descrip", type: "text", width: 250, align: "center" },
                {
                    label: "Total", name: "cargo", type: "number", width: 100, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                { label: "Dimensión cuenta", name: "AccDisplay", type: "text", width: 200, align: "center" },
                { label: "Banco", name: "nambanco", type: "text", width: 110, align: "center", hidden: $(lstCaja.cmbtipo).val() == "2" },
                { label: "Fondo fijo", name: "FondoFijo", type: "text", width: 110, align: "center", hidden: $(lstCaja.cmbtipo).val() == "1" },
                { label: "Referencia", name: "refbanco", type: "text", width: 200, align: "center" },
                { label: "Operador", name: "operador", type: "text", width: 250, align: "center" },
                { label: "RFC", name: "rfc", type: "text", width: 130, align: "center" },
                { label: "Cuenta", name: "cuenta", type: "text", width: 150, align: "center" },
                { label: "Dimensiones", name: "Defaultdim", type: "text", width: 200, align: "center" },
                { label: "ano", name: "ano", type: "text", width: 140, align: "center", hidden: true },
                { label: "mes", name: "mes", type: "text", width: 140, align: "center", hidden: true },
                { label: "poliza", name: "poliza", type: "text", width: 140, align: "center", hidden: true },
                { label: "linea", name: "linea", type: "text", width: 140, align: "center", hidden: true },
                { label: "valido", name: "valido", type: "text", width: 140, align: "center", hidden: true },
                { label: "Mensaje", name: "mensaje", type: "text", width: 200, align: "left" },


            ],
            caption: 'Listado de dispersiones pendientes por emitir',
            multiselect: true,
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    if (row.valido == "0")
                        $(jNameGrid).find("#" + key + "").addClass('bg-danger');
                    if (row.valido == "2")
                        $(jNameGrid).find("#" + key + "").addClass('bg-warning');
                }
            },
            pager: "#jqGridPager"
        });
    }
}
