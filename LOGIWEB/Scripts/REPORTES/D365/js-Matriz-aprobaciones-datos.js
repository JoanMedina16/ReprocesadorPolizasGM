function RequisicionesZAP() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFechaInicio: "#txtFechainicio", txtFechaFin: "#txtFechafin", txtdiario: "#txtdiario", txtcuenta: "#txtcuenta",
            txtsucursal: "#txtsucursal", txtcentro: "#txtcentro", txtdepto: "#txtdepto", txtdocumento: "#txtdocumento",
            cmbestatus:"#cmbestatus"
        },
        lstCaja = {},
        lstElemnts = { classequiv: ".classequiv" },
        lstTablas = { tblDatos: "#tbl-datos-zap" },
        lstBotones = { btnDescarga: "#btnDescarga", btnSerch: "#btnBusca",btnDowdetalle: "#btnDowdetalle" },
        Controller = "Metodos/REPORTES/D365/Requisiciones-Datos.aspx/";

    this.m_InicializaEvents = function () {

        oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
        oGlob.m_OcultaElemnto(lstBotones.btnDowdetalle, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oGlob.m_CargaDetePicker(lstFiltros.txtFechaInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFechaFin);
        oThis.m_EventoBusqueda();

        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });


        $(lstBotones.btnDescarga).unbind();
        $(lstBotones.btnDescarga).click(function () {

            var headers = [];
            var valores = null;
            var data = $(lstTablas.tblDatos).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Fecha registrado", "sType": "texto" },
                    { "sColum": "Estatus", "sType": "texto" },
                    { "sColum": "Número del diario", "sType": "texto" },
                    { "sColum": "Descripción", "sType": "texto" },
                    { "sColum": "Total", "sType": "texto" },
                    { "sColum": "RECID", "sType": "texto" }
                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {
                    var datarow = [
                        { "sValor": data[i].REQUIREDDATE },
                        { "sValor": data[i].ESTATUS },
                        { "sValor": data[i].PURCHREQID },
                        { "sValor": data[i].PURCHREQNAME },
                        { "sValor": data[i].TOTAL },
                        { "sValor": data[i].RECID }
                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'historico_de_requisiciones'";
                oGlob.m_ExportDataTable(pm);
            }

        });
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
    this.m_EventoBusqueda = function () {


        //crea objeto y enviar a ajax
        var oPeriodo = {
            FECHA_INICIO: $(lstFiltros.txtFechaInicio).val(),
            FECHA_FINAL: $(lstFiltros.txtFechaFin).val(),
            SUCURSAL: $(lstFiltros.txtsucursal).val(),
            CENTRO_DE_COSTO: $(lstFiltros.txtcentro).val(),
            DEPARTAMENTO: $(lstFiltros.txtdepto).val(),
            REQUISITIONSTATUS: $(lstFiltros.cmbestatus).val()
        };
        pm = "Periodo: " + JSON.stringify(oPeriodo);
        oThis.m_Cargadatoscuentas(pm);
        oGlob.m_CloseFilters();

    }

    this.m_Cargadatoscuentas = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListarequisicionesZAP",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescargaTXT, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblDatos);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescargaTXT, false);
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
                        oGlob.m_AlertWarning('No se ha encontrado información con los filtros proporcionados');
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
                oGlob.m_HideloaderGrid(lstTablas.tblalmacenes);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblalmacenes);
            }
        });
    }

    this.m_ListaDetallerequi = function (oParams, jTipodocto) {
        $.ajax({
            type: "POST",
            url: Controller + "Listadetallerequi",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
                //oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, true);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        //oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, false);
                        oThis.m_CargaDataTableDetalle(response.data, jTipodocto);
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
                //oGlob.m_HideloaderGrid('#tbl-logs-proceso');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                //oGlob.m_HideloaderGrid('#tbl-logs-proceso');
            }
        });
    }

    this.VisualizaDocto = function (evt, jID) {

        marca_poliza = false;
        oGlob.m_showLoader();
        var row = $(lstTablas.tblDatos).jqGrid('getRowData', jID);
        $('#lbllogtitulo').html('Detalle de la requisición');
        var jNameGrid = '#tbl-logs-proceso';
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);

        oGlob.m_AbreModal('#modal-logs', function (status) {
            var oFiltro = {
                RECID: row.RECID
            };
            pm = "Detalle: " + JSON.stringify(oFiltro);
            oThis.m_ListaDetallerequi(pm, row.id_tipo_doc);
            oGlob.m_hideLoader();
        });
    }

    this.m_CargaDataTable = function (jdataJSON) {


        var jNameGrid = lstTablas.tblDatos;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center" },
                { label: "Fecha registrado", name: "REQUIREDDATE", type: "text", width: 150 },
                { label: "Estatus", name: "ESTATUS", type: "text", width: 110,  },
                { label: "Número del diario", name: "PURCHREQID", type: "text", width: 130 },
                { label: "Descripción", name: "PURCHREQNAME", type: "text", width: 250 },
               

                {
                    label: "Total", name: "TOTAL", type: "number", width: 150, formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' },
                },
                { label: "RECID", name: "RECID", type: "text", width: 250, hidden:true },


            ],
            viewrecords: true,
            caption: 'HISTORICO DE REQUISICIONES DYNAMICS 365',
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" title=\"Visualiza detalle de la requisición\" onClick=\"oRequis.VisualizaDocto(this, " + key + ")\"><i class=\"fa fa-file \" ></i></button>";
                  $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit}); 
                }
            },
            pager: "#jqGridPager"
        });
    }

    this.m_CargaDataTableDetalle = function (jdataJSON, jTipodocto) {

        var jNameGrid = '#tbl-logs-proceso';
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Folio artículo", name: "ITEMID", type: "text", width: 200, align: "center" },
                { label: "Descripción", name: "NAME", type: "text", width: 350, align: "center" },
                { label: "Almacén", name: "DELIVERYNAME", type: "text", width: 280, align: "center" },

                {
                    label: "Precio de compra", name: "PURCHPRICE", type: "number", width: 150, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },

                {
                    label: "Cantidad", name: "PURCHQTY", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },

                {
                    label: "Total", name: "LINEAMOUNT", type: "number", width: 130, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                { label: "Grupo impuesto", name: "TAXGROUP", type: "text", width: 120, align: "center" },

            ],
            caption: 'DETALLE DE LA REQUISICIÓN',
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                var ActivaDescarga = 'btn-sm btn-info';
                var ActiveButton = 'disabled';

                var total = 0;
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    /*//total = parseFloat(row.cargo) + parseFloat(total);
                    //oHistorial.DespliegaTotal(total);
                    //ActivaDescarga = 'alert-secondary';
                    ActiveButton = 'disabled';

                    if (row.usaxml == "1") {
                        ActivaDescarga = 'alert-info';;
                        ActiveButton = "";

                    }
                    var rowDown = "<button type=\"button\" class=\"btn btn-sm " + ActivaDescarga + " " + ActiveButton + " \" title=\"Descarga XML\" onClick=\"oHistorial.DescargaAdjuntoLinea(this, " + key + ")\"><i class=\"fas fa-download \" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rDown: rowDown });*/
                }

            },
            pager: "#jqGridPagerLogs"
        });
    }
}