function ExtraccionZAP() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFechaInicio: "#txtFechainicio", txtFechaFin: "#txtFechafin", txtdiario: "#txtdiario", txtcuenta: "#txtcuenta",
            txtsucursal: "#txtsucursal", txtcentro: "#txtcentro", txtdepto: "#txtdepto", txtdocumento:"#txtdocumento"
        },
        lstCaja = {},
        lstElemnts = {},
        lstTablas = { tblDatos: "#tbl-datos-zap" },
        lstBotones = { btnDescarga: "#btnDescarga",   btnSerch: "#btnBusca" },
        Controller = "Metodos/REPORTES/D365/Extaccion-ZAP-Datos.aspx/";

    this.m_InicializaEvents = function () {


        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oGlob.m_CargaDetePicker(lstFiltros.txtFechaInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFechaFin);

        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });


        $(lstBotones.btnDescarga).unbind();
        $(lstBotones.btnDescarga).click(function () {
            var headers = [];
            var valores = [];
            //var data = $(lstTablas.tblDatos).jqGrid("getGridParam", "data");
            //if (data != null && data.length > 0) {
            //    headers = [
            //        { "sColum": "FECHA", "sType": "texto" },
            //        { "sColum": "DIARIO", "sType": "texto" },
            //        { "sColum": "RECID", "sType": "texto" },
            //        { "sColum": "DIMENSIONES", "sType": "texto" },                    
            //        { "sColum": "CUENTA", "sType": "texto" },
            //        { "sColum": "SUCURSAL", "sType": "texto" },
            //        { "sColum": "FILIAL", "sType": "texto" },
            //        { "sColum": "CENTRO DE COSTO", "sType": "texto" },
            //        { "sColum": "DEPARTAMENTO", "sType": "texto" },
            //        { "sColum": "DESCRIPCIÓN", "sType": "texto" },
            //        { "sColum": "VEHICULO", "sType": "texto" },
            //        { "sColum": "DEBITO", "sType": "texto" },
            //        { "sColum": "CREDITO", "sType": "texto" }


            //    ];

            //    valores = [data.length];
            //    for (var i = 0; i < data.length; i++) {
            //        var datarow = [
            //            { "sValor": data[i].Fecha },
            //            { "sValor": data[i].documento },
            //            { "sValor": data[i].RecID },
            //            { "sValor": data[i].Display },
            //            { "sValor": data[i].cuenta },
            //            { "sValor": data[i].sucursal },
            //            { "sValor": data[i].filial },
            //            { "sValor": data[i].centro },
            //            { "sValor": data[i].depto },
            //            { "sValor": data[i].text },
            //            { "sValor": data[i].vehiculo },
            //            { "sValor": data[i].debito },
            //            { "sValor": data[i].credito } 
            //        ];
            //        valores[i] = datarow;
            //    }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
            pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'informe_de_cuentas_saldos'";
            let oparms = "&fecha_inicio='" + $(lstFiltros.txtFechaInicio).val() + "'&fecha_final='" + $(lstFiltros.txtFechaFin).val() + "'&diario=''&documento=''&cuenta='" + $(lstFiltros.txtcuenta).val() + "'&sucursal='" + $(lstFiltros.txtsucursal).val() + "'&centro='" + $(lstFiltros.txtcentro).val() + "'&depto='" + $(lstFiltros.txtdepto).val()+"'";
            oGlob.m_ExportDataTable(pm, "reportecstm", oparms);
            
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

        if ($(lstFiltros.txtFechaInicio).val() == "" || $(lstFiltros.txtFechaInicio).val() == "") {
            oGlob.m_AlertWarning('Favor de seleccionar el período a filtrar');
        } else {
            //crea objeto y enviar a ajax
            var oPeriodo = {                
                fecha_inicio: $(lstFiltros.txtFechaInicio).val(),
                fecha_final: $(lstFiltros.txtFechaFin).val(),
                diario: $(lstFiltros.txtdiario).val(),
                documento: $(lstFiltros.txtdocumento).val(),
                cuenta: $(lstFiltros.txtcuenta).val(),
                sucursal: $(lstFiltros.txtsucursal).val(),
                centro: $(lstFiltros.txtcentro).val(),
                depto: $(lstFiltros.txtdepto).val()
            };
            pm = "Periodo: " + JSON.stringify(oPeriodo);
            oThis.m_Cargadatoscuentas(pm);
            oGlob.m_CloseFilters();
        }
    }

    this.m_Cargadatoscuentas = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "Extraecuentaszap",
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
                        //oGlob.m_AlertWarning('No se ha encontrado información con los filtros proporcionados');
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

    this.m_CargaDataTable = function (jdataJSON) {


        var jNameGrid = lstTablas.tblDatos;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Fecha registrado", name: "Fecha", type: "text", width: 150 },
                { label: "Número del diario", name: "documento", type: "text", width: 100, hidden:true },
                { label: "REC ID", name: "RecID", type: "text", width: 110 , hidden:true},                
                { label: "Dimensiones", name: "Display", type: "text", width: 250 },
                { label: "Cuenta contable", name: "cuenta", type: "text", width: 150 },
                { label: "Sucursal", name: "sucursal", type: "text", width: 150 },
                { label: "Filial", name: "filial", type: "text", width: 130 },
                { label: "Centro de costo", name: "centro", type: "text", width: 200 },
                { label: "Departamento", name: "depto", type: "text", width: 130 },
                { label: "Descripción", name: "text", type: "text", width: 170 },
                { label: "Vehículo", name: "vehiculo", type: "text", width: 130 },

                {
                    label: "Total", name: "debito", type: "number", width: 150, formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' },
                },
                {
                    label: "Crédito", name: "credito", type: "number", width: 150, formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' },
                    hidden:true
                },

            ],
            viewrecords: true,
            caption: 'Informe de cuentas Dynamics 365',
            height: "25em",
            rowNum: 30,
            gridComplete: function () { },
            pager: "#jqGridPager"
        });
    }
}