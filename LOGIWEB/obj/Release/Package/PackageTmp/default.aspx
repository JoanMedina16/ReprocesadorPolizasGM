<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="LOGIWEB._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Logística del Mayab</title> 
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="Librerias/fontawesome-free/css/all.min.css?<%=sVersion%>" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="Librerias/jqgrid/ui.jqgrid-bootstrap.css?<%=sVersion%>" />
    <link rel="stylesheet" type="text/css" href="../../../css/trirand/ui.jqgrid-bootstrap.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/dist/css/adminlte.min.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/jquery-confirm/jquery-confirm.css?<%=sVersion%>" />
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/MultipleSelect/multiple-select.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Estilos/custom.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/printjson/pretty-print-json.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/chart/Chart.min.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/simTree/simTree.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/overlayScrollbars/OverlayScrollbars.min.css?<%=sVersion%>" />
    <link rel="stylesheet" href="Librerias/AutocompleateChoose/bootstrap-chosen.css?<%=sVersion%>" />
</head>
<body class="sidebar-mini layout-fixed sidebar-collapse">
    <div class="wrapper">
        <!-- Navbar -->
        <nav class="main-header navbar navbar-expand navbar-dark">
            <!-- Left navbar links -->
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
                </li>
            </ul>

            <div class="row ml-auto">
                <div class="col-12 text-center text-white">
                    <p><%=sLeyenda%></p>
                </div>
            </div>

            
            <!-- Right navbar links -->
            <ul class="navbar-nav ml-auto">
                <!-- Notifications Dropdown Menu -->

                <li class="nav-item dropdown">
                    <a class="nav-link" data-toggle="dropdown" href="#">
                        <i class="fas fa-user-cog"></i>
                        <span class="badge badge-warning navbar-badge"></span>
                    </a>
                    <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
                        <span class="dropdown-item dropdown-header"><span id="Perfil"><b><%=sNombre%></b></span></span>
                        <div class="dropdown-divider"></div>
                        <a href="#" class="dropdown-item">
                            <i class="fas fa-warehouse mr-2"></i><%=sSucursal%>
                        </a>
                        <div class="dropdown-divider"></div>
                        <a href="#" class="dropdown-item">
                            <i class="fas fa-calculator mr-2"></i><%=sCentro%>
                        </a>
                        <div class="dropdown-divider"></div>
                        <a href="#" class="dropdown-item" onclick="oLogin.m_Close()">
                            <i class="fas fa-sign-out-alt mr-2"></i>Cerrar sesión
                        </a>
                        <div class="dropdown-divider"></div>
                        <a href="#" class="dropdown-item dropdown-footer">&nbsp;</a>
                    </div>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="#">&nbsp;
                    </a>
                </li>
            </ul>
        </nav>
        <!-- /.navbar -->
        <!-- Main Sidebar Container -->
        <aside class="main-sidebar sidebar-dark-primary elevation-4 sidebar-dark-orange">
            <!-- Brand Logo -->
            <div class="user-panel d-flex text-center justify-content-center">
                <br />
                <br />
            </div>
            <!-- Sidebar -->
            <div class="sidebar">
                <!-- Sidebar user (optional) -->
                <!-- Sidebar Menu -->
                <nav class="mt-2">

                    <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                    
                        <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                           <HeaderTemplate>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li class="nav-header"><%# DataBinder.Eval(Container.DataItem, "nombre") %></li>
                                <li class="nav-item has-treeview">
                                    <a href="#" class="nav-link" id="<%# DataBinder.Eval(Container.DataItem, "clave") %>">
                                        <i class="nav-icon fas  <%# DataBinder.Eval(Container.DataItem, "icono") %>"></i>
                                        <p>
                                            <%# DataBinder.Eval(Container.DataItem, "nombre") %>
                                    <i class="fas fa-angle-left right"></i>
                                        </p>
                                    </a>
                                    <ul class="nav nav-treeview">                        

                                <asp:Repeater runat="server" ID="rptDetalle" >
                                    <ItemTemplate>

                                        <li class="nav-item">
                                    <a href="<%# DataBinder.Eval(Container.DataItem, "navega") %>"" <%# DataBinder.Eval(Container.DataItem, "ref_nav") %> data-title="<%# DataBinder.Eval(Container.DataItem, "nombre") %>" data-lvuno="Catálogos" data-lvdos="" data-padre="<%# DataBinder.Eval(Container.DataItem, "padre") %>" data-page="<%# DataBinder.Eval(Container.DataItem, "pagina") %>" onclick="oGlob.m_CargaFormulario(this)" class="nav-link" id="<%# DataBinder.Eval(Container.DataItem, "clave") %>">
                                        <i class="nav-icon fas <%# DataBinder.Eval(Container.DataItem, "icono") %>""></i>
                                        <p><%# DataBinder.Eval(Container.DataItem, "nombre") %></p>
                                    </a>
                                </li>

                                    </ItemTemplate>
                                </asp:Repeater>
                                        </ul>
                                    </li>

                            </ItemTemplate>
                            <FooterTemplate>
                             
                            </FooterTemplate>
                        </asp:Repeater>

                    </ul>
                </nav>
                <!-- /.sidebar-menu -->
            </div>
            <!-- /.sidebar -->
        </aside>
        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header nonselect" id="headeNav" style="padding-bottom:0px !important">
                <div class="container-fluid">
                    <div class="row mb-2">
                        <div class="col-sm-6">
                            <h1 id="h1Title"></h1>
                        </div>
                        <div class="col-sm-6">
                            <ol class="breadcrumb float-sm-right">
                                <li class="breadcrumb-item"><b>Inicio</b></li>
                                <li class="breadcrumb-item" id="liSegundo">Estadísticos</li>
                                <li class="breadcrumb-item active" id="liTercero" style="display: none">Gestión área</li>
                                <li class="breadcrumb-item active" id="liTitulo">Logística del Mayab</li>
                            </ol>
                        </div>
                    </div>
                </div>
                <!-- /.container-fluid -->
            </section>
            <!-- Main content -->
            <div id="divContenedor-navigation">
                <div class="card-body">
                    <div class="row">
                        <div class="col-12 text-center">
                            <img src="Imagenes/lm_logistico.png" style="max-height: 220px; max-width: 220px !important;" class="brand-image" />
                        </div>

                        <div class="col-12 text-justify">
                            <br />
                            <br />
                            <div class="row">
                                <div class="col-1">&nbsp;</div>
                                <div class="col-10">
                                    <ul class="list-group text-justify">
                                        <li class="list-group-item d-flex align-items-center">
                                           <p>El proceso de replica hacia Dynamics 365 <b>tarda en promedio cinco minutos</b>. Si ha pasado más de una hora y el movimiento no se ve reflejado en D365 valida la bandeja de error.</p> 
                                        </li>
                                        
                                        <li class="list-group-item d-flex align-items-center">
                                          <p>En la bandeja de errores o incidencias encontrarás todas las pólizas que no pudieron ser interfazadas, valida el error y reportalo al departamento correspondiente. <b>Si alguna póliza no es corregida en el período contable que corresponde esta quedará fuera de la sincronización de Dynamics 365.</b> Si una póliza ya ha sido interfazada de manera manual puedes realizar el descarte de este movimiento y no generará afectación contable.</p></li>
                                        <li class="list-group-item d-flex align-items-center">
                                           <p>La bandeja de pólizas pendientes aloja los registros que aún no se han enviado a Dynamics 365. Las facturas que no pueden ser <b>timbradas por el proceso de MasterEdi</b> muestran un botón de color rojo el cual despliega el motivo por el que no se logró realizar el timbrado.</p> 
                                        </li>
                                        <li class="list-group-item d-flex align-items-center">
                                            <p>Puedes emitir búsquedas de pólizas por rangos de fechas creación, fecha contable, folio de factura y folio de asiento contable. En bandejas de historicos se presentan los últimos 150 movimientos.</p>
                                        </li>
                                        <li class="list-group-item d-flex align-items-center">
                                           <p>En la bandeja de errores puedes corregir los documentos XML (Factura de proveedor) para poder interfazar los datos.</p> 
                                        </li>
                                    </ul>
                                </div>

                                <!--<div class="col-2">&nbsp;</div>
                                <div class="col-8">
                                    <ul class="list-group-flush" style="list-style: none">
                                        <li class=""><b>&#8210;</b> El proceso de replica hacia Dynamics 365 <b>tarda en promedio cinco minutos</b>.Si ha pasado más de una hora y el movimiento no se ve reflejado en D365, valida la bandeja de error.</li>
                                        <li class=""><b>&#8210;</b> La bandeja de pólizas pendientes aloja los registros que aún no se han enviado a Dynamics 365.</li>
                                        <li class=""><b>&#8210;</b> Las facturas que no pueden ser timbradas por el proceso de MasterEdi, mostrarán un botón de color rojo el cual despliega el motivo por el que no se logró realizar el timbrado.</li>
                                        <li class=""><b>&#8210;</b> Puedes emitir búsquedas de pólizas por rangos de fechas creación, fecha contable y folio de asiento contable.</li>
                                        <li class=""><b>&#8210;</b> En la bandeja de errores o incidencias encontrarás todas las pólizas que no pudieron ser interfazadas, valida el error y reportalo al departamento correspondiente.</li>
                                        <li class=""><b>&#8210;</b> Si una póliza ya ha sido interfazada de manera manual puedes realizar el descarte de este movimiento y no generará afectación contable.</li>
                                        <li class=""><b>&#8210;</b> En la bandeja de errores puedes corregir los documentos XML (Factura de proveedor) para poder interfazar los datos.</li>
                                    </ul>
                                </div>-->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.content -->

          

            <!-- Contenido de reportes -->
            <form runat="server" autocomplete="off">
                  <!--ESTOS INPUST NOS PERMITEN SABER LOS PERMISOS DEL USUARIO SOBRE LOS MODULOS-->
             <asp:HiddenField runat="server" ID="hdAgregar" Value="0" />
                        <asp:HiddenField runat="server" ID="hdEditar" Value="0" />
                        <asp:HiddenField runat="server" ID="hdEliminar" Value="0" />
                        <asp:HiddenField runat="server" ID="hdDescargar" Value="0" />                                                
                        <asp:HiddenField runat="server" ID="hdBuscar" Value="0" />
                        <asp:HiddenField runat="server" ID="hdUploadcsv" Value="0" />
                        <asp:HiddenField runat="server" ID="hdEntradaAX" Value="0" />
                        <asp:HiddenField runat="server" ID="hdRsterror" Value="0" />
                        <asp:HiddenField runat="server" ID="hdAdjuntaxml" Value="0" />
                        <asp:HiddenField runat="server" ID="hddPrecio" Value="0" />
            <!--ESTOS INPUST NOS PERMITEN SABER LOS PERMISOS DEL USUARIO SOBRE LOS MODULOS-->
            </form>
            <!-- /.Contenido de reportes -->
        </div>

        <div class="modal fade" id="modalsession" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content nonselect">
                    <div class="modal-header">
                        <h5 class="modal-title"><b>¡Su sesión ha expirado!</b></h5>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-3 justify-content-center text-center">
                                <div class="fa-3x text-center" id="div-icon-loader">
                                    <i class="fa fa-exclamation-triangle fa-2x" style="cursor: pointer; color: orange;"></i>
                                </div>
                            </div>
                            <div class="col-9">
                                <p style="padding-top: 1.5em">La sesión ha caducado. Favor de volver a ingresar al sistema para continuar navegando.</p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <a href="login.aspx" class="btn btn-secondary">Entendido</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalpermisoacceso" tabindex="-1" role="dialog" aria-hidden="true" style="z-index: 10000000">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content nonselect">
                    <div class="modal-header">
                        <h3><i class="fas fa-exclamation-triangle text-danger"></i>¡Oops! Algo salió mal</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <p style="padding-top: 1.5em">La acción no se ha podido realizar, no cuenta con los debidos permisos o estos han sido cambiados. Póngase en contacto con su Administrador.</p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-dismiss="modal" onclick="window.location.reload();">Entendido</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-logs" tabindex="-3" role="dialog" data-backdrop="false" style="z-index: 10000000">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="lbllogtitulo">Informe de resultado de procesos</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12 text-danger classequiv" id="mensajeequiv" ></div>
                            <div class="col-12 jgridlogs" id="tbl-logs-div">
                                <table id="tbl-logs-proceso" class="table table-bordered table-hover"></table>
                                <div id="jqGridPagerLogs"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <p class="text-left" id="total"></p>
                        <button class="btn btn-info" type="button" id="btnDowdetalle" title="Descarga información" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button>
                        <button class="btn btn-default" type="button" data-dismiss="modal">Entendido</button>
                    </div>
                </div>
            </div>
        </div>

        <!--<h3><i class="fas fa-exclamation-triangle text-danger"></i> ¡Oops! Algo salió mal</h3>-->
        <div class="modal fade" id="modalcrud" tabindex="-100" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <i class="fas fa-file-contract css-control-add fa-2x text-muted"></i>&nbsp;&nbsp;
                        <i class="fas fa-edit css-control-edit fa-2x text-muted"></i>&nbsp;&nbsp;
                        <h5 class="modal-title" id="lbltitulo" style="padding-top: 2px"></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" id="div-crud-body" style="overflow-y: auto !important;">
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                        <button class="btn btn-success css-control-add" type="button" id="btnGuarda">Guardar registro</button>
                        <button class="btn btn-success css-control-edit" type="button" id="btnEdita">Actualizar registro</button>
                    </div>
                </div>
            </div>
        </div>


        <div class="modal fade" id="modalloader" tabindex="-1" role="dialog" aria-hidden="true" style="z-index: 9000000000000">
            <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
                <div class="modal-content nonselect">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12 justify-content-center text-center">
                                <div class="fa-6x text-center">
                                    <img src="Imagenes/loader.gif" width="170" />  
                                </div>
                            </div>
                            <div class="col-12 text-center">
                                <p>¡Espera un momento la información se está procesando!</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalcliente" tabindex="-100" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Catálogo de clientes</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" style="overflow-y: auto !important;">

                        <div class="row">
                            <div class="col-3">
                                <label>Nombre del cliente:</label>
                                <input class="form-control css-texto" id="txtclientemod" onkeypress="" type="text" placeholder="Nombre del cliente" />
                            </div>
                            <div class="col-3">
                                <label>RFC:</label>
                                <input class="form-control css-texto" id="txtrfcmod" onkeypress="" type="text" placeholder="RFC del cliente" />
                            </div>                            
                            <div class="col-2">
                                <label>Cuenta:</label>
                                <input class="form-control css-texto" id="txtcuentamod" onkeypress="" type="text" placeholder="Cuenta del cliente" />
                            </div>
                            
                            <div class="col-2">
                                <label>Subcuenta:</label>
                                <input class="form-control css-texto" id="txtsubcuentamod" onkeypress="" type="text" placeholder="Subcuenta del cliente" />
                            </div>
                            <div class="col-2">
                                <br />
                                <div class="form-check">
                                    <label>&nbsp;</label>
                                    <button class="btn btn-secondary" type="button" id="btnbusquedaClie">Buscar</button>
                                </div>
                            </div>

                            <div class="col-12">
                                <br />
                                <table id="tbl-clientes" class="table table-bordered table-hover"></table>
                                <div id="jqGridPageclien"></div>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- /.content-wrapper -->
        <footer class="main-footer">
            <strong>Copyright &copy; Logística del Mayab.</strong>Todos los derechos reservados.   
        <div class="float-right d-none d-sm-inline-block">
            <b>Version</b> <%=sVersion%>
        </div>
        </footer>
        <!-- Control Sidebar -->
        <aside class="control-sidebar control-sidebar-dark">
            <!-- Control sidebar content goes here -->
        </aside>
        <!-- /.control-sidebar -->
    </div>
    <script src="Librerias/jquery/jquery.min.js?<%=sVersion%>"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="Librerias/bootstrap/js/bootstrap.bundle.js?<%=sVersion%>"></script>
    <script src="Librerias/jqgrid/jquery.jqGrid.min.js?<%=sVersion%>"></script>
    <script src="Librerias/jqgrid/grid.locale-es.js?<%=sVersion%>"></script>
    <script src="Librerias/printjson/pretty-print-json.js?<%=sVersion%>"></script>
    <script src="Librerias/dist/js/adminlte.min.js?<%=sVersion%>"></script>
    <script src="Librerias/jquery-confirm/jquery-confirm.js?<%=sVersion%>"></script>
    <script src="Librerias/MultipleSelect/jquery.multiple.select.js?<%=sVersion%>"></script>
    <script src="Librerias/simTree/simTree.js?<%=sVersion%>"></script>
    <script src="Librerias/overlayScrollbars/jquery.overlayScrollbars.min.js?<%=sVersion%>"></script>
    <script src="Librerias/AutocompleateChoose/chosen.js?<%=sVersion%>"></script>
    <!---SCRIPTS PARA CONTROL DE MODULOS Y EVENTOS DE LA PÁGINA--->
    <script src="Scripts/js-FuncionesGenerales.js?<%=sVersion%>"></script>
    <script src="Scripts/js-Login.js?<%=sVersion%>"></script> 
    <!--INICIO DE SCRIPS PARA MOTOR DE FACTURACIÓN--->
    <script src="Scripts/MOTF/js-Bandeja-portesviajes-Lista.js?<%=sVersion%>"></script> 
    <!--FIN DE SCRIPS PARA MOTOR DE FACTURACIÓN--->

    <!--INICIO DE SCRIPS PARA REPLICA DE DOCUMENTOS D365--->
    <script src="Scripts/D365/js-Enproceso-polizas-Lista.js?<%=sVersion%>"></script> 
    <script src="Scripts/D365/js-Historial-polizas-Lista.js?<%=sVersion%>"></script> 
    <script src="Scripts/D365/js-Incidencias-polizas-Lista.js?<%=sVersion%>"></script> 
    <script src="Scripts/D365/js-configuraciones-Lista.js?<%=sVersion%>"></script>     
    <script src="Scripts/D365/js-Nuevo-asistente-plantilla.js?<%=sVersion%>"></script>     
    <script src="Scripts/D365/js-Historial-asistente-plantilla.js?<%=sVersion%>"></script>   
    <script src="Scripts/D365/js-Credito-cliente-modal.js?<%=sVersion%>"></script>   
    <script src="Scripts/D365/js-Cierre-periodo-modal.js?<%=sVersion%>"></script>   
    <script src="Scripts/D365/js-Cargainicial-polizas-Lista.js?<%=sVersion%>"></script>       
    <script src="Scripts/D365/js-Configuracion-usuarios-Lista.js?<%=sVersion%>"></script>     
    <script src="Scripts/D365/js-Catalogos-perfiles-Lista.js?<%=sVersion%>"></script>     
    <script src="Scripts/D365/js-Unidades-vh-Lista.js?<%=sVersion%>"></script>     
      <script src="Scripts/D365/js-Consumo-Combustible-Lista.js?<%=sVersion%>"></script>     
      <script src="Scripts/D365/js-Facturas-Bebidas-Lista.js?<%=sVersion%>"></script>     
      <script src="Scripts/D365/js-Consulta-bitacora-Lista.js?<%=sVersion%>"></script>     
<!--FIN DE SCRIPS PARA REPLICA DE DOCUMENTOS D365--->

    <!--REPORTES UTILIZADOS PARA D365--->
        <script src="Scripts/REPORTES/D365/Informe-extraccion-datos.js?<%=sVersion%>"></script>     
        <script src="Scripts/REPORTES/D365/js-Matriz-aprobaciones-datos.js?<%=sVersion%>"></script>     
    
    <!--REPORTES UTILIZADOS PARA D365--->

    <script>
        var oGlob = null, oLogin = null, bEdicion = false, lastValue = null, iRoot = -1;
        $(document).ready(function () {
            oLogin = new Login();
            oGlob = new General();
            oGlob.m_showLoader();
            oLogin.m_validaSession(function (response) {
                if (response === false) {
                    window.location.href = "login.aspx"
                } else {
                    oGlob.m_hideLoader();
                    //oGlob.m_Information("Su sucursal configurada es <%=sSucursal%>: Cualquier cambio hecho al sistema se guardará tomando en cuenta este cambio. Si esto es incorrecto por favor consulte con el Gerente de Administración");
                    $("#btnhome").click();
                }
            });
        });
    </script>
</body>
</html>