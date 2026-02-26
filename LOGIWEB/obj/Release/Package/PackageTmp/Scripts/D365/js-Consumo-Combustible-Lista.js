function Cargascombustible() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txteconomico: "#txteconomico", txtcorporativo: "#txtcorporativo",  
            cmbestacion: "#cmbalmacen", cmbestatus: "#cmbestatus", txtfechainicio: "#txtFechacrinicio", txtfechafin: "#txtFechacrfin",
            txtpoliza: "#txtpoliza"
        },
        lstCaja = { txtFechadocto:"#txtFechadocto"},
        lstElemnts = { div_upload: "#uploadexcelitem", iFramupload: "#iUploadEXCEL" },
        lstmodales = { cargamodal: "#modal_carga_inicial" }
    lstTablas = { tblcombustible: "#tbl-lista-combustibles" },
        lstBotones = {
            btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga", btnAdd: "#btnAdd",
        bntCargadatos: "#bntCargadatos", btnPoliza: "#btnPoliza", btnGraba:"#btnAceptapoliza"
        },
        Controller = "../Metodos/D365/PolizaContable-Datos.aspx/";
    var ListadoCargas = null;

    this.m_InicializaEvents = function () {

        oGlob.m_CargaDetePicker(lstFiltros.txtfechainicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtfechafin);
        oGlob.m_CargaDetePicker(lstCaja.txtFechadocto);
        oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConFin);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreFin);
        oThis.m_CargaAlmacenes(lstFiltros.cmbestacion,false);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnPoliza, true);


 
        $("#txtPreciocombus").keyup(function () {

            if (!isNaN(parseFloat($(this).val()))) {
                let litros = $("#txtLitros").val();
                let precioxlitro = $(this).val();
                let precio_combus = parseFloat(parseFloat(litros) * parseFloat(precioxlitro)).toFixed(4);
                $("#txtImportetot").val(precio_combus);
            }
        });

        $(lstBotones.btnPoliza).unbind();
        $(lstBotones.btnPoliza).click(function () {

            var lstCargas = $(lstTablas.tblcombustible).find("tr.jqgrow:has(td > input.cbox:checked)");
            ListadoCargas = new Array(0);
            if (lstCargas == null || lstCargas == undefined)
                oGlob.m_AlertWarning('No se ha detectado cargas de combustible seleccionadas para agregar a la póliza.');
            
            else {

                if (lstCargas != null && lstCargas.length > 0) {

                    let litros_total = 0; let importe_total = 0; let precio_combus = 0; let bConinua = true;
                    for (var i = 0; i < lstCargas.length; i++) {

                        var row = $(lstTablas.tblcombustible).jqGrid("getRowData", lstCargas[i].id);
                        if (row.poliza != "" || row.foliod365 != "") {
                            bConinua = false;
                            break;
                        }
                        if (row.estatus == 1) {
                            ListadoCargas.push(row);
                            litros_total += parseFloat(row.litros);
                            importe_total += parseFloat(row.total); 
                        }
 
                    }

                    if (bConinua) {
                        let cadena = 'Se configurará la póliza con <b>' + lstCargas.length + '</b> gasto(s). Se presentará el total de cargas para realizar el promedio de costo por litro. ¿Estás seguro de continuar?';

                        oGlob.m_Confirma_Accion(cadena, function (response) {
                            if (response) {
                                precio_combus = parseFloat(parseFloat(importe_total) / parseFloat(litros_total)).toFixed(4);
                                $('#modalimportes').modal('show');
                                $("#txtEstaciont").val($(lstFiltros.cmbestacion + " option:selected").text());
                                $("#txtNum").val(lstCargas.length);
                                $("#txtLitros").val(parseFloat(litros_total).toFixed(4));
                                $("#txtPreciocombus").val(precio_combus);
                                $("#hddPrecio").val(precio_combus);
                                $("#txtImportetot").val(parseFloat(importe_total).toFixed(4));
                            }
                        });
                    } else oGlob.m_AlertWarning('Se ha detectado uno o más registros de consumo de combustible con una poliza contable asociada en D365.');


                } else oGlob.m_AlertWarning('No se ha detectado cargas de combustible seleccionadas para agregar a la póliza.');
            }
             
        });
        

        $(lstBotones.btnGraba).unbind();
        $(lstBotones.btnGraba).click(function () {


            let precio_captura = $("#txtPreciocombus").val();
            let precio_original = $("#hddPrecio").val();
            if ($(lstCaja.txtFechadocto).val() != "") {

                if (!isNaN(parseFloat(precio_captura))) {

                    let cadena = '';

                    if ($("#txtPreciocombus").val() != $("#hddPrecio").val())
                        cadena = 'El precio del combustible ha cambiado de <b>' + precio_original + '</b> a <b>' + precio_captura + '</b>. Este precio por litro será asignado a las cargas seleccionadas, esto cambiará el importe total. Una vez grabado este movimiento no podra ser cancelado ¿Desea continuar?'
                    else
                        cadena = 'Se procesaran las cargas de combustibles con el precio fijado a <b>' + precio_original + '</b>.Una vez grabado este movimiento no podra ser cancelado ¿Desea continuar?';
                    oGlob.m_Confirma_Accion(cadena, function (response) {
                        if (response) {

                            var oParametro = {
                                FechaInicio: $(lstFiltros.txtfechainicio).val(),
                                FechaFinal: $(lstFiltros.txtfechafin).val(),
                                economico: $(lstFiltros.txteconomico).val(),
                                vehiculo: $(lstFiltros.txtcorporativo).val(),
                                estacionclave: $(lstFiltros.cmbestacion).val(),
                                estacion: $(lstFiltros.cmbestacion + " option:selected").text(),
                                estatus: $(lstFiltros.cmbestatus).val(),
                                preciolitro: precio_captura,
                                FechaDocto: $(lstCaja.txtFechadocto).val()
                            };
                            pm = "lstCargascheck: " + JSON.stringify(ListadoCargas) + ", oParam: " + JSON.stringify(oParametro);
                            oThis.m_Creapolizacombustible(pm);
                        }
                    });
                } else oGlob.m_AlertWarning('Favor de establecer el precio de combustible por litro.');
            } else oGlob.m_AlertWarning('Favor de asignar la fecha de documento para el grabado en D365.');

         });

         
        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {

            if ($(lstFiltros.txtfechainicio).val() != "" && $(lstFiltros.txtfechafin).val() != "") {

                if ($(lstFiltros.cmbestacion).val() != "") {

                    oThis.m_EventoBusqueda();

                } else oGlob.m_AlertWarning('Favor de establecer el almacén con el que estará trabajando.');

            } else oGlob.m_AlertWarning('Favor de establecer el rango de fechas.');

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
            var data = $(lstTablas.tblcombustible).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Estatus", "sType": "texto" },
                    { "sColum": "Comentarios", "sType": "texto" },
                    { "sColum": "Fecha liquidacion", "sType": "texto" },
                    { "sColum": "Estación", "sType": "texto" },
                    { "sColum": "Folio", "sType": "texto" },
                    { "sColum": "Viaje", "sType": "texto" },
                    { "sColum": "Litros", "sType": "texto" },
                    { "sColum": "Precio x litro", "sType": "texto" },
                    { "sColum": "Total", "sType": "texto" },
                    { "sColum": "# Corporativo", "sType": "texto" },
                    { "sColum": "# Economico", "sType": "texto" },
                    { "sColum": "Sucursal", "sType": "texto" },
                    { "sColum": "Centro costo", "sType": "texto" },
                    { "sColum": "Departamento", "sType": "texto" },
                    { "sColum": "Area", "sType": "texto" },
                    { "sColum": "Filial", "sType": "texto" },
                    { "sColum": "# Operador", "sType": "texto" },
                    { "sColum": "Operador", "sType": "texto" },
                    { "sColum": "Poliza", "sType": "texto" } 

                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {
                    var TipoDocumento = "";
                    switch (data[i].estatus) {
                        case 1:
                            TipoDocumento = "DISPONIBLE";
                            break;
                        case 2:
                            TipoDocumento = "SINCRONIZADO";
                            break; 
                    }
                    var datarow = [
                        { "sValor": TipoDocumento },
                        { "sValor": data[i].comentarios },
                        { "sValor": data[i].sFechaLiquidacion },
                        { "sValor": data[i].estacion },
                        { "sValor": data[i].folio },
                        { "sValor": data[i].viaje },
                        { "sValor": data[i].litros },
                        { "sValor": data[i].preciolitro },
                        { "sValor": data[i].total },
                        { "sValor": data[i].vehiculo },
                        { "sValor": data[i].economico },
                        { "sValor": data[i].sucursal },
                        { "sValor": data[i].centro },
                        { "sValor": data[i].depto },
                        { "sValor": data[i].area },
                        { "sValor": data[i].filial },
                        { "sValor": data[i].codigooper },
                        { "sValor": data[i].operador },
                        { "sValor": data[i].poliza }
                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'listado_de_consumo_de_combustible_interno'";
                oGlob.m_ExportDataTable(pm);
            }
        });
    }

    this.Desglosaconsumo = function () {

        var lstCargas = $(lstTablas.tblcombustible).find("tr.jqgrow:has(td > input.cbox:checked)");
        ListadoCargas = new Array(0);
        if (lstCargas != null || lstCargas != undefined) {

            if (lstCargas != null && lstCargas.length > 0) {

                let litros_total = 0; let importe_total = 0; let precio_combus = 0; let bConinua = true;
                for (var i = 0; i < lstCargas.length; i++) {

                    var row = $(lstTablas.tblcombustible).jqGrid("getRowData", lstCargas[i].id);
                    if (row.poliza != "" || row.foliod365 != "") {
                        continue;
                    }
                    if (row.estatus == 1) {
                        ListadoCargas.push(row);
                        litros_total += parseFloat(row.litros);
                        importe_total += parseFloat(row.total);
                    }

                }

                if (bConinua) {

                    precio_combus = parseFloat(parseFloat(importe_total) / parseFloat(litros_total)).toFixed(4);
                    $("#txtnum_page").val(lstCargas.length);
                    $("#txlitros_page").val(parseFloat(litros_total).toFixed(4));
                    $("#txtprecio_page").val(precio_combus);
                    $("#txtotal_page").val(parseFloat(importe_total).toFixed(4));
                }
            }
        }
    }

    this.m_CargaAlmacenes = function (jInput, bMultiple) { 

        $.ajax({
            type: "POST",
            url: Controller +"/Listaalmacenescombus",
            data: "{ Documento: {}}",
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
                if (!bMultiple) {
                    opcion = $("<option />", {
                        value: "",
                        text: "Selecciona el almacén de consumo"
                    });
                    select.append(opcion);
                }
                for (var i = 0; i < response.data.length; i++) {

                    opcion = $("<option />", {
                        value: response.data[i].cuentatms,
                        text: response.data[i].cuentad365 + "-" + response.data[i].Concepto + "-" + response.data[i].almacen365 + "-" + response.data[i].sucursal365
                    });
                    select.append(opcion);
                }
                if (bMultiple)
                    select.multipleSelect("refresh");

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }

    this.m_EventoBusqueda = function () {
        var oParametro = {
            FechaInicio: $(lstFiltros.txtfechainicio).val(),
            FechaFinal: $(lstFiltros.txtfechafin).val(),
            economico: $(lstFiltros.txteconomico).val(),
            vehiculo: $(lstFiltros.txtcorporativo).val(),
            estacionclave: $(lstFiltros.cmbestacion).val(),
            estatus: $(lstFiltros.cmbestatus).val(),
            poliza: $(lstFiltros.txtpoliza).val()            
        };
        pm = "oParam: " + JSON.stringify(oParametro);
        oThis.m_ListaCombustible(pm);
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

    this.m_ListaCombustible = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListaConsumosliquidaciones",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnPoliza, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblcombustible); 

            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnPoliza, false);
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
                oGlob.m_HideloaderGrid(lstTablas.tblcombustible);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblcombustible);
            }
        });
    }


    this.m_Creapolizacombustible = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ProcesapolizaCombustible",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnPoliza, true);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        $('#modalimportes').modal('hide');
                        oGlob.m_AlertSuccess(response.mensaje);                        
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnPoliza, false);
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
                oGlob.m_HideloaderGrid(lstTablas.tblcombustible);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblcombustible);
            }
        });
    }


    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblcombustible;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: "Estatus", name: "rEdit", type: "text", width: 80, align: "center" },
                { label: "Comentarios", name: "comentarios", type: "text", width: 300, align: "center" },
                { label: "Fecha liq.", name: "sFechaLiquidacion", type: "text", width: 110, align: "center" },
                { label: "Estación", name: "estacion", type: "text", width: 150, align: "left" },
                { label: "Folio", name: "folio", type: "text", width: 80, align: "center" },


                { label: "Viaje", name: "viaje", type: "text", width: 120, align: "center" },

                { label: "Litros", name: "litros", type: "text", width: 80, align: "center" },
                { label: "Precio x litro", name: "preciolitro", type: "text", width: 100, align: "center" },
                { label: "Total", name: "total", type: "text", width: 100, align: "center" }, 

                { label: "# Corporativo", name: "vehiculo", type: "text", width: 100, align: "center" },
                { label: "# Economico", name: "economico", type: "text", width: 100, align: "center" },

                { label: "Sucursal", name: "sucursal", type: "text", width: 90, align: "center" },
                { label: "Centro costo", name: "centro", type: "text", width: 100, align: "center" },
                { label: "Departamento", name: "depto", type: "text", width: 100, align: "center" },
                { label: "Area", name: "area", type: "text", width: 100, align: "center" },
                { label: "Filial", name: "filial", type: "text", width: 90, align: "center" },
                { label: "Fecha sincronizado", name: "sFecharegistro", type: "text", width: 110, align: "center" },
                { label: "# Operador", name: "codigooper", type: "text", width: 100, align: "center" },
                { label: "Operador", name: "operador", type: "text", width: 250, align: "left" },
                
                { label: "Poliza", name: "poliza", type: "text", width: 130, align: "center" },
                { label: "RECID", name: "foliod365", type: "text", width: 100, align: "center" },
                { label: "bitacoraID", name: "bitacoraID", type: "text", width: 250, align: "center", hidden: true },
                { label: "estatus", name: "estatus", type: "text", width: 250, align: "center", hidden: true},

            ],
            caption: 'Consumo de combustible en estación propia',
            multiselect: true,
            viewrecords: true,
            rownumbers: true,
            height: "28em",
            rowNum: 50,
            gridComplete: function () {

                let keys = $(jNameGrid).jqGrid('getDataIDs');
                for (let i = 0; i < keys.length; i++) {
                    let CssButton = "<span class='success'><i class='fa fa-circle fa-2x success' style='color: green !important'></i></span>";
                    let key = keys[i];
                    let row = $(jNameGrid).jqGrid('getRowData', key);
                    if ( row.estatus == 2) {
                        CssButton = "<span class='danger'><i class='fa fa-circle fa-2x danger' style='color:red !important'></i></span>";
                    }
                    var rowEstatus = CssButton;
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEstatus  });
                }

                var currentPage = $(this).getGridParam('page').toString();

                //retrieve any previously stored rows for this page and re-select them
                var retrieveSelectedRows = $(this).data(currentPage);
                if (retrieveSelectedRows) {
                    $.each(retrieveSelectedRows, function (index, value) {
                        $(lstTablas.tblcombustible).setSelection(value, false);
                    });
                }

            },
            onSelectRow: function (id) {
                oCombuslista.Desglosaconsumo();
            },
            onPaging: function (a) {
                var pagerId = this.p.pager.substr(1); // ger paper id like "pager" 		            
                var pageValue = $('input.ui-pg-input', "#pg_" + $.jgrid.jqID(pagerId)).val();
                var saveSelectedRows = $(this).getGridParam('selarrrow'); 		            //Store any selected rows 		            
                $(this).data(pageValue.toString(), saveSelectedRows);
            },
            pager: "#jqGridPager",

        });
        //$(jNameGrid).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    }
}