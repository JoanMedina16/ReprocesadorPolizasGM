function Usuarios() {
    var oThis = this, oParams = "",
        lstFiltros = {
            txtFConInicio: "#txtFechacinicio", txtFConFin: "#txtFechacfin", txtFCreInicio: "#txtFechacrinicio", txtFCreFin: "#txtFechacrfin",
            txtfolioserie: "#txtfolserie",  txtFolioD365:"#txtFolioD365"
        },
        lstCaja = {
            txtusuarioID: "#txtUsuarioID", txtusuario: "#txtUseralias", cmbusuario: "#cmbusuario", txtcorreo: "#txtcuentacorreo", cmbperfil: "#cmbperfil",
            cmbdiarios:"#cmbdiarios"},
        lstElemnts = { lblderechos: "#lblmensajesistema", dvderechos: "#div_derechos"  },
        lstmodales = {  }
        lstTablas = { tblusuarios: "#tbl-usuarios" },
            lstBotones = { btnSerch: "#btnBusca", btnAdd: "#btnAgrega", btnEdit: "#btnEdita", btnSave: "#btnGuarda" },
            Controller = "../Metodos/Usuarios-Datos.aspx/", div_modulos = "#treeview-menu";

    this.m_InicializaEvents = function () {
        
        oThis.m_EventoBusqueda();
        $(lstBotones.btnSerch).unbind();
        $(lstBotones.btnSerch).click(function () {
            oThis.m_EventoBusqueda();
        }); 

        $(lstBotones.btnAdd).unbind();
        $(lstBotones.btnAdd).click(function () {            
            oGlob.m_FormEditTomodal(this, function (response) {
                if (response) {
                    oThis.m_InicializamodalEvents('');
                    oGlob.m_OcultaElemnto('.css-control-add', false);
                    oGlob.m_OcultaElemnto('.css-control-edit', true);
                }
            });
        });

        $(lstBotones.btnSave).unbind();
        $(lstBotones.btnSave).click(function () {

            if (oThis.m_ValidaCampos(true)) {
                var oUsuario = oThis.m_GetObjetoUsuario();
                pm = "usuario: " + JSON.stringify(oUsuario);
                oThis.m_Guardausuario(pm);
            }
        });

        $(lstBotones.btnEdit).unbind();
        $(lstBotones.btnEdit).click(function () {

            if (oThis.m_ValidaCampos(true)) {
                var oUsuario = oThis.m_GetObjetoUsuario();
                pm = "usuario: " + JSON.stringify(oUsuario);
                oThis.m_Editausuario(pm);
            }

        });

    }

    this.m_GetObjetoUsuario = function () {
        var permisosasig = $(div_modulos).treeview('selectedValues');
        var lstDiarioSelected = oGlob.m_DataFromSelect(lstCaja.cmbdiarios, false);
        return oUsuario = {
            iUsuario: $(lstCaja.txtusuarioID).val(),
            sUsuario: $(lstCaja.txtusuario).val(),
            sNombre: $(lstCaja.cmbusuario + " :selected").text().split("-")[1],
            sCorreo: $(lstCaja.txtcorreo).val(),
            iPerfil: $(lstCaja.cmbperfil).val(),
            sPermisosmod: "" + permisosasig + "",
            sPermisodiario: "" + lstDiarioSelected + ""
            
        };
    }

    this.m_EventoBusqueda = function () {
        var ousuario = { iUsuario: -1 };
        pm = "usuario: " + JSON.stringify(ousuario);
        oThis.Listausuarios(pm);
        oGlob.m_CloseFilters();
        oGlob.m_CierraCrudModal();
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

    this.m_InicializamodalEvents = function (jClaveuser) {

        pm = "usuario:'" + jClaveuser + "'";
        oThis.m_Listamodulos(pm); 
        const oaUsuario = {
            sUsuario: jClaveuser,
            iUsuario: -1
        };
        pm = "usuario: " + JSON.stringify(oaUsuario);
        oThis.ListausuariosOPE(pm);
        oThis.Listaperfiles();

        $(lstCaja.cmbperfil).off('change');
        $(lstCaja.cmbperfil).change(function () {
            oGlob.m_DeshabilitaElemento('.tw-control', false);
            $('.tw-control').prop('checked', false);
            oThis.m_CargadatosPerfil($(this).val());
        });       

        $(lstCaja.cmbusuario).off('change');
        $(lstCaja.cmbusuario).change(function () {
            var data_value = $(this).val().split("-");
            $(lstCaja.txtusuario).val(data_value[0]);
            $(lstCaja.txtusuarioID).val(data_value[1]);
        });

        oGlob.m_CargaDocumentos(lstCaja.cmbdiarios, true);
        
    }

    this.m_CargadatosPerfil = function (perfil) {
        var lblmensj = "";
        
        if (perfil != "") {
            if (perfil == "1") {
                lblmensj = "Los usuarios de tipo <b><u>Sistemas</u></b> tienen acceso sobre todos los derechos y permisos del sistema. <b>No se requiere marcar permisos sobre modulos</b>";
                oGlob.m_DeshabilitaElemento('.tw-control', true);
                $('.tw-control').prop('checked', true);                
            }
            else if (perfil == "2")
                lblmensj = "Los usuarios de tipo <b><u>Administrador</u></b> tienen todo los derechos sobre el sistema <u><b>pero requiere que se le asignen los permisos</b></u> sobre los modulos.";
            else
                lblmensj = "Este usuario contará con los siguientes derechos y <u><b>se le deben asignar sus correspondientes permisos</b></u> a los modulos.";
            pm = "perfil:" + perfil;
            oThis.m_Cargaperfilpermisos(pm);
        }
        $(lstElemnts.lblderechos).html(lblmensj);
    }


    this.m_ValidaCampos = function (bAlert) {

        var permisosasig = $(div_modulos).treeview('selectedValues');
         var bContinua = true;
        var sCadena = "";

        if ($(lstCaja.cmbperfil).val() == "")
            sCadena = "Favor de determinar el perfil del usuario";
        if (!oGlob.ValidaMail($(lstCaja.txtcorreo).val()))
            sCadena = "Favor de proporcionar un correo válido";
        if ($(lstCaja.cmbusuario).val() == "")
            sCadena = "Favor de seleccionar el usuario OPEADM";
        if ($(lstCaja.txtusuario).val() == "")
            sCadena = "Favor de capturar el alias del usuario";
        if ($(lstCaja.cmbperfil).val() != "1" && permisosasig == "")
            sCadena = "Favor de seleccionar los modulos(accesos) para el usuario"; 

        if (sCadena.length > 0) {
            if (bAlert)
                oGlob.m_AlertWarning(sCadena);
            bContinua = false;
        }

        return bContinua;
    }

    this.m_Guardausuario = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "Nuevousuario",
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
                        oGlob.m_AlertSuccess('El registro de usuario se ha guardado con éxito');
                        oThis.m_LimpiaCajasForm();
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
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_Editausuario = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "Editausaurio",
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
                        oGlob.m_AlertSuccess('El registro de usuario se ha editado con éxito');
                        oThis.m_LimpiaCajasForm();
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
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_EntityToFormByID = function (jDataValue) {
        var oUsuario = {
            sUsuario: jDataValue
        };
        pm = "usuario: " + JSON.stringify(oUsuario);
        $.ajax({
            type: "POST",
            url: Controller + "ListaUsuarios",
            data: "{" + pm + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var oJSONUser = response.data[0];
                        
                        $(lstCaja.txtNombre).val(oJSONUser.sNombre);
                        $(lstCaja.txtcorreo).val(oJSONUser.sCorreo);
                        $(lstCaja.cmbperfil).val(oJSONUser.iPerfil).trigger("chosen:updated.chosen");
                        $(lstCaja.cmbusuario).val(oJSONUser.sUsuario + "-" + oJSONUser.iUsuario).trigger("chosen:updated.chosen");
                        $(lstCaja.cmbusuario).prop('disabled', true).trigger("chosen:updated");
                        $(lstCaja.txtusuario).val(oJSONUser.sUsuario);
                        $(lstCaja.txtusuarioID).val(oJSONUser.iUsuario);
                        oThis.m_CargadatosPerfil(oJSONUser.iPerfil);
                        
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
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.Listausuarios = function (oParams) {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "ListaUsuarios",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                //oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, true);
                oGlob.m_ViewloaderGrid(lstTablas.tblusuarios);
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        oThis.m_CargaDataTable(response.data);
                        //oGlob.m_DeshabilitaElemento(lstBotones.btnDescarga, false);
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
                oGlob.m_HideloaderGrid(lstTablas.tblusuarios);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblusuarios);
            }
        });
    }  

    this.Listaperfiles = function () {
        $.ajax({
            type: "POST",
            url: Controller + "Listaperfiles",
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        //oThis.m_CargaDataTable(response.data);
                        var opcion = "";
                        var select = $(lstCaja.cmbperfil);
                        select[0].options.length = 0;

                        opcion = $("<option />", {
                            value: "",
                            text: "Selecciona el perfil del usuario"
                        });
                        select.append(opcion);

                        for (var i = 0; i < response.data.length; i++) {

                            opcion = $("<option />", {
                                value: response.data[i].rol,
                                text: response.data[i].rol + " - " + response.data[i].nombre
                            });
                            select.append(opcion);
                        }
                        $(lstCaja.cmbperfil).chosen();

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

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblusuarios);
            }
        });
    }  


    this.m_Listadocumentos = function (jUsuario) {        
        $.ajax({
            type: "POST",
            url: "../Metodos/D365/Configuracion-Datos.aspx/ListaDocumentosUsuario",
            data: "{Usuario: '" + jUsuario+"' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { 
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var lstdocumentos = new Array();
                        for (var i = 0; i < response.data.length; i++)
                            lstdocumentos.push(response.data[i].numerodoc);
                        $(lstCaja.cmbdiarios).multipleSelect('setSelects', lstdocumentos);
                        break;
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) { 
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
            }
        });
    } 

    this.m_Cargaperfilpermisos = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "Listapermisosperfil",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":

                        var div_checks = '';
                        if (response.data.length > 0) {
                            for (var i = 0; i < response.data.length; i++) {
                                div_checks += '<div class="col-4">';
                                div_checks += '<div class="form-check">';
                                div_checks += '<input type="checkbox" class="form-check-input" id="chk' + i + '" checked disabled>';
                                div_checks += '<label class="form-check-label" for="chk' + i + '">' + response.data[i].descripcion + '</label>';
                                div_checks += '</div > ';
                                div_checks += '</div > ';
                            }
                            div_checks += '</br > ';
                            $(lstElemnts.dvderechos).html(div_checks);
                        }
                        break;
                    default:
                        $(lstElemnts.dvderechos).html('<b>SIN DERECHOS ASIGNADOS</b>');
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_Listamodulos = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "Listamodulos",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var dataHTML = '<ul>';
                        var data_value = new Array();

                        if (response.data.length > 0) {

                            for (var x = 0; x < response.data.length; x++) {

                                if (response.data[x].nivel == 0) {

                                    dataHTML += '<li data-value="' + response.data[x].clave + '">  ' + response.data[x].nombre + '<ul>';

                                    for (var j = 0; j < response.data.length; j++) {

                                        if (response.data[j].padre == response.data[x].clave) {

                                            dataHTML += '<li data-value="' + response.data[j].clave + '" data-padre="' + response.data[j].padre + '">  ' + response.data[j].nombre + '</li>';
                                            if (response.data[j].bCheck)
                                                data_value.push(response.data[j].clave);
                                        }
                                    }

                                    dataHTML += "</ul></li>";
                                }
                            } 
                        }
                        dataHTML += '</ul>';
                        $(div_modulos).html(dataHTML);
                        $(div_modulos).treeview({
                            data: data_value
                        });
                        break;
                    default:
                        oGlob.m_AlertWarning('No se han encontrado los permisos para el usuario');
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }



    this.ListausuariosOPE = function (oParams) {
        $.ajax({
            type: "POST",
            url: Controller + "ListaUsuariosOPE",
            data: "{" + oParams + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        //oThis.m_CargaDataTable(response.data);
                        var opcion = "";
                        var select = $(lstCaja.cmbusuario);
                        select[0].options.length = 0;
                        
                            opcion = $("<option />", {
                                value: "",
                                text: "Selecciona el usuario"
                            });
                            select.append(opcion);
                        
                        for (var i = 0; i < response.data.length; i++) {

                            opcion = $("<option />", {
                                value: response.data[i].sUsuario + "-" + response.data[i].iUsuario,
                                text: response.data[i].sUsuario + " - " + response.data[i].sNombre
                            });                            
                            select.append(opcion);
                        }
                        $(lstCaja.cmbusuario).chosen();            

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
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {                
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se encuentra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblusuarios);
            }
        });
    }  

    this.Editarow = function (evt, jActivo, jUsuario) {

        if (jActivo == 1) {
            oGlob.m_FormEditTomodal(evt, function (response) {
                ///cargar datos del item 
                if (response) {
                    if (response) {

                        oGlob.m_OcultaElemnto('.css-control-add', true);
                        oGlob.m_OcultaElemnto('.css-control-edit', false);
                        oThis.m_InicializamodalEvents(jUsuario);                        
                        $(lstCaja.cmbperfil).prop('readonly', true);                        
                        oThis.m_EntityToFormByID(jUsuario);
                        oThis.m_Listadocumentos(jUsuario);
                    }
                }
            });
        } else oGlob.m_AlertError('No se permite editar un registro que ha sido eliminado');
    }

    this.Eliminarow = function (jActivo, jUsuario) {
        if (jActivo == 1) {
            oGlob.m_Confirma_Accion('Al desactivar la cuenta esta dejará de ser funcional y el usuario perderá acceso al sistema. ¿Deseas continuar?', function (response) {
                if (response)
                    oThis.m_EliminaUsuario(jUsuario);
            });
        } else oGlob.m_AlertError('La acción no se puede realizar sobre un usuario que ya ha sido eliminado.');
    }

    this.m_CargaDataTable = function (jdataJSON) {

        var jNameGrid = lstTablas.tblusuarios;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center",  },
                { label: ".", name: "rDelete", type: "text", width: 50, align: "center", },
                { label: "Usuario", name: "sUsuario", type: "text", width: 220, align: "left" },
                { label: "Nombre", name: "sNombre", type: "text", width: 350, align: "left" },                
                { label: "Fecha registro", name: "sFechaalta", type: "text", width: 150, align: "center" },
                { label: "Usuario registró", name: "sFechaalta", type: "text", width: 150, align: "center" },
                {
                    label: "Activo", name: "iActivo", width: 80, editable: true, edittype: 'checkbox', editoptions: { value: "1:0" }, align: "center",
                    formatter: "checkbox", formatoptions: { disabled: true }
                },
                { label: "Última fecha edición", name: "sFechaalta", type: "text", width: 150, align: "center" },
                { label: "Usuario editó", name: "sFechaalta", type: "text", width: 150, align: "center" },


            ],
            caption: 'Listado de usuarios registrados',
            multiselect: false,
            viewrecords: true,
            rownumbers: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\"  data-title=\"Edita registro de usuario\" data-key=\"" + row.sUsuario + "\" data-page=\"D365/Gestion-usuarios-Formulario\" onClick=\"oGestusuario.Editarow(this, " + row.iActivo + ", '" + row.sUsuario + "')\" ><i class=\"fas fa-pen\"></i></button>";
                    var rowDelete = "<button type=\"button\" class=\"btn btn-sm btn-danger\" onClick=\"oGestusuario.Eliminarow(" + row.iActivo + ", '" + row.sUsuario + "')\"><i class=\"fas fa-trash-alt\"></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit, rDelete: rowDelete });
                }
            },
            pager: "#jqGridPager"
        });
    }
}