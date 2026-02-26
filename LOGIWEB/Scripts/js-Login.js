'use strict';
function Login() {
    var oThis = this, oParams = "",
        lstCajas = { txtUsuario: "#txtusuario", txtPassword: "#txtpassword", txtServer: "#hdnServer", txtDatabase:"#hdnDataBase" },
        lstTablas = { tblaccnts: "#tbl-cuentas" },
        lstBotones = { btnIngresa: "#btnIngresa", btnPass: "#btnviewpass" },
        Controller = "Metodos/Login-datos.aspx/";

    this.m_InicializaEvents = function () {

        $(lstBotones.btnPass).click(function () {
            oGlob.m_ViewPassInput(lstCajas.txtPassword, this);
        });

        $(lstBotones.btnIngresa).click(function () {
            oThis.m_LogionPeticion();
        });

        $(lstCajas.txtUsuario).keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13')
                oThis.m_LogionPeticion();
        });

        $(lstCajas.txtPassword).keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13')
                oThis.m_LogionPeticion();
        });



    }

    this.m_LogionPeticion = function () {
        if ($(lstCajas.txtUsuario).val() != "" && $(lstCajas.txtPassword).val() != "") {

            var ouser = {
                sUsuario: $(lstCajas.txtUsuario).val(),
                sContrasenia: $(lstCajas.txtPassword).val(),
                sServer: $(lstCajas.txtServer).val(),
                sDatabase: $(lstCajas.txtDatabase).val()

            };
            oParams = "user: " + JSON.stringify(ouser);
            oThis.m_Ingresa(oParams);

        } else oGlob.m_AlertError('Favor de proprocionar las credenciales del usuario');
    }

    this.m_Ingresa = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "Login",
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
                        window.location.href = "default.aspx";
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
            error: function (XMLHttpRequest, textStatus, errorThorown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_Close = function () {
        oGlob.m_Confirma_Accion('Estás a punto de finalizar tu sesión. ¿Deseas salir del sistema?', function (response) {
            if (response) {
                $.ajax({
                    type: "POST",
                    url: Controller + "Logout",
                    data: "{}",
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
                                window.location.href = "login.aspx";
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
                    error: function (XMLHttpRequest, textStatus, errorThorown) {
                        oGlob.m_hideLoader();
                        oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                    }
                });

            }
        });

    }

    this.m_validaSession = function (callback) {
        $.ajax({
            type: "POST",
            url: Controller + "ValidaSesion",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        callback(true);
                        break;
                    case "-1":
                        oGlob.m_showModalSession();
                        callback(false);
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThorown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                callback(false);
            }
        });
    }
}