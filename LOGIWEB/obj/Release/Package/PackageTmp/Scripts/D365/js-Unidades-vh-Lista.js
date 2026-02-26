function Listamaestra() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txteconomico: "#txteconomico", txtcorporativo: "#txtcorporativo",  
            cmbsucursal: "#cmbsucursal", cmbcentro: "#cmbcentro", cmbdepto: "#cmbdepto", cmbestatus:"#cmbestatus"
        },
        lstCaja = {},
        lstElemnts = { div_upload: "#uploadexcelitem", iFramupload: "#iUploadEXCEL" },
        lstmodales = { cargamodal: "#modal_carga_inicial" }
    lstTablas = { tblListamaestra: "#tbl-lista-maestra" },
        lstBotones = {
            btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga", btnAdd: "#btnAdd",
            bntCargadatos: "#bntCargadatos"
        },
        Controller = "../Metodos/D365/PolizaContable-Datos.aspx/";

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

            $('#modalfiltro').modal('show');
            $("#modalfiltro").on('shown.bs.modal', function () {
                oThis.ListaCatalogosEquivalencias(lstFiltros.cmbsucursal, oParams, "Listsucursalesactivas", function (status) {

                    if (status) {
                        $(lstFiltros.cmbsucursal).chosen();
                    }
                });

                oThis.ListaCatalogosEquivalencias(lstFiltros.cmbcentro, oParams, "Listacentroscostoactivos", function (status) {

                    if (status) {
                        $(lstFiltros.cmbcentro).chosen();
                    }
                });


                oThis.ListaCatalogosEquivalencias(lstFiltros.cmbdepto, oParams, "Listadepartamentosactivos", function (status) {

                    if (status) {
                        $(lstFiltros.cmbdepto).chosen();
                    }
                });

            });

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
            var data = $(lstTablas.tblListamaestra).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Estatus", "sType": "texto" },
                    { "sColum": "N° Corporativo", "sType": "texto" },
                    { "sColum": "N° Economico", "sType": "texto" },
                    { "sColum": "Tipo", "sType": "texto" },
                    { "sColum": "Marca", "sType": "texto" },
                    { "sColum": "Descripción", "sType": "texto" },
                    { "sColum": "Sucursal OPE", "sType": "texto" },
                    { "sColum": "Sucursal D365+OPE", "sType": "texto" },
                    { "sColum": "Centro costo OPE", "sType": "texto" },
                    { "sColum": "Centro costo D365+OPE", "sType": "texto" },

                    { "sColum": "Departamento OPE", "sType": "texto" },
                    { "sColum": "Departamento D365+OPE", "sType": "texto" },
                    { "sColum": "Año", "sType": "texto" },
                    { "sColum": "Serie", "sType": "texto" },
                    { "sColum": "N° Motor", "sType": "texto" },
                    { "sColum": "Tipo vehículo", "sType": "texto" },
                    { "sColum": "Placa", "sType": "texto" },
                    { "sColum": "Responsable", "sType": "texto" }

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
                        { "sValor": data[i].nombreestatus },
                        { "sValor": data[i].corporativo },
                        { "sValor": data[i].economico },
                        { "sValor": data[i].tipotracto },
                        { "sValor": data[i].marca },
                        { "sValor": data[i].descripcion },
                        { "sValor": data[i].suc },
                        { "sValor": data[i].sucursal },
                        { "sValor": data[i].ccosto },
                        { "sValor": data[i].centrocosto },
                        { "sValor": data[i].depto },
                        { "sValor": data[i].departamento },
                        { "sValor": data[i].anio },
                        { "sValor": data[i].serie },
                        { "sValor": data[i].nmotor },
                        { "sValor": data[i].tipovehiculo },
                        { "sValor": data[i].placa },
                        { "sValor": data[i].responsable }
                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'lista_maestra_OPEFICHAS_DYNAMICS_365'";
                oGlob.m_ExportDataTable(pm);
            }
        });
    }
    this.m_EventoBusqueda = function () {
        var oParametro = {
            filtro_economico: $(lstFiltros.txteconomico).val(),
            filtro_corporativo: $(lstFiltros.txtcorporativo).val(),
            filtro_centro: $(lstFiltros.cmbcentro).val(),
            filtro_sucursal: $(lstFiltros.cmbsucursal).val(),
            filtro_depto: $(lstFiltros.cmbdepto).val(),
            filtro_estatus: $(lstFiltros.cmbestatus).val()
        };
        pm = "oParam: " + JSON.stringify(oParametro);
        oThis.m_Listaunidadesmaestra(pm);
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

    this.ListaCatalogosEquivalencias = function (jInput, oParams, metodo, callback) {

        $.ajax({
            type: "POST",
            url: "../Metodos/D365/Configuracion-Datos.aspx/" + metodo,
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {

            },
            success: function (datos) {
                var response = datos["d"];
                var opcion = "";
                var select = $(jInput);
                select[0].options.length = 0;
                opcion = $("<option />", {
                    value: "",
                    text: "Selecciona una opción"
                });
                select.append(opcion);

                for (var i = 0; i < response.data.length; i++) {

                    opcion = $("<option />", {
                        value: response.data[i].iSubcuenta,
                        text: response.data[i].sAX365 + " - " + response.data[i].sDescripcion
                    });
                    select.append(opcion);
                }
                callback(true);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                callback(false);
            }
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

    this.m_Listaunidadesmaestra = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "Listamaestraunidades",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblListamaestra);
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
                oGlob.m_HideloaderGrid(lstTablas.tblListamaestra);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblListamaestra);
            }
        });
    }
    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblListamaestra;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Estatus", name: "rEdit", type: "text", width: 80, align: "center" },
                { label: ".", name: "rView", type: "text", width: 50, align: "center" },

                { label: "N° Corporativo", name: "corporativo", type: "text", width: 110, align: "center" },
                { label: "N° Economico", name: "economico", type: "text", width: 110, align: "center" },
                { label: "Tipo", name: "tipotracto", type: "text", width: 100, align: "left" },
                { label: "Marca", name: "marca", type: "text", width: 150, align: "left" },
                { label: "Descripción", name: "descripcion", type: "text", width: 120, align: "center" },

                { label: "Sucursal OPE", name: "suc", type: "text", width: 100, align: "center" },
                { label: "Sucursal D365+OPE", name: "sucursal", type: "text", width: 150, align: "center" },
                { label: "Centro costo OPE", name: "ccosto", type: "text", width: 110, align: "center" },
                { label: "Centro costo D365+OPE", name: "centrocosto", type: "text", width: 170, align: "center" },
                { label: "Departamento OPE", name: "depto", type: "text", width: 110, align: "center" },
                { label: "Departamento D365+OPE", name: "departamento", type: "text", width: 170, align: "center" },


                { label: "Año", name: "anio", type: "text", width: 80, align: "center" },
                { label: "Serie", name: "serie", type: "text", width: 200, align: "center" },
                { label: "N° Motor", name: "nmotor", type: "text", width: 110, align: "center" },
                { label: "Tipo vehículo", name: "tipovehiculo", type: "text", width: 100, align: "center" },
                { label: "Placa", name: "placa", type: "text", width: 100, align: "center" },
                { label: "Responsable", name: "responsable", type: "text", width: 250, align: "center" },
                { label: "estatus", name: "estatus", type: "text", width: 100, align: "center", hidden: true },
                { label: "nombreestatus", name: "nombreestatus", type: "text", width: 100, align: "center", hidden: true },

            ],
            caption: 'Listado de unidades registradas',
            multiselect: false,
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {

                let keys = $(jNameGrid).jqGrid('getDataIDs');
                let CssButton = "<span class='success'><i class='fa fa-circle fa-2x success' style='color: green !important'></i></span>";
                for (let i = 0; i < keys.length; i++) {
                    let key = keys[i];
                    let row = $(jNameGrid).jqGrid('getRowData', key);
                    if (row.estatus == "2") {
                        CssButton = "<span class='danger'><i class='fa fa-circle fa-2x danger' style='color:red !important'></i></span>";
                    }
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" title=\"Ver detalle unidad\" onClick=\"oIncidencia.VisualizaDocto(this, " + key + ")\"><i class=\"fa fa-file \" ></i></button>";
                    var rowEstatus = CssButton;
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEstatus, rView: rowEdit });
                }

            },
            pager: "#jqGridPager"
        });
    }
}