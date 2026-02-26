'use strict';
function General() {
    var oThis = this;
    var lstCajas = { txtFechaInicio: "#txtFechainicio", txtFechafin: "#txtFechafin" };

    this.m_InicializaReporte = function () {
        oThis.m_CargaDetePicker(lstCajas.txtFechaInicio);
        oThis.m_CargaDetePicker(lstCajas.txtFechafin);
    }

    this.m_showLoader = function () {

        $('#modalloader').modal({ backdrop: 'static', keyboard: false });
    }
    this.m_hideLoader = function () {
        setTimeout(function () {
            $('#modalloader').modal('hide');
        }, 500);
        setTimeout(function () {
            $('#modalloader').modal('hide');
        }, 600);
        setTimeout(function () {
            $('#modalloader').modal('hide');
        }, 700);
    }
    this.m_AlertWarning = function (jMessage) {
        setTimeout(function () {
            $.confirm({
                columnClass: 'col-md-6 col-md-offset-3',
                title: 'Mensaje del sistema',
                icon: 'fas fa-exclamation-triangle',
                content: jMessage,
                type: 'orange',
                typeAnimated: true,
                buttons: {
                    Aceptar: function () {
                        btnClass: 'btn-red'
                    }
                }
            });
        }, 600);
    }
    this.m_Information = function (jMessage) {
        setTimeout(function () {
            $.confirm({
                columnClass: 'col-md-6 col-md-offset-3',
                title: 'Mensaje del sistema',
                icon: 'fas fa-info-circle',
                content: jMessage,
                type: 'dark',
                typeAnimated: true,
                buttons: {
                    Aceptar: function () {
                        btnClass: 'btn-red'
                    }
                }
            });
        }, 600);
    }
    this.m_AlertError = function (jMessage) {
        setTimeout(function () {
            $.confirm({
                columnClass: 'col-md-6 col-md-offset-3',
                title: 'Mensaje del sistema',
                icon: 'fas fa-exclamation-circle',
                content: jMessage,
                type: 'red',
                typeAnimated: true,
                buttons: {
                    Aceptar: function () {
                        btnClass: 'btn-red'
                    }
                }
            });
        }, 600);
    }

    this.m_AlertSuccess = function (jMessage) {
        setTimeout(function () {
            $.confirm({
                columnClass: 'col-md-6 col-md-offset-3',
                title: 'Mensaje del sistema',
                icon: 'fas fa-check-square',
                content: jMessage,
                type: 'green',
                typeAnimated: true,
                buttons: {
                    Aceptar: function () {
                        btnClass: 'btn-red'
                    }
                }
            });
        }, 600);
    }
    this.m_Confirma_Accion = function (jMessage, callback) {
        $.confirm({
            columnClass: 'col-md-6 col-md-offset-3',
            title: 'Mensaje del sistema',
            content: jMessage,
            type: 'dark',
            typeAnimated: true,
            buttons: {
                yes: {
                    keys: ['y'],
                    text: 'Si',
                    action: function () {
                        callback(true);
                    }
                },
                no: {
                    keys: ['N'],
                    text: 'No',
                    action: function () {
                        callback(false);
                    }
                },
            }
        });
    }

    this.m_CargaInformes = function (evt, jPagina) {
        var dContenedor = "#divContenedor-navigation";
        var dNivelDos = "#liSegundo";
        var dNivelTres = "#liTercero";
        var dNivelTitulo = "#liTitulo";
        var dTitulo = "#h1Title";
        $(dContenedor).html('');
        if ($("#navinfos").hasClass("nav-link")) {
            $(".nav-link").removeClass("active");
            $(evt).addClass("active");
            $("#navinfos").addClass("active");
        }
        $(dNivelDos).html($(evt).attr("data-lvuno"));
        $(dNivelTitulo).html($(evt).attr("data-title"));
        $(dTitulo).html($(evt).attr("data-title"));
        $("#div-content-reporte").show();
        oThis.m_InicializaReporte();
        $("#hddInforme").val("1");
    }

    this.m_FormFilters = function () {
        $('#modalfiltro').modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
    }
    this.m_CloseCrudmodal = function () {
        
        setTimeout(function () {
            $('#modalcrud').modal('hide');
        }, 500);
        setTimeout(function () {
            $('#modalcrud').modal('hide');
        }, 600);
        setTimeout(function () {
            $('#modalcrud').modal('hide');
        }, 700);
    }

    this.m_CloseFilters = function () {
        setTimeout(function () {
            $('#modalfiltro').modal('hide');
        }, 500);
        setTimeout(function () {
            $('#modalfiltro').modal('hide');
        }, 600);
        setTimeout(function () {
            $('#modalfiltro').modal('hide');
        }, 700);
    }

    this.m_CargaFormulario = function (evt, jPagina) {

        if (jPagina == undefined)
            jPagina = true;
        var dContenedor = "#divContenedor-navigation";
        var dNivelDos = "#liSegundo";
        var dNivelTres = "#liTercero";
        var dNivelTitulo = "#liTitulo";
        var dTitulo = "#h1Title";

        $.ajax({
            type: "POST",
            url: (jPagina ? "Paginas/" : "Formularios/") + $(evt).attr("data-page") + ".aspx",
            data: "",
            cache: false,
            async: true,
            beforeSend: function () {
                oThis.m_showLoader();
            },
            success: function (datos) {

                oLogin.m_validaSession(function (response) {
                    if (response) {


                        if ($(evt).attr("data-padre") != "") {
                            $(".nav-link").removeClass("active");
                            var navID = "#" + $(evt).attr("data-padre");
                            $(navID).addClass("active");
                            $(evt).addClass("active");
                        } 

                        if ($(evt).attr("data-notitle") != "")
                            oThis.m_OcultaElemnto("#headeNav", false);
                        else oThis.m_OcultaElemnto("#headeNav", true);
                        $("#hddEmpresaID").val($(evt).attr("data-empresa"));
                        $("#hddLevlDocument").val($(evt).attr("data-type"));
                        $(dContenedor).html(datos);
                        $(dNivelDos).html($(evt).attr("data-lvuno"));
                        $(dNivelTitulo).html($(evt).attr("data-title"));
                        $(dTitulo).html($(evt).attr("data-title"));
                        if ($(evt).attr("data-lvdos") != "")
                            oThis.m_OcultaElemnto(dNivelTres, false);
                        else
                            oThis.m_OcultaElemnto(dNivelTres, true);
                        $(dNivelTres).html($(evt).attr("data-lvdos"));
                        oThis.m_hideLoader();
                        $("#div-content-reporte").hide();
                        $("#hddInforme").val("");
                        oThis.ValidaPermisosUsuario();


                    } else {
                        oThis.m_hideLoader();
                        oThis.m_showModalSession();
                    }
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oThis.m_hideLoader();
            }
        });

    }

    this.m_CierraCrudModal = function () {
        $('#modalcrud').modal('hide');
    }

    this.m_FormAddTomodal = function (evt) {

        $.ajax({
            type: "POST",
            url: "Formularios/" + $(evt).attr("data-page") + ".aspx",
            data: "",
            cache: false,
            async: true,
            beforeSend: function () {
                oThis.m_showLoader();
            },
            success: function (datos) {
                $("#div-crud-body").html('');
                oThis.m_OcultaElemnto('#div-existencias', true);
                $('#modalcrud').data('bs.modal', null);
                oThis.m_AbreModal('#modalcrud', function (status) {
                    if (status) {
                        $("#div-crud-body").html(datos);
                        oThis.m_OcultaElemnto('.css-control-add', false);
                        oThis.m_OcultaElemnto('.css-control-edit', true);
                        $("#lbltitulo").html("<b>" + $(evt).attr("data-title") + "</b>");
                        oThis.m_hideLoader();
                        oThis.ValidaPermisosUsuario();
                    }
                });

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oThis.m_hideLoader();
            }
        });
    }

    this.ValidaPermisosUsuario = function () {

        oThis.m_OcultaElemnto('.acceso-add', $("#hdAgregar").val() == "0");
        oThis.m_OcultaElemnto('.acceso-edit', $("#hdEditar").val() == "0");
        oThis.m_OcultaElemnto('.acceso-del', $("#hdEliminar").val() == "0");
        oThis.m_OcultaElemnto('.acceso-dow', $("#hdDescargar").val() == "0");
        oThis.m_OcultaElemnto('.acceso-find', $("#hdBuscar").val() == "0");
        oThis.m_OcultaElemnto('.acceso-upload-csv', $("#hdUploadcsv").val() == "0");
        oThis.m_OcultaElemnto('.acceso-reset', $("#hdRsterror").val() == "0");
        oThis.m_OcultaElemnto('.acceso-crea-ax', $("#hdEntradaAX").val() == "0");

    }

    this.m_FormEditTomodal = function (evt, callback) {

        $.ajax({
            type: "POST",
            url: "Formularios/" + $(evt).attr("data-page") + ".aspx",
            data: "",
            cache: false,
            async: false,
            beforeSend: function () {
                oThis.m_showLoader();
            },
            success: function (datos) {
                oThis.m_OcultaElemnto('#div-existencias', true);
                oThis.m_AbreModal('#modalcrud', function (status) {

                    if (status) {
                        $("#div-crud-body").html(datos);
                        oThis.m_OcultaElemnto('.css-control-add', true);
                        oThis.m_OcultaElemnto('.css-control-edit', false);
                        $("#lbltitulo").html("<b>" + $(evt).attr("data-title") + "</b>");
                        callback(true);
                        oThis.m_hideLoader();
                    }
                }); 
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                callback(false);
                oThis.m_hideLoader();
            }
        });
    }
    this.m_CargaDocumentos = function (jInput, bMultiple) {

        $.ajax({
            type: "POST",
            url: "Metodos/D365/Configuracion-Datos.aspx/ListaDocumentos",
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
                        text: "Selecciona el documento"
                    });
                    select.append(opcion);
                }
                for (var i = 0; i < response.data.length; i++) {

                    opcion = $("<option />", {
                        value: response.data[i].numerodoc,
                        text: response.data[i].numerodoc + " - " + response.data[i].nombre
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

    this.m_CargaDocumentosProcesa = function (jInput, bMultiple) {

        $.ajax({
            type: "POST",
            url: "Metodos/D365/Configuracion-Datos.aspx/ListaDocumentosProcesa",
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
                        text: "Selecciona el documento"
                    });
                    select.append(opcion);
                }
                for (var i = 0; i < response.data.length; i++) {

                    opcion = $("<option />", {
                        value: response.data[i].Id,
                        text: response.data[i].Proceso
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

    this.m_ExportDataTable = function (oParam, jModo, jParametros) {

        $.ajax({
            type: "POST",
            url: "Paginas/Generic-Table-Export.aspx/GeneraArchivoSalida",
            data: "{" + oParam + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function (html) {

            },
            success: function (result) {
                var response = result["d"];
                switch (response) {
                    case "-1":
                        oThis.m_showModalSession();
                        break;
                    case "-2":
                        oThis.m_showModalPermiso();
                        break;
                    default:
                        var dataRes = response.split(';');
                        var resultado = dataRes[0];
                        var asistente = dataRes[1];
                        var modo = "diferencia";
                        var parametros = "";
                        if (resultado == "OK") {
                            if (jModo == undefined)
                                modo = "datos";
                            else modo = jModo;

                            if (jParametros == undefined)
                                parametros = "";
                            else parametros = jParametros;
                        }
                        console.log('Paginas/Generic-Table-Export.aspx?modo=' + modo + '&identificador=' + asistente + parametros, '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
                        window.open('Paginas/Generic-Table-Export.aspx?modo=' + modo + '&identificador=' + asistente + parametros, '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
                        break;
                }
            },
            error: function (xhr) {
                window.open('Paginas/Generic-Table-Export.aspx?modo=serverror&identificador=x', '_blank', 'location=0,resizable=0,scrollbars=0,height=250,width=300');
            }
        });
    }



    this.m_showModalSession = function () {
        $('#modalsession').modal({ backdrop: 'static', keyboard: false });
    }

    this.m_showModalPermiso = function () {
        $('#modalpermisoacceso').modal({ backdrop: 'static', keyboard: false });
    }


    this.m_OcultaElemnto = function (jElemento, jBoculta) {
        if (jBoculta)
            $(jElemento).hide();
        else
            $(jElemento).show();
    }

    this.m_DeshabilitaElemento = function (jElemento, jHabilita) {

        $(jElemento).prop("disabled", jHabilita);
    }     

  

    this.m_cargaLogsproceso = function (dataJSON) {
        if (dataJSON != null) {
            if (dataJSON.length > 0) {
                oThis.m_AbreModal('#modal-logs', function (status) {
                    if (status) {
                        var jPager = "#jqGridPagerLogs";
                        var jNameGrid = "#tbl-logs-proceso";
                        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
                        $.jgrid.gridUnload(jNameGrid);
                        $(jNameGrid).jqGrid({
                            datatype: "local",
                            data: dataJSON,
                            styleUI: 'Bootstrap',

                            colModel: [
                                { label: "Nombre", name: "sNombre", type: "text", width: 250 },
                                { label: "AX365", name: "sLlave", type: "text", width: 150 },
                                { label: "Cuenta & subcuenta", name: "sIdentificador", type: "text", width: 200 },
                                { label: "Mensaje", name: "sMensaje", type: "text", width: 600 },
                            ],
                            viewrecords: true,
                            height: "18em",
                            rowNum: 30,
                            gridComplete: function () { },
                            pager: jPager
                        });
                    }
                });
            }
        }
    }

    this.m_AbremodalFiltro = function () {

        oThis.m_AbreModal('#modalfiltro', function (status) {
            if (status)
                $('#modalfiltro').find(':input').not(':button, :submit, :reset, :hidden, :checkbox, :radio').val('');
        });
    }

    this.m_AbreModal = function (jNombremodal, callback) {
        $(jNombremodal).modal({
            backdrop: 'static',
            keyboard: true,
            show: true
        });
        setTimeout(function () { callback(true); }, 400);
    }

    this.m_campoEspecialLetras = function (e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == '39' || keycode == '34' || keycode == '35' || keycode == '36' || keycode == '37')
            return false;
        return true;
    }

    this.m_soloNumeros = function (e) {
        var keycode = (e.which) ? e.which : e.keyCode;
        if (keycode > 31 && (keycode < 48 || keycode > 57))
            return false;
        return true;
    }

    this.m_SoloNumeroDecimal = function(txt, evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode == 46) {
            if (txt.value.indexOf('.') === -1) {
                return true;
            } else {
                return false;
            }
        } else {
            if (charCode > 31 &&
                (charCode < 48 || charCode > 57))
                return false;
        }
        return true;
    }

    this.m_isUUID = function (jUUID) {
        return /^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$/i.test(jUUID);
    }

    this.m_DataFromSelect = function (jInput, bString) {
        var itemSelected = "", itemSplits = "";
        itemSelected = $(jInput).multipleSelect('getSelects');
        for (var i = 0; i < itemSelected.length; i++)
            itemSplits += bString ? ("'" + itemSelected[i] + "',") : ("" + itemSelected[i] + ",");
        return itemSplits;

    }

    this.ValidaMail = function (jCorreo) {
        return /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(jCorreo);
    }

    this.m_SimulaClick = function (element) {
        var oevt = document.createEvent("MouseEvents");
        oevt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
        var object = document.getElementById(element);
        var oclicked = !object.dispatchEvent(oevt);
    }

    this.m_ViewPassInput = function (jInputpass, jButtonIcon) {

        if ($(jInputpass).attr("type") == "text") {
            $(jInputpass).prop('type', 'password');
            $(jButtonIcon).addClass('fa-eye');
            $(jButtonIcon).removeClass('fa-eye-slash');
        } else {
            $(jInputpass).prop('type', 'text');
            $(jButtonIcon).addClass('fa-eye-slash');
            $(jButtonIcon).removeClass('fa-eye');
        }
    }

    this.m_ViewloaderGrid = function (jNameGrid) {
        var ts = $(jNameGrid + ".ui-jqgrid-btable")[0];
        ts.grid.hDiv.loading = true;
        if (ts.p.hiddengrid) { return; }
        switch (ts.p.loadui) {
            case "disable":
                break;
            case "enable":
                $("#load_" + ts.p.id).show();
                break;
            case "block":
                $("#lui_" + ts.p.id).show();
                $("#load_" + ts.p.id).show();
                break;
        }
    }

    this.m_HideloaderGrid = function (jNameGrid) {
        var ts = $(jNameGrid + ".ui-jqgrid-btable")[0];
        ts.grid.hDiv.loading = false;
        if (ts.p.hiddengrid) { return; }
        switch (ts.p.loadui) {
            case "disable":
                break;
            case "enable":
                $("#load_" + ts.p.id).hide();
                break;
            case "block":
                $("#lui_" + ts.p.id).hide();
                $("#load_" + ts.p.id).hide();
                break;
        }
    }
    this.m_CargaDetePicker = function (jInput) {
        $(jInput).datepicker({
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            dateFormat: 'dd/mm/yy'
        });
    }

    this.m_CargaDetePickerMonth = function (jInput) {
        $(jInput).datepicker({
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            dateFormat: 'dd/mm/yy',
            minDate: 0,
            maxDate: '+1m',
        });
    }

    this.m_CargaDetePickerDiauno = function (jInput) {
        $(jInput).datepicker({
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            dateFormat: 'dd/mm/yy',
            beforeShowDay: function (date) {
                var lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                if (date.getDate() == 1) {
                    return [true, ''];
                }
                return [false, ''];
            }
        });
    }

    this.m_CargaDetePickerDiaultimo = function (jInput) {
        $(jInput).datepicker({
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            dateFormat: 'dd/mm/yy',
            beforeShowDay: function (date) {
                var lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                if (date.getDate() == lastDayOfMonth.getDate()) {
                    return [true, ''];
                }
                return [false, ''];
            }
        });
    }
}

function filterFloat(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}
function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,4})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }

}