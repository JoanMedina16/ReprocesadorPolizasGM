function FacturaBebidas() {
    var oThis = this, oParams = "",
        lstFiltros = {

        },
        lstCaja = {},
        lstElemnts = { div_upload: "#uploadexcelitem", iFramupload: "#iUploadEXCEL" },
        lstmodales = { cargamodal: "#modal_carga_inicial" }
    lstTablas = { tblfacturas: "#tbl-lista-facturas" },
        lstBotones = {
        btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga",btnDescargaZIP:"#btnDescargaZIP", btnAdd: "#btnAdd",
            bntCargadatos: "#bntCargadatos", btnPoliza: "#btnPoliza", btnGraba: "#btnAceptapoliza"
        },
        Controller = "../Metodos/D365/CargaInicial-Datos.aspx/";
    var ListadoCargas = null;

    this.m_InicializaEvents = function () {

        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDescargaZIP, true);

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {

            $('#modalfiltro').modal('show');
            $("#modalfiltro").on('shown.bs.modal', function () {
                $(lstElemnts.div_upload).html('<iframe id="iUploadEXCEL" class="custom_iframe" frameborder="0" scrolling="no" src="Formularios/UPLOAD/UploadFilesEXCEL.html"></iframe>');
            });

        });

        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {
            var sPathupload = $(lstElemnts.iFramupload).contents().find('#txthhName').val();
            var sNombreFile = sPathupload === undefined ? "" : sPathupload;
            if (sNombreFile == "") {
                oGlob.m_AlertWarning('El archivo para descarga de facturas <b>no se encuentra adjunto</b>, para continuar favor de realizar el adjunto de la plantilla.');
            }
            else {
                oGlob.m_Confirma_Accion('Se ha detectado un archivo adjunto, las facturas proporcionadas en la columna A se procesaran del directorio de bitacoras XML.¿Deseas continuar?', function (response) {
                    if (response) {

                        pm = "sArchivo:'" + sNombreFile + "'";
                        oThis.m_Extraeremesas(pm);
                    }
                });
            }
        });


        $(lstBotones.btnDescarga).unbind();
        $(lstBotones.btnDescarga).click(function () {
            var headers = [];
            var valores = null;
            var data = $(lstTablas.tblfacturas).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Serie", "sType": "texto" },
                    { "sColum": "Folio", "sType": "texto" },
                    { "sColum": "UUID", "sType": "texto" },
                    { "sColum": "Fecha", "sType": "texto" },
                    { "sColum": "Forma de pago", "sType": "texto" },
                    { "sColum": "Metodo de pago", "sType": "texto" },
                    { "sColum": "Emisor", "sType": "texto" },
                    { "sColum": "Receptor", "sType": "texto" },
                    { "sColum": "Tipo comprobante", "sType": "texto" },
                    { "sColum": "Subtotal", "sType": "texto" },
                    { "sColum": "Total", "sType": "texto" },

                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {
                    var datarow = [
                        { "sValor": data[i].Serie },
                        { "sValor": data[i].Folio },
                        { "sValor": data[i].UUID },
                        { "sValor": data[i].Fecha },
                        { "sValor": data[i].FormaPago },
                        { "sValor": data[i].MetodoPago },
                        { "sValor": data[i].Emisor.Rfc },
                        { "sValor": data[i].Receptor.Rfc },
                        { "sValor": data[i].TipoDeComprobante },
                        { "sValor": data[i].SubTotal },
                        { "sValor": data[i].Total }

                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'listado_de_facturas_'";
                oGlob.m_ExportDataTable(pm);
            }
        });


        $(lstBotones.btnDescargaZIP).unbind();
        $(lstBotones.btnDescargaZIP).click(function () {
            var headers = [];
            var valores = null;
            var contenido = {
                lstHeader: headers,
                lstValues: valores
            };
            pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'listado_de_facturas_'";
            oGlob.m_ExportDataTable(pm, "datos_remesa");

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



    this.m_Extraeremesas = function (oParam) {
        $.ajax({
            type: "POST",
            url: Controller + "Extraeremesabebidas",
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
                        oGlob.m_AlertSuccess(response.mensaje);
                        $(lstElemnts.div_upload).html('<iframe id="iUploadEXCEL" class="custom_iframe" frameborder="0" scrolling="no" src="Formularios/UPLOAD/UploadFilesEXCEL.html"></iframe>');
                        oThis.m_CargaDataTable(response.data);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescargaZIP, false);
                        oGlob.m_CloseFilters();
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


    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblfacturas;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Serie", name: "Serie", type: "text", width: 80, align: "center" },
                { label: "Folio", name: "Folio", type: "text", width: 80, align: "center" },
                { label: "UUID", name: "UUID", type: "text", width: 280, align: "center" },
                { label: "Fecha", name: "Fecha", type: "text", width: 120, align: "center" },
                { label: "Forma de pago", name: "FormaPago", type: "text", width: 100, align: "center" },
                { label: "Metodo de pago", name: "MetodoPago", type: "text", width: 110, align: "center" },
                { label: "Emisor", name: "Emisor.Rfc", type: "text", width: 100, align: "center" },
                { label: "Receptor", name: "Receptor.Rfc", type: "text", width: 100, align: "center" },
                { label: "Tipo comprobante", name: "TipoDeComprobante", type: "text", width: 120, align: "center" },
                
                {
                    label: "Subtotal", name: "SubTotal", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                {
                    label: "Total", name: "Total", type: "number", width: 110, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                 

            ],
            caption: 'Registro de facturas para control de remesas',
            multiselect: false,
            viewrecords: true,
            rownumbers: true,
            height: "28em",
            rowNum: 50,
            pager: "#jqGridPager",

        });
        //$(jNameGrid).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    }
}