<div class="card card-secondary card-outline">
    <div class="card-body">


        <div class="row">
            <div class="col-12">
                <h4>&nbsp;</h4>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="card card-info card-tabs">
                    <div class="card-header p-0 pt-1">
                        <ul class="nav nav-tabs" id="custom-tabs-five-tab" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="custom-tabs-five-overlay-tab" data-toggle="pill" href="#custom-tabs-five-overlay" role="tab" aria-controls="custom-tabs-five-overlay" aria-selected="true">Ambiente D365</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-five-conexiones" data-toggle="pill" href="#tab-conexionesd365" role="tab" aria-controls="custom-tabs-five-conexiones" aria-selected="false">Conexiones y equivalencias D365</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-five-overlay-dark-tab" data-toggle="pill" href="#custom-tabs-five-overlay-dark" role="tab" aria-controls="custom-tabs-five-overlay-dark" aria-selected="false">Diarios contables (pólizas)</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-five-normal-tab" data-toggle="pill" href="#custom-tabs-five-normal" role="tab" aria-controls="custom-tabs-five-normal" aria-selected="false">Servicio windows</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-five-tms-tab" data-toggle="pill" href="#custom-tabs-five-tms" role="tab" aria-controls="custom-tabs-five-normal" aria-selected="false">Configuración TMS</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-tmsAPI-tab" data-toggle="pill" href="#custom-tabs-tmsAPI" role="tab" aria-controls="custom-tabs-five-normal" aria-selected="false">Configuración Nueva API</a>
                            </li>
                        </ul>
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-tabs-five-tabContent">
                            <div class="tab-pane fade show active" id="custom-tabs-five-overlay" role="tabpanel" aria-labelledby="custom-tabs-five-overlay-tab">
                                <div class="row">
                                    <div class="col-12">
                                        <p>Establece las propiedades del ambiente de trabajo de Dynamic 365</p>
                                    </div>
                                    <div class="col-2">
                                        <label>Número empresa</label>
                                        <input class="form-control" id="txtnumerocia" onkeypress="return oGlob.m_soloNumeros(event)" type="text" placeholder="Captura el número de empresa" readonly="readonly" disabled>
                                    </div>
                                    <div class="col-6">
                                        <label>Nombre de la empresa</label>
                                        <input class="form-control" id="txtrazon" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la razón social">
                                    </div>
                                    <div class="col-4">
                                        <label>Empresa D365</label>
                                        <input class="form-control" id="txtnomcia" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el valor de la empresa">
                                    </div>
                                    <div class="col-4">
                                        <label>Ambiente de trabajo (URL API)</label>
                                        <input class="form-control" id="txtapi" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la URL de producción">
                                    </div>
                                    <div class="col-4">
                                        <label>Ambiente de acceso (URL API)</label>
                                        <input class="form-control" id="txtapilogin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la URL para el login D365">
                                    </div>
                                    <div class="col-4">
                                        <label>Usuario D365</label>
                                        <input class="form-control" id="txtusario" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el usuario D365">
                                    </div>
                                    <div class="col-4">
                                        <div class="input">
                                            <label>Contraseña D365</label>
                                            <input class="form-control" id="txtpass" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura la contraseña del usuario D365">
                                        </div>

                                    </div>
                                    <div class="col-4">
                                        <label>Aprobador D365</label>
                                        <input class="form-control" id="txtaproba" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el usuario que aprueba documentos">
                                    </div>
                                    <div class="col-4">
                                        <label>Cliente D365</label>
                                        <input class="form-control" id="txtcliente" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el cliente ID">
                                    </div>
                                    <div class="col-4">
                                        <label>Salida plantilla dispersiones</label>
                                        <input class="form-control" id="txtplantilla" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la salida de la plantilla para dispersión de operadores">
                                    </div>

                                    <div class="col-4">
                                        <label>Aprobador dispersiones</label>
                                        <input class="form-control" id="txtaprodisper" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el aprobador para dispersiones">
                                    </div>

                                    <div class="col-4">
                                        <label>Cuenta dispersión</label>
                                        <input class="form-control" id="txtCuentaplan_uno" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la cuenta contable para dispersiones electronicas">
                                    </div>
                                    <div class="col-4">
                                        <label>Diario dispersión</label>
                                        <input class="form-control" id="txtDiariodisp" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el diario para la dispersión" maxlength="20">
                                    </div>

                                    <div class="col-4">
                                        <label>Cuenta fondo fijo</label>
                                        <input class="form-control" id="txtCuentaplan_dos" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la cuenta contable para fondo fijo">
                                    </div>

                                    <div class="col-4">
                                        <label>Diario fondo fijo</label>
                                        <input class="form-control" id="txtDiarioFon" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el diario para el fondo fijo" maxlength="20">
                                    </div>
                                    <div class="col-4">
                                        <label>Cuenta contable viaticos</label>
                                        <input class="form-control" id="txtCuentaviatico" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la cuenta contable" maxlength="20">
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-five-overlay-dark" role="tabpanel" aria-labelledby="custom-tabs-five-overlay-dark-tab">
                                <div class="row">
                                    <div class="col-12">
                                        <p>Configura los diarios contables validas para envio de información hacia Dynamics 365. Todos los documentos inactivos serán omitidos al momento de interfazar</p>
                                    </div>
                                    <div class="col-12 text-right">
                                        <button class="btn btn-success" type="button" data-title="Nuevo documento contable" data-key="" data-page="D365/Gestion-diarios-Formulario" onclick="oGlob.m_FormAddTomodal(this)"><i class="fas fa-plus"></i>&nbsp;Agregar diario</button>
                                    </div>
                                    <div class="col-12 jqGdatos">
                                        <table id="tbl-documentos" class="table table-bordered table-hover"></table>
                                        <div id="jqGridPager"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-five-normal" role="tabpanel" aria-labelledby="custom-tabs-five-normal-tab">
                                <div class="row">
                                    <div class="col-12">
                                        <p>Configura el tiempo de replica de documentos contables, controla las veces que un documento se puede reenviar y el período de tiempo</p>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-check">
                                            <input class="form-check-input" type="checkbox" id="chkSinc365">
                                            <label class="form-check-label" for="chksaldo"><b>Activar sincronización automática de pólizas hacias D365</b></label>
                                        </div>
                                    </div>
                                    <div class="col-12">&nbsp;</div>
                                    <div class="col-3">
                                        <label>Cantidad de intentos</label>
                                        <input class="form-control" id="txintentos" onkeypress="return oGlob.m_soloNumeros(event)" type="number" min="0" max="100" value="0">
                                    </div>

                                    <div class="col-3">
                                        <label>Tiempo de ejecución(horas)</label>
                                        <input class="form-control" id="txthoras" onkeypress="return oGlob.m_soloNumeros(event)" type="number" min="0" max="20" value="0">
                                    </div>

                                    <div class="col-3">
                                        <label>Tiempo de ejecución(minutos)</label>
                                        <input class="form-control" id="txtminutos" onkeypress="return oGlob.m_soloNumeros(event)" type="number" min="5" max="100" value="0">
                                    </div>

                                    <div class="col-3">
                                        <label>Tiempo de ejecución(segundo)</label>
                                        <input class="form-control" id="txtsegundos" onkeypress="return oGlob.m_soloNumeros(event)" type="number" min="0" max="100" value="0">
                                    </div>

                                </div>
                            </div>

                            <div class="tab-pane fade" id="custom-tabs-five-tms" role="tabpanel" aria-labelledby="custom-tabs-five-tms-tab">
                                <div class="row">
                                    <div class="col-12">
                                        <p>Configura los parametros de URLS, usuarios y perfiles para sincronización de información desde sistema TMS GM Transport</p>
                                    </div>

                                    <div class="col-2">
                                        <label>RFC Empresa</label>
                                        <input class="form-control" id="txtRFCTMS" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el RFC de la empresa">
                                    </div>
                                    <div class="col-4">
                                        <label>API Conexión</label>
                                        <input class="form-control" id="txtAPITMS" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura el API de la conexión">
                                    </div>
                                    <div class="col-4">
                                        <label>URL Sincronización</label>
                                        <input class="form-control" id="txtURLAPITMS" type="text" placeholder="Captura la URL de la conexión">
                                    </div>

                                    <div class="col-2">
                                        <label>Host correo</label>
                                        <input class="form-control" id="txtHostMail" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura el host de correo">
                                    </div>

                                    <div class="col-2">
                                        <label>Puerto de correo</label>
                                        <input class="form-control" id="txtpuertoMail" onkeypress="return oGlob.m_soloNumeros(event)" type="text" placeholder="Captura el PUERTO de correo">
                                    </div>

                                    <div class="col-2">
                                        <label>Cuenta correo</label>
                                        <input class="form-control" id="txtuserMail" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura la cuenta de correo">
                                    </div>

                                    <div class="col-2">
                                        <label>Contraseña correo</label>
                                        <input class="form-control" id="txtpassMail" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura la contraseña de correo">
                                    </div>
                                    <div class="col-2">
                                        <div class="form-check" style="padding-top: 1.8em">
                                            <input class="form-check-input" type="checkbox" id="chkSSL">
                                            <label class="form-check-label" for="chkSSL"><b>Activar el uso de SSL</b></label>
                                        </div>
                                    </div>

                                    <div class="col-4">
                                        <label>Cuentas correo CxC</label>
                                        <input class="form-control" id="txtMailCxC" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura las cuentas de correo CxC">
                                    </div>
                                    <div class="col-4">
                                        <label>Cuentas correo CxP</label>
                                        <input class="form-control" id="txtMailCxP" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura las cuentas de correo CxP">
                                    </div>
                                    <div class="col-4">
                                        <label>Cuentas correo mantenimiento</label>
                                        <input class="form-control" id="txtMaiTaller" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura las cuentas de correo Mantenimiento">
                                    </div>
                                    <div class="col-4">
                                        <label>Cuentas correo comprobación gasto</label>
                                        <input class="form-control" id="txtmalGasto" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura las cuentas de correo gastos">
                                    </div>
                                    <div class="col-4">
                                        <label>Cuentas correo soporte</label>
                                        <input class="form-control" id="txtmailsoporte" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura las cuentas de soporte">
                                    </div>

                                    <div class="col-2">
                                        <label>Notificación (horas / intentos)</label>
                                        <div class="row">
                                            <div class="col-6">
                                                <input class="form-control" id="txthoranotif" onkeypress="return oGlob.m_soloNumeros(event)" type="number" min="0" max="24" value="0"></div>
                                            <div class="col-6">
                                                <input class="form-control" id="txtintentonotif" onkeypress="return oGlob.m_soloNumeros(event)" type="number" min="0" max="24" value="0"></div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <%--Pestaña de conexiones D365 b--%>
                            <div class="tab-pane fade show " id="tab-conexionesd365" role="tabpanel" aria-labelledby="custom-tabs-five-conexiones">
                                <div class="row">
                                    <div class="col-12">
                                        <p>Determina las conexiones para el ambiente de equivalencias (proveedores, cuentas, y dimensiones financieras) en D365</p>
                                    </div>
                                    <div class="col-6">
                                        <label>Cadena conexión equivalencias</label>
                                        <input class="form-control" id="txtconexion" type="text" placeholder="Captura la cadena de conexion">
                                        <%--readonly="readonly" disabled--%>
                                    </div>
                                    <div class="col-6">
                                        <label>Servidor equivalencia (IP o nombre)</label>
                                        <input class="form-control" id="txtservidor" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el servidor">
                                    </div>
                                    <div class="col-4">
                                        <label>Usuario BD equivalencia</label>
                                        <input class="form-control" id="txteqvUsuario" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el usuario">
                                    </div>

                                    <div class="col-4">
                                        <div class="input">
                                            <label>Contraseña BD equivalencia </label>
                                            <input class="form-control" id="txtpassword" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura la contraseña del usuario">
                                        </div>

                                    </div>
                                    <div class="col-4">
                                        <label>Catálogo equivalencia (BD)</label>
                                        <input class="form-control" id="txtcatalogo" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el catálogo">
                                    </div>

                                    <div class="col-6">
                                        <label>Cadena conexión ZAP</label>
                                        <input class="form-control" id="txtconnzap" type="text" placeholder="Captura la cadena de conexion">
                                    </div>

                                </div>
                            </div>
                            <%--Pestaña de conexiones D365 b--%>

                            <div class="tab-pane fade" id="custom-tabs-tmsAPI" role="tabpanel" aria-labelledby="custom-tabs-five-tms-tab">
                                <div class="row">
                                    <div class="col-12">
                                        <p>Configura los parámetros de configuración para el consumo de la nueva API TMS</p>
                                    </div>
                                    <div class="col-6">
                                        <label>URL API</label>
                                        <input class="form-control" id="txturlapinueva" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el API de la conexión">
                                    </div>
                                    <div class="col-3">
                                        <label>EndPoint Consulta</label>
                                        <input class="form-control" id="txtendpointconsulta" type="text" placeholder="Captura el EndPoint para Consulta">
                                    </div>

                                    <div class="col-6">
                                        <label>EndPoint Respuesta</label>
                                        <input class="form-control" id="txtendpointrespuesta" placeholder="Captura el EndPoint para respuesta">
                                    </div>

                                    <div class="col-4">
                                        <label>RFC Empresa</label>
                                        <input class="form-control" id="txtrfcapi" placeholder="Captura el RFC para consulta" type="text">
                                    </div>

                                    <div class="col-4">
                                        <label>Usuario</label>
                                        <input class="form-control" id="txtusuarioapi" type="text" placeholder="Captura el usuario para consulta">
                                    </div>

                                    <div class="col-4">
                                        <label>Password</label>
                                        <input class="form-control" id="txtpassapi" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="password" placeholder="Captura la contraseña del usuario de consulta">
                                    </div>

                                </div>
                            </div>



                        </div>
                    </div>
                    <!-- /.card -->
                </div>
            </div>
        </div>

    </div>
    <div class="modal-footer">
        <button class="btn btn-success" type="button" id="btnGuardaconfig">Guardar cambios</button>
    </div>
</div>
<script>
    var oconfigc = null;
    $(document).ready(function () {
        oconfigc = new Configuracion();
        oconfigc.m_InicializaEvents();
    });
</script>
