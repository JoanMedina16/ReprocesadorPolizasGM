function BandejaFacturas() {
    var oThis = this, oParams = "",
        lstFiltros = {},
        lstCaja = {},
        lstElemnts = {},
        lstTablas = { tblBandeja: "#tbl-motorfacturacion" },
        lstBotones = {
            btnSerch: "#btnBusca", btnFiltra:"#btnFiltro", btnDescarga: "#btnDescarga", btnAddcnf: "#btnAddcnf"
        },
        Controller = "../Metodos/MOTF/ViajePortes-Datos.aspx/", abrePorte = false;

    this.m_InicializaEvents = function () {

        oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
        oThis.m_EventoBusqueda();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        });

        $(lstBotones.btnFiltra).unbind();
        $(lstBotones.btnFiltra).click(function () {
            oGlob.m_AbremodalFiltro();
            
        });

        $(lstBotones.btnDescarga).click(function () {
            var headers = [];
            var valores = null;
            var data = $(lstTablas.tblBandeja).jqGrid("getGridParam", "data");
            if (data != null && data.length > 0) {
                headers = [
                    { "sColum": "Arrendatario", "sType": "texto" },
                    { "sColum": "RFC", "sType": "texto" },
                    { "sColum": "Num Ext.", "sType": "texto" },
                    { "sColum": "Num Int.", "sType": "texto" },
                    { "sColum": "País", "sType": "texto" },
                    { "sColum": "Municipio", "sType": "texto" },
                    { "sColum": "Estado", "sType": "texto" },
                    { "sColum": "Localidad", "sType": "texto" },
                    { "sColum": "CP", "sType": "texto" },
                    { "sColum": "Colonia", "sType": "texto" }
                ];
                valores = [data.length];
                for (var i = 0; i < data.length; i++) {
                    var datarow = [
                        { "sValor": data[i].nombre },
                        { "sValor": data[i].RFC },
                        { "sValor": data[i].num_exterior },
                        { "sValor": data[i].num_interior },
                        { "sValor": data[i].pais_nombre },
                        { "sValor": data[i].municipio_nombre },
                        { "sValor": data[i].estado_nombre },
                        { "sValor": data[i].localidad_nombre },
                        { "sValor": data[i].codigopostal },
                        { "sValor": data[i].colonia_nombre }
                    ];
                    valores[i] = datarow;
                }
                var contenido = {
                    lstHeader: headers,
                    lstValues: valores
                };
                pm = "contenido:" + JSON.stringify(contenido) + ",sNombre:'configuraciones_de_arrendatario'";
                oGlob.m_ExportDataTable(pm);
            }
        });
    }

    this.m_InicializaEventoModal = function () {

        oGlob.m_DeshabilitaElemento(lstCaja.cmbestado, true);
        oGlob.m_DeshabilitaElemento(lstCaja.cmbmuni, true);
        oGlob.m_DeshabilitaElemento(lstCaja.cmblocali, true);
        oGlob.m_DeshabilitaElemento(lstCaja.cmbcodigop, true);
        oGlob.m_DeshabilitaElemento(lstCaja.cmbcolonia, true);
        oGlob.m_DeshabilitaElemento(lstCaja.cmbpais, true);

        oParams = "catalogo: { clave:'MEX' }, TIPO:1";
        oThis.ListaCatalogoSAT(lstCaja.cmbpais, oParams, function (status) {

            if (status) {
                $(lstCaja.cmbpais).chosen();
                setTimeout(function () {
                    $(lstCaja.cmbpais).val('MEX').trigger("chosen:updated.chosen");
                    $(lstCaja.cmbpais).val('MEX').change();

                }, 300);
                oGlob.m_DeshabilitaElemento(lstCaja.cmbpais, true);
            }

        });

        $(lstCaja.txtCodigopos).keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13') {
                oThis.DatoSATComplementarios($(this).val());
            }
        });

    }

    this.DatoSATComplementarios = function (jCodigoPostal) {


        var cat = { clave: jCodigoPostal };

        oParams = "catalogo: " + JSON.stringify(cat) + ", TIPO:5";
        oThis.ListaCatalogoCP(oParams, function (status, data) {
            if (status) {
                cat = { clavecodigopostal: data[0].clave };
                oParams = "catalogo: " + JSON.stringify(cat) + ", TIPO:6";
                $(lstCaja.cmbcolonia).chosen("destroy");
                oThis.ListaCatalogoSAT(lstCaja.cmbcolonia, oParams, function (status) {
                    if (status) {
                        oGlob.m_DeshabilitaElemento(lstCaja.cmbcolonia, false);
                        $(lstCaja.cmbcolonia).chosen();
                        if (oLugarEDIT != null) {
                            $(lstCaja.cmbcolonia).val(oLugarEDIT.colonia).trigger("chosen:updated.chosen");
                            $(lstCaja.cmbcolonia).val(oLugarEDIT.colonia).change();
                        }
                    }
                });


                cat = { clavepais: 'MEX', clave: data.length == 1 ? data[0].claveestado : '' };
                oParams = "catalogo: " + JSON.stringify(cat) + ", TIPO:2";
                $(lstCaja.cmbestado).chosen("destroy");
                oThis.ListaCatalogoSAT(lstCaja.cmbestado, oParams, function (status) {
                    if (status) {
                        oGlob.m_DeshabilitaElemento(lstCaja.cmbestado, false);
                        $(lstCaja.cmbestado).chosen();
                        if (oLugarEDIT != null) {
                            $(lstCaja.cmbestado).val(oLugarEDIT.estado).trigger("chosen:updated.chosen");
                            $(lstCaja.cmbestado).val(oLugarEDIT.estado).change();
                        }
                        if (data.length == 1 && oLugarEDIT == null) {
                            $(lstCaja.cmbestado).val(data[0].claveestado).trigger("chosen:updated.chosen");
                            $(lstCaja.cmbestado).val(data[0].claveestado).change();
                        }
                    }
                });

                var bAddMunicipio = true;
                if (data.length == 1) {
                    if (data[0].clavemunicipio == '')
                        bAddMunicipio = false;
                }


                if (bAddMunicipio) {
                    cat = {
                        clave: data.length == 1 ? data[0].clavemunicipio : '', claveestado: data[0].claveestado
                    };
                    oParams = "catalogo: " + JSON.stringify(cat) + ", TIPO:3";
                    $(lstCaja.cmbmuni).chosen("destroy");
                    oThis.ListaCatalogoSAT(lstCaja.cmbmuni, oParams, function (status) {
                        if (status) {
                            oGlob.m_DeshabilitaElemento(lstCaja.cmbmuni, false);
                            $(lstCaja.cmbmuni).chosen();
                            if (oLugarEDIT != null) {
                                $(lstCaja.cmbmuni).val(oLugarEDIT.municipio).trigger("chosen:updated.chosen");
                                $(lstCaja.cmbmuni).val(oLugarEDIT.municipio).change();
                            }

                            if (data.length == 1 && oLugarEDIT == null) {
                                $(lstCaja.cmbmuni).val(data[0].clavemunicipio).trigger("chosen:updated.chosen");
                                $(lstCaja.cmbmuni).val(data[0].clavemunicipio).change();
                            }
                        }
                    });
                }

                var bAddLocalidad = true;
                if (data.length == 1) {
                    if (data[0].clavelocalidad == '')
                        bAddLocalidad = false;
                }
                if (bAddLocalidad) {
                    cat = {
                        clave: data.length == 1 ? data[0].clavelocalidad : '', claveestado: data[0].claveestado
                    };
                    oParams = "catalogo: " + JSON.stringify(cat) + ", TIPO:4";
                    $(lstCaja.cmblocali).chosen("destroy");
                    oThis.ListaCatalogoSAT(lstCaja.cmblocali, oParams, function (status) {
                        if (status) {
                            oGlob.m_DeshabilitaElemento(lstCaja.cmblocali, false);
                            $(lstCaja.cmblocali).chosen();
                            if (oLugarEDIT != null) {
                                $(lstCaja.cmblocali).val(oLugarEDIT.localidad).trigger("chosen:updated.chosen");
                                $(lstCaja.cmblocali).val(oLugarEDIT.localidad).change();
                            }

                            if (data.length == 1 && oLugarEDIT == null) {
                                $(lstCaja.cmblocali).val(data[0].clavelocalidad).trigger("chosen:updated.chosen");
                                $(lstCaja.cmblocali).val(data[0].clavelocalidad).change();
                            }
                        }
                    });
                }

            } else oGlob.m_AlertWarning("No se ha encontrado información del código postal");
        });
    }

    this.m_GuardaInformacion = function (jFileExcel) {

        var oArrendador = oThis.m_mapeArrendador(jFileExcel);
        pm = "arrendador:" + JSON.stringify(oArrendador);
        oThis.m_GuardaRegistro(pm);
    }

    this.m_mapeArrendador = function (jFileExcel) {

        debugger
        return oArrendatario = {
            pais: $(lstCaja.cmbpais).val(),
            estado: $(lstCaja.cmbestado).val(),
            municipio: $(lstCaja.cmbmuni).val(),
            localidad: $(lstCaja.cmblocali).val(),
            codigopostal: $(lstCaja.txtCodigopos).val(),
            colonia: $(lstCaja.cmbcolonia).val(),
            nombre: $(lstCaja.txtnombre).val(),
            pais_nombre: oGlob.m_RecuperaCadenaSplit($(lstCaja.cmbpais + " :selected").text()),
            estado_nombre: oGlob.m_RecuperaCadenaSplit($(lstCaja.cmbestado + " :selected").text()),
            municipio_nombre: oGlob.m_RecuperaCadenaSplit($(lstCaja.cmbmuni + " :selected").text()),
            localidad_nombre: oGlob.m_RecuperaCadenaSplit($(lstCaja.cmblocali + " :selected").text()),
            colonia_nombre: oGlob.m_RecuperaCadenaSplit($(lstCaja.cmbcolonia + " :selected").text()),
            referencia: $(lstCaja.txtreferencia).val(),
            RFC: $(lstCaja.txtrfc).val(),
            calle: $(lstCaja.txtcalle).val(),
            num_interior: $(lstCaja.txtNumInt).val(),
            num_exterior: $(lstCaja.txtnumExt).val(),
            sNombreAdjunto: jFileExcel
        };
    }



    this.m_EventoBusqueda = function () {
        var oViaje = {
            //nombre: $(lstFiltros.txtNombre).val(),
            //RFC: $(lstFiltros.txtRFC).val()
        };
        pm = "Viaje: " + JSON.stringify(oViaje);
        oThis.m_ListaBandejaFacturas(pm);
        oGlob.m_CloseFilters();
    }
    this.m_ValidaCampos = function (bAlert) {

        var bContinua = true;
        var sCadena = "";

        if ($(lstCaja.cmbestado).val() == "")
            sCadena = "Favor de capturar el estado";
        /*if ($(lstCaja.cmbmuni).val() == "")
            sCadena = "Favor de seleccionar el municipio";
        if ($(lstCaja.cmblocali).val() == "")
            sCadena = "Favor de seleccionar la localidad";*/
        if ($(lstCaja.txtCodigopos).val() == "")
            sCadena = "Favor de seleccionar el código postal";
        if ($(lstCaja.txtrfc).val() == "")
            sCadena = "Favor de escribir el RFC";
        else {

            if ($(lstCaja.txtrfc).val() == "XAXX010101000")
                sCadena = "No se permite la captura de RFC generico";
            else if (!oGlob.validaRFC($(lstCaja.txtrfc).val()))
                sCadena = "El RFC proporcionado no es válido";
        }

        if ($(lstCaja.txtnombre).val() == "")
            sCadena = "Favor de escribir el nombre";


        if (sCadena.length > 0) {
            if (bAlert)
                oGlob.m_AlertWarning(sCadena);
            bContinua = false;
        }

        return bContinua;
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

    this.ListaCatalogoCP = function (oParams, callback) {

        $.ajax({
            type: "POST",
            url: "Metodos/Lugares-Datos.aspx/ListaCatalogoSAT",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {

            },
            success: function (datos) {
                var response = datos["d"];

                switch (response.estatus) {
                    case "OK":
                        callback(true, response.data);
                        break;
                    default:
                        callback(false, null);
                        break;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                callback(false, null);
            }
        });
    }

    this.ListaCatalogoSAT = function (jInput, oParams, callback) {

        $.ajax({
            type: "POST",
            url: "Metodos/Lugares-Datos.aspx/ListaCatalogoSAT",
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
                    text: "Selecciona la configuracion SAT"
                });
                select.append(opcion);

                for (var i = 0; i < response.data.length; i++) {

                    opcion = $("<option />", {
                        value: response.data[i].clave,
                        text: response.data[i].clave + " - " + response.data[i].descripcion
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

    this.m_ListaBandejaFacturas = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListaViajesporte",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblBandeja);
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
                        oThis.Onwarning('<p>El viaje no ha sido encontrado</p><b>-Recuerda esperar al menos 10 minutos despues de poner el viaje en ruta en OTM</b></br><b>-Verificar que el viaje ya exista en apex y que no tiene errores</b>');
                        break;
                    default:                        
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
                oGlob.m_HideloaderGrid(lstTablas.tblBandeja);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblBandeja);
            }
        });
    }

    this.m_EditaArrendador = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "InsertaEditaArrendador",
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
                        oGlob.m_AlertSuccess('El registro del arrendatario se ha actualizado con éxito');
                        oThis.m_LimpiaCajasForm();
                        oThis.m_LimpiaFiltros();
                        oThis.m_EventoBusqueda();
                        oGlob.m_cargaLogsproceso(response.data);
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
                        oGlob.m_cargaLogsproceso(response.data);
                        oGlob.m_AlertWarning(response.mensaje);
                        oThis.m_LimpiaCajasForm();
                        oThis.m_LimpiaFiltros();
                        oThis.m_EventoBusqueda();
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

    this.m_GuardaRegistro = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "InsertaEditaArrendador",
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
                        oGlob.m_AlertSuccess('El registro del arrendatario se ha guardado con éxito');
                        oThis.m_LimpiaCajasForm();
                        oThis.m_LimpiaFiltros();
                        oThis.m_EventoBusqueda();
                        oGlob.m_cargaLogsproceso(response.data);
                        break;
                    case "-1":
                        oGlob.m_showModalSession();
                        break;
                    case "-2":
                        oGlob.m_showModalPermiso();
                        break;
                    case "ERROR":
                        oGlob.m_AlertError(response.mensaje);
                        oGlob.m_cargaLogsproceso(response.data);
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        oGlob.m_cargaLogsproceso(response.data);
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

    this.VisualizaPorte = function (evt, jID) {
        abrePorte = true;
        var row = $(lstTablas.tblBandeja).jqGrid('getRowData', jID);
        if (row.facturable == 1)
            oGlob.m_CargaFormulario(evt);
        else oGlob.m_AlertWarning('No es posible facturar el porte existen registros del catálogo (clientes, bodegas, operador) que no están debidamente configurado. Favor de validarlo.');
    }

    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblBandeja;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center" },
                { label: "Folio viaje", name: "Folioviaje", type: "text", width: 130, align: "left" },
                {
                    label: "Total", name: "Total", type: "number", width: 100, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                { label: "Cliente", name: "Cliente", type: "text", width: 220, align: "left" },
                { label: "Origen", name: "Origen", type: "text", width: 240, align: "left", },
                { label: "Destino", name: "Destino", type: "text", width: 240, align: "left", },
                { label: "Operador", name: "Operador", type: "text", width: 240, align: "left", },
                { label: "Fecha", name: "Fechacreacion", type: "text", width: 140, align: "left", },
                { label: "PorteID", name: "PorteId", type: "text", width: 120, align: "center", hidden: true },
                { label: "Cuentacliente", name: "cuentaclie", type: "text", width: 120, align: "center", hidden: true },
                { label: "Subcuentacliente", name: "subclie", type: "text", width: 120, align: "center", hidden: true },
                { label: "Facturable", name: "facturable", type: "text", width: 120, align: "center", hidden: true },

            ],
            caption: 'Listado de viajes pendientes de facturar',
            viewrecords: true,
            multiselect: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-success\" data-notitle=\"\" data-lvuno=\"Motor de facturaci&oacute;n\" data-lvdos=\"\" data-title=\"Viaje a facturar " + row.Folioviaje+" \" data-key=\"" + row.idax + "\" data-page=\"MOTF/Factura-porte-Detalle\" onClick=\"oBandeja.VisualizaPorte(this, " + key + ")\"><i class=\"fas fa-file-invoice-dollar fa-lg \" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit });
                }
            },

            onSelectRow: function (rowid, status) {

                var row = $(jNameGrid).jqGrid('getRowData', rowid);
                if (!abrePorte) {
                    console.log('agregar checked');


                } else
                    $("#jqg_tbl-motorfacturacion_" + rowid).prop("checked", false);
                abrePorte = false;
            },
            onSelectAll: function (ids, status) {
                //$(jNameGrid).resetSelection();
                //return false;
            },
            pager: "#jqGridPager"
        });
    }
}