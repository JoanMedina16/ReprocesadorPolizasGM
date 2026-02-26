<form action="#" autocomplete="off">
    <div class="card card-secondary card-tabs">
        <div class="card-header p-0 pt-1">
            <ul class="nav nav-tabs" id="custom-content-above-tab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="tab-propiedades" data-toggle="pill" href="#tab-propiedades-div" role="tab" aria-controls="custom-content-above-home" aria-selected="true">Información de usuario</a>
                </li>
                <li class="nav-item" aria-disabled="true">
                    <a class="nav-link" aria-disabled="true" id="tab_permisos_usuario" data-toggle="pill" href="#tab-permisos" role="tab" aria-controls="custom-content-above-profile" aria-selected="false">Permisos & accesos al sistema</a>
                </li>
            </ul>
        </div>
        <div class="card-body">
            <div class="tab-content" id="custom-tabs-one-tabContent">
                <div class="tab-pane fade active show" id="tab-propiedades-div" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab">
                    <div class="row">                        
                        <div class="col-2" style="display:none">
                            <label>Usuario</label>
                            <input class="form-control css-texto" id="txtUsuarioID" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el usuario" disabled="disabled" readonly="readonly" >
                        </div>
                        <div class="col-2">
                            <label>Usuario</label>
                            <input class="form-control css-texto" id="txtUseralias" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el usuario" disabled="disabled" readonly="readonly">
                        </div>
                        <div class="col-5">
                            <label>Usuario OpeAdm</label>
                            <select class="form-control" id="cmbusuario"></select>
                        </div>
                        <div class="col-5">
                            <label>Correo electrónico</label>
                            <input class="form-control css-texto" id="txtcuentacorreo" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el correo electrónico del usuario">
                        </div> 
                        <div class="col-4">
                            <label>Determina el perfil del usuario</label>
                            <select class="form-control" id="cmbperfil"></select>
                        </div>
                        <div class="col-8">
                            <label>Derechos sobre el sistema</label>
                            <div class="row">
                                <p id="lblmensajesistema"></p>
                            </div>
                            <div class="row" id="div_derechos">

                            </div>
                        </div>
                       
                    </div>
                     <br />
                        <br />
                        <br />
                </div>
                <div class="tab-pane fade" id="tab-permisos" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab" >
                    <div class="row">
                        <div class="col-6">
                            <label>Listado de modulos activos</label>
                            <p>Selecciona uno o varios modulos a los cuales el usuario pueda tener accesos</p>
                            <div id="treeview-menu">
                            </div> 
                        </div>
                        <div class="col-6">
                            <label>Listado de diarios contables activos</label>
                            <p>Selecciona uno o varios diarios contables con los que el usaurio podrá trabajar (visualizar historicos, correcciones y pendientes)</p>
                            <select class="form-control" id="cmbdiarios" ></select>
                        </div>
                        <div class="col-12"><br /><br /><br /><br /><br /><br /></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>

 