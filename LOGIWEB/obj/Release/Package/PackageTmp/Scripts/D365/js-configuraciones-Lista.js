function Configuracion() {
    var oThis = this, oParams = "",
        lstFiltros = {},
        lstCaja = {
            txtciaid: "#txtnumerocia", txtRazon: "#txtrazon", txtNomD365: "#txtnomcia", txtURL: "#txtapi", txtUsuario: "#txtusario", txtPass: "#txtpass",
            txtaprobador: "#txtaproba", txtcliente: "#txtcliente", txtintentos: "#txintentos", txthoras: "#txthoras", txtmin: "#txtminutos",
            txtsegundo: "#txtsegundos", txtURLlogin: "#txtapilogin", txtconexion: "#txtconexion", txtserv: "#txtservidor",
            txtUsuEqv: "#txteqvUsuario", txteqvPass: "#txtpassword", txtcatalog: "#txtcatalogo", txplantilla: "#txtplantilla",
            txtcuenta_uno: "#txtCuentaplan_uno", txtcuenta_dos: "#txtCuentaplan_dos", txtaprodisper: "#txtaprodisper",
            txtdiariodis: "#txtDiariodisp", txtdiariofondo: "#txtDiarioFon", txtCuentaViatico: "#txtCuentaviatico", txtconnzap: "#txtconnzap",
            txtRFCTMS: "#txtRFCTMS", txtAPITMS: "#txtAPITMS", txtURLAPITMS: "#txtURLAPITMS", txtHostMail: "#txtHostMail", txtpuertoMail: "#txtpuertoMail",
            txtuserMail: "#txtuserMail", txtpassMail: "#txtpassMail", txtMailCxC: "#txtMailCxC", txtMailCxP: "#txtMailCxP", txtMaiTaller: "#txtMaiTaller",
            txtmalGasto: "#txtmalGasto", txtmailsoporte: "#txtmailsoporte", txthoranotif: "#txthoranotif", txtintentonotif: "#txtintentonotif",
            txtintentonotif: "#txtintentonotif",
            txtURLAPINva: "#txturlapinueva", txtEndPointConsulta: "#txtendpointconsulta", txtEndPointRespuesta: "#txtendpointrespuesta", txtRFCAPI: "#txtrfcapi",
            txtUsuarioAPI: "#txtusuarioapi", txtPassAPI: "#txtpassapi",
        },
        lstCajaDocs = {
            txtnumdoc: "#txtnumero", txtnombre: "#txtnombre", txtdiarionom: "#txtdiario", txtmetodo: "#txtmetodo"
        },
        lstTablas = { tbldocumentos: "#tbl-documentos" },
        lstBotones = {
            btnGuarda: "#btnGuardaconfig", chkActivo: "#chkactivo", btnAdd: "#btnGuarda", btnEdit: "#btnEdita",
            btnCarga: "#btnCargaInicial", btnSinc365: "#chkSinc365", chkSSL: "#chkSSL"
        },
        Controller = "../Metodos/D365/Configuracion-Datos.aspx/";

    this.m_InicializaEvents = function () {

        this.m_EventoBusqueda();
        $(lstBotones.btnGuarda).unbind();
        $(lstBotones.btnGuarda).click(function () {
            var oDataconfig = {
                cia: $(lstCaja.txtciaid).val(),
                cianombre: $(lstCaja.txtRazon).val(),
                ciad365: $(lstCaja.txtNomD365).val(),
                URLApi: $(lstCaja.txtURL).val(),
                URLApilogin: $(lstCaja.txtURLlogin).val(),
                usuariod365: $(lstCaja.txtUsuario).val(),
                passusrd365: $(lstCaja.txtPass).val(),
                aprobador: $(lstCaja.txtaprobador).val(),
                clientID: $(lstCaja.txtcliente).val(),
                intentos: $(lstCaja.txtintentos).val(),
                enviohorad365: $(lstCaja.txthoras).val(),
                enviomind365: $(lstCaja.txtmin).val(),
                enviosegd365: $(lstCaja.txtsegundo).val(),
                enviosegd365: $(lstCaja.txtsegundo).val(),
                Conexion_eqv: $(lstCaja.txtconexion).val(),
                Servidor_eqv: $(lstCaja.txtserv).val(),
                Usuario_eqv: $(lstCaja.txtUsuEqv).val(),
                Password_eqv: $(lstCaja.txteqvPass).val(),
                catalogo_eqv: $(lstCaja.txtcatalog).val(),
                plantilla: $(lstCaja.txplantilla).val(),
                cuenta_uno: $(lstCaja.txtcuenta_uno).val(),
                cuenta_dos: $(lstCaja.txtcuenta_dos).val(),
                aprobadordisper: $(lstCaja.txtaprodisper).val(),
                diariodisp: $(lstCaja.txtdiariodis).val(),
                diariofondo: $(lstCaja.txtdiariofondo).val(),
                sincd365: $(lstBotones.btnSinc365).is(':checked') ? 1 : 0,
                cuentaviatico: $(lstCaja.txtCuentaViatico).val(),
                conexionzap: $(lstCaja.txtconnzap).val(),

                rfc_tms: $(lstCaja.txtRFCTMS).val(),
                api_tms: $(lstCaja.txtAPITMS).val(),
                url_tms: $(lstCaja.txtURLAPITMS).val(),
                host_tms: $(lstCaja.txtHostMail).val(),
                puerto_mail: $(lstCaja.txtpuertoMail).val(),
                user_mail: $(lstCaja.txtuserMail).val(),
                password_mail: $(lstCaja.txtpassMail).val(),
                ssl_mail: $(lstBotones.chkSSL).is(':checked') ? 1 : 0,
                cuentas_cxc_mail: $(lstCaja.txtMailCxC).val(),
                cuentas_cxp_mail: $(lstCaja.txtMailCxP).val(),
                cuentas_mtto_mail: $(lstCaja.txtMaiTaller).val(),
                cuentas_gsto_mail: $(lstCaja.txtmalGasto).val(),
                cuentas_soport_mail: $(lstCaja.txtmailsoporte).val(),
                tiempo_mail: $(lstCaja.txthoranotif).val(),
                intento_mail: $(lstCaja.txtintentonotif).val(),

                urlAPIGM: $(lstCaja.txtURLAPINva).val(),
                EndPointConsulta: $(lstCaja.txtEndPointConsulta).val(),
                EndPointRespuesta: $(lstCaja.txtEndPointRespuesta).val(),
                rfcAPI: $(lstCaja.txtRFCAPI).val(),
                UsuarioAPIGM: $(lstCaja.txtUsuarioAPI).val(),
                PasswordAPIGM: $(lstCaja.txtPassAPI).val()

            };
            console.log(oDataconfig);
            oParams = "oConfiguracion:" + JSON.stringify(oDataconfig);;
            oThis.m_GuardaConfiguracion(oParams);
        });

        $(lstBotones.btnAdd).unbind();
        $(lstBotones.btnAdd).click(function () {
            if (oThis.m_ValidaCampos(true)) {
                pm = "Documento: " + JSON.stringify(oThis.m_DocumentoObjeto());
                oThis.m_CreaDocumento(pm);
            }
        });

        $(lstBotones.btnEdit).unbind();
        $(lstBotones.btnEdit).click(function () {
            if (oThis.m_ValidaCampos(true)) {
                pm = "Documento: " + JSON.stringify(oThis.m_DocumentoObjeto());
                oThis.m_EditaDocumento(pm);
            }
        });


    }



    this.m_EventoBusqueda = function () {
        oThis.m_ListaConfiguracion();
        oThis.m_Listadocumentos();
    }
    this.m_LimpiaCajasForm = function () {
        $.each(lstCaja, function (key, value) {
            $(value).val("");
        });
    }

    this.m_LimpiaCajasFormModal = function () {
        $.each(lstCajaDocs, function (key, value) {
            $(value).val("");
        });
    }

    this.m_DocumentoObjeto = function () {

        return oDocumento = {
            numerodoc: $(lstCajaDocs.txtnumdoc).val(),
            nombre: $(lstCajaDocs.txtnombre).val(),
            diario: $(lstCajaDocs.txtdiarionom).val(),
            metodo: $(lstCajaDocs.txtmetodo).val(),
            activo: 1
        };
    }

    this.m_ValidaCampos = function (bAlert) {

        var bContinua = true;
        var sCadena = "";

        if ($(lstCajaDocs.txtmetodo).val() == "")
            sCadena = "Favor de capturar el método para poder interfazarlo a D365";
        if ($(lstCajaDocs.txtdiarionom).val() == "")
            sCadena = "Favor de capturar el diario del documento para asignarlo a D365";
        if ($(lstCajaDocs.txtnombre).val() == "")
            sCadena = "Favor de capturar el nombre para identificar el documento";
        if ($(lstCajaDocs.txtnumdoc).val() == "")
            sCadena = "Favor de capturar el número de documento";

        if (sCadena.length > 0) {
            if (bAlert)
                oGlob.m_AlertWarning(sCadena);
            bContinua = false;
        }

        return bContinua;
    }

    this.m_CargaObjeto = function (oJSON) {

        $(lstCaja.txtciaid).val(oJSON.cia);
        $(lstCaja.txtRazon).val(oJSON.cianombre);
        $(lstCaja.txtNomD365).val(oJSON.ciad365);
        $(lstCaja.txtURL).val(oJSON.URLApi);
        $(lstCaja.txtURLlogin).val(oJSON.URLApilogin);
        $(lstCaja.txtUsuario).val(oJSON.usuariod365);
        $(lstCaja.txtPass).val(oJSON.passusrd365);
        $(lstCaja.txtaprobador).val(oJSON.aprobador);
        $(lstCaja.txtcliente).val(oJSON.clientID);
        $(lstCaja.txtintentos).val(oJSON.intentos);
        $(lstCaja.txthoras).val(oJSON.enviohorad365);
        $(lstCaja.txtmin).val(oJSON.enviomind365);
        $(lstCaja.txtsegundo).val(oJSON.enviosegd365);
        $(lstCaja.txtconexion).val(oJSON.Conexion_eqv);
        $(lstCaja.txtserv).val(oJSON.Servidor_eqv);
        $(lstCaja.txtUsuEqv).val(oJSON.Usuario_eqv);
        $(lstCaja.txteqvPass).val(oJSON.Password_eqv);
        $(lstCaja.txtcatalog).val(oJSON.catalogo_eqv);
        $(lstCaja.txplantilla).val(oJSON.plantilla);
        $(lstCaja.txtcuenta_uno).val(oJSON.cuenta_uno);
        $(lstCaja.txtcuenta_dos).val(oJSON.cuenta_dos);
        $(lstCaja.txtaprodisper).val(oJSON.aprobadordisper);
        $(lstCaja.txtdiariodis).val(oJSON.diariodisp);
        $(lstCaja.txtdiariofondo).val(oJSON.diariofondo);
        $(lstBotones.btnSinc365).prop('checked', oJSON.sincd365 == 1);
        $(lstCaja.txtCuentaViatico).val(oJSON.cuentaviatico);
        $(lstCaja.txtconnzap).val(oJSON.conexionzap);

        $(lstCaja.txtRFCTMS).val(oJSON.rfc_tms);
        $(lstCaja.txtAPITMS).val(oJSON.api_tms);
        $(lstCaja.txtURLAPITMS).val(oJSON.url_tms);
        $(lstCaja.txtHostMail).val(oJSON.host_tms);
        $(lstCaja.txtpuertoMail).val(oJSON.puerto_mail);
        $(lstCaja.txtuserMail).val(oJSON.user_mail);
        $(lstCaja.txtpassMail).val(oJSON.password_mail);
        $(lstCaja.txtMailCxC).val(oJSON.cuentas_cxc_mail);
        $(lstCaja.txtMailCxP).val(oJSON.cuentas_cxp_mail);
        $(lstCaja.txtMaiTaller).val(oJSON.cuentas_mtto_mail);
        $(lstCaja.txtmalGasto).val(oJSON.cuentas_gsto_mail);
        $(lstCaja.txtmailsoporte).val(oJSON.cuentas_soport_mail);
        $(lstCaja.txthoranotif).val(oJSON.tiempo_mail);
        $(lstCaja.txtintentonotif).val(oJSON.intento_mail);
        $(lstBotones.chkSSL).prop('checked', oJSON.ssl_mail == 1);

        $(lstCaja.txtURLAPINva).val(oJSON.urlAPIGM);
        $(lstCaja.txtEndPointConsulta).val(oJSON.EndPointConsulta);
        $(lstCaja.txtEndPointRespuesta).val(oJSON.EndPointRespuesta);
        $(lstCaja.txtRFCAPI).val(oJSON.rfcAPI);
        $(lstCaja.txtUsuarioAPI).val(oJSON.UsuarioAPIGM);
        $(lstCaja.txtPassAPI).val(oJSON.PasswordAPIGM);
    }
    this.m_GuardaConfiguracion = function (oParam) {
        $.ajax({
            type: "POST",
            url: Controller + "ActualizaConfiguracion",
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
                        oGlob.m_AlertSuccess('Se ha actualizado con éxito la configuración general');
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

    this.m_EntityFormDocumento = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "ListaDocumentos",
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
                        console.log(JSON.stringify(response.data));
                        var oJSON = response.data[0];
                        $(lstCajaDocs.txtnumdoc).val(oJSON.numerodoc);
                        $(lstCajaDocs.txtnombre).val(oJSON.nombre);
                        $(lstCajaDocs.txtdiarionom).val(oJSON.diario);
                        $(lstCajaDocs.txtmetodo).val(oJSON.metodo);
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

    this.m_ListaConfiguracion = function () {
        $.ajax({
            type: "POST",
            url: Controller + "ListaConfiguracion",
            data: null,
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
                        oThis.m_CargaObjeto(response.data);
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

    this.m_Listadocumentos = function () {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListaDocumentos",
            data: "{Documento:{}}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_ViewloaderGrid(lstTablas.tbldocumentos);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
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
                oGlob.m_HideloaderGrid(lstTablas.tbldocumentos);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_HideloaderGrid(lstTablas.tbldocumentos);
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    }
    this.m_CreaDocumento = function (oParam) {
        $.ajax({
            type: "POST",
            url: Controller + "Creadocumento",
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
                        oGlob.m_CloseCrudmodal();
                        oGlob.m_AlertSuccess('El registro documento contable ha sido agregado con éxito');
                        oThis.m_LimpiaCajasFormModal();
                        oThis.m_Listadocumentos();
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
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });

    }

    this.m_EditaDocumento = function (oParam) {
        $.ajax({
            type: "POST",
            url: Controller + "ActualizaDocumento",
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
                        oGlob.m_CloseCrudmodal();
                        oGlob.m_AlertSuccess('El registro documento contable se ha actualizado con éxito');
                        oThis.m_LimpiaCajasFormModal();
                        oThis.m_Listadocumentos();
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
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });

    }

    this.m_EliminaDocumento = function (jDataValue) {

        var oDocumento = {
            numerodoc: jDataValue,
            activo: 0
        };
        pm = "Documento: " + JSON.stringify(oDocumento);
        $.ajax({
            type: "POST",
            url: Controller + "ActualizaDocumento",
            data: "{" + pm + "}",
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
                        oGlob.m_AlertSuccess('El registro documento contable se ha eliminado con éxito');
                        oThis.m_Listadocumentos();
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
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });

    }

    this.Editarow = function (evt, jActivo, jDocumento) {

        if (jActivo == 1) {
            oGlob.m_FormEditTomodal(evt, function (response) {
                ///cargar datos del item 
                if (response) {
                    $(lstCajaDocs.txtnumdoc).prop('readonly', true);
                    var oDoc = { numerodoc: jDocumento };
                    oParams = "Documento:" + JSON.stringify(oDoc);
                    oThis.m_EntityFormDocumento(oParams);
                }
            });
        } else oGlob.m_AlertError('No se permite editar un registro que ha sido inactivado');
    }

    this.Eliminarow = function (jActivo, jDocumento) {
        if (jActivo == 1) {
            oGlob.m_Confirma_Accion('Una vez eliminado el registro este no se puede ser utilizado y las pólizas creadas bajo este documento no podrán ser replicadas. ¿Estás seguro que deseas continuar?', function (response) {
                if (response)
                    oThis.m_EliminaDocumento(jDocumento);
            });
        } else oGlob.m_AlertError('La acción no se puede realizar sobre un documento contable que ya ha sido eliminada.');
    }

    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tbldocumentos;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center" },
                { label: ".", name: "rDelete", type: "text", width: 50, align: "center" },
                { label: "N° Documento", name: "numerodoc", type: "text", width: 100, align: "center" },
                { label: "Nombre", name: "nombre", type: "text", width: 400, align: "left" },
                { label: "Nombre del diario", name: "diario", type: "text", width: 180, align: "center" },
                { label: "Método", name: "metodo", type: "text", width: 150, align: "center" },
                {
                    label: "Activo", name: "activo", type: "text", width: 70, align: "center", editable: false, edittype: 'checkbox', editoptions: { value: "1:0" }, align: "center",
                    formatter: "checkbox", formatoptions: { disabled: true }
                }

            ],
            caption: 'Diarios contables activos',
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" data-title=\"Edita documento contable\"   data-page=\"D365/Gestion-diarios-Formulario\" onClick=\"oconfigc.Editarow(this, " + row.activo + ", '" + row.numerodoc + "')\"> <i class=\"fas fa-pen\" ></i></button>";
                    var rowDelete = "<button type=\"button\" class=\"btn btn-sm btn-danger\" onClick=\"oconfigc.Eliminarow(" + row.activo + ", '" + row.numerodoc + "')\"><i class=\"fas fa-trash-alt\" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit, rDelete: rowDelete });
                }
            },
            pager: "#jqGridPager"
        });
    }
}