function Perfil() {
    var oThis = this, oParams = "", size_permisos = -1,
        lstCajas = {
            txtNombrep: "#txtFormNombre", txtID: "#txtPerfilID", txtdescrip: "#txtdescrip", txtcuenta_desde: "#txtcuenta_desde",
            txtcuenta_hasta: "#txtcuenta_hasta", txtmonto_desde: "#txtmonto_desde", txtmonto_hasta: "#txtmonto_hasta"
        },
        lstElemnts = { checks_per: "#div_accesos" },
        lstTablas = { tblperfiles: "#tbl-pefiles", tblmatriz:"#tbl-cuentas-matriz" },
        lstBotones = {
            btnModalAdd: "#btnModAdd", btnAdd: "#btnGuarda", btnEdit: "#btnEdita", chkactivo: "#chkactivos",
            chkdel: "#chkeliminados", btnAddConcepto: "#btnAddconcepto"
        },
        Controller = "Metodos/D365/Perfiles-Datos.aspx/";

    this.m_InicializaEvents = function () {
        oThis.m_ListaPefiles();
        $(lstBotones.btnAdd).unbind();
        $(lstBotones.btnAdd).click(function () {

            if (oThis.m_ValidaCampos(true))
                oThis.m_GuardaInformacion();

        });

        $(lstBotones.btnModalAdd).unbind();
        $(lstBotones.btnModalAdd).click(function () {
            oGlob.m_FormEditTomodal(this, function (response) {
                if (response) {
                    oThis.m_InicializamodalEvents();
                    oGlob.m_OcultaElemnto('.css-control-add', false);
                    oGlob.m_OcultaElemnto('.css-control-edit', true);
                }
            });

        });

        $(lstBotones.btnEdit).unbind();
        $(lstBotones.btnEdit).click(function () {

            if (oThis.m_ValidaCampos(true)) {
                let permisosasig = $(lstElemnts.checks_per).treeview('selectedValues');
                let data = $(lstTablas.tblmatriz).jqGrid("getGridParam", "data");                

                var oPerfil = {
                    nombre: $(lstCajas.txtNombrep).val(),
                    rol: $(lstCajas.txtID).val(),
                    activo: 1,
                    permisos: "" + permisosasig + "",
                    lstMatriz: data
                };
                pm = "perfil:" + JSON.stringify(oPerfil);
                oThis.m_EditaRegistroPerfil(pm);
            }
        });

    }

    this.m_InicializamodalEvents = function () {

        
        oThis.m_ListaAccesos([]);
        oThis.m_CargaDataTableMatriz([]);
        $(lstBotones.btnAddConcepto).unbind();
        $(lstBotones.btnAddConcepto).click(function () {
            if (oThis.m_ValidaConceptoCampos(true)) {
                let bContinuar = true;
                var lstDatos = new Array(0);
                var data = $(lstTablas.tblmatriz).jqGrid("getGridParam", "data");
                 

                var orecord = {
                    descripcion: $(lstCajas.txtdescrip).val(),
                    cuenta_inicial: $(lstCajas.txtcuenta_desde).val(),
                    //cuenta_final: $(lstCajas.txtcuenta_hasta).val(),
                    rango_inicial: $(lstCajas.txtmonto_desde).val(),
                    rango_final: $(lstCajas.txtmonto_hasta).val() 
                };

                if (data != null) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].cuenta_inicio == $(lstCajas.txtcuenta_desde).val() ) {
                            oGlob.m_AlertWarning('El rango de cuentas: <b>' + $(lstCajas.txtcuenta_desde).val()  + '</b> ya se encuentra configurado para la matriz.');
                            bContinuar = false;
                            break;
                        }
                        var orow = {
                            descripcion: data[i].descripcion,
                            cuenta_inicial: data[i].cuenta_inicial,
                            //cuenta_final: $(lstCajas.txtcuenta_hasta).val(),
                            rango_inicial: data[i].rango_inicial,
                            rango_final: data[i].rango_final 
                        };
                        lstDatos.push(orow);
                    }
                    lstDatos.push(orecord);
                } else lstDatos.push(orecord);

                if (bContinuar) {
                    oGlob.m_AlertSuccess('El concepto <b>' + $(lstCajas.txtdescrip).val() + '</b> ha sido agregado');
                    $(lstCajas.txtdescrip).val("");
                    $(lstCajas.txtcuenta_desde).val("");
                    //$(lstCajas.txtcuenta_hasta).val("");
                    $(lstCajas.txtmonto_desde).val("");
                    $(lstCajas.txtmonto_hasta).val(""); 
                    oThis.m_CargaDataTableMatriz(lstDatos);
                }


            }
        });
    }
    this.m_GuardaInformacion = function (jFileExcel) {

        var permisosasig = $(lstElemnts.checks_per).treeview('selectedValues');
        let data = $(lstTablas.tblmatriz).jqGrid("getGridParam", "data"); 
            
        var oPerfil = {
            nombre: $(lstCajas.txtNombrep).val(),
            permisos: "" + permisosasig + "",
            lstMatriz: data
        };
        pm = "perfil:" + JSON.stringify(oPerfil);
        oThis.m_GuardaRegistroPerfil(pm);
    }
    this.m_ValidaCampos = function (bAlert) {

        var bContinua = true;
        var sCadena = "";
        var permisosasig = $(lstElemnts.checks_per).treeview('selectedValues');
        let data = $(lstTablas.tblmatriz).jqGrid("getGridParam", "data");

        if ($(lstCajas.txtNombrep).val() == "")
            sCadena = "Favor de capturar el nombre del Perfil/rol";

        if (permisosasig == "")
            sCadena = "Favor de asignar los permisos del Perfil/rol";

        if (permisosasig.length == size_permisos)
            sCadena = "Ya existen perfiles que tienen acceso a todos los permisos";

        if (data != null) {

            if (data.length == 0)
                sCadena = "No se ha determinado la matriz de aprobación para el perfil";

        } else sCadena = "No se ha determinado la matriz de aprobación para el perfil";

        if (sCadena.length > 0) {
            if (bAlert)
                oGlob.m_AlertWarning(sCadena);
            bContinua = false;
        }

        return bContinua;
    }

    this.m_ValidaConceptoCampos = function (bAlert) {

        var bContinua = true;
        var sCadena = ""; 

        if ($(lstCajas.txtmonto_desde).val() == "")
            sCadena = "Favor de capturar el monto para el rango inicial";

        if ($(lstCajas.txtmonto_hasta).val() == "")
            sCadena = "Favor de capturar el monto para el rango final";

        if ($(lstCajas.txtcuenta_desde).val() == "")
            sCadena = "Favor de capturar el rango de cuenta inicial";

        //if ($(lstCajas.txtcuenta_hasta).val() == "")
         //   sCadena = "Favor de capturar el rango de cuenta final";

        if ($(lstCajas.txtdescrip).val() == "")
            sCadena = "Favor de capturar la descripción de la matriz";

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
        $.each(lstCajas, function (key, value) {
            $(value).val("");
        });
    }

    this.m_ListaAccesos = function (jDataChecks) {

        $.ajax({
            type: "POST",
            url: Controller + "Listapermisos",
            data: null,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                //oGlob.m_showLoader();
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        size_permisos = response.data.length;
                        var dataHTML = '';
                        for (var c = 0; c < response.data.length; c++) {
                            dataHTML += "<div class='col-4'>";
                            dataHTML += '<li data-value="' + response.data[c].permiso + '">  ' + response.data[c].descripcion + '';
                            dataHTML += "</li>";
                            dataHTML += "</div>";
                        }
                        dataHTML += '';
                        $(lstElemnts.checks_per).html(dataHTML);
                        $(lstElemnts.checks_per).treeview({
                            data: jDataChecks
                        });
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                //oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //oGlob.m_hideLoader();
                //oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_ListaAccesosByPerfil = function (jRrolID) {

        $.ajax({
            type: "POST",
            url: Controller + "Listaperfilpermisos",
            data: "{perfilID: " + jRrolID + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                //oGlob.m_showLoader();
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var datos = new Array();
                        for (var c = 0; c < response.data.length; c++) {
                            datos.push(response.data[c].permiso);
                        }
                        oThis.m_ListaAccesos(datos);
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                //oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //oGlob.m_hideLoader();
                //oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_ListaMatrizByPerfil = function (jRrolID) {

        $.ajax({
            type: "POST",
            url: Controller + "Listamatrizperfil",
            data: "{perfilID: " + jRrolID + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                //oGlob.m_showLoader();
            },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":                        
                        oThis.m_CargaDataTableMatriz(response.data);
                        break;
                    default:
                        oGlob.m_AlertWarning('No se ha encontrado la matriz de aprobación para el pefil');
                        break;
                }
                //oGlob.m_hideLoader();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //oGlob.m_hideLoader();
                //oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
            }
        });
    }

    this.m_ListaPefiles = function () {
        oThis.m_CargaDataTable([]);
        $.ajax({
            type: "POST",
            url: Controller + "Listaperfiles",
            data: "{perfil: {} }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () {
                oGlob.m_showLoader();
                oGlob.m_ViewloaderGrid(lstTablas.tblperfiles);
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
                        //oGlob.m_AlertWarning('No se ha encontrado información con los filtros proporcionados');
                        break;
                    default:
                        oGlob.m_AlertWarning(response.mensaje);
                        break;
                }
                oGlob.m_hideLoader();
                oGlob.m_HideloaderGrid(lstTablas.tblperfiles);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                oGlob.m_hideLoader();
                oGlob.m_AlertError('Error de conexión con el servidor. El servidor no responde o se enceuntra muy ocupado. Intente nuevamente');
                oGlob.m_HideloaderGrid(lstTablas.tblperfiles);
            }
        });
    }

    this.m_EditaRegistroPerfil = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "Editaperfil",
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
                        oGlob.m_AlertSuccess('El registro de perfil/rol se ha actualizado con éxito');
                        oThis.m_LimpiaCajasForm();
                        oThis.m_ListaPefiles();
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
                        oThis.m_LimpiaCajasForm();
                        oThis.m_ListaPefiles();
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

    this.m_GuardaRegistroPerfil = function (oParams) {

        $.ajax({
            type: "POST",
            url: Controller + "Nuevoperfil",
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
                        oGlob.m_AlertSuccess('El registro del perfil/rol se ha guardado con éxito');
                        oThis.m_LimpiaCajasForm();
                        oThis.m_ListaPefiles();
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
        var oPerfil = {
            rol: jDataValue
        };
        pm = "perfil:" + JSON.stringify(oPerfil);
        $.ajax({
            type: "POST",
            url: Controller + "Listaperfiles",
            data: "{" + pm + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function () { },
            success: function (result) {
                var response = result["d"];
                switch (response.estatus) {
                    case "OK":
                        var oJSONPerfil = response.data[0];
                        $(lstCajas.txtNombrep).val(oJSONPerfil.nombre);
                        $(lstCajas.txtID).val(oJSONPerfil.rol);
                        oThis.m_ListaAccesosByPerfil(oJSONPerfil.rol);
                        oThis.m_ListaMatrizByPerfil(oJSONPerfil.rol);                        
                        break;
                    case "-1":
                        oGlob.oGlob.m_showModalSession();
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

    this.Editarow = function (evt, jEliminado, jRolid) {

        //if (jEliminado == 0) {
            oGlob.m_FormEditTomodal(evt, function (response) {
                ///cargar datos del item 
                if (response) {
                    oThis.m_EntityToFormByID(jRolid);                    
                    oThis.m_InicializamodalEvents();
                    //oGlob.m_OcultaElemnto('.css-control-add', false);
                    //oGlob.m_OcultaElemnto('.css-control-edit', true);
                    
                    //oThis.m_CargaDataTableMatriz([]);
                }
            });
        //} else oGlob.m_AlertError('No se permite editar un registro que ha sido eliminado');
    }
    this.eliminaconcepto = function (evt, jID) {

        var row = $(lstTablas.tblmatriz).jqGrid('getRowData', jID);

        oGlob.m_Confirma_Accion('Al elimiar el registro este no puede ser utilizado y la información de cuentas no será visible para el usuario que tenga asignado el perfil.¿Estás seguro de <b>eliminar</b> el concepto <b>' + row.concepto + '</b>?', function (response) {
            if (response) {
                var jNameGrid = lstTablas.tblmatriz;
                $(jNameGrid).jqGrid('delRowData', jID);
            }
        });
    } 


    this.m_CargaDataTable = function (jdataJSON) {


        var jNameGrid = lstTablas.tblperfiles;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rEdit", type: "text", width: 50, align: "center", hidden: $("#hdEditar").val() == "0" },
                { label: "Nombre", name: "rol", type: "text", width: 300, hidden: true },
                { label: "Nombre", name: "nombre", type: "text", width: 300 },
                { label: "Fecha de registro", name: "fechacreacion", type: "text", width: 180 },
                { label: "Usuario registró", name: "usuarioID", type: "text", width: 200 },
                {
                    label: "Activo", name: "activo", width: 80, editable: true, edittype: 'checkbox', editoptions: { value: "1:0" }, align: "center",
                    formatter: "checkbox", formatoptions: { disabled: true }
                },
                { label: "Última fecha edición", name: "fechacreacion", type: "text", width: 180 },
            ],
            caption: 'Listado de pefiles válidos',
            viewrecords: true,
            //multiselect: true,
            height: "25em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowEdit = "<button type=\"button\" class=\"btn btn-sm btn-info\" data-title=\"Edita registro de perfil/rol\" data-key=\"" + row.rol + "\" data-page=\"D365/Gestion-perfiles-Formulario\" onClick=\"oPerfilctrol.Editarow(this, " + row.activo + ", '" + row.rol + "')\"><i class=\"fas fa-pen \" ></i></button>";
                    $(jNameGrid).jqGrid('setRowData', key, { rEdit: rowEdit });
                }
            },
            pager: "#jqGridPager"
        });
    }

    this.m_CargaDataTableMatriz = function (jdataJSON) {

        var jNameGrid = lstTablas.tblmatriz;
        $(jNameGrid).jqGrid('clearGridData', true).trigger('reloadGrid');
        $.jgrid.gridUnload(jNameGrid);
        $(jNameGrid).jqGrid({
            datatype: "local",
            data: jdataJSON,
            styleUI: 'Bootstrap',

            colModel: [
                { label: ".", name: "rDelete", type: "text", width: 50, align: "center", },
                { label: "Descripción de cuenta", name: "descripcion", type: "text", width: 300, align: "left" },
                { label: "Cuenta contable", name: "cuenta_inicial", type: "text", width: 140, align: "left" },
                //{ label: "Cuenta fin", name: "cuenta_final", type: "text", width: 140, align: "center" },
                {
                    label: "Importe inicial", name: "rango_inicial", type: "number", width: 140, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },
                {
                    label: "Importe inicial", name: "rango_final", type: "number", width: 140, align: "center", formatter: 'currency',
                    formatoptions: { prefix: '$', suffix: '', thousandsSeparator: ',', decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
                },


            ],
            caption: 'Matriz de aprobaciones para el perfil',
            multiselect: false,
            viewrecords: true,
            rownumbers: true,
            height: "18em",
            rowNum: 30,
            gridComplete: function () {
                var keys = $(jNameGrid).jqGrid('getDataIDs');
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    var row = $(jNameGrid).jqGrid('getRowData', key);
                    var rowDelete = "<button type=\"button\" title=\"Eliminar concepto\" class=\"btn btn-sm btn-danger\"  onClick=\"oPerfilctrol.eliminaconcepto(this, " + key + ")\"><i class=\"fas fa-trash-alt\" ></i></button>";

                    $(jNameGrid).jqGrid('setRowData', key, { rDelete: rowDelete });
                }
            },
            pager: "#jqGridPagermatriz"
        });
    }
}