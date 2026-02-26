<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="LOGIWEB.login" %>

<html>
<head>    
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Logística del Mayab</title> 
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="google-signin-client_id" content="885332903508-409qkp9botd3c4h5pfsq83h25qfp6odc.apps.googleusercontent.com">
    <link rel="stylesheet" href="Librerias/fontawesome-free/css/all.min.css"> 
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="Librerias/dist/css/adminlte.min.css">
    <link rel="stylesheet" href="Librerias/jquery-confirm/jquery-confirm.css"> 
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet" />
    <link rel="stylesheet" href="Estilos/custom.css" />
    <link rel="icon" href="Imagenes/cropped32x32.png">
</head>
<body class="hold-transition login-page" style="background-color: #FFFFFF">
    <div class="login-box" style=" width: 35rem;">
        <div class="login-logo">
            <a href="#"><img src="Imagenes/lm_logistico.png" style="max-height: 140px; max-width: 350px !important;" class="brand-image"></a>
        </div>
        <br />
        <div class="card" style="background-color: transparent !important;  border: solid; border-color: #C18648">

            <div class="card-body login-card-body" style="background-color: transparent !important; padding-top: 1.8em !important;">
                <form runat="server" autocomplete="off">

                    <div class="row">
                        <div class="col-4">
                            <asp:Label ID="lblError" runat="server" ForeColor="Maroon" Font-Size="XX-Small">Servidor: sin seleccionar</asp:Label>
                            <asp:ListBox ID="lstServer" runat="server" OnSelectedIndexChanged="lstServer_SelectedIndexChanged" AutoPostBack="True" Style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" Height="132px" Width="132px"></asp:ListBox>
                        </div>
                        <div class="col-8">
                            <div class="row">
                                <div class="col-12">
                                    <div class="input-group mb-3">
                                        <input type="email" class="form-control css-texto" id="txtusuario" placeholder="Proporciona el usuario" required>

                                        <div class="input-group-append">
                                            <div class="input-group-text">
                                                <span class="fas fa-user"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div class="input-group mb-3">
                                        <input type="password" class="form-control css-texto" id="txtpassword" placeholder="Proporciona la contraseña" required>
                                        <div class="input-group-append handclass">
                                            <div class="input-group-text">
                                                <span class="fas fa-eye" id="btnviewpass"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12" style="margin-top:5px">                                    
                                    <button type="button" id="btnIngresa" class="btn btn-block btn-secondary" style="opacity:0.8">INICIAR SESIÓN</button>
                                </div>
                                <div class="col-12" style="margin-top:5px">
                                    <div class="g-signin2" data-onsuccess="onSignIn"></div>
                                </div>
                            </div>
                        </div>                        

                        <div class="row">                            
                            <div class="col-12">
                                <br />
                                    <asp:Label ID="lblTitulo" runat="server" Font-Bold="True" Font-Size="XX-Small">Sistema Integral de Administracion de Transportes.</asp:Label>                                
                                </div>
                            <div class="col-12">
                                <asp:Label ID="lblLeyenda" runat="server" Font-Size="XX-Small" >Bienvenido al sistema, Escriba su nombre de usuario y contraseña y haga clic en iniciar sesión.</asp:Label>
                            </div>
                        </div>
                    </div> 
                    <asp:HiddenField ID="hdnServer" runat="server" />
                    <asp:HiddenField ID="hdnDataBase" runat="server" />
                </form>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 text-center">
                <p style="color:#808080; opacity:0.3;">&copy; <%= DateTime.Now.Year %> - Logística del Mayab. Todos los derechos resevados.</p>
            </div>
           <div class="col-12 text-center">
              <a href="Manual/default.html" target="_blank" style="color:#808080; opacity:0.35">Consulta el manual de usuario</a>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalloader" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="col-12 justify-content-center text-center">
                            <div class="fa-6x text-center">
                               <img src="Imagenes/loader.gif" width="170" />                                
                            </div>
                        </div>
                        <div class="col-12 text-center"><p>¡Espera un momento los datos proporcionados se están validando...!</p></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="Librerias/jquery/jquery.min.js"></script>
    <script src="Librerias/bootstrap/js/bootstrap.bundle.js"></script>
    <script src="Librerias/jquery-confirm/jquery-confirm.js"></script>
    <script src="Scripts/js-FuncionesGenerales.js?0.0001"></script>
    <script src="Librerias/dist/js/adminlte.min.js"></script>
    <script src="Scripts/js-Login.js?0.0001"></script>
    <!--<script src="https://apis.google.com/js/platform.js" async defer></script>-->

    <script>
        var oGlob = null, iRoot = -1;
        $(document).ready(function () {
            oGlob = new General();
            var oLogin = new Login();
            oLogin.m_InicializaEvents();
            oLogin.m_validaSession(function (response) {
                if (response) 
                    window.location.href = "default.aspx"
                
            });
        });
        /*function onSignIn(googleUser) {
            var profile = googleUser.getBasicProfile();
            var sMessage = '<img src="' + profile.getImageUrl() + '"/>';
            sMessage += '<p><b>Nombre:</b> ' + profile.getName() + '</p>';
            sMessage += '<p><b>Correo:</b> ' + profile.getEmail() + '</p>';
            oGlob.m_AlertWarning(sMessage);
            console.log(JSON.stringify(profile));
            console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
            console.log('Name: ' + profile.getName());
            console.log('Image URL: ' + profile.getImageUrl());
            console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
            logout();
        }*/
    </script>
</body>
</html>