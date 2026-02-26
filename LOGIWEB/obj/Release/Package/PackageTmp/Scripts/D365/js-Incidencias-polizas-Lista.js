function Incidencias() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFConInicio: "#txtFechacinicio", txtFConFin: "#txtFechacfin", txtFCreInicio: "#txtFechacrinicio", txtFCreFin: "#txtFechacrfin",
            cmbdoc: "#cmbdocs", txtfolioserie: "#txtfolserie", txtFolio: "#txtFolio", cmbprocdoc: "#cmbprocdocs", txtFechaProcesa: "#txtFechaProcesa"
        },
        lstCaja = {},
        lstElemnts = { div_upload: "#uploadxmlitem", iFramupload: "#iUploadXML", div_copiado: "#div_copiado", classequiv: ".classequiv", msgequiv: "#mensajeequiv", elTotal: "#total" },
        lstTablas = { tblpolizasIncidencias: "#tbl-incidencias" },
        lstmodales = { modBug: "#modalbug", modXML: "#div_modal_xml", modalReprocesa: "#modalReprocesa" }
    lstBotones = {
        btnSerch: "#btnBusca", btnFiltra: "#btnFiltro", btnDescarga: "#btnDescarga", btnAddcnf: "#btnAddcnf", btnReinicia: "#btnReincio", btnReprocesa: "#btnReproceso", btnInsertaPeticion: "#btnInsertaPeticion",
        tabSend: "#tab_send", bntUpload: "#bntUpload", btnElimina: "#btnElimina", btnDowdetalle: "#btnDowdetalle", bntUploadViatico: "#bntUploadViatico"
    },
        Controller = "../Metodos/D365/PolizaContable-Datos.aspx/", upload_xml_id = null, upload_xml_linea = null, upload_token = null, marca_poliza = true, detalle_valido = true;

    this.m_InicializaEvents = function () {

        oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFConFin);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreInicio);
        oGlob.m_CargaDetePicker(lstFiltros.txtFCreFin);
        oGlob.m_CargaDetePicker(lstFiltros.txtFechaProcesa);
        oGlob.m_CargaDocumentos(lstFiltros.cmbdoc, true);
        oGlob.m_CargaDocumentosProcesa(lstFiltros.cmbprocdoc, false);
        oGlob.m_OcultaElemnto(lstElemnts.div_copiado, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnReinicia, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnReprocesa, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnElimina, true);
        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, true);
        oThis.m_EventoBusqueda();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {
            oGlob.m_AbremodalFiltro();

        });

        this.m_OnCopyObject = function (parent) {

            oGlob.m_OcultaElemnto(lstElemnts.div_copiado, false);
            setTimeout(function () {
                $(lstElemnts.div_copiado).hide(1200);
            }, 1500);
            const output = $(parent).find('output');
            const selection = window.getSelection();
            const range = document.createRange();
            range.selectNode(output[0]);
            selection.empty();
            selection.addRange(range);
            document.execCommand('copy');

        }

        $(lstBotones.btnElimina).unbind();
        $(lstBotones.btnElimina).click(function () {
            var lstChecks = $(lstTablas.tblpolizasIncidencias).jqGrid('getGridParam', 'selarrrow');
            var lstPolizasChks = new Array(0);
            if (lstChecks != null && lstChecks.length > 0) {

                oGlob.m_Confirma_Accion('Se ha detectado la selección de <b>' + lstChecks.length + ' póliza(s)</b> para su descarte. Una vez eliminadas estas no pueden ponerse nuevamente en transición hacia D365.¿Deseas continuar?', function (response) {
                    if (response) {

                        for (var i = 0; i < lstChecks.length; i++) {
                            var row = $(lstTablas.tblpolizasIncidencias).jqGrid('getRowData', lstChecks[i]);
                            lstPolizasChks.push({
                                FolioAsiento: row.FolioAsiento
                            });
                        }
                        pm = "lstPolizas: " + JSON.stringify(lstPolizasChks) + "";
                        oThis.m_DescartaPolizas(pm);
                    }
                });
            } else oGlob.m_AlertWarning('No se ha detectado la selección de póliza(s) para ser descartadas. Favor de reintentarlo');


        });

        $(lstBotones.bntUpload).unbind();
        $(lstBotones.bntUpload).click(function () {

            var sPathupload = $(lstElemnts.iFramupload).contents().find('#txthhName').val();
            var sNombreFile = sPathupload === undefined ? "" : sPathupload;
            if (sNombreFile == "")
                oGlob.m_AlertWarning('El XML no se encuentra adjunto, para realizar la acción primero debe cargar un archivo XML');
            else {
                oGlob.m_Confirma_Accion('Se ha detectado un nuevo XML, este archivo reemplazará al cargado previamente. Recuerda que los importes deben cuadrar con los datos del registro.¿Deseas continuar?', function (response) {
                    if (response) {

                        pm = "polizaID: '" + upload_xml_id + "', sArchivo:'" + sNombreFile + "'";
                        oThis.m_CargaNuevoXML(pm);
                    }
                });
            }

        });

        $(lstBotones.bntUploadViatico).unbind();
        $(lstBotones.bntUploadViatico).click(function () {

            var sPathupload = $(lstElemnts.iFramupload).contents().find('#txthhName').val();
            var sNombreFile = sPathupload === undefined ? "" : sPathupload;
            if (sNombreFile == "")
                oGlob.m_AlertWarning('El XML no se encuentra adjunto, para realizar la acción primero debe cargar un archivo XML');
            else {
                oGlob.m_Confirma_Accion('Se ha detectado un nuevo comprobante de viaticos, este archivo reemplazará al cargado previamente. Recuerda que los importes deben cuadrar con los datos del registro.¿Deseas continuar?', function (response) {
                    if (response) {

                        pm = "polizaID: '" + upload_xml_id + "', Linea: " + upload_xml_linea + ",sTokendocto:'" + upload_token + "', sArchivo:'" + sNombreFile + "'";
                        oThis.m_CargaXMLViaticoLinea(pm);
                    }
                });
            }

        });

        $(lstBotones.btnReinicia).unbind();
        $(lstBotones.btnReinicia).click(function () {
            var lstChecks = $(lstTablas.tblpolizasIncidencias).jqGrid('getGridParam', 'selarrrow');
            var lstPolizasChks = new Array(0);
            if (lstChecks != null && lstChecks.length > 0) {

                oGlob.m_Confirma_Accion('Se ha detectado la selección de <b>' + lstChecks.length + ' póliza(s)</b> para reenvío a D365. Favor de validar que los errores generados hayan sido previamente solucionados.¿Deseas continuar?', function (response) {
                    if (response) {

                        for (var i = 0; i < lstChecks.length; i++) {
                            var row = $(lstTablas.tblpolizasIncidencias).jqGrid('getRowData', lstChecks[i]);
                            lstPolizasChks.push({
                                FolioAsiento: row.FolioAsiento
                            });
                        }
                        pm = "lstPolizas: " + JSON.stringify(lstPolizasChks) + "";
                        oThis.m_ReiniciaErrores(pm);
                    }
                });
            } else oGlob.m_AlertWarning('No se ha detectado la selección de póliza(s) para establecer el reenvío hacia D365. Favor de reintentarlo');

        });

        this.m_InsertaPeticion = function (oParams) {
            $.ajax({
                type: "POST",
                url: Controller + "InsertaPeticion",
                data: "{" + oParams + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                beforeSend: function () {
                    oGlob.m_showLoader();
                    oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                    oGlob.m_DeshabilitaElemento(lstBotones.btnReinicia, true);
                    oGlob.m_DeshabilitaElemento(lstBotones.btnReprocesa, true);
                    oGlob.m_DeshabilitaElemento(lstBotones.btnElimina, true);
                },
                success: function (result) {
                    var response = result["d"];
                    switch (response.estatus) {
                        case "OK":
                            oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                            oGlob.m_DeshabilitaElemento(lstBotones.btnReinicia, false);
                            oGlob.m_DeshabilitaElemento(lstBotones.btnReprocesa, false);
                            oGlob.m_DeshabilitaElemento(lstBotones.btnElimina, false);
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

        this.m_EventoReprocesa = function () {
            var oDocumento = {
                Proceso: $(lstFiltros.cmbprocdoc).val(),
                FechaInicial: $(lstFiltros.txtFechaProcesa).val(),
                Usuario: ""
            };
            pm = "Peticion: " + JSON.stringify(oDocumento);
            oThis.m_InsertaPeticion(pm);
        }

        $(lstBotones.btnInsertaPeticion).unbind();
        $(lstBotones.btnInsertaPeticion).click(function () {

            oGlob.m_Confirma_Accion('Se programará el procesamiento ' + '' + '¿Deseas continuar?', function (response) {
                if (response) {
                    oThis.m_EventoReprocesa();
                    oGlob.m_AlertSuccess('¡Procesamiento programado!');
                    $(lstmodales.modalReprocesa).modal('hide');
                }
            });

        });

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

        this.VisulizaReprocesa = function () {
            oGlob.m_AbreModal(lstmodales.modalReprocesa, function (status) {
            });
        }

        $(lstBotones.btnReprocesa).unbind();
        $(lstBotones.btnReprocesa).click(function () {
            oThis.VisulizaReprocesa();
        });

        $(lstBotones.btnDescarga).unbind();
        $(lstBotones.btnDescarga).click(function () {
            var headers = [];
            var valores = null;
            var data = $(lstTablas.tblpolizasIncidencias).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Tipo documento", "sType": "texto" },
                    { "sColum": "Folio GM", "sType": "texto" },
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
                        { "sValor": data[i].FolioAsiento },
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
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'polizas_con_incidencias'";
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
            estatus: 1,
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
                oGlob.m_DeshabilitaElemento(lstBotones.btnReinicia, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnReprocesa, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnElimina, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblpolizasIncidencias);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnReinicia, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnReprocesa, false);
                        oGlob.m_DeshabilitaElemento(lstBotones.btnElimina, false);
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
                oGlob.m_HideloaderGrid(lstTablas.tblpolizasIncidencias);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblpolizasIncidencias);
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
                detalle_valido = false;
                oGlob.m_OcultaElemnto(lstElemnts.classequiv, true);
                oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, true);
                //oGlob.m_ViewloaderGrid('#tbl-logs-proceso');
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, false);
                        oThis.m_CargaDataTableDetalle(response.data, jTipodocto);
                        detalle_valido = true;
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
                    case "EQUIVALENCIAS":
                        oGlob.m_DeshabilitaElemento(lstBotones.btnDowdetalle, false);
                        $(lstElemnts.msgequiv).html("<p>Se ha detectado una o varias líneas que no cuentan con equivalencias. Favor de corregirlo <b>para poder interfazar hacía Dynamics 365</b></p>");
                        oGlob.m_OcultaElemnto(lstElemnts.classequiv, false);
                        oGlob.m_AlertWarning('Se ha detectado una o varias líneas que no cuentan con equivalencias. Favor de corregirlo para poder interfazar hacía Dynamics 365');
                        oThis.m_CargaDataTableDetalle(response.data, jTipodocto);
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

    this.m_CargaNuevoXML = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "CargaXML",
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
                        $(lstmodales.modXML).modal('hide');
                        oGlob.m_AlertSuccess('La actualización del archivo XML se ha realizado con éxito');
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

    this.m_CargaXMLViaticoLinea = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "CargaViaticosLineaXML",
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
                        $(lstmodales.modXML).modal('hide');
                        oGlob.m_AlertSuccess('La actualización del archivo XML se ha realizado con éxito');
                        var oDetalle = {
                            FolioAsiento: upload_xml_id,
                            tipo_documento: 9
                        };
                        pm = "Detalle: " + JSON.stringify(oDetalle);
                        oThis.m_ListaDetallePoliza(pm, 9);
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
                        oGlob.m_AlertSuccess('El descarte de los registro(s) seleccionados se ha realizado con éxito');
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


    this.m_ReiniciaErrores = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "ReiniciaErrores",
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
                        oGlob.m_AlertSuccess('Los registros seleccionados han sido enviado nuevamente hacia D365. Favor de esperar <b>cinco minutos como mínimo</b> para verificar que la información ha sido creada');
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

    this.m_recuperaJSON = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "RecuperaTransaccion",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        $('#txtEndPoint').val(response.data.urlweb);
                        try {
                            $("#div_objeto").html(prettyPrintJson.toHtml(JSON.parse(response.data.peticion)));
                        } catch {
                            $("#div_objeto").html(prettyPrintJson.toHtml(JSON.parse('{"data":"SIN DATOS ENVIADOS"}')));
                        }
                        try {
                            $("#div_response").html(prettyPrintJson.toHtml(JSON.parse(response.data.respuesta)));
                        } catch {
                            $("#div_response").html(prettyPrintJson.toHtml(JSON.parse('{"data":"SIN DATOS RECIBIDOS"}')));
                        }
                        $('#txtFechaTransacc').val(response.data.FechaModificacion);
                        $('#txtcomentarios').val(response.data.mensaje);
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
    this.VisulizaBug = function (evt, jID) {
        oGlob.m_showLoader();
        marca_poliza = false;
        $(lstBotones.tabSend).click();
        oGlob.m_AbreModal(lstmodales.modBug, function (status) {
            if (status) {
                var row = $(lstTablas.tblpolizasIncidencias).jqGrid('getRowData', jID);
                var oParamtra = { FolioAsiento: row.FolioAsiento };
                var oParamBug = "Transacc: " + JSON.stringify(oParamtra);
                oThis.m_recuperaJSON(oParamBug);
            }
        });
    }

    this.m_AdjuntaCFDI = function (evt, jID) {
        marca_poliza = false;
        var row = $(lstTablas.tblpolizasIncidencias).jqGrid('getRowData', jID);
        if (row.id_tipo_doc == 2) {
            oGlob.m_OcultaElemnto(lstBotones.bntUploadViatico, true);
            oGlob.m_OcultaElemnto(lstBotones.bntUpload, false);
            upload_xml_id = row.FolioAsiento;
            upload_xml_linea = null;
            upload_token = null;
            $(lstElemnts.div_upload).html('<iframe id="iUploadXML" class="custom_iframe" frameborder="0" scrolling="no" src="Formularios/UPLOAD/UploadFilesXML.html"></iframe>');
            oGlob.m_AbreModal(lstmodales.modXML, function (status) { });
        }
    }

    this.m_AdjuntaCFDIViatico = function (evt, jID) {
        var row = $('#tbl-logs-proceso').jqGrid('getRowData', jID);
        if (row.usaxml == 1) {
            if (detalle_valido) {
                upload_xml_id = row.FolioAsiento;
                upload_xml_linea = row.linea;
                upload_token = row.refdoc;
                oGlob.m_OcultaElemnto(lstBotones.bntUploadViatico, false);
                oGlob.m_OcultaElemnto(lstBotones.bntUpload, true);
                $(lstElemnts.div_upload).html('<iframe id="iUploadXML" class="custom_iframe" frameborder="0" scrolling="no" src="Formularios/UPLOAD/UploadFilesXML.html"></iframe>');
                oGlob.m_AbreModal(lstmodales.modXML, function (status) { });
            } else oGlob.m_AlertWarning("Se ha detectado una o varias líneas que no cuentan con equivalencias. Favor de corregirlo para poder continuar");
        } else oGlob.m_AlertWarning("No es posible cargar un XML a una línea que es de tipo iva o pago a operador");
    }

    this.DescargaAdjuntoLinea = function (evt, jID) {
        var row = $('#tbl-logs-proceso').jqGrid('getRowData', jID);
        if (row.usaxml == 1) {
            window.open('Paginas/Generic-Table-Export.aspx?modo=comprobanteviatico&identificador=' + row.FolioAsiento + '&linea=' + row.linea, '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
        } else oGlob.m_AlertWarning("No es posible descargar un XML a una línea que es de tipo iva o pago a operador");
    }



    this.VisualizaDocto = function (evt, jID) {
        marca_poliza = false;
        oGlob.m_showLoader();
        var row = $(lstTablas.tblpolizasIncidencias).jqGrid('getRowData', jID);
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
            oThis.m_ListaDetallePoliza(pm, row.id_tipo_doc);
            oGlob.m_hideLoader();
        });
    }

    this.DespliegaTotal = function (jTotal) {
        $(lstElemnts.elTotal).html("<b>Total: </b>$" + jTotal.toLocaleString('es-MX'));
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
                { label: ".", name: "rUpload", type: "text", width: 50, align: "center", hidden: (jTipodocto != "9") },
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
                { label: "valido", name: "valido", type: "text", width: 140, align: "center", hidden: true },
                { label: "usaxml", name: "usaxml", type: "text", width: 140, align: "center", hidden: true },
                { label: "refdoc", name: "refdoc", type: "text", width: 140, align: "center", hidden: true },


            ],
            caption: 'Detalle del la póliza contable',
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                var total = 0;
                var ActiveButton = 'disabled';
                var ActivaDescarga = 'btn-sm btn-info';
                var tipoBoton = 'alert-secondary';
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    if (row.valido == "0")
                        $(jNameGrid).find("#" + key + "").addClass('bg-danger');
                    total = parseFloat(row.cargo) + parseFloat(total);
                    oIncidencia.DespliegaTotal(total);
                    ActiveButton = 'disabled';
                    tipoBoton = 'alert-secondary';
                    ActivaDescarga = 'alert-secondary';
                    if (row.usaxml == "1") {
                        ActiveButton = "";
                        tipoBoton = "alert-success";
                        ActivaDescarga = 'alert-info';;
                    }
                    var rowDown = "<button type=\"button\" class=\"btn btn-sm " + ActivaDescarga + " " + ActiveButton + " \" title=\"Descarga XML\" onClick=\"oIncidencia.DescargaAdjuntoLinea(this, " + key + ")\"><i class=\"fas fa-download \" ></i></button>";
                    var rowUpload = "<button  type=\"button\" class=\"btn btn-sm " + tipoBoton + " " + ActiveButton + " \"  title=\"Adjunta factura XML\" onClick=\"oIncidencia.m_AdjuntaCFDIViatico(this, " + key + ")\"> <i class=\"fas fa-upload\" data-label=\"Adjunta archivo XML\" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rUpload: rowUpload, rDown: rowDown });
                }
            },
            pager: "#jqGridPagerLogs"
        });
        $(jNameGrid).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    }

    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblpolizasIncidencias;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rBug", type: "text", width: 50, align: "center" },
                { label: "Folio GM", name: "FolioAsiento", type: "text", width: 140, align: "center" },
                { label: "Tipo documento", name: "Nombredocumento", type: "text", width: 150, align: "center" },
                { label: "Emisor/Receptor", name: "nombrerfc", type: "text", width: 300, align: "left" },
                { label: "RFC", name: "rfc", type: "text", width: 110, align: "center" },
                { label: "Documento", name: "folioserie", type: "text", width: 100, align: "center" },
                { label: "UUID", name: "uuid", type: "text", width: 250, align: "center" },
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

            ],
            caption: 'Listado de pólizas con errores en D365',
            multiselect: false,
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            onSelectRow: function (rowid, status) {

                var row = $(jNameGrid).jqGrid('getRowData', rowid);
                if (marca_poliza) {

                } else
                    $(jNameGrid).jqGrid('setSelection', rowid, false);

                marca_poliza = true;
            },
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                var ActiveUpload = 'disabled';
                var tipoBoton = 'alert-secondary';

                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    ActiveUpload = 'disabled';
                    if (row.id_tipo_doc == "2") {
                        ActiveUpload = "alert-secondary";
                        tipoBoton = "alert-success";
                    }

                    var rowBug = "<button type=\"button\" id=\"btn_view_bug_" + key + "\" class=\"btn btn-sm btn-danger\" title=\"Visualiza información de errores\" onClick=\"oIncidencia.VisulizaBug(this, " + key + ")\"> <i class=\"fas fa-code rowicon\"></i></button>";
                    var rowUpload = "<button  type=\"button\" class=\"btn btn-sm  " + tipoBoton + " " + ActiveUpload + " \"  title=\"Adjunta factura XML\" onClick=\"oIncidencia.m_AdjuntaCFDI(this, " + key + ")\"> <i class=\"fas fa-upload\" data-label=\"Adjunta archivo XML\" ></i></button>";
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" title=\"Visualiza detalle de la póliza\" onClick=\"oIncidencia.VisualizaDocto(this, " + key + ")\"><i class=\"fa fa-file \" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit, rUpload: rowUpload, rBug: rowBug });
                }
            },
            pager: "#jqGridPager"
        });
    }
}