function Cierreperiodo() {
    var oThis = this, 
        lstCaja = { txtciaid: "#txtCia", txtRazon: "#txtNombre", txtnuevoperiodo:"#txtnuevoperiodo"},
        lstElemnts = { modalperiodo: "#modalperiodo", leyenda:"#leyendaperiodo"},
        lstBotones = {
            btnSaldo:"#chksaldo", btnGuarda: "#btnGuarda"
        },
        Controller = "../Metodos/D365/Configuracion-Datos.aspx/";

    this.m_InicializaEvents = function () {

        this.m_EventoBusqueda();
        oGlob.m_CargaDetePickerMonth(lstCaja.txtnuevoperiodo);
        $(lstBotones.btnGuarda).unbind();
        $(lstBotones.btnGuarda).click(function () {
            if ($(lstCaja.txtnuevoperiodo).val() != "") {
                oGlob.m_Confirma_Accion('Una vez aplicado el cierre esta operación no puede ser revertida.¿Estás seguro que deseas continuar?', function (response) {
                    if (response) {

                        let oPerido = {
                            cia: $(lstCaja.txtciaid).val(),
                            fecha: $(lstCaja.txtnuevoperiodo).val()
                        };
                        pm = "Periodo: " + JSON.stringify(oPerido);
                        oThis.m_GuardaConfiguracion(pm);

                    }
                });
            } else oGlob.m_AlertWarning("Favor de seleccionar el periodo que se desea abrir.");
        });

        $(lstElemnts.modalperiodo).on("hidden.bs.modal", function () {
            window.location.reload();
        });
    }
    this.m_EventoBusqueda = function () {        
        oThis.m_ListaConfiguracion();
        oThis.m_ListaPeriodoActual();
    }


    this.m_ListaPeriodoActual = function () {
        $.ajax({
            type: "POST",
            url: Controller + "ListaPeriodoActual",
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        $(lstElemnts.leyenda).html(response.mensaje);
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
                }
                oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) { }
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
                        oGlob.m_AbreModal(lstElemnts.modalperiodo, function (status) { });
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

    this.m_GuardaConfiguracion = function (oParam) {
        $.ajax({
            type: "POST",
            url: Controller + "CierraPeriodo",
            data: "{" + oParam +"}",
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
                        oGlob.m_AlertSuccess("La configuración ha sido actualizada, el nuevo periodo ha quedado abierto.");
                        oThis.m_EventoBusqueda();
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

    this.m_CargaObjeto = function (oJSON) {

        $(lstCaja.txtciaid).val(oJSON.cia);
        $(lstCaja.txtRazon).val(oJSON.cianombre); 
    }

    
}