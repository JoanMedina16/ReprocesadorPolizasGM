<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Generic-Table-Export.aspx.cs" Inherits="LOGIWEB.Paginas.Generic_Table_Export" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>BEPENSA</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../Librerias/fontawesome-free/css/all.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="../Librerias/dist/css/adminlte.min.css" />
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="../Librerias/jquery/jquery.min.js"></script>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../Librerias/bootstrap/js/bootstrap.bundle.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card-body">
            <div class="row">
                <div class="col-12 justify-content-center text-center">
                    <div class="fa-6x text-center" id="div-icon-loader">
                        <i class="fas fa-spinner fa-pulse" style="color: #f57c00"></i>
                    </div>
                </div>
                <div class="col-12 text-center">
                    <p id="lbl-mensaje">¡Procesando la información del archivo!</p>
                </div>
            </div>
        </div>
    </form>
    
</body>
</html>
