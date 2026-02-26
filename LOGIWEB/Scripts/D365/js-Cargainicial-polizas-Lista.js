function Cargainicial() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFConInicio: "#txtFechacinicio", txtFConFin: "#txtFechacfin", txtFCreInicio: "#txtFechacrinicio", txtFCreFin: "#txtFechacrfin",
            txtfolioserie: "#txtfolserie",  txtFolioD365:"#txtFolioD365"
        },
        lstCaja = {  },
        lstElemnts = { div_upload: "#uploadexcelitem", iFramupload: "#iUploadEXCEL" },
        lstmodales = { cargamodal:"#modal_carga_inicial" }
        lstTablas = { tblcargainicial: "#tbl-carga-inicial" },
        lstBotones = {
            btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga", btnAdd: "#btnAdd",       
            bntCargadatos:"#bntCargadatos"
        },
            Controller = "../Metodos/D365/CargaInicial-Datos.aspx/";

    this.m_InicializaEvents = function () {

        oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConFin);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreFin);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        

        oThis.m_EventoBusqueda();
        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });

        $(lstBotones.btnAdd).unbind();
        $(lstBotones.btnAdd).click(function () {

            oGlob.m_AbreModal(lstmodales.cargamodal, function (status) {
                $(lstElemnts.div_upload).html('<iframe id="iUploadEXCEL" class="custom_iframe" frameborder="0" scrolling="no" src="Formularios/UPLOAD/UploadFilesEXCEL.html"></iframe>');
            });
        });

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {
            oGlob.m_AbremodalFiltro();

        });


        $(lstBotones.bntCargadatos).unbind();
        $(lstBotones.bntCargadatos).click(function () {            

            var sPathupload = $(lstElemnts.iFramupload).contents().find('#txthhName').val();
            var sNombreFile = sPathupload === undefined ? "" : sPathupload;
            if (sNombreFile == "") {
                oGlob.m_AlertWarning('El archivo para carga inicial <b>no se encuentra adjunto</b>, para continuar favor de realizar el adjunto de la plantilla.');
            }
            else {
                oGlob.m_Confirma_Accion('Se ha detectado un nuevo archivo, asegurate que no existan registros vacios. Una vez procesado los datos no pueden ser eliminados.¿Deseas continuar?', function (response) {
                    if (response) {

                        pm = "sArchivo:'" + sNombreFile + "'";
                        oThis.m_GuardaCargaInicial(pm);
                    }
                });
            } 
        });

        $(lstBotones.btnDescarga).unbind();
        $(lstBotones.btnDescarga).click(function () {
            var headers = [];
            var valores = null;
            var data = $(lstTablas.tblcargainicial).jqGrid("getGridParam", "data");
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
                    { "sColum": "Impuesto", "sType": "texto" },
                    { "sColum": "Folio D365", "sType": "texto" }
                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {
                    var TipoDocumento = "";
                    switch (data[i].id_tipo_doc) {
                        case 1:
                            TipoDocumento = "Combustibles costo";
                            break;
                        case 2:
                            TipoDocumento = "Combustibles proveedor";
                            break;
                        case 3:
                            TipoDocumento = "Mano de obra";
                            break;
                        default:
                            TipoDocumento = "No clasificado";
                            break;
                    }
                    var datarow = [
                        { "sValor": TipoDocumento },
                        { "sValor": data[i].nombrerfc },
                        { "sValor": data[i].rfc },
                        { "sValor": data[i].uuid },
                        { "sValor": data[i].fechaContable },
                        { "sValor": data[i].fechaCreacion },
                        { "sValor": data[i].total },
                        { "sValor": data[i].subtotal },
                        { "sValor": data[i].impuesto },
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
    }
    this.m_EventoBusqueda = function () {
        var oPoliza = {
            folioserie: $(lstFiltros.txtfolioserie).val(),            
            FechaConInicio: $(lstFiltros.txtFConInicio).val(),
            FechaConFin: $(lstFiltros.txtFConFin).val(),
            FechaCreInicio: $(lstFiltros.txtFCreInicio).val(),
            FechaCreFin: $(lstFiltros.txtFCreFin).val(),
            recIdD365: $(lstFiltros.txtFolioD365).val()
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

    this.m_GuardaCargaInicial = function (oParam) {
        $.ajax({
            type: "POST",
            url: Controller + "CargaAsientosInicial",
            data: "{" + oParam + "}",
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
                        oGlob.m_AlertSuccess('La información ha sido procesada con éxito.</b>');
                        $(lstElemnts.div_upload).html('<iframe id="iUploadEXCEL" class="custom_iframe" frameborder="0" scrolling="no" src="Formularios/UPLOAD/UploadFilesEXCEL.html"></iframe>');
                        oThis.m_EventoBusqueda();
                        $(lstmodales.cargamodal).modal('hide');
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
                oGlob.m_ViewloaderGrid(lstTablas.tblcargainicial);
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
                oGlob.m_HideloaderGrid(lstTablas.tblcargainicial);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblcargainicial);
            }
        });
    }  
    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblcargainicial;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Folio asiento", name: "FolioAsiento", type: "text", width: 140, align: "center" },
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
                { label: "Folio D365", name: "recIdD365", type: "text", width: 120, align: "center" },

                { label: "id_tipo_doc", name: "id_tipo_doc", type: "text", width: 140, align: "center", hidden: true },
                { label: "estatus", name: "estatus", type: "text", width: 140, align: "center", hidden: true },

            ],
            caption: 'Listado de pólizas sincronizadas en D365',
            multiselect: true,
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {  },
            pager: "#jqGridPager"
        });
    }
}