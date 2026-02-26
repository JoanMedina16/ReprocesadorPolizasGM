<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Informe-esatado-resultados.aspx.cs" Inherits="LOGIWEB.Paginas.D365.Informe_esatado_resultados" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
    <meta charset="utf-8" />
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Estado de resultados</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../../Librerias/fontawesome-free/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="../../Librerias/dist/css/adminlte.min.css" />
    <link rel="stylesheet" href="../../Librerias/jquery-confirm/jquery-confirm.css" />

    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="../../Librerias/jquery/jquery.min.js"></script>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../../Librerias/bootstrap/js/bootstrap.bundle.js"></script>
</head>
<body>
    <form id="form1" runat="server">
                <asp:ScriptManager runat="server" ID="Scrptmager" AsyncPostBackTimeout="36000"></asp:ScriptManager>

        <div class="card-body">
            <section class="content-header">
                <div class="container-fluid">
                    <div class="col-12">
                        <div class="row">
                            <div class="col-4">
                                <label>Fecha contable desde</label>
                                <asp:TextBox class="form-control" ID="txtDesde" runat="server" placeholder="dd/MM/yyyy"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <label>fecha contable hasta</label>
                                <asp:TextBox class="form-control" ID="txtHasta" runat="server" placeholder="dd/MM/yyyy"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                      <div class="col-12 text-right">
                            <div class="row justify-content-end">
                                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important; padding-top: 1.8em;">                                                                        
                                    <li class="">
                                        <asp:Button runat="server" ID="btnInforme" class="btn btn-info" Text="Genera reporte" OnClick="btnInforme_Click"  />
                                    </li>
                                    <li>&nbsp;</li>
                                </ul>
                            </div>
                        </div>

                    <div class="col-12" style="margin-top: 1.5em; margin-bottom: 3em">
                        <rsweb:ReportViewer ID="rptInforme" runat="server" AsyncRendering="false" Width="100%" Height="600" WaitMessageFont-Bold="true" Visible="true">
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </section>
        </div>
    </form>
       <script src="../../Librerias/jquery/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../../Librerias/bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Librerias/dist/js/adminlte.js"></script>
    <script src="../../Librerias/jquery-confirm/jquery-confirm.js"></script>
    <script src="../../Librerias/jqgrid/jquery.jqGrid.min.js"></script>
    <script src="../../Librerias/jqgrid/grid.locale-es.js"></script>
    <script>
        $(document).ready(function () {
            IniciaCalendario("#txtDesde");
            IniciaCalendario("#txtHasta");
        });

        function IniciaCalendario (jInput) {

            $(jInput).datepicker({
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                dateFormat: 'dd/mm/yy'
            });
        }
        function Onwarning (jMessage) {

            $.confirm({
                title: '¡Advertencia!',
                content: jMessage,
                type: 'orange',
                typeAnimated: true,
                buttons: {
                    close: function () {
                        btnClass: 'btn-red'
                    }
                }
            });
        }
    </script>
</body>
</html>
