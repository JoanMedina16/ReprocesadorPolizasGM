function Creditocliente() {
    var oThis = this, 
        lstCaja = { txtciaid: "#txtCia", txtRazon:"#txtNombre"},
         lstElemnts = { modalcredito: "#modalcredito"},
        lstBotones = {
            btnSaldo:"#chksaldo", btnGuarda: "#btnGuarda"
        },
        Controller = "../Metodos/D365/Configuracion-Datos.aspx/";

    this.m_InicializaEvents = function () {

        this.m_EventoBusqueda();         

        $(lstBotones.btnGuarda).unbind();
        $(lstBotones.btnGuarda).click(function () {
            var oDataconfig = {
                cia: $(lstCaja.txtciaid).val(),
                cianombre: $(lstCaja.txtRazon).val(),                
                validasaldo: $(lstBotones.btnSaldo).is(':checked') ? 1 : 0
            };
            oParams = "oConfiguracion:" + JSON.stringify(oDataconfig);;
            oThis.m_GuardaConfiguracion(oParams);
        });

        $(lstElemnts.modalcredito).on("hidden.bs.modal", function () {
            window.location.reload();
        });
    }
    this.m_EventoBusqueda = function () {        
        oThis.m_ListaConfiguracion();        
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
                        oGlob.m_AbreModal(lstElemnts.modalcredito, function (status) { });
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
            url: Controller + "DisponibilidaSaldoCliente",
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

    this.m_CargaObjeto = function (oJSON) {

        $(lstCaja.txtciaid).val(oJSON.cia);
        $(lstCaja.txtRazon).val(oJSON.cianombre); 
        $(lstBotones.btnSaldo).prop('checked', oJSON.validasaldo == 1);

    }

    
}