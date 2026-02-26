function Historicos() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFConInicio: "#txtFechacinicio", txtFConFin: "#txtFechacfin", txtFCreInicio: "#txtFechacrinicio", txtFCreFin: "#txtFechacrfin",
            cmbdoc: "#cmbdocs", txtfolioserie: "#txtfolserie", txtFolio: "#txtFolio", txtFolioD365:"#txtFolioD365"
        },
        lstCaja = { txtFolio: "#txtpolizafolio", txtComments:"#txtcomments" },
        lstElemnts = { classequiv: ".classequiv", elTotal:"#total"},
        lstmodales = { modalcomments: "#div_modal_commentario" }
        lstTablas = { tblpolizashistorial: "#tbl-historiales" },
        lstBotones = {
            btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga", btnAddcnf: "#btnAddcnf",
            btnEliminado: "#chkEliminado", btnInicial: "#chkInicial", btnElimina: "#btnElimina", btnDowdetalle: "#btnDowdetalle", bntDescarta: "#bntDescarta"
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
            var data = $(lstTablas.tblpolizashistorial).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Tipo documento", "sType": "texto" },
                    { "sColum": "Documento GM", "sType": "texto" },
                    { "sColum": "Fecha Log", "sType": "texto" },
                    { "sColum": "Folio D365", "sType": "texto" }
                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {                    
                    var datarow = [
                        { "sValor": data[i].Nombredocumento },
                        { "sValor": data[i].serie },
                        { "sValor": data[i].fechaCreacion },
                        { "sValor": data[i].recIdD365 }
                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'polizas_replicadas_en_D365'";
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

        
        $(lstBotones.bntDescarta).unbind();
        $(lstBotones.bntDescarta).click(function () {

            oGlob.m_Confirma_Accion('Una vez eliminado el registro este no podrá ser recuperado.¿Deseas continuar?', function (response) {
                if (response) {
                    var lstPolizasChks = new Array(0);
                    lstPolizasChks.push({
                        FolioAsiento: $(lstCaja.txtFolio).val(),
                        comments: $(lstCaja.txtComments).val()
                    });
                    pm = "lstPolizas: " + JSON.stringify(lstPolizasChks) + "";
                    oThis.m_DescartaPolizas(pm);
                }
            });
        });


        $(lstBotones.btnElimina).unbind();
        $(lstBotones.btnElimina).click(function () {
            var lstChecks = $(lstTablas.tblpolizashistorial).jqGrid('getGridParam', 'selarrrow');            
            if (lstChecks != null && lstChecks.length > 0) {
                if (lstChecks.length == 1) {
                    var bContinua = true;
                    for (var i = 0; i < lstChecks.length; i++) {
                        var row = $(lstTablas.tblpolizashistorial).jqGrid('getRowData', lstChecks[i]);
                        if (row.id_tipo_doc != "11") {
                            oGlob.m_AlertWarning('Solo es posible eliminar documentos relacionados con mano de obra');
                            bContinua = false;
                            break;
                        }
                        if (row.estatus != "2") {
                            oGlob.m_AlertWarning('Solo se contemplan registros que ya hayan sido interfazados hacia D365');
                            bContinua = false;
                            break;
                        }
                        $(lstCaja.txtFolio).val(row.FolioAsiento);
                        $(lstCaja.txtComments).val('');
                        $(lstCaja.txtComments).html('');                        
                        oGlob.m_DeshabilitaElemento(lstCaja.txtComments, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.bntDescarta, false);
                    }
                    if (bContinua)
                        oGlob.m_AbreModal(lstmodales.modalcomments, function (status) { });
                }
                else oGlob.m_AlertWarning('No es posible descartar multiples polizas favor de seleccionar solo un registro');
            } else oGlob.m_AlertWarning('No se ha detectado la selección de la poliza para ser descartada. Favor de reintentarlo');
        });
    }
    this.m_EventoBusqueda = function () {
        var oPoliza = {
            estatus: 2,
            FolioAsiento: $(lstFiltros.txtFolio).val(),
            folioserie: $(lstFiltros.txtfolioserie).val(),
            sDocumentosIN: "" + oGlob.m_DataFromSelect(lstFiltros.cmbdoc, false) + "",
            FechaConInicio: $(lstFiltros.txtFConInicio).val(),
            FechaConFin: $(lstFiltros.txtFConFin).val(),
            FechaCreInicio: $(lstFiltros.txtFCreInicio).val(),
            FechaCreFin: $(lstFiltros.txtFCreFin).val(),
            recIdD365: $(lstFiltros.txtFolioD365).val(),
            descartados: $(lstBotones.btnEliminado).is(':checked') ? 1 : 0,
            iniciales: $(lstBotones.btnInicial).is(':checked') ? 1 : 0,
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

    this.m_DescartaPolizas = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "Eliminapolizas",
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
                        oGlob.m_AlertSuccess('La póliza se ha cancelado con éxito. Recuerda que esta acción no cancela el movimiento en D365. <b>Por lo que la cancelación se debe realizar manual</b>');
                        $(lstmodales.modalcomments).modal('hide');
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
                oGlob.m_ViewloaderGrid(lstTablas.tblpolizashistorial);
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
                oGlob.m_HideloaderGrid(lstTablas.tblpolizashistorial);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblpolizashistorial);
            }
        });
    }

    this.m_ListaDetallePoliza = function (oParams, jTipodocto) {
        $.ajax({
            type: "POST",
            url: Controller + "ListadetallePoliza",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                //oGlob.m_ViewloaderGrid('#tbl-logs-proceso');
                oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, true);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, false);                        
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

    this.Visualizacomment = function (evt, jID) {
        var row = $(lstTablas.tblpolizashistorial).jqGrid('getRowData', jID);
        if (row.estatus == 3) {
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
                            oGlob.m_DeshabilitaElemento(lstBotones.bntDescarta, true);
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
        } else oGlob.m_AlertWarning('Comentarios no disponible');
    }

    this.VisualizaDocto = function (evt, jID) {

        marca_poliza = false;
        oGlob.m_showLoader();
        var row = $(lstTablas.tblpolizashistorial).jqGrid('getRowData', jID);
        $('#lbllogtitulo').html('Detalle de la póliza creada en D365');
        var jNameGrid = '#tbl-logs-proceso';
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);

        oGlob.m_AbreModal('#modal-logs', function (status) {
            var oDetalle = {
                FolioAsiento: row.FolioAsiento,
                tipo_documento: row.id_tipo_doc
            };
            pm = "Detalle: " + JSON.stringify(oDetalle);
            oThis.m_ListaDetallePoliza(pm, row.id_tipo_doc);
            oGlob.m_hideLoader();
        }); 
    }
    this.DespliegaTotal = function (jTotal) {        
        $(lstElemnts.elTotal).html("<b>Total: </b>$" + jTotal.toLocaleString('es-MX'));
    }

    this.DescargaAdjuntoLinea = function (evt, jID) {
        var row = $('#tbl-logs-proceso').jqGrid('getRowData', jID);
        if (row.usaxml == 1) {
            window.open('Paginas/Generic-Table-Export.aspx?modo=comprobanteviatico&identificador=' + row.FolioAsiento + '&linea=' + row.linea, '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
        } else oGlob.m_AlertWarning("No es posible descargar un XML a una línea que es de tipo iva o pago a operador");
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
                { label: ".", name: "rDown", type: "text", width: 50, align: "center", hidden: (jTipodocto != "9") },
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
                { label: "linea", name: "linea", type: "text", width: 140, align: "center", hidden: true },
                { label: "usaxml", name: "usaxml", type: "text", width: 140, align: "center", hidden: true },

            ],
            caption: 'Detalle del la póliza contable',
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
                    total = parseFloat(row.cargo) + parseFloat(total);
                    oHistorial.DespliegaTotal(total);
                    ActivaDescarga = 'alert-secondary';
                    ActiveButton = 'disabled';            

                    if (row.usaxml == "1") {
                        ActivaDescarga = 'alert-info';;
                        ActiveButton = "";

                    }
                    var rowDown = "<button type=\"button\" class=\"btn btn-sm " + ActivaDescarga + " " + ActiveButton + " \" title=\"Descarga XML\" onClick=\"oHistorial.DescargaAdjuntoLinea(this, " + key + ")\"><i class=\"fas fa-download \" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rDown: rowDown });
                }
                
            },
            pager: "#jqGridPagerLogs"
        });
    }

    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblpolizashistorial;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Tipo documento", name: "Nombredocumento", type: "text", width: 150, align: "center" },
                { label: "Documento GM", name: "folioserie", type: "text", width: 150, align: "center" },
                { label: "Fecha Log", name: "fechaCreacion", type: "text", width: 120, align: "center" },
                { label: "Folio D365", name: "rDFolio", type: "text", width: 120, align: "center" },
                { label: "Folio D365", name: "recIdD365", type: "text", width: 120, align: "center", hidden: true },
                { label: "id_tipo_doc", name: "id_tipo_doc", type: "text", width: 140, align: "center", hidden: true },
                { label: "estatus", name: "estatus", type: "text", width: 140, align: "center", hidden: true },

            ],
            caption: 'Listado de pólizas sincronizadas en D365',
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
                    ActiveComment = 'disabled';
                    CssButton = 'secondary';
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" title=\"Visualiza detalle de la póliza\" onClick=\"oHistorial.VisualizaDocto(this, " + key + ")\"><i class=\"fa fa-file \" ></i></button>";                    
                    var rFolio = "";
                    rFolio = row.recIdD365;
                    if (row.estatus == "3") {
                        rFolio = "ELIMINADO";
                        ActiveComment = '';
                        CssButton = 'danger';
                    }

                    var rowCommt = "<button  type=\"button\" class=\"btn btn-sm alert-" + CssButton + " " + ActiveComment + " \"  title=\"Visualiza comentarios\" onClick=\"oHistorial.Visualizacomment(this, " + key + ")\"> <i class=\"fas fa-comment-alt\" data-label=\"Visualiza comentarios\" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit, rDFolio: rFolio, rComent: rowCommt});
                    if (row.estatus == 3)
                        $(jNameGrid).find("#" + key + "").addClass('bg-warning');
                }
            },
            pager: "#jqGridPager"
        });
    }
}