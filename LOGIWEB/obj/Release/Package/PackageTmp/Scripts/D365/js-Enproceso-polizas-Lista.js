function Pendietes() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFConInicio: "#txtFechacinicio", txtFConFin: "#txtFechacfin", txtFCreInicio: "#txtFechacrinicio", txtFCreFin: "#txtFechacrfin",
            cmbdoc: "#cmbdocs", txtfolioserie: "#txtfolserie", txtFolio: "#txtFolio"
        },
        lstCaja = { txtFolio: "#txtpolizafolio", txtComments: "#txtcomments" },
        lstElemnts = { classequiv: ".classequiv", msgequiv: "#mensajeequiv", elTotal: "#total" },
        lstmodales = { modalcomments: "#div_modal_commentario" }
        lstTablas = { tblpolizaspendientes: "#tbl-pendientes" },
        lstBotones = {
            btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga", btnAddcnf: "#btnAddcnf", btnDowdetalle:"#btnDowdetalle"
        },
        Controller = "../Metodos/D365/PolizaContable-Datos.aspx/";

    this.m_InicializaEvents = function () {

        oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConFin);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreFin);
        oGlob.m_CargaDocumentos(lstFiltros.cmbdoc, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, true);
        oThis.m_EventoBusqueda();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {
            oGlob.m_AbremodalFiltro();

        });

        $(lstBotones.btnDescarga).unbind();
        $(lstBotones.btnDescarga).click(function () {
            var headers = [];
            var valores = null;
            var data = $(lstTablas.tblpolizaspendientes).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Tipo documento", "sType": "texto" },
                    { "sColum": "Emisor/Receptor", "sType": "texto" },
                    { "sColum": "RFC", "sType": "texto" },
                    { "sColum": "UUID", "sType": "texto" },
                    { "sColum": "Fecha contable", "sType": "texto" },
                    { "sColum": "Fecha creación", "sType": "texto" },
                    { "sColum": "Total", "sType": "texto" },
                    { "sColum": "Subtotal", "sType": "texto" },
                    { "sColum": "Impuesto", "sType": "texto" }
                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {                    
                    var datarow = [
                        { "sValor": data[i].Nombredocumento },
                        { "sValor": data[i].nombrerfc },
                        { "sValor": data[i].rfc },
                        { "sValor": data[i].uuid },
                        { "sValor": data[i].fechaContable },
                        { "sValor": data[i].fechaCreacion },
                        { "sValor": data[i].total },
                        { "sValor": data[i].subtotal },
                        { "sValor": data[i].impuesto }
                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'polizas_en_proceso_de_envio'";
                oGlob.m_ExportDataTable(pm);
            }
        });

        $(lstBotones.btnDowdetalle).unbind();
        $(lstBotones.btnDowdetalle).click(function () {
            var headers = [];
            var valores = null;
            var data = $("#tbl-logs-proceso").jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "DESCRIPCION", "sType": "texto" },
                    { "sColum": "MAYOR OPE", "sType": "texto" },
                    { "sColum": "CUENTA OPE", "sType": "texto" },
                    { "sColum": "SUB CUENTA OPE", "sType": "texto" },
                    { "sColum": "CUENTA D365", "sType": "texto" },
                    { "sColum": "CARGO", "sType": "texto" },
                    { "sColum": "ABONO", "sType": "texto" },
                    { "sColum": "SUCURSAL OPE", "sType": "texto" },
                    { "sColum": "SUCURSAL D365", "sType": "texto" },
                    { "sColum": "CENTRO COSTO OPE", "sType": "texto" },
                    { "sColum": "CENTRO COSTO D365", "sType": "texto" },
                    { "sColum": "DEPARTAMENTO OPE", "sType": "texto" },
                    { "sColum": "DEPARTAMENTO D365", "sType": "texto" },
                    { "sColum": "AREA OPE", "sType": "texto" },
                    { "sColum": "AREA D365", "sType": "texto" },
                    { "sColum": "VEHICULO", "sType": "texto" },
                    { "sColum": "FILIAL TERCERO", "sType": "texto" },
                    { "sColum": "MENSAJE", "sType": "texto" }
                    
                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {
                     
                    var datarow = [
                        { "sValor": data[i].descrip },
                        { "sValor": data[i].mayor },
                        { "sValor": data[i].cuenta },
                        { "sValor": data[i].scuenta },
                        { "sValor": data[i].cuenta_AX },
                        { "sValor": data[i].cargo },
                        { "sValor": data[i].abono },
                        { "sValor": data[i].sucursal },
                        { "sValor": data[i].sucursal_D365 },
                        { "sValor": data[i].centrocosto },
                        { "sValor": data[i].centrocosto_D365 },
                        { "sValor": data[i].departamento },
                        { "sValor": data[i].departamento_D365 },
                        { "sValor": data[i].area },
                        { "sValor": data[i].area_D365 },
                        { "sValor": data[i].vehiculo },
                        { "sValor": data[i].filialtercero_D365 },
                        { "sValor": data[i].mensaje }
                    ];
                    valores[i] = datarow; 
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'detalle_de_poliza_contable'";
                oGlob.m_ExportDataTable(pm);
            }
        });
    }
    this.m_EventoBusqueda = function () {
        var oPoliza = {
            estatus: 0,
            FolioAsiento: $(lstFiltros.txtFolio).val(),
            folioserie: $(lstFiltros.txtfolioserie).val(),
            sDocumentosIN: "" + oGlob.m_DataFromSelect(lstFiltros.cmbdoc, false) + "",
            FechaConInicio: $(lstFiltros.txtFConInicio).val(),
            FechaConFin: $(lstFiltros.txtFConFin).val(),
            FechaCreInicio: $(lstFiltros.txtFCreInicio).val(),
            FechaCreFin: $(lstFiltros.txtFCreFin).val()
        };
        pm = "Poliza: " + JSON.stringify(oPoliza);
        oThis.m_ListaPolizascontable(pm);
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

    this.m_ListaPolizascontable = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListaPolizas",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblpolizaspendientes);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
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
                oGlob.m_HideloaderGrid(lstTablas.tblpolizaspendientes);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblpolizaspendientes);
            }
        });
    }

    this.m_ListaDetallePoliza = function (oParams) {
        
        $.ajax({
            type: "POST",
            url: Controller + "ListadetallePoliza",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {                
                oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, true);
               ///oGlob.m_ViewloaderGrid('#tbl-logs-proceso');
            },
            success: function (result) {
                var responsedetalle = result["d"];
                switch (responsedetalle.estatus) {
                    case "OK":
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, false);                        
                        oThis.m_CargaDataTableDetalle(responsedetalle.data);                        
                        break;
                    case "-1":
                        oGlob.m_showModalSession();
                        break;
                    case "-2":
                        oGlob.m_showModalPermiso();
                        break;
                    case "EQUIVALENCIAS":
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, false);                        
                        $(lstElemnts.msgequiv).html("<p>Se ha detectado una o varias líneas que no cuentan con equivalencias. Favor de corregirlo <b>para poder interfazar hacía Dynamics 365</b></p>");
                        oGlob.m_OcultaElemnto(lstElemnts.classequiv, false);
                        oGlob.m_AlertWarning('Se ha detectado una o varias líneas que no cuentan con equivalencias. Favor de corregirlo para poder interfazar hacía Dynamics 365');
                        oThis.m_CargaDataTableDetalle(responsedetalle.data);                        
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(responsedetalle.mensaje);
                        break;
                    case "SIN RESULTADOS":
                        oGlob.m_AlertWarning('No se han encontrado documentos con los filtros proporcionados');
                        break;
                    default:
                        oGlob.m_AlertWarning(responsedetalle.mensaje);
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

    this.Visualizacomment = function (evt, jID) {
        var row = $(lstTablas.tblpolizaspendientes).jqGrid('getRowData', jID);
        if (row.errortimbrado == "1") {
            var oParam = {
                FolioAsientoMatch: row.FolioAsiento
            };
            oParams = "oParam:" + JSON.stringify(oParam);
            $.ajax({
                type: "POST",
                url: Controller + "ListaComentario",
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
                            $(lstCaja.txtFolio).val(row.FolioAsiento);
                            $(lstCaja.txtComments).html(response.data.comments);
                            oGlob.m_DeshabilitaElemento(lstCaja.txtComments, true);
                            oGlob.m_AbreModal(lstmodales.modalcomments, function (status) { });
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
        } else oGlob.m_AlertWarning('El documento no presenta incidencias al momento de timbrar');
    }

    this.VisualizaDocto = function (evt, jID) {

        marca_poliza = false;
        oGlob.m_showLoader();
        var row = $(lstTablas.tblpolizaspendientes).jqGrid('getRowData', jID);
        $('#lbllogtitulo').html('Detalle de la póliza a replicar en D365');
        var jNameGrid = '#tbl-logs-proceso';
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);

        oGlob.m_AbreModal('#modal-logs', function (status) {
            var oDetalle = {
                FolioAsiento: row.FolioAsiento,
                tipo_documento: row.id_tipo_doc
            };
            pm = "Detalle: " + JSON.stringify(oDetalle);
            oThis.m_ListaDetallePoliza(pm);
            oGlob.m_hideLoader();
        }); 
    }
    this.DespliegaTotal = function (jTotal) {
        $(lstElemnts.elTotal).html("<b>Total: </b>$" + jTotal.toLocaleString('es-MX'));
    }

    this.m_CargaDataTableDetalle = function (jdataJSON) {

        var jNameGrid = '#tbl-logs-proceso';
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [                
                { label: "Descripción", name: "descrip", type: "text", width: 250, align: "center" },
                { label: "Mayor", name: "mayor", type: "text", width: 90, align: "center" },
                { label: "Cuenta", name: "cuenta", type: "text", width: 90, align: "center" },
                { label: "Subcuenta", name: "scuenta", type: "text", width: 110, align: "center" },
                { label: "Cuenta AX", name: "cuenta_AX", type: "text", width: 120, align: "center" },
                {
                    label: "Cargo", name: "cargo", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                {
                    label: "Abono", name: "abono", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                { label: "Sucursal", name: "sucursal", type: "text", width: 100, align: "center" },
                { label: "Sucursal AX", name: "sucursal_D365", type: "text", width: 120, align: "center" },
                { label: "Centro", name: "centrocosto", type: "text", width: 90, align: "center" },
                { label: "Centro AX", name: "centrocosto_D365", type: "text", width: 120, align: "center" },
                { label: "Departamento", name: "departamento", type: "text", width: 130, align: "center" },
                { label: "Departamento AX", name: "departamento_D365", type: "text", width: 150, align: "center" },
                { label: "Área", name: "area", type: "text", width: 90, align: "center" },
                { label: "Área AX", name: "area_D365", type: "text", width: 90, align: "center" },
                { label: "Vehículo", name: "vehiculo", type: "text", width: 90, align: "center" },
                { label: "Filial AX", name: "filialtercero_D365", type: "text", width: 100, align: "center" },
                { label: "Mensaje", name: "mensaje", type: "text", width: 300, align: "center" },
                { label: "FolioAsiento", name: "FolioAsiento", type: "text", width: 140, align: "center", hidden: true },
                { label: "valido", name: "valido", type: "text", width: 140, align: "center", hidden: true },

            ],
            caption: 'Detalle del la póliza contable',
            viewrecords: true,
            rownumbers: true,
            height: "20em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                var total = 0;
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key); 
                    if (row.valido == "0")
                        $(jNameGrid).find("#" + key + "").addClass('bg-danger');
                    total = parseFloat(row.cargo) + parseFloat(total);
                    oPendiente.DespliegaTotal(total);   
                }
            },
            pager: "#jqGridPagerLogs"
        });
        $(jNameGrid).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    }


    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblpolizaspendientes;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center" },
                { label: ".", name: "rComent", type: "text", width: 50, align: "center" },
                { label: "Folio asiento", name: "FolioAsiento", type: "text", width: 140, align: "center"},
                { label: "Tipo documento", name: "Nombredocumento", type: "text", width: 150, align: "center" },
                { label: "Emisor/Receptor", name: "nombrerfc", type: "text", width: 300, align: "left" },
                { label: "RFC", name: "rfc", type: "text", width: 110, align: "center" },
                { label: "Factura", name: "folioserie", type: "text", width: 100, align: "center" },
                { label: "UUID", name: "uuid", type: "text", width: 250, align: "center" },
                { label: "Factura ref", name: "doctoref", type: "text", width: 110, align: "center" },
                { label: "UUID ref", name: "uuidref", type: "text", width: 250, align: "center" },
                { label: "Fecha contable", name: "fechaContable", type: "text", width: 120, align: "center" },
                { label: "Fecha creación", name: "fechaCreacion", type: "text", width: 120, align: "center" },
                {
                    label: "Total", name: "total", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                {
                    label: "Subtotal", name: "subtotal", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                {
                    label: "Impuesto", name: "impuesto", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                { label: "id_tipo_doc", name: "id_tipo_doc", type: "text", width: 140, align: "center", hidden: true },
                { label: "errortimbrado", name: "errortimbrado", type: "text", width: 140, align: "center", hidden: true },

            ],
            caption: 'Listado de pólizas pendientes de crear en D365',
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                var ActiveComment = '', CssButton = 'secondary';                                
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    ActiveComment = 'disabled';
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    if (row.errortimbrado == "1") {                        
                        ActiveComment = '';
                        CssButton = 'danger';
                    }

                    var rowCommt = "<button  type=\"button\" class=\"btn btn-sm alert-" + CssButton + " " + ActiveComment + " \"  title=\"Visualiza comentarios\" onClick=\"oPendiente.Visualizacomment(this, " + key + ")\"> <i class=\"fas fa-comment-alt\" data-label=\"Visualiza comentarios\" ></i></button>";
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" title=\"Visualiza detalle de la póliza\" onClick=\"oPendiente.VisualizaDocto(this, " + key + ")\"><i class=\"fa fa-file \" ></i></button>";                    
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit, rComent: rowCommt });
                }
            },
            pager: "#jqGridPager"
        });
    }
}